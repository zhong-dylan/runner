using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class WXBuilder : IBuilder
{
    protected override string PlatformDefine => WxDefine;
    protected override string Channel => "wx";
    protected override string BuildFolderName => "WX";

    public static void BuildWX()
    {
        new WXBuilder().Build();
    }

    protected override bool TrySdkBuild()
    {
        var exportConfig = GetWxExportConfig();
        if (exportConfig != null)
            ApplyExportConfig(exportConfig);

        var convertType = FindWxConvertCore();
        if (convertType == null)
            return false;

        if (!TryInvokeWxExport(convertType))
            return false;

        Debug.Log($"WX SDK export invoked. Channel={Channel}, CDN={CdnUrl}");
        return true;
    }

    private void ApplyExportConfig(ScriptableObject exportConfig)
    {
        var projectConf = GetProjectConf(exportConfig);
        if (projectConf == null)
            return;

        var config = D3MiniGamePublishConfig.Load();
        SetProjectConfValue(projectConf, "projectName", GameName);
        SetProjectConfValue(projectConf, "Appid", config.wxAppId);
        SetProjectConfValue(projectConf, "CDN", CdnUrl);
        SetProjectConfValue(projectConf, "assetLoadType", 0);
        SetProjectConfValue(projectConf, "MemorySize", 496);
        SetProjectConfValue(projectConf, "relativeDST", BuildOutputPath);
        SetProjectConfValue(projectConf, "DST", Path.GetFullPath(BuildOutputPath));
        ApplyCompileOptions(exportConfig);
        EditorUtility.SetDirty(exportConfig);
        AssetDatabase.SaveAssets();
        Debug.Log($"WX export config applied. AppID={config.wxAppId}, CDN={CdnUrl}, Output={BuildOutputPath}");
    }

    private static object GetProjectConf(ScriptableObject exportConfig)
    {
        var type = exportConfig.GetType();
        return type.GetProperty("ProjectConf")?.GetValue(exportConfig) ??
               type.GetField("ProjectConf")?.GetValue(exportConfig);
    }

    private static ScriptableObject GetWxExportConfig()
    {
        var convertType = FindWxConvertCore();
        var configProperty = convertType?.GetProperty("config", BindingFlags.Public | BindingFlags.Static);
        var config = configProperty?.GetValue(null) as ScriptableObject;
        if (config != null)
        {
            return config;
        }

        return AssetDatabase.LoadAssetAtPath<ScriptableObject>("Assets/WX-WASM-SDK-V2/Editor/MiniGameConfig.asset") ??
               AssetDatabase.LoadAssetAtPath<ScriptableObject>("Assets/WX-WASM-SDK/Editor/WXExportConfig.asset") ??
               AssetDatabase.LoadAssetAtPath<ScriptableObject>("Packages/com.qq.weixin.minigame/Editor/MiniGameConfig.asset");
    }

    private static void SetProjectConfValue(object projectConf, string name, object value)
    {
        SetProperty(projectConf, name, value);
        SetField(projectConf, name, value);
    }

    private static void ApplyCompileOptions(ScriptableObject exportConfig)
    {
        var compileOptions = GetCompileOptions(exportConfig);
        if (compileOptions == null)
        {
            Debug.LogWarning("WX CompileOptions not found. iOS performance plus was not applied.");
            return;
        }

        SetProjectConfValue(compileOptions, "enableIOSPerformancePlus", true);
        SetProjectConfValue(compileOptions, "enableiOSMetal", false);
    }

    private static object GetCompileOptions(ScriptableObject exportConfig)
    {
        var type = exportConfig.GetType();
        return type.GetProperty("CompileOptions")?.GetValue(exportConfig) ??
               type.GetField("CompileOptions")?.GetValue(exportConfig);
    }

    private bool TryInvokeWxExport(Type convertType)
    {
        var methods = convertType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
            .Where(method => method.Name == "DoExport")
            .OrderBy(method => method.GetParameters().Length);

        foreach (var method in methods)
        {
            if (!TryCreateWxExportArgs(method.GetParameters(), out var args))
                continue;

            try
            {
                method.Invoke(null, args);
                return true;
            }
            catch (TargetInvocationException)
            {
                throw;
            }
            catch (ArgumentException)
            {
            }
            catch (TargetParameterCountException)
            {
            }
        }

        Debug.LogError("WX SDK DoExport method found, but no compatible parameter list could be invoked.");
        return false;
    }

    private bool TryCreateWxExportArgs(ParameterInfo[] parameters, out object[] args)
    {
        args = new object[parameters.Length];
        for (int i = 0; i < parameters.Length; i++)
        {
            var parameter = parameters[i];
            var parameterType = parameter.ParameterType;

            if (parameter.HasDefaultValue)
            {
                args[i] = parameter.DefaultValue;
                continue;
            }

            if (parameterType == typeof(bool))
            {
                args[i] = false;
                continue;
            }

            if (parameterType == typeof(string))
            {
                args[i] = BuildOutputPath;
                continue;
            }

            if (parameterType == typeof(BuildOptions))
            {
                args[i] = BuildOptions.CleanBuildCache;
                continue;
            }

            if (parameterType.IsEnum)
            {
                args[i] = Enum.GetValues(parameterType).GetValue(0);
                continue;
            }

            return false;
        }

        return true;
    }

    private static System.Type FindWxConvertCore()
    {
        foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
        {
            var type = assembly.GetType("WeChatWASM.WXConvertCore");
            if (type != null)
                return type;
        }

        return null;
    }
}
