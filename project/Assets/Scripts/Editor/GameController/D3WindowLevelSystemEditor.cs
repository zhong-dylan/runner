using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(D3LevelSystem))]
public class D3WindowLevelSystemEditor : Editor
{
    D3LevelSystem LevelSystem;
    private void OnEnable()
    {
        LevelSystem = target as D3LevelSystem;
    }
    public override void OnInspectorGUI()
    {
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

        GUILayout.BeginVertical("GroupBox");
        GUILayout.Space(10f);

        if (LevelSystem)
        {
            LevelSystem.TemplateLevelPrefab = EditorGUILayout.ObjectField("Template Level Prefab: ", LevelSystem.TemplateLevelPrefab, typeof(GameObject), true) as GameObject;
            GUILayout.Space(5f);
            LevelSystem.RootCanvasLevelsParent = EditorGUILayout.ObjectField("Root Canvas Levels: ", LevelSystem.RootCanvasLevelsParent, typeof(Transform), true) as Transform;
            
        }
        GUILayout.Space(10f);
        GUILayout.EndVertical();


    }
}

