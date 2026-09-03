using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class D3RunnerTitleSceneEditor : EditorWindow
{
    
    private static D3RunnerTitleSceneEditor window;

    D3TitleScene TitleScene;
    D3RunnerGame GameM;


    [MenuItem("Denvzla Estudio/3D Infinity Runner Engine/Welcome Window")]
    public static void Init()
    {

        // Get existing open window or if none, make a new one:
        window = (D3RunnerTitleSceneEditor)EditorWindow.GetWindow(typeof(D3RunnerTitleSceneEditor), true, "Welcome To 3D Runner Editor"); 
        window.Show();
    }

    [MenuItem("Denvzla Estudio/3D Infinity Runner Engine/Online Manual")]
    public static void OnlineManual()
    {
        Application.OpenURL("https://denvzla-estudio.gitbook.io/infinite-runner-engine-3d/");
    }

    [MenuItem("Denvzla Estudio/Official Website")]
    public static void OfficialWebsite()
    {
        Application.OpenURL("https://denvzlaestudio.com");
    }

    [MenuItem("Denvzla Estudio/Forum Oficial")]
    public static void OfficialForum()
    {
        Application.OpenURL("https://denvzlaestudio.proboards.com/");
    }

    void AEnable()
    {
        if (FindObjectOfType<D3TitleScene>())
        {
            TitleScene = FindObjectOfType<D3TitleScene>();
        }

        if (FindObjectOfType<D3RunnerGame>())
        {
            GameM = FindObjectOfType<D3RunnerGame>();
        }


    }

    void OnGUI()
    {
        AEnable();

        var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true),GUILayout.Height(170f));
        Texture2D m_Logo = (Texture2D)Resources.Load("Img/D3Icon", typeof(Texture2D));
        GUILayout.Label(m_Logo, style, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        GUILayout.EndVertical();

        GUILayout.Space(10f);
        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
        GUILayout.Label("3D Runner Editor", style);
        GUILayout.EndVertical();
        GUILayout.Space(5f);

        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));

        if (GameM)
        {

            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            EditorGUILayout.TextArea("Current Scene: Game Editor", style);
            GUILayout.EndVertical();

            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            if (GUILayout.Button("Edit in the Inspector Window"))
            {
                EditorGUIUtility.PingObject(GameM);
                Selection.activeGameObject = GameM.gameObject;
                SceneView.lastActiveSceneView.FrameSelected();
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            if (GUILayout.Button("Delete and Create a New Scene"))
            {
                EditorGUIUtility.PingObject(GameM);
                DestroyImmediate(GameM.gameObject);
                GameM = null;
            }
            GUILayout.EndVertical();
        }

        if (TitleScene)
        {

            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            EditorGUILayout.TextArea("Current Scene: Title And Shop Editor", style);
            GUILayout.EndVertical();

            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            if (GUILayout.Button("Edit in the Inspector Window"))
            {
                EditorGUIUtility.PingObject(TitleScene);
                Selection.activeGameObject = TitleScene.gameObject;
                SceneView.lastActiveSceneView.FrameSelected();
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            if (GUILayout.Button("Delete and Create a New Scene"))
            {
                EditorGUIUtility.PingObject(TitleScene);
                DestroyImmediate(TitleScene.gameObject);
                TitleScene = null;
            }
            GUILayout.EndVertical();
        }




        if (!TitleScene && !GameM)
        {
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            EditorGUILayout.TextArea("ATTENTION: No Game Scene Detected", style);
            GUILayout.EndVertical();

            GUILayout.Space(10f);

            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            if (GUILayout.Button("Create New Title And Shop Scene"))
            {
                //instantiate ui canvas
                GameObject ASS = Instantiate(Resources.Load("Game/3DRunnerTitleScene")) as GameObject;
                //rename it
                ASS.name = "3DRunnerTitleScene";
                ASS.transform.position = new Vector3(0f, 0f, 0f);

                Debug.Log("Infinity Runner Title And Shop Scene Prefabs Created!");
            }
            GUILayout.EndVertical();


            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            if (GUILayout.Button("Create New Infinity Runner Game Scene"))
            {
                //instantiate ui canvas
                GameObject ASS = Instantiate(Resources.Load("Game/3DRunnerGameScene")) as GameObject;
                //rename it
                ASS.name = "3DRunnerGameScene";
                ASS.transform.position = new Vector3(0f, 0f, 0f);

                Debug.Log("Infinity Runner Game Prefabs Created!");
            }
            GUILayout.EndVertical();

            GUILayout.Space(10f);

            GUILayout.EndVertical();

        }
        GUILayout.EndVertical();

        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
        GUILayout.Label("*Remember to keep the Template Updated for better performance.");
        GUILayout.Space(10f);
        GUILayout.Label("*Your Rating is important go to Unity Asset Store and leave your comment.");
        GUILayout.Space(10f);
        if (GUILayout.Button("Visit Infinite Runner Engine 3D in Unity Asset Store"))
        {
            Application.OpenURL("https://assetstore.unity.com/packages/templates/systems/infinite-runner-engine-3d-60326");
        }

        GUILayout.EndVertical();
    }

}