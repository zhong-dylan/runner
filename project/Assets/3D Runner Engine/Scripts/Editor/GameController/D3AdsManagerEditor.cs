using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(D3ADSManager))]
public class D3AdsManagerEditor : Editor
{
    D3ADSManager AdsManager;
    private void OnEnable()
    {
        AdsManager = target as D3ADSManager;
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true), GUILayout.Height(170f));
        Texture2D m_Logo = (Texture2D)Resources.Load("Img/D3Icon", typeof(Texture2D));
        GUILayout.Label(m_Logo, style, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        GUILayout.EndVertical();

        GUILayout.BeginVertical("GroupBox");
        GUILayout.Space(10f);

        GUILayout.Label("This Script is Controlled by Infinity Runner Engine.\nTo Edit Go to Unity Menu:\nDenvzla Estudio/3D Infinity Runner Engine/ Welcome Window", EditorStyles.boldLabel);

        GUILayout.Space(10f);

        GUILayout.EndVertical();

        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
        GUILayout.Label("ADS Manager Editor", style);
        GUILayout.EndVertical();

        if (AdsManager)
        {

            GUILayout.Space(10f);
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("Basic", style);

            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));

#if UNITY_ANDROID
GUILayout.Label("Current Platform: " + "ANDROID" );
#endif
#if UNITY_IOS
GUILayout.Label("Current Platform: " + "iOS"    );
#endif

            GUILayout.EndVertical();
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
#if UNITY_ANDROID
            GUILayout.Label("Unity ANDROID ADS ID: ");
            GUILayout.Space(10);
            AdsManager.UNITY_ADSID_ANDROID = GUILayout.TextField(AdsManager.UNITY_ADSID_ANDROID, 25);
#endif
#if UNITY_IOS
			GUILayout.Label("Unity IOS ADS ID: ");
            GUILayout.Space(10);
			AdsManager.UNITY_ADSID_IOS = GUILayout.TextField(AdsManager.UNITY_ADSID_IOS, 25);
			GUILayout.Space(10);

#endif
            GUILayout.EndVertical();
            GUILayout.Space(10f);
            GUILayout.EndVertical();

        }

        if (EditorGUI.EndChangeCheck())
        {
            if (!Application.isPlaying)
            {
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

        }
    }
}
