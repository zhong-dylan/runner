using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public abstract class IBuilder
{
    public const string WxDefine = "UNITY_WX";
    public const string TtDefine = "UNITY_TT";
    public const string FlagRunnerDefine = "FLAG_RUNNER";
    public const string CdnUrl = "https://qzz2d.qzzres.com/M5_BUILD_TEST";
    public const string GameName = "Flag Runner";
    public const string MainScenePath = "Assets/Main.unity";

    protected abstract string PlatformDefine { get; }
    protected abstract string Channel { get; }
    protected abstract string BuildFolderName { get; }

    public void Build()
    {
        D3MiniGamePlatformTool.SwitchPlatform(PlatformDefine);

        var sdkHandled = TrySdkBuild();
        if (sdkHandled)
            return;

        var outputPath = Path.Combine("Build", BuildFolderName);
        Directory.CreateDirectory(outputPath);

        var options = new BuildPlayerOptions
        {
            scenes = new[] { MainScenePath },
            locationPathName = outputPath,
            target = BuildTarget.WebGL,
            targetGroup = BuildTargetGroup.WebGL,
            options = BuildOptions.CleanBuildCache
        };

        var report = BuildPipeline.BuildPlayer(options);
        if (report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
        {
            Debug.Log($"{BuildFolderName} WebGL build succeeded: {report.summary.totalSize} bytes, CDN={CdnUrl}");
            return;
        }

        Debug.LogError($"{BuildFolderName} WebGL build failed: {report.summary.result}");
    }

    protected virtual bool TrySdkBuild()
    {
        return false;
    }

    protected static bool TryInvokeStaticMethod(string typeName, string methodName)
    {
        var type = FindType(typeName);
        if (type == null)
            return false;

        var method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        if (method == null)
            return false;

        method.Invoke(null, null);
        return true;
    }

    protected static object GetStaticProperty(string typeName, string propertyName)
    {
        var type = FindType(typeName);
        var property = type?.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        return property?.GetValue(null);
    }

    protected static void SetProperty(object target, string propertyName, object value)
    {
        var property = target?.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (property == null || !property.CanWrite)
            return;

        var convertedValue = ConvertValue(value, property.PropertyType);
        property.SetValue(target, convertedValue);
    }

    protected static void SetField(object target, string fieldName, object value)
    {
        var field = target?.GetType().GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (field == null)
            return;

        var convertedValue = ConvertValue(value, field.FieldType);
        field.SetValue(target, convertedValue);
    }

    protected static object ConvertValue(object value, Type targetType)
    {
        if (value == null)
            return null;

        var valueType = value.GetType();
        if (targetType.IsAssignableFrom(valueType))
            return value;

        if (targetType.IsEnum && value is string enumName)
            return Enum.GetNames(targetType).Contains(enumName)
                ? Enum.Parse(targetType, enumName)
                : Enum.GetValues(targetType).GetValue(0);

        return Convert.ChangeType(value, targetType);
    }

    private static Type FindType(string typeName)
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .Select(assembly => assembly.GetType(typeName))
            .FirstOrDefault(type => type != null);
    }
}
