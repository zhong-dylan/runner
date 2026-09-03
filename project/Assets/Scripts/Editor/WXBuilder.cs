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
        var exportConfig = AssetDatabase.LoadAssetAtPath<ScriptableObject>("Assets/WX-WASM-SDK/Editor/WXExportConfig.asset");
        if (exportConfig != null)
            ApplyExportConfig(exportConfig);

        var convertType = FindWxConvertCore();
        if (convertType == null)
            return false;

        var exportMethod = convertType.GetMethod("DoExport", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        if (exportMethod == null)
            return false;

        exportMethod.Invoke(null, null);
        Debug.Log($"WX SDK export invoked. Channel={Channel}, CDN={CdnUrl}");
        return true;
    }

    private static void ApplyExportConfig(ScriptableObject exportConfig)
    {
        var projectConf = exportConfig.GetType().GetProperty("ProjectConf")?.GetValue(exportConfig);
        if (projectConf == null)
            return;

        SetProperty(projectConf, "projectName", GameName);
        SetProperty(projectConf, "CDN", CdnUrl);
        SetProperty(projectConf, "assetLoadType", 0);
        SetProperty(projectConf, "MemorySize", 496);
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
