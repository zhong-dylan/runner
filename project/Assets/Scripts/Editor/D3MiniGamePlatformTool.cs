using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

public static class D3MiniGamePlatformTool
{
    private const string ManifestPath = "Packages/manifest.json";
    private const string WxPackageName = "com.qq.weixin.minigame";
    private const string WxPackageUrl = "https://gitee.com/wechat-minigame/minigame-tuanjie-transform-sdk.git#main";
    private const string TtPackageName = "com.gameframex.unity.tuyoogame.yooasset.minigame.tiktok";
    private const string TtPackageUrl = "https://github.com/GameFrameX/com.gameframex.unity.tuyoogame.yooasset.minigame.tiktok.git";
    private static readonly string[] UploadIgnoreExtensions = { ".meta" };

    private static readonly string[] PlatformDefines = { IBuilder.WxDefine, IBuilder.TtDefine };

    [MenuItem("D3 Runner/Mini Game/Platform Tool")]
    public static void OpenPlatformTool()
    {
        D3MiniGamePlatformWindow.Open();
    }

    [MenuItem("D3 Runner/Build/Mini Game Builder")]
    public static void OpenBuildTool()
    {
        D3MiniGameBuildWindow.Open();
    }

    public static void ApplyStartupConfig()
    {
        ApplyPlayerSettings();
        WriteStartupConfig();
        Debug.Log($"Mini game startup config applied. Game={IBuilder.GameName}, CDN={GetCdnUrl(GetActivePlatformDefine())}");
    }

    public static void SwitchPlatform(string activeDefine)
    {
        if (activeDefine != IBuilder.WxDefine && activeDefine != IBuilder.TtDefine)
            throw new ArgumentException($"Unsupported mini game define: {activeDefine}");

        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);
        ApplyPlayerSettings();
        WriteStartupConfig();
        SetWebGLDefines(activeDefine);
        SwitchPlatformPackage(activeDefine);
        ApplyAddressablesSettings(activeDefine);
        AssetDatabase.SaveAssets();

