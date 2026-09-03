using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class D3MainUIExtractor
{
    private const string MainScenePath = "Assets/Main.unity";
    private const string PrefabFolder = "Assets/Game/Prefabs/MainUI";

    private static readonly string[] UiNames =
    {
        "TitleGUI",
        "Settings",
        "Shop",
        "Hero",
        "WindowNoLife",
        "RewardWindow",
        "TextObject"
    };

    [MenuItem("D3 Runner/UI/Extract Main UI Prefabs")]
    public static void ExtractMainUI()
    {
        EnsureFolder(PrefabFolder);

        var scene = OpenMainScene();
        var parent = FindCommonParent(scene);
        var uiManager = GetOrCreateUIManager(parent);

        foreach (var uiName in UiNames)
        {
            ExtractPrefab(scene, uiName);
        }

        var serializedManager = new SerializedObject(uiManager);
        serializedManager.FindProperty("uiRoot").objectReferenceValue = parent;
        SetAddress(serializedManager, "TitleGUIAddress", "Prefabs/MainUI/TitleGUI");
        SetAddress(serializedManager, "SettingsAddress", "Prefabs/MainUI/Settings");
        SetAddress(serializedManager, "ShopAddress", "Prefabs/MainUI/Shop");
        SetAddress(serializedManager, "HeroAddress", "Prefabs/MainUI/Hero");
        SetAddress(serializedManager, "WindowNoLifeAddress", "Prefabs/MainUI/WindowNoLife");
        SetAddress(serializedManager, "RewardWindowAddress", "Prefabs/MainUI/RewardWindow");
        SetAddress(serializedManager, "TextObjectAddress", "Prefabs/MainUI/TextObject");
        serializedManager.ApplyModifiedPropertiesWithoutUndo();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        D3AddressablesAutoConfigurator.ConfigureProject();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static Scene OpenMainScene()
    {
        var scene = SceneManager.GetSceneByPath(MainScenePath);
        if (scene.IsValid() && scene.isLoaded)
        {
            return scene;
        }

        return EditorSceneManager.OpenScene(MainScenePath, OpenSceneMode.Single);
    }

    private static Transform FindCommonParent(Scene scene)
    {
        foreach (var uiName in UiNames)
        {
            var target = FindInScene(scene, uiName);
            if (target != null && target.transform.parent != null)
            {
                return target.transform.parent;
            }
        }

        var canvas = Object.FindObjectOfType<Canvas>();
        return canvas != null ? canvas.transform : null;
    }

    private static D3MainUIManager GetOrCreateUIManager(Transform parent)
    {
        var manager = Object.FindObjectOfType<D3MainUIManager>();
        if (manager != null)
        {
            manager.transform.SetParent(parent, false);
            manager.gameObject.name = "uimgr";
            return manager;
        }

        var gameObject = new GameObject("uimgr");
        gameObject.transform.SetParent(parent, false);
        return gameObject.AddComponent<D3MainUIManager>();
    }

    private static GameObject ExtractPrefab(Scene scene, string uiName)
    {
        var target = FindInScene(scene, uiName);
        if (target == null || target.GetComponent<D3MainUIManager>() != null)
        {
            return null;
        }

        var prefabPath = GetPrefabPath(uiName);
        var prefab = PrefabUtility.SaveAsPrefabAsset(target, prefabPath);
        Object.DestroyImmediate(target);
        return prefab;
    }

    private static GameObject FindInScene(Scene scene, string name)
    {
        foreach (var root in scene.GetRootGameObjects())
        {
            var found = FindChild(root.transform, name);
            if (found != null)
            {
                return found.gameObject;
            }
        }

        return null;
    }

    private static Transform FindChild(Transform parent, string name)
    {
        if (parent.name == name)
        {
            return parent;
        }

        foreach (Transform child in parent)
        {
            var found = FindChild(child, name);
            if (found != null)
            {
                return found;
            }
        }

        return null;
    }

    private static string GetPrefabPath(string uiName)
    {
        return $"{PrefabFolder}/{uiName}.prefab";
    }

    private static void SetAddress(SerializedObject serializedManager, string propertyName, string address)
    {
        serializedManager.FindProperty(propertyName).stringValue = address;
    }

    private static void EnsureFolder(string folder)
    {
        var parts = folder.Split('/');
        var current = parts[0];
        for (var i = 1; i < parts.Length; i++)
        {
            var next = $"{current}/{parts[i]}";
            if (!AssetDatabase.IsValidFolder(next))
            {
                AssetDatabase.CreateFolder(current, parts[i]);
            }

            current = next;
        }
    }
}
