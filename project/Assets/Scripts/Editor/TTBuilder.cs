using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TTBuilder : IBuilder
{
    protected override string PlatformDefine => TtDefine;
    protected override string Channel => "tt";
    protected override string BuildFolderName => "TT";

    public static void BuildTT()
    {
        new TTBuilder().Build();
    }

    protected override bool TrySdkBuild()
    {
        var settings = GetStaticProperty("TTSDK.Tool.StarkBuilderSettings", "Instance");
        if (settings == null)
            return false;

        SetProperty(settings, "CDN", CdnUrl);
        SetProperty(settings, "urlCacheList", AppendUnique(GetStringArrayProperty(settings, "urlCacheList"), GetUrlRoot(CdnUrl)));
        SetProperty(settings, "wasmMemorySize", 496);
        SetProperty(settings, "framework", "Wasm");
        SetProperty(settings, "needCompress", true);
        SetProperty(settings, "OutputDir", BuildOutputPath);
        SetProperty(settings, "symbolMode", "External");
        SetProperty(settings, "buildOptions", BuildOptions.CleanBuildCache);

        if (TryInvokeBuildManager())
        {
            Debug.Log($"TT SDK build invoked. Channel={Channel}, CDN={CdnUrl}");
            return true;
        }

        return false;
    }

    private static string GetUrlRoot(string url)
    {
        if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
            return $"{uri.Scheme}://{uri.Host}";

        return url;
    }

    private static string[] GetStringArrayProperty(object target, string propertyName)
    {
        var property = target.GetType().GetProperty(propertyName);
        return property?.GetValue(target) as string[];
    }

    private static string[] AppendUnique(string[] values, string value)
    {
        value = value?.TrimEnd('/');
        if (string.IsNullOrEmpty(value))
            return values;

        if (values == null || values.Length == 0)
            return new[] { value };

        return values.Any(item => string.Equals(item?.TrimEnd('/'), value, StringComparison.OrdinalIgnoreCase))
            ? values
            : values.Concat(new[] { value }).ToArray();
    }

    private static bool TryInvokeBuildManager()
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            var type = assembly.GetType("TTSDK.Tool.BuildManager");
            if (type == null)
                continue;

            var methods = type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .Where(method => method.Name == "Build")
                .OrderBy(method => method.GetParameters().Length);

            foreach (var method in methods)
            {
                var parameters = method.GetParameters();
                var args = new object[parameters.Length];
                var canInvoke = true;

                for (int i = 0; i < parameters.Length; i++)
                {
                    var parameterType = parameters[i].ParameterType;
                    if (parameterType == typeof(bool))
                    {
                        args[i] = false;
                        continue;
                    }

                    if (parameterType.IsEnum)
                    {
                        args[i] = Enum.GetNames(parameterType).Contains("Wasm")
                            ? Enum.Parse(parameterType, "Wasm")
                            : Enum.GetValues(parameterType).GetValue(0);
                        continue;
                    }

                    canInvoke = false;
                    break;
                }

                if (!canInvoke)
                    continue;

                method.Invoke(null, args);
                return true;
            }
        }

        return false;
    }
}