        Debug.Log($"Switched mini game platform to {activeDefine}. Target=WebGL, CDN={GetCdnUrl(activeDefine)}");
    }

    public static void BuildPlatform(string activeDefine)
    {
        bool buildSucceeded;
        bool uploadSucceeded;
        if (activeDefine == IBuilder.WxDefine)
        {
            buildSucceeded = new WXBuilder().Build();
            if (buildSucceeded)
            {
                uploadSucceeded = UploadBuildWebglToTos(activeDefine);
                ShowPublishResult(uploadSucceeded, activeDefine);
            }
            return;
        }

        if (activeDefine == IBuilder.TtDefine)
        {
            buildSucceeded = new TTBuilder().Build();
            if (buildSucceeded)
            {
                uploadSucceeded = UploadBuildWebglToTos(activeDefine);
                ShowPublishResult(uploadSucceeded, activeDefine);
            }
            return;
        }

        throw new ArgumentException($"Unsupported mini game define: {activeDefine}");
    }

    public static bool BuildAddressables(string activeDefine)
    {
        if (!ClearAddressablesBuildPaths(activeDefine))
        {
            return false;
        }

        ApplyAddressablesSettings(activeDefine);

        var settingsType = AppDomain.CurrentDomain.GetAssemblies()
            .Select(assembly => assembly.GetType("UnityEditor.AddressableAssets.Settings.AddressableAssetSettings"))
            .FirstOrDefault(type => type != null);
        var buildMethod = settingsType?.GetMethod(
            "BuildPlayerContent",
            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static,
            null,
            Type.EmptyTypes,
            null);
        if (buildMethod == null)
        {
            Debug.LogError("Addressables BuildPlayerContent method not found.");
            return false;
        }

        try
        {
            buildMethod.Invoke(null, null);
            Debug.Log($"Addressables built. Local bundles will be included under StreamingAssets/aa/WebGL. CDN={GetCdnUrl(activeDefine)}");
            return true;
        }
        catch (Exception exception)
        {
            Debug.LogException(GetInnermostException(exception));
            return false;
        }
    }

    public static bool UploadBuildWebglToTos(string activeDefine)
    {
        var config = D3MiniGamePublishConfig.Load();
        if (!ValidateTosConfig(config))
        {
            return false;
        }

        var buildPath = D3MiniGamePublishConfig.GetBuildWebglPath(activeDefine);
        var fullBuildPath = Path.GetFullPath(buildPath);
        if (!Directory.Exists(fullBuildPath))
        {
            Debug.LogError("WebGL build folder not found. TOS upload skipped: " + buildPath);
            return false;
        }

        var files = Directory.GetFiles(fullBuildPath, "*", SearchOption.AllDirectories)
            .Where(file => !ShouldSkipUploadFile(file))
            .ToArray();
        if (files.Length == 0)
        {
            Debug.LogWarning("No WebGL build files to upload: " + buildPath);
            return false;
        }

        var client = CreateTosClient(config);
        if (client == null)
        {
            return false;
        }

        var inputType = FindType("Volcengine.TOS.Model.PutObjectFromFileInput");
        if (inputType == null)
        {
            Debug.LogError("TOS SDK PutObjectFromFileInput type not found. Check Assets/Plugins/TOSDLL/Volcengine.TOS.dll.");
            return false;
        }

        var putObjectMethod = client.GetType().GetMethod("PutObjectFromFile", new[] { inputType });
        if (putObjectMethod == null)
        {
            Debug.LogError("TOS SDK PutObjectFromFile API not found. Check Assets/Plugins/TOSDLL/Volcengine.TOS.dll.");
            return false;
        }

        var objectPrefix = D3MiniGamePublishConfig.GetTosObjectPrefix(activeDefine);
        var parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = Mathf.Max(1, config.tosConcurrentUploadCount)
        };
        var successCount = 0;
        var failedCount = 0;

        Debug.Log($"TOS upload started. source={buildPath}, bucket={config.tosBucketName}, prefix={objectPrefix}, cdn={GetCdnUrl(activeDefine)}, files={files.Length}, concurrency={parallelOptions.MaxDegreeOfParallelism}");
        Parallel.ForEach(files, parallelOptions, file =>
        {
            try
            {
                var normalizedFile = NormalizePath(file);
                var key = GetUploadKey(fullBuildPath, normalizedFile, objectPrefix);
                var input = Activator.CreateInstance(inputType);
                SetObjectProperty(input, "Bucket", config.tosBucketName);
                SetObjectProperty(input, "Key", key);
                SetObjectProperty(input, "FilePath", normalizedFile);

                putObjectMethod.Invoke(client, new[] { input });
                Interlocked.Increment(ref successCount);
                Debug.Log("TOS upload success: " + key);
            }
            catch (Exception exception)
            {
                Interlocked.Increment(ref failedCount);
                Debug.LogException(GetInnermostException(exception));
            }
        });

        Debug.Log($"TOS upload finished. success={successCount}, failed={failedCount}, total={files.Length}");
        return failedCount == 0 && successCount == files.Length;
    }

    private static void ShowPublishResult(bool succeeded, string activeDefine)
    {
        var platform = D3MiniGamePublishConfig.GetPlatformName(activeDefine);
        if (succeeded)
        {
            EditorUtility.DisplayDialog(
                "发布成功",
                $"平台：{platform}\nCDN：{GetCdnUrl(activeDefine)}",
                "OK");
            return;
        }

        EditorUtility.DisplayDialog(
            "发布失败",
            $"平台：{platform}\n请查看 Console 中的 TOS 上传错误。",
            "OK");
    }

    private static bool ValidateTosConfig(D3MiniGamePublishConfigData config)
    {
        if (string.IsNullOrWhiteSpace(config.tosEndpoint) ||
            string.IsNullOrWhiteSpace(config.tosRegion) ||
            string.IsNullOrWhiteSpace(config.tosBucketName) ||
            string.IsNullOrWhiteSpace(config.tosAccessKey) ||
            string.IsNullOrWhiteSpace(config.tosSecretKey))
        {
            Debug.LogError("TOS config is incomplete. Please fill Endpoint, Region, Bucket, Access Key and Secret Key in Mini Game Builder.");
            return false;
        }

        return true;
    }

    private static object CreateTosClient(D3MiniGamePublishConfigData config)
    {
        var builderType = FindType("Volcengine.TOS.TosClientBuilder");
        var builderMethod = builderType?.GetMethod("Builder", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
        if (builderType == null || builderMethod == null)
        {
            Debug.LogError("TOS SDK builder API not found. Check Assets/Plugins/TOSDLL/Volcengine.TOS.dll.");
            return null;
        }

        var builder = builderMethod.Invoke(null, null);
        InvokeBuilderSetter(builder, "SetAk", config.tosAccessKey);
        InvokeBuilderSetter(builder, "SetSk", config.tosSecretKey);
        InvokeBuilderSetter(builder, "SetRegion", config.tosRegion);
        InvokeBuilderSetter(builder, "SetEndpoint", config.tosEndpoint);
        var client = builder.GetType().GetMethod("Build")?.Invoke(builder, null);
        if (client == null)
        {
            Debug.LogError("Failed to create TOS client.");
        }

        return client;
    }

    private static void InvokeBuilderSetter(object builder, string methodName, string value)
    {
        builder.GetType().GetMethod(methodName, new[] { typeof(string) })?.Invoke(builder, new object[] { value });
    }

    private static void SetObjectProperty(object target, string propertyName, object value)
    {
        var property = target.GetType().GetProperty(propertyName);
        property?.SetValue(target, value);
    }

    private static Type FindType(string typeName)
    {
        var exactType = AppDomain.CurrentDomain.GetAssemblies()
            .Select(assembly => assembly.GetType(typeName))
            .FirstOrDefault(type => type != null);
        if (exactType != null)
        {
            return exactType;
        }

        var shortName = typeName.Split('.').Last();
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(GetTypesSafely)
            .FirstOrDefault(type => type.Name == shortName);
    }

    private static Type[] GetTypesSafely(System.Reflection.Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (System.Reflection.ReflectionTypeLoadException exception)
        {
            return exception.Types.Where(type => type != null).ToArray();
        }
        catch
        {
            return Type.EmptyTypes;
        }
    }

    private static bool ShouldSkipUploadFile(string file)
    {
        var extension = Path.GetExtension(file).ToLowerInvariant();
        return UploadIgnoreExtensions.Contains(extension) || Path.GetFileName(file) == ".DS_Store";
    }

    private static string GetUploadKey(string rootPath, string file, string objectPrefix)
    {
        var normalizedRoot = NormalizePath(rootPath).TrimEnd('/');
        var relativePath = NormalizePath(file).Substring(normalizedRoot.Length).TrimStart('/');
        return NormalizePath($"{objectPrefix}/{relativePath}");
    }

    private static string NormalizePath(string path)
    {
        return path.Replace("\\", "/");
    }

    private static Exception GetInnermostException(Exception exception)
    {
        while (exception is System.Reflection.TargetInvocationException targetInvocationException &&
               targetInvocationException.InnerException != null)
        {
            exception = targetInvocationException.InnerException;
        }

        return exception;
    }

    private static bool ClearAddressablesBuildPaths(string activeDefine)
    {
        var remoteBuildPath = D3MiniGamePublishConfig.GetRemoteBuildPath(activeDefine);
        return ClearAddressablesBuildPath(remoteBuildPath, "Addressables remote build path") &&
               ClearAddressablesBuildPath(Path.Combine("Library", "com.unity.addressables", "aa", "WebGL"), "Addressables local build path");
    }

    private static bool ClearAddressablesBuildPath(string buildPath, string label)
    {
        if (string.IsNullOrWhiteSpace(buildPath) || Path.IsPathRooted(buildPath))
        {
            Debug.LogError($"Can not clear {label}. Unsafe path: {buildPath}");
            return false;
        }

        var fullPath = Path.GetFullPath(buildPath);
        var projectPath = Path.GetFullPath(".");
        if (fullPath == projectPath ||
            !fullPath.StartsWith(projectPath + Path.DirectorySeparatorChar, StringComparison.Ordinal))
        {
            Debug.LogError($"Can not clear {label} outside project: {buildPath}");
            return false;
        }

        if (!Directory.Exists(fullPath))
        {
            return true;
        }

        Directory.Delete(fullPath, true);
        AssetDatabase.Refresh();
        Debug.Log($"Cleared {label}: {buildPath}");
        return true;
    }

    private static void ApplyPlayerSettings()
    {
        PlayerSettings.productName = IBuilder.GameName;
        PlayerSettings.SplashScreen.show = false;
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Brotli;
        PlayerSettings.WebGL.dataCaching = true;
    }

    public static string GetActivePlatformDefine()
    {
        var defines = GetDefines();
        if (defines.Contains(IBuilder.TtDefine))
        {
            return IBuilder.TtDefine;
        }

        return IBuilder.WxDefine;
    }

    public static string GetCdnUrl(string activeDefine)
    {
        return IBuilder.GetCdnUrl(activeDefine);
    }

    private static void WriteStartupConfig()
    {
        var config = D3MiniGamePublishConfig.Load();
        var content =
            "public static class D3MiniGameStartupConfig\n" +
            "{\n" +
            $"    public const string GameName = \"{Escape(config.productName)}\";\n" +
            $"    public const string CdnRoot = \"{Escape(config.cdnRoot)}\";\n" +
            $"    public const string ResourceGameName = \"{Escape(config.resourceGameName)}\";\n" +
            "\n" +
            "#if UNITY_TT\n" +
            "    public const string Platform = \"tt\";\n" +
            "#else\n" +
            "    public const string Platform = \"wx\";\n" +
            "#endif\n" +
            "\n" +
            "    public const string CdnUrl = CdnRoot + \"/\" + ResourceGameName + \"/WebGL/\" + Platform + \"/webgl\";\n" +
            "}\n";

        File.WriteAllText("Assets/Scripts/Addressables/D3MiniGameStartupConfig.cs", content);
        AssetDatabase.ImportAsset("Assets/Scripts/Addressables/D3MiniGameStartupConfig.cs");
    }

    private static string Escape(string value)
    {
        return (value ?? string.Empty).Replace("\\", "\\\\").Replace("\"", "\\\"");
    }

    private static void SetWebGLDefines(string activeDefine)
    {
        var defines = GetDefines();
        defines.RemoveWhere(define => PlatformDefines.Contains(define));
        defines.Add(activeDefine);
        SetDefines(defines);
    }

    private static HashSet<string> GetDefines()
    {
#if UNITY_2021_2_OR_NEWER
        var definesText = PlayerSettings.GetScriptingDefineSymbols(NamedBuildTarget.WebGL);
#else
        var definesText = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.WebGL);
#endif
        return definesText.Split(';')
            .Select(define => define.Trim())
            .Where(define => !string.IsNullOrEmpty(define))
            .ToHashSet();
    }

    private static void SetDefines(HashSet<string> defines)
    {
        var definesText = string.Join(";", defines.OrderBy(define => define));
#if UNITY_2021_2_OR_NEWER
        PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.WebGL, definesText);
