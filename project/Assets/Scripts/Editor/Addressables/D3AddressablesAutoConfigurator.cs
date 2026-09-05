using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;

[InitializeOnLoad]
public static class D3AddressablesAutoConfigurator
{
    private const string GameRoot = "Assets/Game";
    private const string MainScenePath = "Assets/Main.unity";
    private const string GroupName = "Game";
    private static readonly char[] InvalidAddressCharacters = { '[', ']' };

    static D3AddressablesAutoConfigurator()
    {
        EditorApplication.delayCall += ConfigureProject;
    }

    [MenuItem("D3 Runner/Addressables/Configure Game Assets")]
    public static void ConfigureProject()
    {
        ConfigureProject(D3MiniGamePlatformTool.GetActivePlatformDefine());
    }

    public static void ConfigureProject(string platformDefine)
    {
        var settings = GetOrCreateSettings();
        if (settings == null)
        {
            return;
        }

        ApplyRemoteProfile(settings, platformDefine);
        var group = GetOrCreateGroup(settings);
        AddGameAssets(settings, group);
        KeepOnlyMainSceneInBuildSettings();

        EditorUtility.SetDirty(settings);
        AssetDatabase.SaveAssets();
    }

    private static AddressableAssetSettings GetOrCreateSettings()
    {
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        if (settings != null)
        {
            return settings;
        }

        settings = AddressableAssetSettings.Create(
            AddressableAssetSettingsDefaultObject.kDefaultConfigFolder,
            AddressableAssetSettingsDefaultObject.kDefaultConfigAssetName,
            true,
            true);
        AddressableAssetSettingsDefaultObject.Settings = settings;
        return settings;
    }

    private static AddressableAssetGroup GetOrCreateGroup(AddressableAssetSettings settings)
    {
        var group = settings.FindGroup(GroupName);
        if (group == null)
        {
            group = settings.CreateGroup(
                GroupName,
                false,
                false,
                true,
                null,
                typeof(BundledAssetGroupSchema),
                typeof(ContentUpdateGroupSchema));
        }

        var bundledSchema = group.GetSchema<BundledAssetGroupSchema>();
        if (bundledSchema != null)
        {
            bundledSchema.BuildPath.SetVariableByName(settings, AddressableAssetSettings.kLocalBuildPath);
            bundledSchema.LoadPath.SetVariableByName(settings, AddressableAssetSettings.kLocalLoadPath);
            bundledSchema.BundleMode = BundledAssetGroupSchema.BundlePackingMode.PackSeparately;
            bundledSchema.IncludeInBuild = true;
        }

        return group;
    }

    private static void ApplyRemoteProfile(AddressableAssetSettings settings, string platformDefine)
    {
        settings.BuildRemoteCatalog = false;
        settings.BuildAddressablesWithPlayerBuild = AddressableAssetSettings.PlayerBuildOption.DoNotBuildWithPlayer;
        settings.NonRecursiveBuilding = false;
    }

    private static void AddGameAssets(AddressableAssetSettings settings, AddressableAssetGroup group)
    {
        var guids = AssetDatabase.FindAssets(string.Empty, new[] { GameRoot });
        var paths = new List<string>();
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (!IsAddressableAsset(path))
            {
                continue;
            }

            paths.Add(path);
        }

        paths.Sort(System.StringComparer.Ordinal);
        var usedAddresses = new Dictionary<string, string>();
        foreach (var path in paths)
        {
            var guid = AssetDatabase.AssetPathToGUID(path);
            var entry = settings.CreateOrMoveEntry(guid, group, false, false);
            entry.address = GetUniqueAddress(path, usedAddresses);
            entry.SetLabel("game", true, true);

            if (Path.GetExtension(path).ToLowerInvariant() == ".unity")
            {
                entry.SetLabel("scene", true, true);
            }
        }

        settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, group, true);
    }

    private static bool IsAddressableAsset(string path)
    {
        if (string.IsNullOrEmpty(path) || AssetDatabase.IsValidFolder(path))
        {
            return false;
        }

        if (!path.StartsWith(GameRoot + "/"))
        {
            return false;
        }

        var extension = Path.GetExtension(path).ToLowerInvariant();
        return extension != ".cs" && extension != ".meta";
    }

    private static string GetAddress(string path)
    {
        var relativePath = path.Substring(GameRoot.Length + 1);
        var withoutExtension = Path.ChangeExtension(relativePath, null);
        return SanitizeAddress(withoutExtension.Replace("\\", "/"));
    }

    private static string SanitizeAddress(string address)
    {
        foreach (var invalidCharacter in InvalidAddressCharacters)
        {
            address = address.Replace(invalidCharacter.ToString(), string.Empty);
        }

        return address.Trim();
    }

    private static string GetUniqueAddress(string path, Dictionary<string, string> usedAddresses)
    {
        var address = GetAddress(path);
        var uniqueAddress = address;
        var suffix = 2;
        while (usedAddresses.ContainsKey(uniqueAddress))
        {
            uniqueAddress = $"{address}_{suffix}";
            suffix++;
        }

        usedAddresses.Add(uniqueAddress, path);
        return uniqueAddress;
    }

    private static void KeepOnlyMainSceneInBuildSettings()
    {
        EditorBuildSettings.scenes = new[]
        {
            new EditorBuildSettingsScene(MainScenePath, true)
        };
    }
}

public class D3AddressablesAssetPostprocessor : AssetPostprocessor
{
    private static bool configureQueued;

    private static void OnPostprocessAllAssets(
        string[] importedAssets,
        string[] deletedAssets,
        string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        if (configureQueued || !ContainsGameAsset(importedAssets) && !ContainsGameAsset(movedAssets))
        {
            return;
        }

        configureQueued = true;
        EditorApplication.delayCall += () =>
        {
            configureQueued = false;
            D3AddressablesAutoConfigurator.ConfigureProject();
        };
    }

    private static bool ContainsGameAsset(string[] paths)
    {
        foreach (var path in paths)
        {
            if (!string.IsNullOrEmpty(path) && path.StartsWith("Assets/Game/"))
            {
                return true;
            }
        }

        return false;
    }
}
