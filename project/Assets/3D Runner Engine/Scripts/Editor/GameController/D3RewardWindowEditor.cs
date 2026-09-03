using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(D3RewardWindow))]
public class D3RewardWindowEditor : Editor
{
    D3RewardWindow itemTarget;
    private void OnEnable()
    {
        itemTarget = target as D3RewardWindow;
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true), GUILayout.Height(170f));
        Texture2D m_Logo = (Texture2D)Resources.Load("Img/D3Icon", typeof(Texture2D));
        GUILayout.Label(m_Logo, style, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        GUILayout.EndVertical();

        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
        GUILayout.Label("Reward Window Editor", style);
        GUILayout.EndVertical();

        if (itemTarget)
        {

            GUILayout.Space(10f);
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("Basic", style);

            GUILayout.Space(10f);
            itemTarget.TextReward = EditorGUILayout.ObjectField("Reward Text: ", itemTarget.TextReward, typeof(Text), true) as Text;

            GUILayout.Space(10f);
            itemTarget.ImageReward = EditorGUILayout.ObjectField("Image Reward: ", itemTarget.ImageReward, typeof(Image), true) as Image;

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
