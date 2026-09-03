using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(D3LevelObject))]
public class D3LevelObjectEditor : Editor
{
    D3LevelObject LevelObject;
    SerializedObject SerializedLevelObject;
    private void OnEnable()
    {
        LevelObject = target as D3LevelObject;
        if (LevelObject)
        {
            SerializedLevelObject = new SerializedObject(LevelObject);
        }

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

        if (LevelObject)
        {
            LevelObject.LevelText = EditorGUILayout.ObjectField("Level Text : ", LevelObject.LevelText, typeof(Text), true) as Text;
            GUILayout.Space(10f);

            LevelObject.LevelButton = EditorGUILayout.ObjectField("Level Button : ", LevelObject.LevelButton, typeof(Button), true) as Button;
            GUILayout.Space(10f);

            LevelObject.LockedImg = EditorGUILayout.ObjectField("Locked Img : ", LevelObject.LockedImg, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);

            SerializedLevelObject.Update();

            for (int i = 0; i < LevelObject.Star_Icons.Length; i++)
            {
                if (LevelObject.Star_Icons[i] != null)
                {
                    GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));

                    LevelObject.Star_Icons[i] = EditorGUILayout.ObjectField("Star Object "+ (i+1) + " : ", LevelObject.Star_Icons[i], typeof(Image), true) as Image;

                    GUILayout.EndVertical();
                }

            }
            SerializedLevelObject.ApplyModifiedProperties();

            GUILayout.Space(10f);

            LevelObject.Star_NoFill = EditorGUILayout.ObjectField("Star No Fill  : ", LevelObject.Star_NoFill, typeof(Sprite), true) as Sprite;
            GUILayout.Space(10f);

            LevelObject.Star_Fill = EditorGUILayout.ObjectField("Star Fill  : ", LevelObject.Star_Fill, typeof(Sprite), true) as Sprite;
            GUILayout.Space(10f);
        }
        GUILayout.Space(10f);
        GUILayout.EndVertical();


    }

}