#else
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.WebGL, definesText);
#endif
    }

    private static void SwitchPlatformPackage(string activeDefine)
    {
        if (!File.Exists(ManifestPath))
        {
            Debug.LogError($"Can not switch mini game package. Manifest not found: {ManifestPath}");
            return;
        }

        var manifest = File.ReadAllText(ManifestPath);
        manifest = RemovePackageLine(manifest, WxPackageName);
        manifest = RemovePackageLine(manifest, TtPackageName);

        if (activeDefine == IBuilder.WxDefine)
        {
            manifest = InsertPackageLine(manifest, WxPackageName, WxPackageUrl);
        }
        else
        {
            manifest = InsertPackageLine(manifest, TtPackageName, TtPackageUrl);
        }

        File.WriteAllText(ManifestPath, manifest);
        AssetDatabase.Refresh();
        UnityEditor.PackageManager.Client.Resolve();
        Debug.Log($"Mini game package switched for {activeDefine}. Unity will refresh packages from {ManifestPath}.");
    }

    private static string RemovePackageLine(string manifest, string packageName)
    {
        var lines = manifest.Replace("\r\n", "\n").Split('\n').ToList();
        var packageToken = $"\"{packageName}\"";
        lines.RemoveAll(line => line.Contains(packageToken));
        return string.Join("\n", lines);
    }

    private static string InsertPackageLine(string manifest, string packageName, string packageUrl)
    {
        var lines = manifest.Replace("\r\n", "\n").Split('\n').ToList();
        var dependencyIndex = lines.FindIndex(line => line.Contains("\"dependencies\""));
        if (dependencyIndex < 0)
        {
            Debug.LogError("Can not switch mini game package. Dependencies section not found.");
            return manifest;
        }

        var insertIndex = dependencyIndex + 1;
        lines.Insert(insertIndex, $"    \"{packageName}\": \"{packageUrl}\",");
        return string.Join("\n", lines);
    }

    private static void ApplyAddressablesSettings(string activeDefine)
    {
        var type = AppDomain.CurrentDomain.GetAssemblies()
            .Select(assembly => assembly.GetType("D3AddressablesAutoConfigurator"))
            .FirstOrDefault(foundType => foundType != null);
        var method = type?.GetMethod(
            "ConfigureProject",
            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static,
            null,
            new[] { typeof(string) },
            null);
        method?.Invoke(null, new object[] { activeDefine });
    }
}

