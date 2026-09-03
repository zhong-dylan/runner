using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(D3RunnerShopScene))]

public class D3RunnerShopSceneEditor : Editor
{

    public override void OnInspectorGUI()
    {

        var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true), GUILayout.Height(170f));
        Texture2D m_Logo = (Texture2D)Resources.Load("Img/D3Icon", typeof(Texture2D));
        GUILayout.Label(m_Logo, style, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        GUILayout.EndVertical();
    }

}