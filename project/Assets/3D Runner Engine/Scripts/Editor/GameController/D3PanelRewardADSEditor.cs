using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(D3PanelRewardADS))]
public class D3PanelRewardADSEditor : Editor
{
    D3PanelRewardADS MyTarget;
    D3GUIManager GameGui;

    private void OnEnable()
    {
        MyTarget = target as D3PanelRewardADS;

        if (FindObjectOfType<D3GUIManager>())
        {
            GameGui = FindObjectOfType<D3GUIManager>();
        }
        else
        {
            Debug.LogWarning("<b>ERROR</b> D3GUIManager Script not found in the scene, it is recommended to delete the scene and create a new one or review the demo scene and compare the objects.</b>");
        }
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
        GUILayout.Label("Panel Reward ADS Editor", style);
        GUILayout.EndVertical();
        if (MyTarget)
        {
            GUILayout.Space(10f);
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("Config", style);
            GUILayout.Space(10f);
            MyTarget.m_Animator = EditorGUILayout.ObjectField("Animator: ", MyTarget.m_Animator, typeof(Animator), true) as Animator;
            GUILayout.Space(10f);
            MyTarget.RewardButton = EditorGUILayout.ObjectField("Reward Button: ", MyTarget.RewardButton, typeof(Button), true) as Button;
            GUILayout.Space(10f);
            MyTarget.TextReward = EditorGUILayout.ObjectField("Text Reward: ", MyTarget.TextReward, typeof(TextMeshProUGUI), true) as TextMeshProUGUI;
            GUILayout.Space(10f);
            MyTarget.ImgReward = EditorGUILayout.ObjectField("Img Reward: ", MyTarget.ImgReward, typeof(Image), true) as Image;
            GUILayout.Space(10f);
            GUILayout.EndVertical();

        }
        if (EditorGUI.EndChangeCheck())
        {
            if (!Application.isPlaying)
            {
                if (GameGui)
                {
                    EditorUtility.SetDirty(GameGui);
                    PrefabUtility.RecordPrefabInstancePropertyModifications(GameGui);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

            }

        }
    }

}