public class D3MiniGamePlatformWindow : EditorWindow
{
    private int selectedPlatform;
    private readonly string[] platformNames = { "WeChat", "Douyin" };
    private D3MiniGamePublishConfigData config;

    public static void Open()
    {
        var window = GetWindow<D3MiniGamePlatformWindow>("Mini Game Platform");
        window.minSize = new Vector2(420f, 320f);
        window.Show();
    }

    private void OnEnable()
    {
        config = D3MiniGamePublishConfig.Load();
    }

    private void OnGUI()
    {
        if (config == null)
        {
            config = D3MiniGamePublishConfig.Load();
        }

        GUILayout.Label("Platform", EditorStyles.boldLabel);
        selectedPlatform = GUILayout.Toolbar(selectedPlatform, platformNames);

        EditorGUILayout.Space(8f);
        EditorGUI.BeginChangeCheck();
        config.productName = EditorGUILayout.TextField("Product Name", config.productName);
        config.resourceGameName = EditorGUILayout.TextField("CDN Game Name", config.resourceGameName);
        config.cdnRoot = EditorGUILayout.TextField("CDN Root", config.cdnRoot);
        config.wxAppId = EditorGUILayout.TextField("WX AppID", config.wxAppId);
        EditorGUILayout.Space(6f);
        GUILayout.Label("TOS", EditorStyles.boldLabel);
        config.tosEndpoint = EditorGUILayout.TextField("Endpoint", config.tosEndpoint);
        config.tosRegion = EditorGUILayout.TextField("Region", config.tosRegion);
        config.tosBucketName = EditorGUILayout.TextField("Bucket", config.tosBucketName);
        config.tosAccessKey = EditorGUILayout.TextField("Access Key", config.tosAccessKey);
        config.tosSecretKey = EditorGUILayout.PasswordField("Secret Key", config.tosSecretKey);
        config.tosConcurrentUploadCount = EditorGUILayout.IntField("Upload Concurrency", config.tosConcurrentUploadCount);
        if (EditorGUI.EndChangeCheck())
        {
            D3MiniGamePublishConfig.Save(config);
        }

        EditorGUILayout.LabelField("Target", "WebGL");
        EditorGUILayout.LabelField("Platform", D3MiniGamePublishConfig.GetPlatformName(GetSelectedDefine()));
        EditorGUILayout.LabelField("Addressables", "StreamingAssets/aa/WebGL");
        EditorGUILayout.LabelField("CDN", D3MiniGamePlatformTool.GetCdnUrl(GetSelectedDefine()));
        EditorGUILayout.LabelField("Define", GetSelectedDefine());

        EditorGUILayout.Space(12f);
        if (GUILayout.Button("Switch Platform", GUILayout.Height(32f)))
        {
            D3MiniGamePlatformTool.SwitchPlatform(GetSelectedDefine());
        }

        if (GUILayout.Button("Apply Startup Config", GUILayout.Height(26f)))
        {
            D3MiniGamePlatformTool.ApplyStartupConfig();
        }
    }

