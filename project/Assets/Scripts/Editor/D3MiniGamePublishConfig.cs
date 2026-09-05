using System;
using System.IO;
using UnityEditor;
using UnityEngine;

[Serializable]
public class D3MiniGamePublishConfigData
{
    public string productName = "Flag Runner";
    public string cdnRoot = "https://qzz2d.qzzres.com/M5_BUILD_TEST";
    public string resourceGameName = "FlagRunner";
    public string wxAppId = "wx0ebdbae42011b015";
    public string tosEndpoint = "https://tos-cn-beijing.volces.com";
    public string tosRegion = "cn-beijing";
    public string tosBucketName = "quzizi-2dgame-res";
    public string tosAccessKey = "";
    public string tosSecretKey = "";
    public int tosConcurrentUploadCount = 8;
}

public static class D3MiniGamePublishConfig
{
    private const string ConfigPath = "Assets/MiniGamePublishConfig.json";

    public static D3MiniGamePublishConfigData Load()
    {
        if (!File.Exists(ConfigPath))
        {
            var defaultConfig = new D3MiniGamePublishConfigData();
            Save(defaultConfig);
            return defaultConfig;
        }

        try
        {
            var config = JsonUtility.FromJson<D3MiniGamePublishConfigData>(File.ReadAllText(ConfigPath));
            return Normalize(config);
        }
        catch (Exception exception)
        {
            Debug.LogWarning("Failed to load mini game publish config. Use defaults. " + exception.Message);
            return new D3MiniGamePublishConfigData();
        }
    }

    public static void Save(D3MiniGamePublishConfigData config)
    {
        config = Normalize(config);
        File.WriteAllText(ConfigPath, JsonUtility.ToJson(config, true));
        AssetDatabase.ImportAsset(ConfigPath);
    }

    public static string GetCdnUrl(string platformDefine)
    {
        var config = Load();
        return BuildCdnUrl(config.cdnRoot, config.resourceGameName, "WebGL", GetPlatformName(platformDefine), "webgl");
    }

    public static string GetRemoteBuildPath(string platformDefine)
    {
        return "ServerData";
    }

    public static string GetTosObjectPrefix(string platformDefine)
    {
        var config = Load();
        var cdnPath = GetCdnPath(config.cdnRoot);
        return string.Join("/",
            SanitizePathSegment(cdnPath),
            SanitizePathSegment(config.resourceGameName),
            "WebGL",
            GetPlatformName(platformDefine),
            "webgl");
    }

    public static string GetBuildFolderName(string platformDefine)
    {
        return platformDefine == IBuilder.TtDefine ? "TT" : "WX";
    }

    public static string GetBuildOutputPath(string platformDefine)
    {
        return NormalizePath(Path.Combine("Build", GetBuildFolderName(platformDefine)));
    }

    public static string GetBuildWebglPath(string platformDefine)
    {
        return NormalizePath(Path.Combine(GetBuildOutputPath(platformDefine), "webgl"));
    }

    public static string GetPlatformName(string platformDefine)
    {
        return platformDefine == IBuilder.TtDefine ? "tt" : "wx";
    }

    private static string BuildCdnUrl(string cdnRoot, string gameName, string buildTarget, string platform, string outputFolder)
    {
        return string.Join("/",
            TrimSlashes(cdnRoot),
            SanitizePathSegment(gameName),
            SanitizePathSegment(buildTarget),
            SanitizePathSegment(platform),
            SanitizePathSegment(outputFolder));
    }

    private static D3MiniGamePublishConfigData Normalize(D3MiniGamePublishConfigData config)
    {
        if (config == null)
        {
            config = new D3MiniGamePublishConfigData();
        }

        if (string.IsNullOrWhiteSpace(config.productName))
        {
            config.productName = "Flag Runner";
        }

        if (string.IsNullOrWhiteSpace(config.cdnRoot))
        {
            config.cdnRoot = "https://qzz2d.qzzres.com/M5_BUILD_TEST";
        }

        if (string.IsNullOrWhiteSpace(config.resourceGameName))
        {
            config.resourceGameName = config.productName.Replace(" ", string.Empty);
        }

        if (string.IsNullOrWhiteSpace(config.wxAppId))
        {
            config.wxAppId = "wx0ebdbae42011b015";
        }

        if (string.IsNullOrWhiteSpace(config.tosEndpoint))
        {
            config.tosEndpoint = "https://tos-cn-beijing.volces.com";
        }

        if (string.IsNullOrWhiteSpace(config.tosRegion))
        {
            config.tosRegion = "cn-beijing";
        }

        if (string.IsNullOrWhiteSpace(config.tosBucketName))
        {
            config.tosBucketName = "quzizi-2dgame-res";
        }

        if (config.tosConcurrentUploadCount <= 0)
        {
            config.tosConcurrentUploadCount = 8;
        }

        config.cdnRoot = config.cdnRoot.Trim().TrimEnd('/');
        config.resourceGameName = SanitizePathSegment(config.resourceGameName);
        config.wxAppId = config.wxAppId.Trim();
        config.tosEndpoint = config.tosEndpoint.Trim().TrimEnd('/');
        config.tosRegion = config.tosRegion.Trim();
        config.tosBucketName = config.tosBucketName.Trim();
        config.tosAccessKey = (config.tosAccessKey ?? string.Empty).Trim();
        config.tosSecretKey = (config.tosSecretKey ?? string.Empty).Trim();
        return config;
    }

    private static string GetCdnPath(string cdnRoot)
    {
        if (Uri.TryCreate(cdnRoot, UriKind.Absolute, out var uri))
        {
            return uri.AbsolutePath.Trim('/');
        }

        return TrimSlashes(cdnRoot);
    }

    private static string TrimSlashes(string value)
    {
        return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim().TrimEnd('/');
    }

    private static string SanitizePathSegment(string value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? string.Empty
            : value.Trim().Replace("\\", "/").Trim('/');
    }

    private static string NormalizePath(string value)
    {
        return value.Replace("\\", "/");
    }
}
