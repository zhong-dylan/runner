#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[InitializeOnLoad]
public static class D3TextObjectPrefabBinderInstaller
{
    private const string PrefabPath = "Assets/Game/Prefabs/MainUI/TextObject.prefab";
    private const string SessionKey = "D3TextObjectPrefabBinderInstaller.Done";

    static D3TextObjectPrefabBinderInstaller()
    {
        EditorApplication.delayCall += InstallOnce;
    }

    [MenuItem("D3 Runner/Addressables/Fix TextObject Button Binder")]
    public static void Install()
    {
        InstallInternal();
    }

    private static void InstallOnce()
    {
        if (SessionState.GetBool(SessionKey, false))
        {
            return;
        }

        SessionState.SetBool(SessionKey, true);
        InstallInternal();
    }

    private static void InstallInternal()
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
        if (prefab == null)
        {
            return;
        }

        var instance = PrefabUtility.LoadPrefabContents(PrefabPath);
        if (instance == null)
        {
            return;
        }

        try
        {
            var binder = instance.GetComponent<D3TextObjectButtonBinder>();
            if (binder == null)
            {
                binder = instance.AddComponent<D3TextObjectButtonBinder>();
            }

            var serializedObject = new SerializedObject(binder);
            serializedObject.FindProperty("totalLifeButton").objectReferenceValue = FindButton(instance.transform, "TotalLIFE");
            serializedObject.FindProperty("totalHoverBoardButton").objectReferenceValue = FindButton(instance.transform, "TotalHoverBoard");
            serializedObject.ApplyModifiedPropertiesWithoutUndo();

            PrefabUtility.SaveAsPrefabAsset(instance, PrefabPath);
            AssetDatabase.SaveAssets();
        }
        finally
        {
            PrefabUtility.UnloadPrefabContents(instance);
        }
    }

    private static Button FindButton(Transform root, string buttonName)
    {
        var transforms = root.GetComponentsInChildren<Transform>(true);
        foreach (var item in transforms)
        {
            if (item.name != buttonName)
            {
                continue;
            }

            var button = item.GetComponent<Button>();
            return button != null ? button : item.GetComponentInChildren<Button>(true);
        }

        return null;
    }
}
#endif