    private string GetSelectedDefine()
    {
        return selectedPlatform == 0 ? IBuilder.WxDefine : IBuilder.TtDefine;
    }
}

public class D3MiniGameBuildWindow : EditorWindow
{
    private int selectedPlatform;
    private readonly string[] platformNames = { "WXBuilder", "TTBuilder" };
    private D3MiniGamePublishConfigData config;

    public static void Open()
    {
        var window = GetWindow<D3MiniGameBuildWindow>("Mini Game Builder");
        window.minSize = new Vector2(420f, 360f);
        window.Show();
    }

    private void OnEnable()
    {
        config = D3MiniGamePublishConfig.Load();
    }

    private void OnGUI()
    {
        if (config == null)
        {
            config = D3MiniGamePublishConfig.Load();
        }

        GUILayout.Label("Build", EditorStyles.boldLabel);
        selectedPlatform = GUILayout.Toolbar(selectedPlatform, platformNames);

        EditorGUILayout.Space(8f);
        EditorGUI.BeginChangeCheck();
        config.productName = EditorGUILayout.TextField("Product Name", config.productName);
        config.resourceGameName = EditorGUILayout.TextField("CDN Game Name", config.resourceGameName);
        config.cdnRoot = EditorGUILayout.TextField("CDN Root", config.cdnRoot);
        config.wxAppId = EditorGUILayout.TextField("WX AppID", config.wxAppId);
        EditorGUILayout.Space(6f);
        GUILayout.Label("TOS", EditorStyles.boldLabel);
        config.tosEndpoint = EditorGUILayout.TextField("Endpoint", config.tosEndpoint);
        config.tosRegion = EditorGUILayout.TextField("Region", config.tosRegion);
        config.tosBucketName = EditorGUILayout.TextField("Bucket", config.tosBucketName);
        config.tosAccessKey = EditorGUILayout.TextField("Access Key", config.tosAccessKey);
        config.tosSecretKey = EditorGUILayout.PasswordField("Secret Key", config.tosSecretKey);
        config.tosConcurrentUploadCount = EditorGUILayout.IntField("Upload Concurrency", config.tosConcurrentUploadCount);
        if (EditorGUI.EndChangeCheck())
        {
            D3MiniGamePublishConfig.Save(config);
        }

        EditorGUILayout.LabelField("Target", "WebGL");
        EditorGUILayout.LabelField("Output", D3MiniGamePublishConfig.GetBuildOutputPath(GetSelectedDefine()));
        EditorGUILayout.LabelField("Build WebGL Path", D3MiniGamePublishConfig.GetBuildWebglPath(GetSelectedDefine()));
        EditorGUILayout.LabelField("Platform", D3MiniGamePublishConfig.GetPlatformName(GetSelectedDefine()));
        EditorGUILayout.LabelField("Addressables", "StreamingAssets/aa/WebGL");
        EditorGUILayout.LabelField("CDN", D3MiniGamePlatformTool.GetCdnUrl(GetSelectedDefine()));
        EditorGUILayout.LabelField("TOS Prefix", D3MiniGamePublishConfig.GetTosObjectPrefix(GetSelectedDefine()));

        EditorGUILayout.Space(12f);
        if (GUILayout.Button("Switch Platform", GUILayout.Height(28f)))
        {
            D3MiniGamePlatformTool.SwitchPlatform(GetSelectedDefine());
        }

        if (GUILayout.Button("Build Selected And Upload WebGL", GUILayout.Height(32f)))
        {
            D3MiniGamePlatformTool.BuildPlatform(GetSelectedDefine());
        }
    }

    private string GetSelectedDefine()
    {
        return selectedPlatform == 0 ? IBuilder.WxDefine : IBuilder.TtDefine;
    }
}
