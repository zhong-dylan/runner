using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        Debug.Log($"Mini game startup config applied. Game={IBuilder.GameName}, CDN={IBuilder.CdnUrl}");
    }

    public static void SwitchPlatform(string activeDefine)
    {
        if (activeDefine != IBuilder.WxDefine && activeDefine != IBuilder.TtDefine)
            throw new ArgumentException($"Unsupported mini game define: {activeDefine}");

        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);
        ApplyPlayerSettings();
        SetWebGLDefines(activeDefine);
        SwitchPlatformPackage(activeDefine);
        ApplyAddressablesSettings();
        AssetDatabase.SaveAssets();

        Debug.Log($"Switched mini game platform to {activeDefine}. Target=WebGL, CDN={IBuilder.CdnUrl}");
    }

    public static void BuildPlatform(string activeDefine)
    {
        if (activeDefine == IBuilder.WxDefine)
        {
            new WXBuilder().Build();
            return;
        }

        if (activeDefine == IBuilder.TtDefine)
        {
            new TTBuilder().Build();
            return;
        }

        throw new ArgumentException($"Unsupported mini game define: {activeDefine}");
    }

    private static void ApplyPlayerSettings()
    {
        PlayerSettings.productName = IBuilder.GameName;
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Brotli;
        PlayerSettings.WebGL.dataCaching = true;
    }

    private static void SetWebGLDefines(string activeDefine)
    {
        var defines = GetDefines();
        defines.RemoveWhere(define => PlatformDefines.Contains(define));
        defines.Add(activeDefine);
        defines.Add(IBuilder.FlagRunnerDefine);
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

    private static void ApplyAddressablesSettings()
    {
        var type = AppDomain.CurrentDomain.GetAssemblies()
            .Select(assembly => assembly.GetType("D3AddressablesAutoConfigurator"))
            .FirstOrDefault(foundType => foundType != null);
        var method = type?.GetMethod("ConfigureProject", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
        method?.Invoke(null, null);
    }
}

public class D3MiniGamePlatformWindow : EditorWindow
{
    private int selectedPlatform;
    private readonly string[] platformNames = { "WeChat", "Douyin" };

    public static void Open()
    {
        var window = GetWindow<D3MiniGamePlatformWindow>("Mini Game Platform");
        window.minSize = new Vector2(360f, 180f);
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Platform", EditorStyles.boldLabel);
        selectedPlatform = GUILayout.Toolbar(selectedPlatform, platformNames);

        EditorGUILayout.Space(8f);
        EditorGUILayout.LabelField("Target", "WebGL");
        EditorGUILayout.LabelField("Game", IBuilder.GameName);
        EditorGUILayout.LabelField("CDN", IBuilder.CdnUrl);
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

    public static void Open()
    {
        var window = GetWindow<D3MiniGameBuildWindow>("Mini Game Builder");
        window.minSize = new Vector2(360f, 180f);
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Build", EditorStyles.boldLabel);
        selectedPlatform = GUILayout.Toolbar(selectedPlatform, platformNames);

        EditorGUILayout.Space(8f);
        EditorGUILayout.LabelField("Target", "WebGL");
        EditorGUILayout.LabelField("Output", selectedPlatform == 0 ? "Build/WX" : "Build/TT");
        EditorGUILayout.LabelField("CDN", IBuilder.CdnUrl);

        EditorGUILayout.Space(12f);
        if (GUILayout.Button("Switch Platform", GUILayout.Height(28f)))
        {
            D3MiniGamePlatformTool.SwitchPlatform(GetSelectedDefine());
        }

        if (GUILayout.Button("Build Selected", GUILayout.Height(32f)))
        {
            D3MiniGamePlatformTool.BuildPlatform(GetSelectedDefine());
        }
    }

    private string GetSelectedDefine()
    {
        return selectedPlatform == 0 ? IBuilder.WxDefine : IBuilder.TtDefine;
    }
}
