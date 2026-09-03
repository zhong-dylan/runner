using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using static D3SoundManager;
using Unity.VisualScripting;
using static D3ADSRandomReward;
using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager;
using System.ComponentModel;




#if UNITY_ADS && ENABLE_UNITY_ADS
using UnityEngine.Advertisements;
#endif


[CustomEditor(typeof(D3RunnerGame))]
[ExecuteInEditMode]
public class D3GameEditor : Editor
{

    Vector2 scrollPos, scrollPos2, scrollPos3, scrollPos4, scrollPos5, scrollPos6, scrollPos7, scrollPos8, scrollPos9, scrollPos10, AdsScrollPos;
    SerializedObject GameGuiSe, GameSS, GamePSS,GameCS, SerializedADSManager;

    private string[] m_tabs = { "Game Manager", "Curved World", "Main Camera", "Audio Manager", "Level System", "Obstacles", "Enemy Follower", "Buildings", "Collectables", "Pattern System",  "UI Manager", "ADS Manager", "Instantiate Object InGame", "Input And Touch Controller", "Contact" };
    private int m_tabsSelected = -1;
    D3RunnerGame GameM;
    D3GameController Gamec;
    D3WorldCurver GameW;
    D3GameAttribute GameA;
    D3CameraFollow GameCamera;
    D3SetFPS GameFPS;
    D3GUIManager GameGui;
    D3SoundManager GameS;
    D3PatternSystem GamePS;
    D3ADSManager AdsManager;
    D3RewardWindow RewardWindow;
    D3PanelRewardADS PanelReward;
    int selected = 0;

   
    int CurvatureID;
    int DistanceID;

    string[] options = new string[]
    {
      "None","InGame Window", "Pause Window", "GameOver Window", "WinGame Window", "Settings Window", "Best Score Window", "Hero Window","Reward Window"
    };

    void OnEnable()
    {
        GameM = target as D3RunnerGame;

        if (GameM)
        {
            if (FindObjectOfType<D3GameController>())
            {
                Gamec = FindObjectOfType<D3GameController>();
                GameCS = new SerializedObject(Gamec);
         
            } else
            {
                Debug.LogWarning("<b>ERROR</b> D3GameController Script not found in the scene, it is recommended to delete the scene and create a new one or review the demo scene and compare the objects.</b>");
            }

            if (FindObjectOfType<D3GameAttribute>())
            {
                GameA = FindObjectOfType<D3GameAttribute>();
            }
            else
            {
                Debug.LogWarning("<b>ERROR</b> D3GameAttribute Script not found in the scene, it is recommended to delete the scene and create a new one or review the demo scene and compare the objects.</b>");
            }

            if (FindObjectOfType<D3CameraFollow>())
            {
                GameCamera = FindObjectOfType<D3CameraFollow>();
            }
            else
            {
                Debug.LogWarning("<b>ERROR</b> D3CameraFollow Script not found in the scene, it is recommended to delete the scene and create a new one or review the demo scene and compare the objects.</b>");
            }

            if (FindObjectOfType<D3SetFPS>())
            {
                GameFPS = FindObjectOfType<D3SetFPS>();
            }
            else
            {
                Debug.LogWarning("<b>ERROR</b> D3SetFPS Script not found in the scene, it is recommended to delete the scene and create a new one or review the demo scene and compare the objects.</b>");
            }

            if (FindObjectOfType<D3GUIManager>())
            {
                GameGui = FindObjectOfType<D3GUIManager>();
                GameGuiSe = new SerializedObject(GameGui);
                if (GameGui.RewardWindow)
                {
                    if (GameGui.RewardWindow.GetComponent<D3RewardWindow>())
                    {
                        RewardWindow = GameGui.RewardWindow.GetComponent<D3RewardWindow>();
                    }
                    else
                    {
                        Debug.LogWarning("<b>ERROR</b> D3RewardWindow Script not found in the scene, it is recommended to delete the scene and create a new one or review the demo scene and compare the objects.</b>");

                    }
                }
                if (GameGui.PanelRandomReward)
                {
                    if (GameGui.PanelRandomReward.GetComponent<D3PanelRewardADS>())
                    {
                        PanelReward = GameGui.PanelRandomReward.GetComponent<D3PanelRewardADS>();
                    }
                    else
                    {
                        Debug.LogWarning("<b>ERROR</b> D3PanelRewardADS Script not found in the scene, it is recommended to delete the scene and create a new one or review the demo scene and compare the objects.</b>");

                    }
                }
            }
            else
            {
                Debug.LogWarning("<b>ERROR</b> D3GUIManager Script not found in the scene, it is recommended to delete the scene and create a new one or review the demo scene and compare the objects.</b>");
            }


            if (FindObjectOfType<D3SoundManager>())
            {
                GameS = FindObjectOfType<D3SoundManager>();
                GameSS = new SerializedObject(GameS);
            }
            else
            {
                Debug.LogWarning("<b>ERROR</b> D3SoundManager Script not found in the scene, it is recommended to delete the scene and create a new one or review the demo scene and compare the objects.</b>");
            }

            if (FindObjectOfType<D3PatternSystem>())
            {
                GamePS = FindObjectOfType<D3PatternSystem>();
                GamePSS = new SerializedObject(GamePS);
            }
            else
            {
                Debug.LogWarning("<b>ERROR</b> D3PatternSystem Script not found in the scene, it is recommended to delete the scene and create a new one or review the demo scene and compare the objects.</b>");
            }


            if (FindObjectOfType<D3WorldCurver>())
            {
                GameW = FindObjectOfType<D3WorldCurver>();
                CurvatureID = Shader.PropertyToID("_Curvature");
                DistanceID = Shader.PropertyToID("_Distance");
            }
            else
            {
                Debug.LogWarning("<b>ERROR</b> D3PatternSystem Script not found in the scene, it is recommended to delete the scene and create a new one or review the demo scene and compare the objects.</b>");
            }

            if (FindAnyObjectByType<D3ADSManager>())
            {
                AdsManager = FindAnyObjectByType<D3ADSManager>();
                SerializedADSManager = new SerializedObject(AdsManager);
            }
            else
            {
                Debug.LogWarning("<b>ERROR</b> ADS manager is not installed in this scene, click on the ADS manager tab of 3DRunnerEditor for more information..</b>");
            }


        }
    }
    public override void OnInspectorGUI()
    {
        var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true), GUILayout.Height(170f));
        Texture2D m_Logo = (Texture2D)Resources.Load("Img/D3Icon", typeof(Texture2D));
        GUILayout.Label(m_Logo, style, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        GUILayout.EndVertical();

        if (GameM)
        {
            GUILayout.Space(10f);
            if (GUILayout.Button("Online Manual"))
            {
                Application.OpenURL("https://denvzla-estudio.gitbook.io/infinite-runner-engine-3d/");
            }

            if (!GameGui)
            {
                Color defaultColor = GUI.color;
                GUI.color = Color.red;
                GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                GUI.color = defaultColor;
                if (GUILayout.Button("UI manager not found, Click to add"))
                {
                    //instantiate ui canvas
                    GameObject ASS = PrefabUtility.InstantiatePrefab(Resources.Load("Game/GUI_Manager")) as GameObject;
                    //rename it
                    ASS.name = "UI Manager";
                    ASS.transform.position = new Vector3(0f, 0f, 0f);
                    ASS.transform.SetParent(GameM.gameObject.transform);
                    EditorGUIUtility.PingObject(ASS);
                    Selection.activeGameObject = ASS.gameObject;
                    SceneView.lastActiveSceneView.FrameSelected();

                    Debug.Log("UI Manager Prefabs Created!");
                }
                GUILayout.EndVertical();
            }

            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            
            GUILayout.Label("Game Scene Editor", style);
            GUILayout.EndVertical();
            GUILayout.Space(10f);
            EditorGUILayout.TextArea("To play without error, do it from the main scene, the assets must be loaded first.", GUI.skin.GetStyle("HelpBox"));

            RunnerGameEditor();
        }
    }
    void RunnerGameEditor()
    {
        EditorGUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
        m_tabsSelected = GUILayout.SelectionGrid(m_tabsSelected, m_tabs, 2);
        EditorGUILayout.EndVertical();
        if (m_tabsSelected >= 0 && m_tabsSelected < m_tabs.Length)
        {
            switch (m_tabs[m_tabsSelected])
            {
                case "Game Manager":
                    GameManager();
                    break;
                case "Curved World":
                    CurvedWorld();
                    break;
                case "Obstacles":
                    Obstacles();
                    break;
                case "Main Camera":
                    MainCamera();
                    break;
                case "Buildings":
                    Building();
                    break;
                case "Audio Manager":
                    SoundManager();
                    break;
                case "Pattern System":
                    if (Application.isPlaying == false)
                    {
                        PatternSystem();
                    }
                    else
                    {
                        GUILayout.Label("Not available in Play Mode", EditorStyles.boldLabel);
                    }
                    break;
                case "Collectables":
                    Collectables();
                    break;
                case "Enemy Follower":
                    EnemyEditor();
                    break;
                case "UI Manager":
                    if (Application.isPlaying == false)
                    {
                        UIManager();
                    }
                    else {
                        GUILayout.Label("Not available in Play Mode", EditorStyles.boldLabel);
                    }
                    break;
                case "Level System":
                    if (Application.isPlaying == false)
                    {
                        LevelSystem();
                    }
                    else
                    {
                        GUILayout.Label("Not available in Play Mode", EditorStyles.boldLabel);
                    }
                    break;
                case "ADS Manager":
                    ADSManager();
                    break;
                case "Instantiate Object InGame":
                    InstantiateObjectInGame();
                    break;
                case "Input And Touch Controller":
                    InputManager();
                    break;
                case "Contact":
                    Contact();
                    break;
            }

        }
    }
    void Contact()
    {
        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));

        EditorGUILayout.TextArea("Thanks For Choosing 3D Infinite Runner Engine, Currently Still In Development We do not stop until we create a quality product, if you have problems write to us at denvzla@gmail.com", GUI.skin.GetStyle("HelpBox"));

        EditorGUILayout.TextArea("Do you want a Redesign?  We can do it - Includes Reeskim. - Code change to make it unique - Port to Android, IOS, PC, etc. denvzla@gmail.com or https://denvzlaestudio.com", GUI.skin.GetStyle("HelpBox"));

        var style0 = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };

        Texture2D m_Logo0 = (Texture2D)Resources.Load("Img/Promo2", typeof(Texture2D));
        GUILayout.Label(m_Logo0, style0, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        GUILayout.EndVertical();
    }
    void GameManager()
    {
        EditorGUI.BeginChangeCheck();
        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
        GUILayout.Label("Game Manager Editor", EditorStyles.boldLabel);
        if (GameM)
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.BeginVertical("GroupBox");
            GUILayout.Space(5f);

            GUILayout.Label("System Components (Do not delete)", EditorStyles.boldLabel);

            GUILayout.Space(10f);

            EditorGUILayout.TextArea("Add the PatternSystem", GUI.skin.GetStyle("HelpBox"));
            Gamec.patSysm = EditorGUILayout.ObjectField("PatternSystem: ", Gamec.patSysm, typeof(D3PatternSystem), true) as D3PatternSystem;
            GUILayout.Space(10f);

            EditorGUILayout.TextArea("Add the Camera Follow", GUI.skin.GetStyle("HelpBox"));
            Gamec.cameraFol = EditorGUILayout.ObjectField("CameraFollow: ", Gamec.cameraFol, typeof(D3CameraFollow), true) as D3CameraFollow;
            GUILayout.Space(10f);

            GUILayout.Space(10f);

            GUILayout.EndVertical();


            GUILayout.BeginVertical("GroupBox");
            GUILayout.Space(5f);

            GUILayout.Label("Config", EditorStyles.boldLabel);

            GUILayout.Space(10f);

            GameA.SceneTitleToload = EditorGUILayout.TextField("Name Scene Title to Load: ", GameA.SceneTitleToload);
            GUILayout.Space(10f);

            GameA.starterSpeed = EditorGUILayout.FloatField("Starter Speed: ", GameA.starterSpeed);
            GUILayout.Space(10f);

            EditorGUILayout.TextArea("Add the Speed Every Distance", GUI.skin.GetStyle("HelpBox"));
            Gamec.speedAddEveryDistance = EditorGUILayout.FloatField("Speed Add Every Distance: ", Gamec.speedAddEveryDistance);
            GUILayout.Space(10f);

            EditorGUILayout.TextArea("Speed to add ", GUI.skin.GetStyle("HelpBox"));
            Gamec.speedAdd = EditorGUILayout.FloatField("Speed Add: ", Gamec.speedAdd);
            GUILayout.Space(10f);

            EditorGUILayout.TextArea("Speed Max to add (18) ", GUI.skin.GetStyle("HelpBox"));
            Gamec.speedMax = EditorGUILayout.FloatField("Speed Max: ", Gamec.speedMax);
            if (Gamec.speedMax > 18)
            {
                Gamec.speedMax = 18;
            }
            
           
            GUILayout.Space(10f);

            EditorGUILayout.TextArea("Add the Pos Start Player, (Minimum position in Z > 32 position of the first block)", GUI.skin.GetStyle("HelpBox"));
            Gamec.posStart = EditorGUILayout.Vector3Field("Pos Start: ", Gamec.posStart);
            if (Gamec.posStart.z < 32)
            { 
            Gamec.posStart.z = 32;
            }

            GUILayout.Space(10f);


            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));

            GameCS.Update();
            GUILayout.Label("Player Prefabs: ", EditorStyles.boldLabel);
            EditorGUILayout.TextArea("Add the Player Prefabs (D3Controller)", GUI.skin.GetStyle("HelpBox"));
            GUILayout.Space(10f);
            Rect myRect = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
            GUI.Box(myRect, "Drag and drop Character Prefabs \n into this box!");

            if (myRect.Contains(Event.current.mousePosition))
            {
                if (Event.current.type == EventType.DragUpdated)
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    Event.current.Use();
                }
                else if (Event.current.type == EventType.DragPerform)
                {
                    for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
                    {

                        D3Controller AddGO = DragAndDrop.objectReferences[i].GetComponent<D3Controller>();

                        if (AddGO)
                        {
                            Gamec.playerPref.Add(AddGO);
                            GameCS.ApplyModifiedProperties();
                            if (!Application.isPlaying)
                            {
                                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                            }
                        }
                        else
                        {
                            Debug.Log("Invalid Character Prefabs");
                        }

                    }
                    Event.current.Use();
                }
            }

            if (Gamec.playerPref.Count > 0)
            {
                GUILayout.Space(10);
                if (GUILayout.Button("Remove Last Player Prefabs"))
                {
                    Gamec.playerPref.RemoveAt(Gamec.playerPref.Count - 1);
                    EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                }

                for (int i = 0; i < Gamec.playerPref.Count; i++)
                {
                    if (Gamec.playerPref[i] != null)
                    {
                        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                        GUILayout.Space(10f);
                        GUILayout.Label("ID For Shop Manager:  " + i, EditorStyles.boldLabel);
                        EditorGUILayout.TextArea("This is the ID Code that you must use in the shop administrator tab in the main scene.", GUI.skin.GetStyle("HelpBox"));
                        GUILayout.Space(10f);

                        Gamec.playerPref[i] = EditorGUILayout.ObjectField("Player Prefabs: ", Gamec.playerPref[i], typeof(D3Controller), true) as D3Controller;

                        GUILayout.EndVertical();
                    }
                }
            }
            GUILayout.Space(10f);
            GUILayout.EndVertical();
            GameCS.ApplyModifiedProperties();

            GUILayout.EndVertical();

            EditorGUILayout.EndScrollView();

        }
        GUILayout.Space(10f);
        GUILayout.EndVertical();
        if (EditorGUI.EndChangeCheck())
        {
            if (!Application.isPlaying)
            {
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }
    }
    void CurvedWorld()
    {
        EditorGUI.BeginChangeCheck();
        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
        if (GameW)
        {
            GUILayout.BeginVertical("GroupBox");
            GUILayout.Space(5f);
            GUILayout.Label("Curved World", EditorStyles.boldLabel);
            GUILayout.Space(10f);

            GameW.Curvature = EditorGUILayout.Vector3Field("Curvature: ", GameW.Curvature);
           
            GUILayout.Space(10f);
            GameW.Distance = EditorGUILayout.FloatField("Distance: ", GameW.Distance);

            GUILayout.EndVertical();

            GUILayout.BeginVertical("GroupBox");

            EditorGUILayout.TextArea("Attention Editing the Asset with the curve is difficult, disable it and enable it when necessary", GUI.skin.GetStyle("HelpBox"));

            GUILayout.Space(10f);
            if (GUILayout.Button("See Curve Applied to this Scene"))
            {

                Vector3 curvature = GameW.CurvatureScaleUnit == 0 ? GameW.Curvature : GameW.Curvature / GameW.CurvatureScaleUnit;

                Shader.SetGlobalVector(CurvatureID, curvature);
                Shader.SetGlobalFloat(DistanceID, GameW.Distance);
            }
            GUILayout.Space(10f);
            if (GUILayout.Button("Disbled Curve"))
            {

                Vector3 curvature2 = GameW.CurvatureScaleUnit == 0 ? new Vector3(0f, 0f, 0f) : new Vector3(0f, 0f, 0f) / GameW.CurvatureScaleUnit;

                Shader.SetGlobalVector(CurvatureID, curvature2);
                Shader.SetGlobalFloat(DistanceID, 0);

            }

            GUILayout.EndVertical();
        }
        GUILayout.Space(10f);
        GUILayout.EndVertical();
        if (EditorGUI.EndChangeCheck())
        {
            if (!Application.isPlaying)
            {
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }

    }
    void MainCamera()
    {
        EditorGUI.BeginChangeCheck();
        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
        if (GameM)
        {
            scrollPos2 = EditorGUILayout.BeginScrollView(scrollPos2, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.BeginVertical("GroupBox");
            GUILayout.Space(5f);

            GUILayout.Label("Main Camera", EditorStyles.boldLabel);
            GUILayout.Space(10f);


            EditorGUILayout.TextArea("Attention: A bad position of the camera can cause the floor to be removed and the character to fall into the void.", GUI.skin.GetStyle("HelpBox"));

            GUILayout.Space(10f);

            GameCamera.angle = EditorGUILayout.FloatField("Angle: ", GameCamera.angle);
            GUILayout.Space(10f);

            GameCamera.distance = EditorGUILayout.FloatField("Distance: ", GameCamera.distance);
            GUILayout.Space(10f);

            GameCamera.height = EditorGUILayout.FloatField("Height: ", GameCamera.height);
            GUILayout.Space(10f);

            GameFPS.fpsTarget = EditorGUILayout.IntField("Set FPS: ", GameFPS.fpsTarget);
            
            if (GameGui)
            {
                GUILayout.Space(10f);
                GameGui.PanelFade = EditorGUILayout.ObjectField("Panel Fade: ", GameGui.PanelFade, typeof(GameObject), true) as GameObject;
            }
            GUILayout.Space(10f);

            if (Gamec)
            {
                Gamec.SpawnPlayerOnDead = GUILayout.Toggle(Gamec.SpawnPlayerOnDead, "Spawn Player In UI");

                if (Gamec.SpawnPlayerOnDead)
                {
                    GUILayout.Space(10f);
                    Gamec.PlayerView = EditorGUILayout.ObjectField("Panel Player View: ", Gamec.PlayerView, typeof(GameObject), true) as GameObject;
                }
            }

            GUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }
        GUILayout.Space(10f);
        GUILayout.EndVertical();
        if (EditorGUI.EndChangeCheck())
        {
            if (!Application.isPlaying)
            {
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }
    }
    void WindowsValidation()
    {
        ///1
        ///
        if (GameGui.PauseGui)
        {
            GameGui.PauseGui.SetActive(false);
        }
        else {
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("ERROR: window not found, please assign:", EditorStyles.boldLabel);
            GameGui.PauseGui = EditorGUILayout.ObjectField("Pause Window: ", GameGui.PauseGui, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GUILayout.EndVertical();
        }
        ////2
        ///
        if (GameGui.GameOverGui)
        {
            GameGui.GameOverGui.SetActive(false);
        }
        else
        {
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("ERROR: window not found, please assign:", EditorStyles.boldLabel);
            GameGui.GameOverGui = EditorGUILayout.ObjectField("GameOver Window: ", GameGui.GameOverGui, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GUILayout.EndVertical();
        }
        ///3
        ///
        if (GameGui.InGameGui)
        {
            GameGui.InGameGui.SetActive(false);
        }
        else
        {
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("ERROR: window not found, please assign:", EditorStyles.boldLabel);
            GameGui.InGameGui = EditorGUILayout.ObjectField("InGameGui Window: ", GameGui.InGameGui, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GUILayout.EndVertical();
        }
        ////4
        ///
        if (GameGui.WinGameGui)
        {
            GameGui.WinGameGui.SetActive(false);
        }
        else
        {
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("ERROR: window not found, please assign:", EditorStyles.boldLabel);
            GameGui.WinGameGui = EditorGUILayout.ObjectField("WinGameGui: ", GameGui.WinGameGui, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GUILayout.EndVertical();
        }
        ///5
        ///
        if (GameGui.CoinTObj)
        {
            GameGui.CoinTObj.SetActive(false);
        }
        else
        {
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("ERROR: window not found, please assign:", EditorStyles.boldLabel);
            GameGui.CoinTObj = EditorGUILayout.ObjectField("Coin Obj: ", GameGui.CoinTObj, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GUILayout.EndVertical();
        }
        ////6
        ///
        if (GameGui.LifeTObj)
        {
            GameGui.LifeTObj.SetActive(false);
        }
        else
        {
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("ERROR: window not found, please assign:", EditorStyles.boldLabel);
            GameGui.LifeTObj = EditorGUILayout.ObjectField("Life Obj: ", GameGui.LifeTObj, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GUILayout.EndVertical();
        }
        ///7
        ///
        if (GameGui.SettingsGUI)
        {
            GameGui.SettingsGUI.SetActive(false);
        }
        else
        {
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("ERROR: window not found, please assign:", EditorStyles.boldLabel);
            GameGui.SettingsGUI = EditorGUILayout.ObjectField("Settings Window: ", GameGui.SettingsGUI, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GUILayout.EndVertical();
        }
        ///8
        ///
        if (GameGui.BestScoreGUI)
        {
            GameGui.BestScoreGUI.SetActive(false);
        }
        else
        {
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("ERROR: window not found, please assign:", EditorStyles.boldLabel);
            GameGui.BestScoreGUI = EditorGUILayout.ObjectField("BestScore Window: ", GameGui.BestScoreGUI, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GUILayout.EndVertical();
        }
        ///9
        ///
        if (GameGui.HeroGui)
        {
            GameGui.HeroGui.SetActive(false);
        }
        else
        {
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("ERROR: window not found, please assign:", EditorStyles.boldLabel);
            GameGui.HeroGui = EditorGUILayout.ObjectField("Hero Window: ", GameGui.HeroGui, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GUILayout.EndVertical();
        }
        ///10
        ///
        if (GameGui.RewardWindow)
        {
            GameGui.RewardWindow.SetActive(false);
            
        }
        else
        {
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("ERROR: window not found, please assign:", EditorStyles.boldLabel);
            GameGui.RewardWindow = EditorGUILayout.ObjectField("Reward Window: ", GameGui.RewardWindow, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GUILayout.EndVertical();
        }
        ///11
        ///
        if (GameGui.PanelRandomReward)
        {
            GameGui.PanelRandomReward.SetActive(false);

        }
        else
        {
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("ERROR: Panel Random Reward ADS not found, please assign:", EditorStyles.boldLabel);
            GameGui.PanelRandomReward = EditorGUILayout.ObjectField("Panel Random Reward: ", GameGui.PanelRandomReward, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GUILayout.EndVertical();
        }
    }
    void UIManager()
    {
        if (GameGui && !Application.isPlaying)
        {
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("Attention: ", EditorStyles.boldLabel);
            EditorGUILayout.TextArea("It is recommended to delete the old GUI Manager and create it from this editor. When you delete it, the option to create it will appear. If you already have it, skip this message.", GUI.skin.GetStyle("HelpBox"));
            EditorGUILayout.TextArea("Do not break the connection of the new UI manager, the idea is to have the same one in all the scenes and apply the changes in only one.", GUI.skin.GetStyle("HelpBox"));

            GUILayout.Space(10f);
            GUILayout.EndVertical();

            EditorGUI.BeginChangeCheck();

            GUILayout.BeginVertical("GroupBox");
            GUILayout.Space(5f);


            GUILayout.BeginVertical("GroupBox");
            GUILayout.Space(5f);
            GUILayout.Label("UI Manager", EditorStyles.boldLabel);
            GUILayout.Space(10f);

            if (Gamec)
            {
                Gamec.SpawnPlayerOnDead = GUILayout.Toggle(Gamec.SpawnPlayerOnDead, "Spawn Player In UI");

                if (Gamec.SpawnPlayerOnDead)
                {
                    GUILayout.Space(10f);
                    Gamec.PlayerView = EditorGUILayout.ObjectField("Panel Player View: ", Gamec.PlayerView, typeof(GameObject), true) as GameObject;
                }
            }
            GUILayout.Space(10f);

            selected = EditorGUILayout.Popup("Select Window to Edit: ", selected, options);

            GUILayout.Space(10f);
            GUILayout.EndVertical();

            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            if (GameM)
            {
                scrollPos3 = EditorGUILayout.BeginScrollView(scrollPos3, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

                switch (selected)
                {
                    case 0:
                        WindowsValidation();
                        break;
                    case 1:
                        WindowsValidation();
                        if (GameGui.InGameGui && GameGui.CoinTObj && GameGui.LifeTObj && GameGui.PanelRandomReward)
                        {
                            GameGui.InGameGui.SetActive(true);
                            GameGui.CoinTObj.SetActive(true);
                            GameGui.LifeTObj.SetActive(true);
                            GameGui.PanelRandomReward.SetActive(true);

                            GUILayout.Label("General Configuration", EditorStyles.boldLabel);

                            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                            GUILayout.Space(5f);
                            GUILayout.Label("Gui Text", EditorStyles.boldLabel);
                            GUILayout.Space(10f);
                            GameGui.TextScore = EditorGUILayout.TextField("Text Score: ", GameGui.TextScore);

                            GUILayout.Space(10f);

                            GameGui.TextToWinInfo = EditorGUILayout.TextField("Informational Text from Victory: ", GameGui.TextToWinInfo);

                            GUILayout.Space(10f);
                            GameGui.TextBestScore = EditorGUILayout.TextField("Text to Best Distance: ", GameGui.TextBestScore);
                            GUILayout.Space(10f);

                            GUILayout.Space(20f);

                            GUILayout.Label("Start ", EditorStyles.boldLabel);
                            GUILayout.Space(10f);
                            GameGui.StartOFF1 = EditorGUILayout.ObjectField("Start OFF 1: ", GameGui.StartOFF1, typeof(GameObject), true) as GameObject;
                            GUILayout.Space(10f);
                            GameGui.StartON1 = EditorGUILayout.ObjectField("Start ON 1: ", GameGui.StartON1, typeof(GameObject), true) as GameObject;
                            GUILayout.Space(20f);


                            GameGui.StartOFF2 = EditorGUILayout.ObjectField("Start OFF 2: ", GameGui.StartOFF2, typeof(GameObject), true) as GameObject;
                            GUILayout.Space(10f);
                            GameGui.StartON2 = EditorGUILayout.ObjectField("Start ON 2: ", GameGui.StartON2, typeof(GameObject), true) as GameObject;
                            GUILayout.Space(20f);


                            GameGui.StartOFF3 = EditorGUILayout.ObjectField("Start OFF 3: ", GameGui.StartOFF3, typeof(GameObject), true) as GameObject;
                            GUILayout.Space(10f);
                            GameGui.StartON3 = EditorGUILayout.ObjectField("Start ON 3: ", GameGui.StartON3, typeof(GameObject), true) as GameObject;

                            GUILayout.EndVertical();


                            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                            GUILayout.Label("InGame Gui", EditorStyles.boldLabel);
                            GUILayout.Space(10f);

                            GameGui.TextCoinCollect = EditorGUILayout.ObjectField("Text Coin Collect: ", GameGui.TextCoinCollect, typeof(Text), true) as Text;
                            GUILayout.Space(10f);

                            GUILayout.Label("Item State Set", EditorStyles.boldLabel);
                            GUILayout.Space(10f);

                            SerializedProperty stringsProperty3 = GameGuiSe.FindProperty("itemStateSet");
                            EditorGUILayout.PropertyField(stringsProperty3, true);
                            GUILayout.Space(10f);

                            GameGui.CoinTObj = EditorGUILayout.ObjectField("Current Coin: ", GameGui.CoinTObj, typeof(GameObject), true) as GameObject;

                            GUILayout.Space(10f);

                            GameGui.LifeTObj = EditorGUILayout.ObjectField("Current Life: ", GameGui.LifeTObj, typeof(GameObject), true) as GameObject;

                            GUILayout.Space(10f);

                            GameGui.BGLoading = EditorGUILayout.ObjectField("Background loading: ", GameGui.BGLoading, typeof(GameObject), true) as GameObject;

                            GUILayout.Space(10f);

                            GUILayout.Label("Progress Bar", EditorStyles.boldLabel);

                            GUILayout.Space(10f);

                            Gamec.useShowPercent = GUILayout.Toggle(Gamec.useShowPercent, "Use Show Percent");
                            GUILayout.Space(10f);

                            GameGui.LoadingBar = EditorGUILayout.ObjectField("Texture Progress: ", GameGui.LoadingBar, typeof(Slider), true) as Slider;
                            GUILayout.Space(10f);

                            GameGui.LoadingBarText = EditorGUILayout.ObjectField("Text Progress: ", GameGui.LoadingBarText, typeof(Text), true) as Text;
                            GUILayout.Space(10f);

                            GUILayout.EndVertical();
                        }
                        else
                        {
                            OnEnable();
                        }
                        break;

                    case 2:
                        WindowsValidation();
                        if (GameGui.PauseGui)
                        {
                            GameGui.PauseGui.SetActive(true);
                            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                            GUILayout.Label("Pause Gui", EditorStyles.boldLabel);
                            GUILayout.Space(10f);

                            GameGui.PauseScore = EditorGUILayout.ObjectField("Text Pause Score: ", GameGui.PauseScore, typeof(Text), true) as Text;
                            GUILayout.Space(10f);
                            GameGui.PauseToWinScore = EditorGUILayout.ObjectField("Text Pause To Win: ", GameGui.PauseToWinScore, typeof(Text), true) as Text;
                            GUILayout.Space(10f);
                            GameGui.PauseBestScore = EditorGUILayout.ObjectField("Text Pause Best Score: ", GameGui.PauseBestScore, typeof(Text), true) as Text;

                            GUILayout.Space(10f);
                            GUILayout.EndVertical();
                        }
                        else
                        {
                            OnEnable();
                        }
                        break;

                    case 3:
                        WindowsValidation();
                        if (GameGui.GameOverGui)
                        {
                            GameGui.GameOverGui.SetActive(true);
                            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                            GUILayout.Label("Game Over Gui", EditorStyles.boldLabel);


                            GUILayout.Space(10f);

                            GameA.SceneTitleToload = EditorGUILayout.TextField("Name Scene Title to Load: ", GameA.SceneTitleToload);
                            GUILayout.Space(10f);

                            GUILayout.Space(10f);

                            GameA.TimeToShowTheWindowDeath = EditorGUILayout.IntField("Second To Show Window Death: ", GameA.TimeToShowTheWindowDeath);

                            GUILayout.Space(10f);
                            GameGui.GameOverScore = EditorGUILayout.ObjectField("Text Score: ", GameGui.GameOverScore, typeof(Text), true) as Text;
                            GUILayout.Space(10f);
                            GameGui.GameToWinScore = EditorGUILayout.ObjectField("Text To Win Score: ", GameGui.GameToWinScore, typeof(Text), true) as Text;
                            GUILayout.Space(10f);
                            GameGui.GameOverBestScore = EditorGUILayout.ObjectField("Text Best Score: ", GameGui.GameOverBestScore, typeof(Text), true) as Text;
                            GUILayout.Space(10f);

                            GUILayout.EndVertical();
                        }
                        else
                        {
                            OnEnable();
                        }

                        break;

                    case 4:
                        WindowsValidation();
                        if (GameGui.WinGameGui)
                        {
                            GameGui.WinGameGui.SetActive(true);
                            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                            GUILayout.Label("Game Win Gui", EditorStyles.boldLabel);
                            GUILayout.Space(5f);

                            GUILayout.Space(10f);
                            GameGui.WinScore = EditorGUILayout.ObjectField("Text Score: ", GameGui.WinScore, typeof(Text), true) as Text;
                            GUILayout.Space(10f);
                            GameGui.WinDistance = EditorGUILayout.ObjectField("Text Distance: ", GameGui.WinDistance, typeof(Text), true) as Text;
                            GUILayout.Space(10f);
                            GameGui.WinTextBestScore = EditorGUILayout.ObjectField("Text Best Score: ", GameGui.WinTextBestScore, typeof(Text), true) as Text;
                            GUILayout.Space(10f);

                            GameGui.StartWinON1 = EditorGUILayout.ObjectField("Start Win ON 1: ", GameGui.StartWinON1, typeof(GameObject), true) as GameObject;
                            GUILayout.Space(10f);
                            GameGui.StartWinOFF1 = EditorGUILayout.ObjectField("Start Win OFF 1: ", GameGui.StartWinOFF1, typeof(GameObject), true) as GameObject;
                            GUILayout.Space(10f);

                            GameGui.StartWinON2 = EditorGUILayout.ObjectField("Start Win ON 2: ", GameGui.StartWinON2, typeof(GameObject), true) as GameObject;
                            GUILayout.Space(10f);
                            GameGui.StartWinOFF2 = EditorGUILayout.ObjectField("Start Win OFF 2: ", GameGui.StartWinOFF2, typeof(GameObject), true) as GameObject;
                            GUILayout.Space(10f);

                            GameGui.StartWinON3 = EditorGUILayout.ObjectField("Start Win ON 3: ", GameGui.StartWinON3, typeof(GameObject), true) as GameObject;
                            GUILayout.Space(10f);
                            GameGui.StartWinOFF3 = EditorGUILayout.ObjectField("Start Win OFF 3: ", GameGui.StartWinOFF3, typeof(GameObject), true) as GameObject;
                            GUILayout.Space(10f);
                           

                            GUILayout.EndVertical();
                        }
                        else
                        {
                            OnEnable();
                        }

                        break;
                    
                    case 5:
                        WindowsValidation();
                        if (GameGui.SettingsGUI)
                        {
                            GameGui.SettingsGUI.SetActive(true);
                            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                            GUILayout.Label("Settings Gui", EditorStyles.boldLabel);
                            GUILayout.Space(5f);

                            GUILayout.Space(10f);
                            GameS.musicVolumeSlider = EditorGUILayout.ObjectField("Music Volumen Slider: ", GameS.musicVolumeSlider, typeof(Slider), true) as Slider;
                            GUILayout.Space(10f);
                            GameS.SFXVolumeSlider = EditorGUILayout.ObjectField("SFX Volumen Slider: ", GameS.SFXVolumeSlider, typeof(Slider), true) as Slider;

                            GUILayout.Space(10f);
                            GUILayout.EndVertical();
                        }
                        else
                        {
                            OnEnable();
                        }
                        break;
                    
                    case 6:
                        WindowsValidation();
                        if (GameGui.BestScoreGUI)
                        {
                            GameGui.BestScoreGUI.SetActive(true);
                            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                            GUILayout.Label("Best Score Gui", EditorStyles.boldLabel);
                            GUILayout.Space(5f);

                            GUILayout.Space(10f);
                            GameGui.BestScoreGUI = EditorGUILayout.ObjectField("Best Score: ", GameGui.BestScoreGUI, typeof(GameObject), true) as GameObject;
                            GUILayout.Space(10f);
                            GameGui.BestScoreText = EditorGUILayout.ObjectField("(BestScore) Best Score: ", GameGui.BestScoreText, typeof(Text), true) as Text;
                            GUILayout.Space(10f);

                            GUILayout.Space(10f);
                            GUILayout.EndVertical();
                        }
                        else
                        {
                            OnEnable();
                        }
                        break;
                   
                    case 7:
                        WindowsValidation();
                        if (GameGui.HeroGui)
                        {
                            GameGui.HeroGui.SetActive(true);
                            GUILayout.Label("Hero Gui", EditorStyles.boldLabel);
                            GUILayout.Space(5f);

                            var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };

                            GUILayout.Space(10f);
                            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                            GUILayout.Space(10f);
                            GUILayout.Label("Basic", style);
                            GUILayout.Space(10f);
                            GameGui.NamePlayerText = EditorGUILayout.ObjectField("Player Name Text: ", GameGui.NamePlayerText, typeof(Text), true) as Text;
                            GUILayout.Space(10f);
                            GameGui.ImageButtonBuy = EditorGUILayout.ObjectField("Image Button Buy: ", GameGui.ImageButtonBuy, typeof(Sprite), true) as Sprite;
                            GUILayout.Space(10f);
                            GameGui.ImageButtonNoCoin = EditorGUILayout.ObjectField("Image Button No Coin: ", GameGui.ImageButtonNoCoin, typeof(Sprite), true) as Sprite;
                            GUILayout.Space(10f);
                            GameGui.BuyText = EditorGUILayout.TextField("Buy Text: ", GameGui.BuyText);
                            GUILayout.Space(10f);
                            GameGui.NOCoinText = EditorGUILayout.TextField("NO Coin Text: ", GameGui.NOCoinText);
                            GUILayout.Space(10f);
                            GameGui.MultiplyPriceWhenPurchasing = EditorGUILayout.IntField("Cant Multiply Price When Purchasing :", GameGui.MultiplyPriceWhenPurchasing);
                            GUILayout.Space(10f);
                            GUILayout.EndVertical();


                            GUILayout.Space(10f);
                            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                            GUILayout.Space(10f);
                            GUILayout.Label("Upgrades", style);

                            GUILayout.Space(10f);
                            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                            GUILayout.Space(10f);
                            GUILayout.Label("Sprint", style);
                            GUILayout.Space(10f);
                            GameGui.PriceTextSprintTime = EditorGUILayout.ObjectField("Object Text Price: ", GameGui.PriceTextSprintTime, typeof(Text), true) as Text;
                            GUILayout.Space(10f);
                            GameGui.ScrollbarAddTimeSprintTime = EditorGUILayout.ObjectField("Object Slider: ", GameGui.ScrollbarAddTimeSprintTime, typeof(Slider), true) as Slider;
                            GUILayout.Space(10f);
                            GameGui.TextAddTimeSprintTime = EditorGUILayout.ObjectField("Object Text Add Time: ", GameGui.TextAddTimeSprintTime, typeof(TextMeshProUGUI), true) as TextMeshProUGUI;
                            GUILayout.Space(10f);
                            GameGui.ButtonSprintTime = EditorGUILayout.ObjectField("Object Button: ", GameGui.ButtonSprintTime, typeof(Button), true) as Button;
                            GUILayout.Space(10f);
                            GameGui.ButtonTextSprintTime = EditorGUILayout.ObjectField("Object Text Button: ", GameGui.ButtonTextSprintTime, typeof(Text), true) as Text;
                            GUILayout.Space(10f);
                            GUILayout.EndVertical();

                            GUILayout.Space(10f);
                            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                            GUILayout.Space(10f);
                            GUILayout.Label("Special", style);
                            GUILayout.Space(10f);
                            GameGui.PriceTextSpecialTime = EditorGUILayout.ObjectField("Object Text Price: ", GameGui.PriceTextSpecialTime, typeof(Text), true) as Text;
                            GUILayout.Space(10f);
                            GameGui.ScrollbarAddTimeSpecialTime = EditorGUILayout.ObjectField("Object Slider: ", GameGui.ScrollbarAddTimeSpecialTime, typeof(Slider), true) as Slider;
                            GUILayout.Space(10f);
                            GameGui.TextAddTimeSpecialTime = EditorGUILayout.ObjectField("Object Text Add Time: ", GameGui.TextAddTimeSpecialTime, typeof(TextMeshProUGUI), true) as TextMeshProUGUI;
                            GUILayout.Space(10f);
                            GameGui.ButtonSpecialTime = EditorGUILayout.ObjectField("Object Button: ", GameGui.ButtonSpecialTime, typeof(Button), true) as Button;
                            GUILayout.Space(10f);
                            GameGui.ButtonTextSpecialTime = EditorGUILayout.ObjectField("Object Text Button: ", GameGui.ButtonTextSpecialTime, typeof(Text), true) as Text;
                            GUILayout.Space(10f);
                            GUILayout.EndVertical();

                            GUILayout.Space(10f);
                            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                            GUILayout.Space(10f);
                            GUILayout.Label("Multiply", style);
                            GUILayout.Space(10f);
                            GameGui.PriceTextMultiplyTime = EditorGUILayout.ObjectField("Object Text Price: ", GameGui.PriceTextMultiplyTime, typeof(Text), true) as Text;
                            GUILayout.Space(10f);
                            GameGui.ScrollbarAddTimeMultiplyTime = EditorGUILayout.ObjectField("Object Slider: ", GameGui.ScrollbarAddTimeMultiplyTime, typeof(Slider), true) as Slider;
                            GUILayout.Space(10f);
                            GameGui.TextAddTimeMultiplyTime = EditorGUILayout.ObjectField("Object Text Add Time: ", GameGui.TextAddTimeMultiplyTime, typeof(TextMeshProUGUI), true) as TextMeshProUGUI;
                            GUILayout.Space(10f);
                            GameGui.ButtonMultiplyTime = EditorGUILayout.ObjectField("Object Button: ", GameGui.ButtonMultiplyTime, typeof(Button), true) as Button;
                            GUILayout.Space(10f);
                            GameGui.ButtonTextMultiplyTime = EditorGUILayout.ObjectField("Object Text Button: ", GameGui.ButtonTextMultiplyTime, typeof(Text), true) as Text;
                            GUILayout.Space(10f);
                            GUILayout.EndVertical();

                            GUILayout.Space(10f);
                            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                            GUILayout.Space(10f);
                            GUILayout.Label("Magnet", style);
                            GUILayout.Space(10f);
                            GameGui.PriceTextMagnetTime = EditorGUILayout.ObjectField("Object Text Price: ", GameGui.PriceTextMagnetTime, typeof(Text), true) as Text;
                            GUILayout.Space(10f);
                            GameGui.ScrollbarAddTimeMagnetTime = EditorGUILayout.ObjectField("Object Slider: ", GameGui.ScrollbarAddTimeMagnetTime, typeof(Slider), true) as Slider;
                            GUILayout.Space(10f);
                            GameGui.TextAddTimeMagnetTime = EditorGUILayout.ObjectField("Object Text Add Time: ", GameGui.TextAddTimeMagnetTime, typeof(TextMeshProUGUI), true) as TextMeshProUGUI;
                            GUILayout.Space(10f);
                            GameGui.ButtonMagnetTime = EditorGUILayout.ObjectField("Object Button: ", GameGui.ButtonMagnetTime, typeof(Button), true) as Button;
                            GUILayout.Space(10f);
                            GameGui.ButtonTextMagnetTime = EditorGUILayout.ObjectField("Object Text Button: ", GameGui.ButtonTextMagnetTime, typeof(Text), true) as Text;
                            GUILayout.Space(10f);
                            GUILayout.EndVertical();

                            GUILayout.Space(10f);
                            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                            GUILayout.Space(10f);
                            GUILayout.Label("Shield", style);
                            GUILayout.Space(10f);
                            GameGui.PriceTextShieldTime = EditorGUILayout.ObjectField("Object Text Price: ", GameGui.PriceTextShieldTime, typeof(Text), true) as Text;
                            GUILayout.Space(10f);
                            GameGui.ScrollbarAddTimeShieldTime = EditorGUILayout.ObjectField("Object Slider: ", GameGui.ScrollbarAddTimeShieldTime, typeof(Slider), true) as Slider;
                            GUILayout.Space(10f);
                            GameGui.TextAddTimeShieldTime = EditorGUILayout.ObjectField("Object Text Add Time: ", GameGui.TextAddTimeShieldTime, typeof(TextMeshProUGUI), true) as TextMeshProUGUI;
                            GUILayout.Space(10f);
                            GameGui.ButtonShieldTime = EditorGUILayout.ObjectField("Object Button: ", GameGui.ButtonShieldTime, typeof(Button), true) as Button;
                            GUILayout.Space(10f);
                            GameGui.ButtonTextShieldTime = EditorGUILayout.ObjectField("Object Text Button: ", GameGui.ButtonTextShieldTime, typeof(Text), true) as Text;
                            GUILayout.Space(10f);
                            GUILayout.EndVertical();

                            GUILayout.Space(10f);
                            GUILayout.EndVertical();
                        }
                        else
                        {
                            OnEnable();
                        }
                        break;
                    
                    case 8:
                        WindowsValidation();
                        if (GameGui.RewardWindow)
                        {
                            GameGui.RewardWindow.SetActive(true);
                            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                            GUILayout.Label("Reward Window", EditorStyles.boldLabel);

                            GUILayout.Space(10f);

                            if (RewardWindow)
                            {

                                GUILayout.Space(10f);
                                RewardWindow.TextReward = EditorGUILayout.ObjectField("Reward Text: ", RewardWindow.TextReward, typeof(Text), true) as Text;

                                GUILayout.Space(10f);
                                RewardWindow.ImageReward = EditorGUILayout.ObjectField("Image Reward: ", RewardWindow.ImageReward, typeof(Image), true) as Image;

                            }
                            else
                            {
                                OnEnable();
                            }
                            GUILayout.Space(10f);
                            GUILayout.EndVertical();
                        }
                        break;
                }
                /////

                GameGuiSe.ApplyModifiedProperties();

                EditorGUILayout.EndScrollView();

            }
            GUILayout.Space(10f);
            GUILayout.EndVertical();

            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(5f);
            GUILayout.Label("Menu Button Set", EditorStyles.boldLabel);
            GUILayout.Space(10f);

            SerializedProperty stringsProperty4 = GameGuiSe.FindProperty("menuButtonSet");
            EditorGUILayout.PropertyField(stringsProperty4, true);
            GUILayout.Space(10f);

            GUILayout.Space(10f);
            GUILayout.EndVertical();

            GUILayout.Space(10f);
            GUILayout.EndVertical();

            if (EditorGUI.EndChangeCheck())
            {
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }
        else {
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);

            GUILayout.Label("UI manager not found", EditorStyles.boldLabel);

            GUILayout.Space(10f);
            GUILayout.EndVertical();

        }

    }
    void SoundManager()
    {
        EditorGUI.BeginChangeCheck();
        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
        if (GameM)
        {
            scrollPos4 = EditorGUILayout.BeginScrollView(scrollPos4, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(5f);

            GUILayout.Label("System", EditorStyles.boldLabel);
            GUILayout.Space(10f);


            
            GameS.bgmSound = EditorGUILayout.ObjectField("Audio Source: ", GameS.bgmSound, typeof(AudioSource), true) as AudioSource;
            GUILayout.Space(10f);

            GameS.SFXSound = EditorGUILayout.ObjectField("SFX Source: ", GameS.SFXSound, typeof(AudioSource), true) as AudioSource;
            GUILayout.Space(10f);

            GameS.BGSound = EditorGUILayout.ObjectField("BG Music: ", GameS.BGSound, typeof(AudioClip), true) as AudioClip;
            GUILayout.Space(10f);

            GameS.BGSoundSpecial = EditorGUILayout.ObjectField("BG Music Item Special: ", GameS.BGSoundSpecial, typeof(AudioClip), true) as AudioClip;

            GUILayout.Space(10f);

            if (GameGui)
            {
                if (GameS.musicVolumeSlider == null && GameGui.musicVolumeSlider != null)
                {
                    GameS.musicVolumeSlider = GameGui.musicVolumeSlider;
                }

                if (GameS.SFXVolumeSlider == null && GameGui.SFXVolumeSlider !=null)
                {
                    GameS.SFXVolumeSlider = GameGui.SFXVolumeSlider;
                }

            }


            GameS.musicVolumeSlider = EditorGUILayout.ObjectField("Music Volumen Slider: ", GameS.musicVolumeSlider, typeof(Slider), true) as Slider;
            GUILayout.Space(10f);
            GameS.SFXVolumeSlider = EditorGUILayout.ObjectField("SFX Volumen Slider: ", GameS.SFXVolumeSlider, typeof(Slider), true) as Slider;

            GUILayout.Space(10f);
            GameSS.Update();

            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));

            GUILayout.Label("Sound List ", EditorStyles.boldLabel);


            GUILayout.Space(10f);

            if (GUILayout.Button("Add New Custom Sound"))
            {
                GameS.sound_List.Add(new SoundGroup());
                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
            }
            if (GameS.sound_List.Count >=8)
            {
                GUILayout.Space(10);
                if (GUILayout.Button("Remove Last Custom Sound"))
                {
                    GameS.sound_List.RemoveAt(GameS.sound_List.Count - 1);
                    EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                }
            }

            GUILayout.Space(10f);

            EditorGUILayout.TextArea("To play a custom sound you must call the following line of code: D3SoundManager.instance.PlayingAudioClip(_soundName);", GUI.skin.GetStyle("HelpBox"));




            GUILayout.EndVertical();

            if (GameS.sound_List.Count > 0)
            {

                for (int i = 0; i < GameS.sound_List.Count; i++)
                {
                    if (GameS.sound_List[i] != null)
                    {
                        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                        if (i == 0)
                        {
                            GUILayout.Space(10f);
                            GUILayout.Label("Default Sound: ", EditorStyles.boldLabel);
                            GUILayout.Space(10f);
                            GameS.sound_List[i].audioClip = EditorGUILayout.ObjectField("Audio Clip: ", GameS.sound_List[i].audioClip, typeof(AudioClip), true) as AudioClip;
                            GUILayout.Space(10f);
                            GUILayout.Label("Sound Name: " + GameS.sound_List[i].soundName);
                            if (GameS.sound_List[i].soundName != "GetCoin")
                            {
                                GameS.sound_List[i].soundName = "GetCoin";
                            }
                        }
                        if (i == 1)
                        {
                            GUILayout.Space(10f);
                            GUILayout.Label("Default Sound: ", EditorStyles.boldLabel);
                            GUILayout.Space(10f);
                            GameS.sound_List[i].audioClip = EditorGUILayout.ObjectField("Audio Clip: ", GameS.sound_List[i].audioClip, typeof(AudioClip), true) as AudioClip;
                            GUILayout.Space(10f);
                            GUILayout.Label("Sound Name: " + GameS.sound_List[i].soundName);
                            if (GameS.sound_List[i].soundName != "Jump")
                            {
                                GameS.sound_List[i].soundName = "Jump";
                            }
                        }
                        if (i == 2)
                        {
                            GUILayout.Space(10f);
                            GUILayout.Label("Default Sound: ", EditorStyles.boldLabel);
                            GUILayout.Space(10f);
                            GameS.sound_List[i].audioClip = EditorGUILayout.ObjectField("Audio Clip: ", GameS.sound_List[i].audioClip, typeof(AudioClip), true) as AudioClip;
                            GUILayout.Space(10f);
                            GUILayout.Label("Sound Name: " + GameS.sound_List[i].soundName);
                            if (GameS.sound_List[i].soundName != "HitOBJ")
                            {
                                GameS.sound_List[i].soundName = "HitOBJ";
                            }
                        }
                        if (i == 3)
                        {
                            GUILayout.Space(10f);
                            GUILayout.Label("Default Sound: ", EditorStyles.boldLabel);
                            GUILayout.Space(10f);
                            GameS.sound_List[i].audioClip = EditorGUILayout.ObjectField("Audio Clip: ", GameS.sound_List[i].audioClip, typeof(AudioClip), true) as AudioClip;
                            GUILayout.Space(10f);
                            GUILayout.Label("Sound Name: " + GameS.sound_List[i].soundName);
                            if (GameS.sound_List[i].soundName != "GetItem")
                            {
                                GameS.sound_List[i].soundName = "GetItem";
                            }
                        }
                        if (i == 4)
                        {
                            GUILayout.Space(10f);
                            GUILayout.Label("Default Sound: ", EditorStyles.boldLabel);
                            GUILayout.Space(10f);
                            GameS.sound_List[i].audioClip = EditorGUILayout.ObjectField("Audio Clip: ", GameS.sound_List[i].audioClip, typeof(AudioClip), true) as AudioClip;
                            GUILayout.Space(10f);
                            GUILayout.Label("Sound Name: " + GameS.sound_List[i].soundName);
                            if (GameS.sound_List[i].soundName != "Step")
                            {
                                GameS.sound_List[i].soundName = "Step";
                            }
                        }
                        if (i == 5)
                        {
                            GUILayout.Space(10f);
                            GUILayout.Label("Default Sound: ", EditorStyles.boldLabel);
                            GUILayout.Space(10f);
                            GameS.sound_List[i].audioClip = EditorGUILayout.ObjectField("Audio Clip: ", GameS.sound_List[i].audioClip, typeof(AudioClip), true) as AudioClip;
                            GUILayout.Space(10f);
                            GUILayout.Label("Sound Name: " + GameS.sound_List[i].soundName);
                            if (GameS.sound_List[i].soundName != "Roll")
                            {
                                GameS.sound_List[i].soundName = "Roll";
                            }
                        }
                        if (i == 6)
                        {
                            GUILayout.Space(10f);
                            GUILayout.Label("Default Sound: ", EditorStyles.boldLabel);
                            GUILayout.Space(10f);
                            GameS.sound_List[i].audioClip = EditorGUILayout.ObjectField("Audio Clip: ", GameS.sound_List[i].audioClip, typeof(AudioClip), true) as AudioClip;
                            GUILayout.Space(10f);
                            GUILayout.Label("Sound Name: " + GameS.sound_List[i].soundName);
                            if (GameS.sound_List[i].soundName != "Button")
                            {
                                GameS.sound_List[i].soundName = "Button";
                            }
                        }
                        if (i > 6)
                        {
                            GUILayout.Space(10f);
                            GUILayout.Label("Custom Sound: ", EditorStyles.boldLabel);
                            GUILayout.Space(10f);
                            GameS.sound_List[i].audioClip = EditorGUILayout.ObjectField("Audio Clip: ", GameS.sound_List[i].audioClip, typeof(AudioClip), true) as AudioClip;
                            GUILayout.Space(10f);
                            GameS.sound_List[i].soundName = EditorGUILayout.TextField("Sound Name: ", GameS.sound_List[i].soundName);
                        }

                        GUILayout.EndVertical();
                    }

                }

            }
            GUILayout.EndVertical();
            EditorGUILayout.EndScrollView();

        }
        GUILayout.Space(10f);
        GUILayout.EndVertical();
        if (EditorGUI.EndChangeCheck())
        {
            if (!Application.isPlaying)
            {
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }
    }
    void PatternSystem()
    {
        const int width = 780;
        const int height = 970;

        var x = (Screen.currentResolution.width - width) / 2;
        var y = (Screen.currentResolution.height - height) / 2;

        EditorGUI.BeginChangeCheck();
        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
        if (GameM)
        {
            scrollPos5 = EditorGUILayout.BeginScrollView(scrollPos5, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(5f);

            GUILayout.Label("System", EditorStyles.boldLabel);
            GUILayout.Space(10f);

            EditorGUILayout.TextArea("Pattern Setup: Before configuring the distribution pattern, first create the items and build them, then add each item to the distribution list.", GUI.skin.GetStyle("HelpBox"));

            if (GUILayout.Button("Modify Distribution Pattern"))
            {
                EditorWindow.GetWindow(typeof(PaternWindowEditor)).position = new Rect(x, y, width, height);

            }

            GUILayout.Space(20f);

            EditorGUILayout.TextArea("System Object (do not delete)", GUI.skin.GetStyle("HelpBox"));

            GamePS.spawnObj_Pref = EditorGUILayout.ObjectField("Spawn Obj Pref: ", GamePS.spawnObj_Pref, typeof(GameObject), true) as GameObject;


            GUILayout.EndVertical();

            GUILayout.Space(10f);
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));

            GUILayout.Space(10f);
            GUILayout.Label("Config", EditorStyles.boldLabel);
            GUILayout.Space(10f);
            GamePS.FirstFloorClean = GUILayout.Toggle(GamePS.FirstFloorClean, " First Floor Clean");
            EditorGUILayout.TextArea("When starting or restarting the game, the first generated block will be unobstructed to prevent the user from losing immediately.", GUI.skin.GetStyle("HelpBox"));


            GUILayout.Space(20f);
            GamePS.TimeForTheTransitionToSpecial = EditorGUILayout.FloatField("Time For The Transition To Special: ", GamePS.TimeForTheTransitionToSpecial);
            GUILayout.Space(20f);
            GamePS.PositionAnimationYItemSpecial = EditorGUILayout.FloatField("Position Animation Y Item Special: ", GamePS.PositionAnimationYItemSpecial);

            GUILayout.Space(20f);
            GamePS.TimeForTheTransitionToRocket = EditorGUILayout.FloatField("Time For The Transition To Rocket: ", GamePS.TimeForTheTransitionToRocket);

            GUILayout.Space(20f);
            GamePS.CleanwhenFinishingRocketItem = GUILayout.Toggle(GamePS.CleanwhenFinishingRocketItem, " Clean Floor when Finishing Rocket Item");

            if (GamePS.CleanwhenFinishingRocketItem)
            {
                GUILayout.Space(10f);
                GamePS.FloorsCleanwhenFinishingRocketItem = EditorGUILayout.IntField("Number of Floors to Clean when Finishing Rocket Item: ", GamePS.FloorsCleanwhenFinishingRocketItem);
            }

            GUILayout.Space(20f);

            GamePS.ChangueSkybox = GUILayout.Toggle(GamePS.ChangueSkybox, "Change Skybox in Special tem");

            if (GamePS.ChangueSkybox)
            {
                GUILayout.Space(20f);

                GamePS.SkyBoxForDefaultPatternSystem = EditorGUILayout.ObjectField("SkyBox Default Pattern System: ", GamePS.SkyBoxForDefaultPatternSystem, typeof(Material), true) as Material;

                GUILayout.Space(10f);
                GamePS.SkyBoxForSpecialtPatternSystem = EditorGUILayout.ObjectField("SkyBox Special Pattern System: ", GamePS.SkyBoxForSpecialtPatternSystem, typeof(Material), true) as Material;


                GUILayout.Space(20f);
            }
            GUILayout.EndVertical();

            GUILayout.Space(10f);


            GamePSS.Update();

            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Label("List of Buildings ", EditorStyles.boldLabel);
            GUILayout.Space(10f);
            EditorGUILayout.TextArea("List of Buildings to be distributed", GUI.skin.GetStyle("HelpBox"));

            
            Rect myRect = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
            GUI.Box(myRect, "Drag and drop Building Prefab \n into this box!");

            if (myRect.Contains(Event.current.mousePosition))
            {
                if (Event.current.type == EventType.DragUpdated)
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    Event.current.Use();
                }
                else if (Event.current.type == EventType.DragPerform)
                {
                    for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
                    {

                        D3Building AddGO = DragAndDrop.objectReferences[i].GetComponent<D3Building>();

                        if (AddGO)
                        {
                            GamePS.building_Pref.Add(AddGO.gameObject);
                            GamePSS.ApplyModifiedProperties();
                            if (!Application.isPlaying)
                            {
                                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                            }
                        }
                        else
                        {
                            Debug.Log("Invalid Building Prefab");
                        }

                    }
                    Event.current.Use();
                }
            }

            if (GamePS.building_Pref.Count >= 1)
            {
                GUILayout.Space(10);
                if (GUILayout.Button("Remove Last Building Prefab"))
                {
                    GamePS.building_Pref.RemoveAt(GamePS.building_Pref.Count - 1);
                    EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                }
                GUILayout.Space(10);
                GUILayout.Label("Total Buildings added: " + GamePS.building_Pref.Count);
                for (int i = 0; i < GamePS.building_Pref.Count; i++)
                {
                    if (GamePS.building_Pref[i] != null)
                    {
                        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                        GamePS.building_Pref[i] = EditorGUILayout.ObjectField("Building Prefab: ", GamePS.building_Pref[i], typeof(GameObject), true) as GameObject;
                        GUILayout.EndVertical();
                    }
                }
                if (GUILayout.Button("Remove Last Building Prefab"))
                {
                    GamePS.building_Pref.RemoveAt(GamePS.building_Pref.Count - 1);
                    EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                }
            }
            GUILayout.EndVertical();


            GUILayout.Space(10f);

            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Label("List of Item ", EditorStyles.boldLabel);
            GUILayout.Space(10f);
            EditorGUILayout.TextArea("List of item to be distributed", GUI.skin.GetStyle("HelpBox"));



            Rect myRect2 = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
            GUI.Box(myRect2, "Drag and drop Item Prefab \n into this box!");

            if (myRect2.Contains(Event.current.mousePosition))
            {
                if (Event.current.type == EventType.DragUpdated)
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    Event.current.Use();
                }
                else if (Event.current.type == EventType.DragPerform)
                {
                    for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
                    {

                        GameObject AddGO = DragAndDrop.objectReferences[i] as GameObject;

                        if (AddGO)
                        {
                            GamePS.item_Pref.Add(AddGO);
                            GamePSS.ApplyModifiedProperties();
                            if (!Application.isPlaying)
                            {
                                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                            }
                        }
                        else
                        {
                            Debug.Log("Invalid Item Prefab");
                        }

                    }
                    Event.current.Use();
                }
            }

            if (GamePS.item_Pref.Count >= 1)
            {
                GUILayout.Space(10);
                if (GUILayout.Button("Remove Last Item Prefab"))
                {
                    GamePS.item_Pref.RemoveAt(GamePS.item_Pref.Count - 1);
                    EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                }
                GUILayout.Space(10);
                GUILayout.Label("Total items added: "+ GamePS.item_Pref.Count);
                for (int i = 0; i < GamePS.item_Pref.Count; i++)
                {
                    if (GamePS.item_Pref[i] != null)
                    {
                        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                        GamePS.item_Pref[i] = EditorGUILayout.ObjectField("Item Prefab: ", GamePS.item_Pref[i], typeof(GameObject), true) as GameObject;
                        GUILayout.EndVertical();
                    }
                }

                if (GUILayout.Button("Remove Last Item Prefab"))
                {
                    GamePS.item_Pref.RemoveAt(GamePS.item_Pref.Count - 1);
                    EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                }
            }


            GUILayout.EndVertical();

            GUILayout.Space(10f);

            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));

            GUILayout.Space(10f);
            GUILayout.Label("Floor ", EditorStyles.boldLabel);
            EditorGUILayout.TextArea("Add the object that contains the floor", GUI.skin.GetStyle("HelpBox"));

            GUILayout.Space(10f);
            GamePS.floor_Pref = EditorGUILayout.ObjectField("3D Floor Pref: ", GamePS.floor_Pref, typeof(D3Floor), true) as D3Floor;
           
            GUILayout.Space(10f);

            GUILayout.EndVertical();

            GamePSS.ApplyModifiedProperties();
            EditorGUILayout.EndScrollView();

        }
        GUILayout.Space(10f);
        GUILayout.EndVertical();
        if (EditorGUI.EndChangeCheck())
        {
            if (!Application.isPlaying)
            {
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }
    }
    void Collectables()
    {
        EditorGUI.BeginChangeCheck();
        Texture CoinImg = Resources.Load<Texture>("Img/D3coin");
        Texture CoinDouble = Resources.Load<Texture>("Img/D3Seek");
        Texture CoinSprint = Resources.Load<Texture>("Img/D3InvicibilityPowerup");
        Texture CoinMagnet = Resources.Load<Texture>("Img/D3MagnetPowerup");
        Texture CoinMultiply = Resources.Load<Texture>("Img/D3Multiplierpowerup");
        Texture CoinShield = Resources.Load<Texture>("Img/D3Shield");
        Texture CoinSpecial = Resources.Load<Texture>("Img/D3ItemBonus");
        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
        if (GameM)
        {
            scrollPos6 = EditorGUILayout.BeginScrollView(scrollPos6, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(5f);

            GUILayout.Label("Items", EditorStyles.boldLabel);
            GUILayout.Space(10f);


            GUILayout.Label("Coin", EditorStyles.boldLabel);
            GUILayout.Space(10f);
            if (GUILayout.Button(CoinImg, GUILayout.Width(90), GUILayout.Height(90)))
            {
                //instantiate ui canvas
                GameObject ASS = Instantiate(Resources.Load("Game/Coin")) as GameObject;
                //rename it
                ASS.name = "Coin";

                EditorGUIUtility.PingObject(ASS);
                Selection.activeGameObject = ASS.gameObject;
                SceneView.lastActiveSceneView.FrameSelected();

                Debug.Log("Coin Prefabs Created!");

            }
            GUILayout.Space(10f);
            GUILayout.Label("Double Jump", EditorStyles.boldLabel);
            GUILayout.Space(10f);
            if (GUILayout.Button(CoinDouble, GUILayout.Width(90), GUILayout.Height(90)))
            {
                //instantiate ui canvas
                GameObject ASS = Instantiate(Resources.Load("Game/Item_Jump")) as GameObject;
                //rename it
                ASS.name = "Item_Jump";

                EditorGUIUtility.PingObject(ASS);
                Selection.activeGameObject = ASS.gameObject;
                SceneView.lastActiveSceneView.FrameSelected();

                Debug.Log("Item Jump Prefabs Created!");
            }
            GUILayout.Space(10f);
            GUILayout.Label("Sprint", EditorStyles.boldLabel);
            GUILayout.Space(10f);
            if (GUILayout.Button(CoinSprint, GUILayout.Width(90), GUILayout.Height(90)))
            {
                //instantiate ui canvas
                GameObject ASS = Instantiate(Resources.Load("Game/Item_Sprint")) as GameObject;
                //rename it
                ASS.name = "Item Sprint";

                EditorGUIUtility.PingObject(ASS);
                Selection.activeGameObject = ASS.gameObject;
                SceneView.lastActiveSceneView.FrameSelected();

                Debug.Log("Item Sprint Prefabs Created!");

            }
            GUILayout.Space(10f);
            GUILayout.Label("Magnet", EditorStyles.boldLabel);
            GUILayout.Space(10f);
            if (GUILayout.Button(CoinMagnet, GUILayout.Width(90), GUILayout.Height(90)))
            {
                //instantiate ui canvas
                GameObject ASS = Instantiate(Resources.Load("Game/Item_Magnet")) as GameObject;
                //rename it
                ASS.name = "Item_Magnet";

                EditorGUIUtility.PingObject(ASS);
                Selection.activeGameObject = ASS.gameObject;
                SceneView.lastActiveSceneView.FrameSelected();

                Debug.Log("Item Magnet Prefabs Created!");
            }
            GUILayout.Space(10f);
            GUILayout.Label("Multiplier", EditorStyles.boldLabel);
            GUILayout.Space(10f);
            if (GUILayout.Button(CoinMultiply, GUILayout.Width(90), GUILayout.Height(90)))
            {
                //instantiate ui canvas
                GameObject ASS = Instantiate(Resources.Load("Game/Item_Multiply")) as GameObject;
                //rename it
                ASS.name = "Item_Multiply";

                EditorGUIUtility.PingObject(ASS);
                Selection.activeGameObject = ASS.gameObject;
                SceneView.lastActiveSceneView.FrameSelected();

                Debug.Log("Item Multiply Prefabs Created!");
            }
            GUILayout.Space(10f);
            GUILayout.Label("Shield", EditorStyles.boldLabel);
            GUILayout.Space(10f);
            if (GUILayout.Button(CoinShield, GUILayout.Width(90), GUILayout.Height(90)))
            {
                //instantiate ui canvas
                GameObject ASS = Instantiate(Resources.Load("Game/Item_Shield")) as GameObject;
                //rename it
                ASS.name = "Item_Shield";

                EditorGUIUtility.PingObject(ASS);
                Selection.activeGameObject = ASS.gameObject;
                SceneView.lastActiveSceneView.FrameSelected();

                Debug.Log("Item Shield Prefabs Created!");
            }
            GUILayout.Space(10f);
            GUILayout.Label("Special", EditorStyles.boldLabel);
            GUILayout.Space(10f);
            if (GUILayout.Button(CoinSpecial, GUILayout.Width(90), GUILayout.Height(90)))
            {
                //instantiate ui canvas
                GameObject ASS = Instantiate(Resources.Load("Game/Item_Special")) as GameObject;
                //rename it
                ASS.name = "Item_Special";

                EditorGUIUtility.PingObject(ASS);
                Selection.activeGameObject = ASS.gameObject;
                SceneView.lastActiveSceneView.FrameSelected();

                Debug.Log("Item Special Prefabs Created!");
            }


            GUILayout.EndVertical();
            EditorGUILayout.EndScrollView();

        }
        GUILayout.Space(10f);
        GUILayout.EndVertical();
        if (EditorGUI.EndChangeCheck())
        {
            if (!Application.isPlaying)
            {
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }

    }
    void Obstacles()
    {
        EditorGUI.BeginChangeCheck();
        Texture CoinBarrier = Resources.Load<Texture>("Img/D3barrier");
        Texture CoinBridge = Resources.Load<Texture>("Img/D3bridge");
        Texture CoinBus = Resources.Load<Texture>("Img/D3bus");
        Texture CoinCar = Resources.Load<Texture>("Img/D3car");
        Texture CoinRoll = Resources.Load<Texture>("Img/D3roll");
        Texture CoinTunnel = Resources.Load<Texture>("Img/D3tunnel");
        Texture D3busUP = Resources.Load<Texture>("Img/D3busUP");


        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
        if (GameM)
        {
            scrollPos7 = EditorGUILayout.BeginScrollView(scrollPos7, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(5f);

            GUILayout.Label("Obstacles", EditorStyles.boldLabel);
            GUILayout.Space(10f);

            GUILayout.Label("Barrier", EditorStyles.boldLabel);
            GUILayout.Space(10f);
            if (GUILayout.Button(CoinBarrier, GUILayout.Width(90), GUILayout.Height(90)))
            {
                //instantiate ui canvas
                GameObject ASS = Instantiate(Resources.Load("Game/Barrier")) as GameObject;
                //rename it
                ASS.name = "Barrier";

                EditorGUIUtility.PingObject(ASS);
                Selection.activeGameObject = ASS.gameObject;
                SceneView.lastActiveSceneView.FrameSelected();

                Debug.Log("Barrier Prefabs Created!");

            }
            GUILayout.Space(10f);


            ////////////////////////
            ///

            GUILayout.Label("Bridge", EditorStyles.boldLabel);
            GUILayout.Space(10f);
            if (GUILayout.Button(CoinBridge, GUILayout.Width(90), GUILayout.Height(90)))
            {
                //instantiate ui canvas
                GameObject ASS = Instantiate(Resources.Load("Game/Bridge")) as GameObject;
                //rename it
                ASS.name = "Bridge";

                EditorGUIUtility.PingObject(ASS);
                Selection.activeGameObject = ASS.gameObject;
                SceneView.lastActiveSceneView.FrameSelected();

                Debug.Log("Bridge Prefabs Created!");

            }
            GUILayout.Space(10f);


            ////////////////////////
            ///
            ////////////////////////
            ///

            GUILayout.Label("Board Up", EditorStyles.boldLabel);
            GUILayout.Space(10f);
            if (GUILayout.Button(D3busUP, GUILayout.Width(90), GUILayout.Height(90)))
            {
                //instantiate ui canvas
                GameObject ASS = Instantiate(Resources.Load("Game/BoardUp")) as GameObject;
                //rename it
                ASS.name = "Board Up";

                EditorGUIUtility.PingObject(ASS);
                Selection.activeGameObject = ASS.gameObject;
                SceneView.lastActiveSceneView.FrameSelected();

                Debug.Log("Board Up Prefabs Created!");

            }
            GUILayout.Space(10f);


            ////////////////////////
            ///////////////////////////
            ///

            GUILayout.Label("Object Moving", EditorStyles.boldLabel);
            GUILayout.Space(10f);
            if (GUILayout.Button(CoinBus, GUILayout.Width(90), GUILayout.Height(90)))
            {
                //instantiate ui canvas
                GameObject ASS = Instantiate(Resources.Load("Game/BusMoving")) as GameObject;
                //rename it
                ASS.name = "Object Moving";

                EditorGUIUtility.PingObject(ASS);
                Selection.activeGameObject = ASS.gameObject;
                SceneView.lastActiveSceneView.FrameSelected();

                Debug.Log("Object Moving Prefabs Created!");

            }
            GUILayout.Space(10f);


            ////////////////////////

            GUILayout.Label("Bus", EditorStyles.boldLabel);
            GUILayout.Space(10f);
            if (GUILayout.Button(CoinBus, GUILayout.Width(90), GUILayout.Height(90)))
            {
                //instantiate ui canvas
                GameObject ASS = Instantiate(Resources.Load("Game/Bus")) as GameObject;
                //rename it
                ASS.name = "Bus";

                EditorGUIUtility.PingObject(ASS);
                Selection.activeGameObject = ASS.gameObject;
                SceneView.lastActiveSceneView.FrameSelected();

                Debug.Log("Bus Prefabs Created!");

            }
            GUILayout.Space(10f);

            ////////////////////////
            ///

            GUILayout.Label("Car", EditorStyles.boldLabel);
            GUILayout.Space(10f);
            if (GUILayout.Button(CoinCar, GUILayout.Width(90), GUILayout.Height(90)))
            {
                //instantiate ui canvas
                GameObject ASS = Instantiate(Resources.Load("Game/Car")) as GameObject;
                //rename it
                ASS.name = "Car";

                EditorGUIUtility.PingObject(ASS);
                Selection.activeGameObject = ASS.gameObject;
                SceneView.lastActiveSceneView.FrameSelected();

                Debug.Log("Car Prefabs Created!");

            }
            GUILayout.Space(10f);
            ////////////////////////
            ///

            GUILayout.Label("Obstacle Roll", EditorStyles.boldLabel);
            GUILayout.Space(10f);
            if (GUILayout.Button(CoinRoll, GUILayout.Width(90), GUILayout.Height(90)))
            {
                //instantiate ui canvas
                GameObject ASS = Instantiate(Resources.Load("Game/Obstacle_Roll")) as GameObject;
                //rename it
                ASS.name = "Obstacle Roll";

                EditorGUIUtility.PingObject(ASS);
                Selection.activeGameObject = ASS.gameObject;
                SceneView.lastActiveSceneView.FrameSelected();

                Debug.Log("Obstacle Roll Prefabs Created!");

            }
            GUILayout.Space(10f);

            ////////////////////////
            ///

            GUILayout.Label("Tunnel", EditorStyles.boldLabel);
            GUILayout.Space(10f);
            if (GUILayout.Button(CoinTunnel, GUILayout.Width(90), GUILayout.Height(90)))
            {
                //instantiate ui canvas
                GameObject ASS = Instantiate(Resources.Load("Game/Tunnel_Long")) as GameObject;
                //rename it
                ASS.name = "Tunnel Long";

                EditorGUIUtility.PingObject(ASS);
                Selection.activeGameObject = ASS.gameObject;
                SceneView.lastActiveSceneView.FrameSelected();

                Debug.Log("Tunnel Long Prefabs Created!");

            }
            GUILayout.Space(10f);



            GUILayout.EndVertical();
            EditorGUILayout.EndScrollView();

        }
        GUILayout.Space(10f);
        GUILayout.EndVertical();
        if (EditorGUI.EndChangeCheck())
        {
            if (!Application.isPlaying)
            {
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }

    }
    void Building()
    {
        EditorGUI.BeginChangeCheck();
        Texture CoinBuilding = Resources.Load<Texture>("Img/D3building");

        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
        if (GameM)
        {
            scrollPos8 = EditorGUILayout.BeginScrollView(scrollPos8, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(5f);

            GUILayout.Label("Building", EditorStyles.boldLabel);
            GUILayout.Space(10f);

            GUILayout.Label("New Building", EditorStyles.boldLabel);
            GUILayout.Space(10f);
            if (GUILayout.Button(CoinBuilding, GUILayout.Width(90), GUILayout.Height(90)))
            {
                //instantiate ui canvas
                GameObject ASS = Instantiate(Resources.Load("Game/Building")) as GameObject;
                //rename it
                ASS.name = "Building";

                EditorGUIUtility.PingObject(ASS);
                Selection.activeGameObject = ASS.gameObject;
                SceneView.lastActiveSceneView.FrameSelected();

                Debug.Log("Building Prefabs Created!");

            }
            GUILayout.Space(10f);


            GUILayout.EndVertical();
            EditorGUILayout.EndScrollView();

        }
        GUILayout.Space(10f);
        GUILayout.EndVertical();
        if (EditorGUI.EndChangeCheck())
        {
            if (!Application.isPlaying)
            {
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }

    }
    void LevelSystem()
    {
        EditorGUI.BeginChangeCheck();
        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
        GUILayout.Label("Level System", EditorStyles.boldLabel);
        GUILayout.Space(10f);
        EditorGUILayout.TextArea("Level System, Place a random number and each time the game scene starts, the score to win will be random, including the star system, everything is automatic.", GUI.skin.GetStyle("HelpBox"));


        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));

        GUILayout.Space(10f);

        GameA.UseRandomScore = GUILayout.Toggle(GameA.UseRandomScore, "Use Random Score To Win Level:");

        if (GameA.UseRandomScore)
        {
            GUILayout.Space(10f);
            EditorGUILayout.TextArea("Every time you win the level, the next level will be a random number to win", GUI.skin.GetStyle("HelpBox"));
            GUILayout.Space(10f);
            GameA.MinScoreToWin = EditorGUILayout.IntField("Min Score To Win:", GameA.MinScoreToWin);
            GUILayout.Space(10f);
            GameA.MaxScoreToWin = EditorGUILayout.IntField("Max Score To Win:", GameA.MaxScoreToWin);
            GUILayout.Space(10f);
        }

        if (!GameA.UseRandomScore && Application.isPlaying == false)
        {
            GUILayout.Space(10f);
            GameA.UseSpecificScore = GUILayout.Toggle(GameA.UseSpecificScore, "Use Specific Score To Win Level:");
            if (GameA.UseSpecificScore)
            {
                GUILayout.Space(10f);
                GameA.MinScoreToWin = EditorGUILayout.IntField("Score To Win:", GameA.MinScoreToWin);
                GUILayout.Space(10f);
            }
            if (!GameA.UseSpecificScore)
            {
                GUILayout.Space(10f);
                EditorGUILayout.TextArea("No Score to Win, always win", GUI.skin.GetStyle("HelpBox"));
                GUILayout.Space(10f);
            }


        }

        GUILayout.EndVertical();

        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));

        GUILayout.Space(10f);

        GameGui.ButtonToNewLevel = EditorGUILayout.ObjectField("Button To New Level: ", GameGui.ButtonToNewLevel, typeof(GameObject), true) as GameObject;
        
        GUILayout.Space(10f);

        EditorGUILayout.TextArea("This button is automatically disabled and enabled if a new level is available and if you have activated the level system on the title screen.", GUI.skin.GetStyle("HelpBox"));
        
        GUILayout.Space(10f);

        EditorGUILayout.TextArea("Just set the scene on the main screen, the system automatically detects the current level and the level that comes next.", GUI.skin.GetStyle("HelpBox"));

        GUILayout.Space(10f);
        GUILayout.EndVertical();

        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));

        GUILayout.Space(10f);

        GameA.UseStartSystem = GUILayout.Toggle(GameA.UseStartSystem, "Use: Start System");

        GUILayout.Space(10f);


        if (GameA.UseStartSystem && Application.isPlaying == false)
        {
            GUILayout.Space(10f);

            GUILayout.Label("Start System", EditorStyles.boldLabel);

            GUILayout.Space(10f);
            if (GameGui.StartON1 != null)
            {
                GameGui.StartON1.SetActive(true);
            }
            if (GameGui.StartON2 != null)
            {
                GameGui.StartON2.SetActive(true);
            }
            if (GameGui.StartON3 != null)
            {
                GameGui.StartON3.SetActive(true);
            }
            if (GameGui.StartOFF1 != null)
            {
                GameGui.StartOFF1.SetActive(true);
            }
            if (GameGui.StartOFF2 != null)
            {
                GameGui.StartOFF2.SetActive(true);
            }
            if (GameGui.StartOFF3 != null)
            {
                GameGui.StartOFF3.SetActive(true);
            }

            GameGui.StartOFF1 = EditorGUILayout.ObjectField("Start OFF 1: ", GameGui.StartOFF1, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);

            GameGui.StartON1 = EditorGUILayout.ObjectField("Start ON 1: ", GameGui.StartON1, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);

            GameGui.StartOFF2 = EditorGUILayout.ObjectField("Start OFF 2: ", GameGui.StartOFF2, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);

            GameGui.StartON2 = EditorGUILayout.ObjectField("Start ON 2: ", GameGui.StartON2, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);

            GameGui.StartOFF3 = EditorGUILayout.ObjectField("Start OFF 3: ", GameGui.StartOFF3, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);

            GameGui.StartON3 = EditorGUILayout.ObjectField("Start ON 3: ", GameGui.StartON3, typeof(GameObject), true) as GameObject;

        }
        if (!GameA.UseStartSystem && Application.isPlaying == false)
        {
            if (GameGui.StartON1 != null)
            {
                GameGui.StartON1.SetActive(false);
            }
            if (GameGui.StartON2 != null)
            {
                GameGui.StartON2.SetActive(false);
            }
            if (GameGui.StartON3 != null)
            {
                GameGui.StartON3.SetActive(false);
            }
            if (GameGui.StartOFF1 != null)
            {
                GameGui.StartOFF1.SetActive(false);
            }
            if (GameGui.StartOFF2 != null)
            {
                GameGui.StartOFF2.SetActive(false);
            }
            if (GameGui.StartOFF3 != null)
            {
                GameGui.StartOFF3.SetActive(false);
            }
        }

        GUILayout.EndVertical();

        GUILayout.EndVertical();
        if (EditorGUI.EndChangeCheck())
        {
            if (!Application.isPlaying)
            {
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }
    }
    void EnemyEditor()
    {
        EditorGUI.BeginChangeCheck();
        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
        GUILayout.Label("Game Manager Editor", EditorStyles.boldLabel);
        if (GameM)
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos9, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.BeginVertical("GroupBox");
            GUILayout.Space(10f);

            GUILayout.Label("Enemy Follower Editor", EditorStyles.boldLabel);

            GUILayout.Space(20f);

            Gamec.UseEnemyFollower = GUILayout.Toggle(Gamec.UseEnemyFollower, " Use Enemy Follower");

            if (Gamec.UseEnemyFollower)
            {
                GUILayout.Space(10f);
                Gamec.Enemy = EditorGUILayout.ObjectField("Enemy: ", Gamec.Enemy, typeof(D3EnemyController), true) as D3EnemyController;

            }
            GUILayout.Space(10f);
            GUILayout.EndVertical();

            if (Gamec.Enemy != null && Gamec.UseEnemyFollower)
            {
                GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                GUILayout.Label("Enemy Settings", EditorStyles.boldLabel);
                GUILayout.Space(10f);

                Gamec.Enemy.decreaseLife = EditorGUILayout.IntField("Decrease Life Player: ", Gamec.Enemy.decreaseLife);
                GUILayout.Space(10f);

                Gamec.Enemy.DistanceEnemy = EditorGUILayout.FloatField("Player distance: ", Gamec.Enemy.DistanceEnemy);

                GUILayout.Space(10f);
                EditorGUILayout.TextArea("The enemy will get closer to the player as he collides with the obstacles, here you can modify the distance that the enemy will get when the player collides, the first collision of the player the enemy gets a little closer, the third collision (Random) the enemy approaches completely and takes life away from the player.", GUI.skin.GetStyle("HelpBox"));

                GUILayout.Space(10f);

                Gamec.Enemy.FirsHitPlayerDistanceEnemy = EditorGUILayout.FloatField("Player's First Collision (Distance): ", Gamec.Enemy.FirsHitPlayerDistanceEnemy);

                GUILayout.Space(10f);

                Gamec.Enemy.SecondHitPlayerDistanceEnemy = EditorGUILayout.FloatField("Player's Second Collision (Distance): ", Gamec.Enemy.SecondHitPlayerDistanceEnemy);
              
                GUILayout.Space(10f);

                Gamec.Enemy.ThirdHitPlayerDistanceEnemy = EditorGUILayout.FloatField("Player's Third Collision (Distance): ", Gamec.Enemy.ThirdHitPlayerDistanceEnemy);

                GUILayout.Space(10f);

                Gamec.Enemy.PosEnemyWhenArrestPlayer = EditorGUILayout.Vector3Field("Pos Enemy When Arrest Player: ", Gamec.Enemy.PosEnemyWhenArrestPlayer);

                GUILayout.Space(10f);
                Gamec.Enemy.FarPolice = EditorGUILayout.ObjectField("Sound: Far Police: ", Gamec.Enemy.FarPolice, typeof(AudioClip), true) as AudioClip;
                GUILayout.Space(10f);

                Gamec.Enemy.ArrestPlayer = EditorGUILayout.ObjectField("Sound: ArrestPlayer: ", Gamec.Enemy.ArrestPlayer, typeof(AudioClip), true) as AudioClip;
                GUILayout.Space(10f);

                GUILayout.EndVertical();
            }


            

            EditorGUILayout.EndScrollView();

        }
        GUILayout.Space(10f);
        GUILayout.EndVertical();
        if (EditorGUI.EndChangeCheck())
        {
            if (!Application.isPlaying)
            {
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                if (Gamec.Enemy != null && Gamec.UseEnemyFollower)
                {
                    EditorUtility.SetDirty(Gamec.Enemy);
                    PrefabUtility.RecordPrefabInstancePropertyModifications(Gamec.Enemy);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }
        }

    }
    void ADSManager()
    {
#if UNITY_ANDROID || UNITY_IOS
        EditorGUI.BeginChangeCheck();
        if (!AdsManager)
        {
            Color defaultColor = GUI.color;
            GUI.color = Color.red;
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUI.color = defaultColor;
            if (GUILayout.Button("Monetization Manager not found, Click to add"))
            {
                //instantiate ui canvas
                GameObject ASS = Instantiate(Resources.Load("Game/MonetizationManager")) as GameObject;
                //rename it
                ASS.name = "Monetization Manager";
                ASS.transform.position = new Vector3(0f, 0f, 0f);
                ASS.transform.SetParent(GameM.gameObject.transform);
                AdsManager = ASS.GetComponent<D3ADSManager>();

                OnEnable();

                Debug.Log("Monetization Manager Prefabs Created!");
            }
            GUILayout.EndVertical();
        }

        if (AdsManager)
        {
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));

            GUILayout.Label("ADS Manager", EditorStyles.boldLabel);

            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
#if UNITY_ANDROID
GUILayout.Label("Current Platform: " + "ANDROID" );
#endif
#if UNITY_IOS
GUILayout.Label("Current Platform: " + "iOS"    );
#endif
            GUILayout.EndVertical();

            if (AdsManager.EnableUnityADS)
            {
#if !UNITY_ADS || !ENABLE_UNITY_ADS
                Color defaultColor = GUI.color;
                GUI.color = Color.red;
                GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                GUI.color = defaultColor;
                GUILayout.Label("Unity ADS SDK not found");
                GUILayout.Space(10);
                if (GUILayout.Button("Import Unity Ads Package"))
                {
                    const int width = 600;
                    const int height = 150;

                    var x = (Screen.currentResolution.width - width) / 2;
                    var y = (Screen.currentResolution.height - height) / 2;

                    WindowPackageUnityInstallGame window = (WindowPackageUnityInstallGame)EditorWindow.GetWindow(typeof(WindowPackageUnityInstallGame), false, "Import Unity Ads Package");
                    window.position = new Rect(x, y, width, height);
                    window.CodePackage = 1;
                    window.packageToImport = "com.unity.ads";
                    window.Install = true;
                    window.Show();

                }
                GUILayout.Label("Attention, after installing the SDK, restart Unity so that the changes are detected.");
                
                GUILayout.EndVertical();
#endif

#if UNITY_ADS && ENABLE_UNITY_ADS
                GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
#if UNITY_ANDROID
                GUILayout.Label("Unity ANDROID ADS ID: ");
                AdsManager.UNITY_ADSID_ANDROID = GUILayout.TextField(AdsManager.UNITY_ADSID_ANDROID, 25);
#endif
#if UNITY_IOS
			GUILayout.Label("Unity IOS ADS ID: ");
			AdsManager.UNITY_ADSID_IOS = GUILayout.TextField(AdsManager.UNITY_ADSID_IOS, 25);
#endif

                GUILayout.Label("Interstitia ID Name: ");
                AdsManager._AdInterstitiaUnitName = GUILayout.TextField(AdsManager._AdInterstitiaUnitName, 25);

                GUILayout.Label("Video ID Name: ");
                AdsManager._AdVideoUnitName = GUILayout.TextField(AdsManager._AdVideoUnitName, 25);

                GUILayout.Label("Banner ID Name: ");
                AdsManager._AdBannerUnitName = GUILayout.TextField(AdsManager._AdBannerUnitName, 25);

                GUILayout.EndVertical();

                GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));

                GUILayout.Space(10);

                Gamec.EnableADSOnScene = GUILayout.Toggle(Gamec.EnableADSOnScene, " Enable ADS On Scene");

                GUILayout.Space(10);

                AdsManager.EnableTESTMODE = GUILayout.Toggle(AdsManager.EnableTESTMODE, " Enable Test Mode");

                AdsManager.GameScene = Gamec;


                GUILayout.EndVertical();

                if (Gamec.EnableADSOnScene)
                {
                    AdsScrollPos = EditorGUILayout.BeginScrollView(AdsScrollPos, GUILayout.ExpandWidth(true), GUILayout.Height(600));

                    SerializedADSManager.Update();

                    GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                    GUILayout.Label("Banner ADS");
                    GUILayout.Space(10);
                    Gamec.UseBanner = GUILayout.Toggle(Gamec.UseBanner, " Use Banner");

                    if (Gamec.UseBanner)
                    {
                        SerializedProperty prop = SerializedADSManager.FindProperty("_bannerPosition");
                        if (prop != null)
                            EditorGUILayout.PropertyField(prop, true);

                        GUILayout.Space(10);

                        GUILayout.Label("Banner Events");

                        GUILayout.Space(10);

                        SerializedProperty prop2 = SerializedADSManager.FindProperty("ShowCompleteUnityBanner");
                        if (prop2 != null)
                            EditorGUILayout.PropertyField(prop2, true);
                        GUILayout.Space(10f);

                        SerializedProperty prop3 = SerializedADSManager.FindProperty("BannerDestroyOrHide");
                        if (prop3 != null)
                            EditorGUILayout.PropertyField(prop3, true);
                        GUILayout.Space(10f);

                    }

                    GUILayout.EndVertical();


                    GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                    GameCS.Update();
                    GUILayout.Label("Rewarded Video ADS");
                    Gamec.EnableRewardedADOnScene = GUILayout.Toggle(Gamec.EnableRewardedADOnScene, " Use Rewarded AD Buttons");

                    if (Gamec.EnableRewardedADOnScene)
                    {
                        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                        GUILayout.Label("Rewarded Window");

                        Gamec.EnableRewardedWindow = GUILayout.Toggle(Gamec.EnableRewardedWindow, " Enable Rewarded Window");

                        GUILayout.Space(10);

                        EditorGUILayout.TextArea("This window is used to show the player the amount of reward obtained.", GUI.skin.GetStyle("HelpBox"));

                        GUILayout.Space(10);
                        if (Gamec.EnableRewardedWindow)
                        {
                            GameGui.RewardWindow = EditorGUILayout.ObjectField("Reward Window: ", GameGui.RewardWindow, typeof(GameObject), true) as GameObject;
                            if (GameGui.RewardWindow)
                            {
                                if (RewardWindow)
                                {
                                    RewardWindow.TextReward = EditorGUILayout.ObjectField("Reward Text: ", RewardWindow.TextReward, typeof(Text), true) as Text;
                                    RewardWindow.ImageReward = EditorGUILayout.ObjectField("Image Reward: ", RewardWindow.ImageReward, typeof(Image), true) as Image;

                                }
                                else
                                {
                                    OnEnable();
                                }
                            }
                            GUILayout.Space(10);
                        }

                        GUILayout.EndVertical();

                        if (PanelReward)
                        {
                            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                            GUILayout.Label("Panel Reward ");
                            PanelReward.RewardButton = EditorGUILayout.ObjectField("Ad Button: ", PanelReward.RewardButton, typeof(Button), true) as Button;
                            PanelReward.m_Animator = EditorGUILayout.ObjectField("Animator: ", PanelReward.m_Animator, typeof(Animator), true) as Animator;
                            PanelReward.TextReward = EditorGUILayout.ObjectField("Text Reward: ", PanelReward.TextReward, typeof(TextMeshProUGUI), true) as TextMeshProUGUI;
                            PanelReward.ImgReward = EditorGUILayout.ObjectField("Img Reward: ", PanelReward.ImgReward, typeof(Image), true) as Image;
                            GUILayout.EndVertical();
                            GUILayout.Space(10);

                            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));

                            GUILayout.Label("Random Reward");
                            GUILayout.Space(10);

                            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                            if (GUILayout.Button("Add Random Reward"))
                            {
                                Gamec.ListRandomRewardedAD.Add(new D3ADSRandomReward());
                                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                            }
                            if (Gamec.ListRandomRewardedAD.Count > 0)
                            {
                                GUILayout.Space(10);
                                if (GUILayout.Button("Remove Random Reward"))
                                {
                                    Gamec.ListRandomRewardedAD.RemoveAt(Gamec.ListRandomRewardedAD.Count - 1);
                                    EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                                }
                            }
                            GUILayout.EndVertical();


                            if (Gamec.ListRandomRewardedAD.Count > 0)
                            {
                                for (int i = 0; i < Gamec.ListRandomRewardedAD.Count; i++)
                                {
                                    if (Gamec.ListRandomRewardedAD[i] != null)
                                    {
                                        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                                        GUILayout.Label("AD Reward ID: " + i);

                                        Gamec.ListRandomRewardedAD[i].IdButton = i;
                                        Gamec.ListRandomRewardedAD[i].TypeReward = (D3TypeRandomReward)EditorGUILayout.EnumPopup("Type Reward: ", Gamec.ListRandomRewardedAD[i].TypeReward);

                                        if (Gamec.ListRandomRewardedAD[i].TypeReward == D3TypeRandomReward.Life || Gamec.ListRandomRewardedAD[i].TypeReward == D3TypeRandomReward.Coin || Gamec.ListRandomRewardedAD[i].TypeReward == D3TypeRandomReward.HoverboardKeyUse)
                                        {
                                            Gamec.ListRandomRewardedAD[i].CantReward = EditorGUILayout.IntField("Reward Value: ", Gamec.ListRandomRewardedAD[i].CantReward);
                                        }

                                        if (Gamec.ListRandomRewardedAD[i].TypeReward == D3TypeRandomReward.ItemMultiply || Gamec.ListRandomRewardedAD[i].TypeReward == D3TypeRandomReward.ItemShield || Gamec.ListRandomRewardedAD[i].TypeReward == D3TypeRandomReward.ItemJump || Gamec.ListRandomRewardedAD[i].TypeReward == D3TypeRandomReward.ItemMagnet)
                                        {
                                            Gamec.ListRandomRewardedAD[i].duration = EditorGUILayout.FloatField("Duration Item: ", Gamec.ListRandomRewardedAD[i].duration);
                                        }
                                        Gamec.ListRandomRewardedAD[i].ImgReward = EditorGUILayout.ObjectField("Img Reward: ", Gamec.ListRandomRewardedAD[i].ImgReward, typeof(Sprite), true) as Sprite;

                                        GUILayout.EndVertical();
                                    }
                                }
                                if (Gamec.ListRandomRewardedAD.Count > 0)
                                {
                                    if (GUILayout.Button("Remove Random Reward"))
                                    {
                                        Gamec.ListRandomRewardedAD.RemoveAt(Gamec.ListRandomRewardedAD.Count - 1);
                                        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                                    }
                                }
                            }
                            GUILayout.EndVertical();

                        }
                        else {
                            OnEnable();
                        }

                        GUILayout.Space(10);

                        GUILayout.Label("Rewarded Events");

                        SerializedProperty prop5 = SerializedADSManager.FindProperty("ShowCompleteUnityRewardedVideo");
                        if (prop5 != null)
                            EditorGUILayout.PropertyField(prop5, true);
                        GUILayout.Space(10f);

                        SerializedProperty prop6 = SerializedADSManager.FindProperty("SkippedUnityRewardedVideo");
                        if (prop6 != null)
                            EditorGUILayout.PropertyField(prop6, true);
                        GUILayout.Space(10f);

                    }

                    GUILayout.EndVertical();



                    GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                    GUILayout.Space(10);
                    GUILayout.Label("Interstitial ADS");
                    GUILayout.Space(10f);
                    Gamec.EnableInterstitialADSOnGameOver = GUILayout.Toggle(Gamec.EnableInterstitialADSOnGameOver, " Use Interstitials in Game Over");
                    GUILayout.Space(10f);
                    EditorGUILayout.TextArea("Enable interstitial ads when the player loses the game", GUI.skin.GetStyle("HelpBox"));

                    GUILayout.Space(10f);

                    Gamec.EnableInterstitialADSOnWim = GUILayout.Toggle(Gamec.EnableInterstitialADSOnWim, " Use Interstitials in Win Game");
                    GUILayout.Space(10f);
                    EditorGUILayout.TextArea("Enable interstitial ads when the player wins the game", GUI.skin.GetStyle("HelpBox"));
                    GUILayout.Space(10f);

                    if (Gamec.EnableInterstitialADSOnGameOver || Gamec.EnableInterstitialADSOnWim)
                    {

                        GUILayout.Label("Interstitial Events");

                        GUILayout.Space(10);

                        SerializedProperty prop7 = SerializedADSManager.FindProperty("ShowCompleteUnityInterstitial");
                        if (prop7 != null)
                            EditorGUILayout.PropertyField(prop7, true);
                        GUILayout.Space(10f);

                        SerializedProperty prop8 = SerializedADSManager.FindProperty("SkippedUnityinterstitial");
                        if (prop8 != null)
                            EditorGUILayout.PropertyField(prop8, true);
                        GUILayout.Space(10f);

                    }

                    GUILayout.EndVertical();


                    SerializedADSManager.ApplyModifiedProperties();
                    GameCS.ApplyModifiedProperties();

                    EditorGUILayout.EndScrollView();
                }

                GUILayout.Space(10);



#endif

                GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                if (GUILayout.Button("Disable Unity ADS"))
                {
                    AdsManager.EnableUnityADS = false;
                    Gamec.EnableInterstitialADSOnWim = false;
                    Gamec.EnableInterstitialADSOnGameOver = false;
                    Gamec.EnableRewardedADOnScene = false;
                    Gamec.UseBanner = false;
                    Gamec.EnableADSOnScene = false;

#if UNITY_ADS && ENABLE_UNITY_ADS
                    const int width = 600;
                    const int height = 150;

                    var x = (Screen.currentResolution.width - width) / 2;
                    var y = (Screen.currentResolution.height - height) / 2;

                    WindowPackageUnityInstallGame window = (WindowPackageUnityInstallGame)EditorWindow.GetWindow(typeof(WindowPackageUnityInstallGame), false, "Uninstall Unity Ads Package");
                    window.position = new Rect(x, y, width, height);
                    window.CodePackage = 1;
                    window.packageToImport = "com.unity.ads";
                    window.Install = false;
                    window.Show();
#endif

                }
                GUILayout.EndVertical();
            }

            if (!AdsManager.EnableUnityADS)
            {
                GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));

                if (GUILayout.Button("Enable Unity ADS"))
                {
                    AdsManager.EnableUnityADS = true;
                }

                GUILayout.EndVertical();
            }



            GUILayout.EndVertical();

        }
        
        if (EditorGUI.EndChangeCheck())
        {
            if (!Application.isPlaying)
            {
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }
#endif
#if !UNITY_ANDROID && !UNITY_IOS
        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
        GUILayout.Space(10f);
        EditorGUILayout.TextArea("ADS Only Available for Android and IOS", GUI.skin.GetStyle("HelpBox"));
        GUILayout.Space(10f);
        GUILayout.EndVertical();

#endif
    }

    void InstantiateObjectInGame()
    {
        const int width = 780;
        const int height = 990;

        var x = (Screen.currentResolution.width - width) / 2;
        var y = (Screen.currentResolution.height - height) / 2;

        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));

        EditorGUILayout.TextArea("To prevent items from not appearing or being instantiated incorrectly, they are only instantiated in the main Pattern System, not in Rocket Mode or Special Item... The distance counter for instantiating items is paused when the player is in Rocket Mode or Special Item, this way you can be sure that the items will be seen by the player.", GUI.skin.GetStyle("HelpBox"));

        EditorGUILayout.TextArea("Even the user can avoid these objects, if a Rocket or a special Item appears just before.", GUI.skin.GetStyle("HelpBox"));

        EditorGUILayout.TextArea("This Setting is only valid for this Scene", GUI.skin.GetStyle("HelpBox"));


        GUILayout.EndVertical();

        EditorGUI.BeginChangeCheck();
        if (!Application.isPlaying)
        {
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            if (GameM)
            {
                scrollPos10 = EditorGUILayout.BeginScrollView(scrollPos10, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));


                GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                GUILayout.Space(5f);

                GUILayout.Label("System", EditorStyles.boldLabel);
                GUILayout.Space(5f);

                EditorGUILayout.TextArea("Pattern Setup: Before configuring the distribution pattern, first create the items, then add each item to the distribution list.", GUI.skin.GetStyle("HelpBox"));

                if (GUILayout.Button("Modify Distribution Pattern To Instantiate Items"))
                {
                    EditorWindow.GetWindow(typeof(D3InstantiateItemsWindowEditor)).position = new Rect(x, y, width, height);

                }
                GUILayout.EndVertical();


                GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                GUILayout.Space(5f);
                GamePSS.Update();
                GUILayout.Label("List of Items To Instantiate", EditorStyles.boldLabel);
                EditorGUILayout.TextArea("List of Buildings to be distributed", GUI.skin.GetStyle("HelpBox"));
                GUILayout.Space(5f);
                Rect myRect2 = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
                GUI.Box(myRect2, "Drag and drop Item Prefab \n into this box!");

                if (myRect2.Contains(Event.current.mousePosition))
                {
                    if (Event.current.type == EventType.DragUpdated)
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                        Event.current.Use();
                    }
                    else if (Event.current.type == EventType.DragPerform)
                    {
                        for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
                        {

                            GameObject AddGO = DragAndDrop.objectReferences[i] as GameObject;

                            if (AddGO)
                            {
                                GamePS.ObjectToInstantiateObject.Add(AddGO);
                                GamePSS.ApplyModifiedProperties();
                                if (!Application.isPlaying)
                                {
                                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                                }
                            }
                            else
                            {
                                Debug.Log("Invalid Item Prefab");
                            }

                        }
                        Event.current.Use();
                    }
                }
                GUILayout.Space(5);
                GUILayout.Label("Total items added: " + GamePS.ObjectToInstantiateObject.Count);
                if (GamePS.ObjectToInstantiateObject.Count >= 1)
                {
                    GUILayout.Space(5);
                    if (GUILayout.Button("Remove Last Item Prefab"))
                    {
                        GamePS.ObjectToInstantiateObject.RemoveAt(GamePS.ObjectToInstantiateObject.Count - 1);
                        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                    }

                    for (int i = 0; i < GamePS.ObjectToInstantiateObject.Count; i++)
                    {
                        if (GamePS.ObjectToInstantiateObject[i] != null)
                        {
                            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                            GamePS.ObjectToInstantiateObject[i] = EditorGUILayout.ObjectField("Item Prefab: ", GamePS.ObjectToInstantiateObject[i], typeof(GameObject), true) as GameObject;
                            GUILayout.EndVertical();
                        }
                    }
                }



                GamePSS.ApplyModifiedProperties();
                GUILayout.EndVertical();
                EditorGUILayout.EndScrollView();
            }
            GUILayout.Space(5f);
            GUILayout.EndVertical();
        }
        else
        {

            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);

            GUILayout.Label("Not available in Play Mode", EditorStyles.boldLabel);

            GUILayout.Space(10f);
            GUILayout.EndVertical();

        }
        if (EditorGUI.EndChangeCheck())
        {
            if (!Application.isPlaying)
            {
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }
    }

    void InputManager()
    {
        EditorGUI.BeginChangeCheck();
        if (!Application.isPlaying)
        {
            if (GameGui)
            {
                GameGuiSe.Update();
                GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                GUILayout.Label("Attention: ", EditorStyles.boldLabel);
                EditorGUILayout.TextArea("This configuration is valid for all Scenes, (Global Configuration)", GUI.skin.GetStyle("HelpBox"));
                GUILayout.EndVertical();

                GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                GUILayout.Space(5f);
                GUILayout.Label("Input Manager", EditorStyles.boldLabel);
                GUILayout.Space(10f);
                
                GameGui.TouchInput = GUILayout.Toggle(GameGui.TouchInput, " Use Touch Input");
                EditorGUILayout.TextArea("Enable or Disable Touch Control", GUI.skin.GetStyle("HelpBox"));
                GUILayout.Space(10f);
                GameGui.KeyInput = GUILayout.Toggle(GameGui.KeyInput, " Use Key Input");
                EditorGUILayout.TextArea("Enable or Disable Control by Keyboard or Controller", GUI.skin.GetStyle("HelpBox"));
                GUILayout.Space(10f);
                if (GameGui.KeyInput)
                {
                    GameGui.KeyCodeUP = (KeyCode)EditorGUILayout.EnumPopup("Key Up:", GameGui.KeyCodeUP);
                    GUILayout.Space(10f);
                    GameGui.KeyCodeLeft = (KeyCode)EditorGUILayout.EnumPopup("Key Left:", GameGui.KeyCodeLeft);
                    GUILayout.Space(10f);
                    GameGui.KeyCodeRight = (KeyCode)EditorGUILayout.EnumPopup("Key Right:", GameGui.KeyCodeRight);
                    GUILayout.Space(10f);
                    GameGui.KeyCodeDown = (KeyCode)EditorGUILayout.EnumPopup("Key Down:", GameGui.KeyCodeDown);

                }
                else
                {
                    GUILayout.Label("Key Input Disabled", EditorStyles.boldLabel);
                }
                GUILayout.Space(10f);
                GUILayout.EndVertical();

                GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                GUILayout.Label("Auto Run", EditorStyles.boldLabel);
                GUILayout.Space(10f);

                GameGui.AutoRun = GUILayout.Toggle(GameGui.AutoRun, "Auto Run On Game Start");
                EditorGUILayout.TextArea("Enable or Disable Auto Runner on Game Start", GUI.skin.GetStyle("HelpBox"));
                
                GUILayout.Space(10f);

                if (!GameGui.AutoRun)
                {
                    GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                    GUILayout.Label("Auto Run Disable", EditorStyles.boldLabel);
                    EditorGUILayout.TextArea("Note: This option sets the player's initial Y position to 0, to prevent the player from falling with the animation stopped.", GUI.skin.GetStyle("HelpBox"));
                    Gamec.posStart.y = 0;

                    GUILayout.Space(10f);
                    GameGui.TouchInputContinueRun = GUILayout.Toggle(GameGui.TouchInputContinueRun, " Use Touch Input To Init Run");
                    EditorGUILayout.TextArea("Enable or Disable Touch Input To Init Run", GUI.skin.GetStyle("HelpBox"));
                    GUILayout.Space(10f);

                    GameGui.KeyInputContinueRun = GUILayout.Toggle(GameGui.KeyInputContinueRun, " Use Key Input To Init Run");
                    EditorGUILayout.TextArea("Enable or Disable Key Input To Init Run", GUI.skin.GetStyle("HelpBox"));
                    GUILayout.Space(10f);
                    if (GameGui.KeyInputContinueRun)
                    {
                        GameGui.KeyCodeContinueRun = (KeyCode)EditorGUILayout.EnumPopup("Key To Init Run:", GameGui.KeyCodeContinueRun);
                    }
                    GUILayout.Space(10f);

                    EditorGUILayout.TextArea("Optional: You can leave empty, Press to run information image.", GUI.skin.GetStyle("HelpBox"));
                    GameGui.ImgInfoToRun = EditorGUILayout.ObjectField("Img Info To Run: ", GameGui.ImgInfoToRun, typeof(GameObject), true) as GameObject;
                    GUILayout.Space(10f);

                    GUILayout.EndVertical();
                }
                GUILayout.Space(10f);
                GUILayout.EndVertical();

                GameGuiSe.ApplyModifiedProperties();
            }
            else
            {
                GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                GUILayout.Space(10f);

                GUILayout.Label("UI manager not found", EditorStyles.boldLabel);

                GUILayout.Space(10f);
                GUILayout.EndVertical();

            }
        }
        else {

            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);

            GUILayout.Label("Not available in Play Mode", EditorStyles.boldLabel);

            GUILayout.Space(10f);
            GUILayout.EndVertical();

        }
        if (EditorGUI.EndChangeCheck())
        {
            if (!Application.isPlaying)
            {
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                EditorUtility.SetDirty(GameGui);
                PrefabUtility.RecordPrefabInstancePropertyModifications(GameGui);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

        }
    }

}

    class WindowPackageUnityInstallGame : EditorWindow
{
    private static AddRequest Request;
    public bool Install = true;
    public string packageToImport;
    public int CodePackage = 0;
    void OnGUI()
    {
        GUILayout.BeginVertical("GroupBox");
       
        if (Install)
        {
            GUILayout.Space(10f);

            GUILayout.Label("Attention, after installing the SDK, restart Unity so that the changes are detected.", EditorStyles.boldLabel);

            GUILayout.Space(10f);

            if (GUILayout.Button("Import Unity Package"))
            {
                DownloadUnity();
            }
        }
        if (!Install)
        {
            GUILayout.Space(10f);

            GUILayout.Label("Attention, after Uninstall Unity SDK, restart Unity so that the changes are detected.", EditorStyles.boldLabel);

            GUILayout.Space(10f);

            if (GUILayout.Button("Disable and Uninstall Unity Package"))
            {
                RemoveADSUnity();
            }
        }
        

        GUILayout.Space(20f);

        GUILayout.EndVertical();
    }
    private void DownloadUnity()
    {
        if (CodePackage == 1)
        {
            Debug.Log("Unity Ads installation started. Please wait");
        }

        Request = Client.Add(packageToImport);
        EditorApplication.update += Progress;
    }
    private void Progress()
    {
        if (Request.IsCompleted)
        {
            if (Request.Status == StatusCode.Success)
            {
                Debug.Log("Installed: " + Request.Result.packageId);
                if (CodePackage == 1)
                {
                    Debug.LogWarning("<b>UNITY_ADS</b> ADDED TO Scripting Define Symbols in <b>Player Settings</b>");

                    AddToPlatform("UNITY_ADS");
                }
                if (CodePackage == 2)
                {
                    Debug.LogWarning("<b>UNITY_In_APP</b> ADDED TO Scripting Define Symbols in <b>Player Settings</b>");

                    AddToPlatform("UNITY_In_APP");
                }

                Debug.Log("Please restart Unity to apply the changes.");
                EditorUtility.SetDirty(this);
            }
            else if (Request.Status >= StatusCode.Failure)
                Debug.Log(Request.Error.message);
            EditorApplication.update -= Progress;
        }
    }
    private void RemoveADSUnity()
    {
        Debug.Log("Unity Package Uninstallation started. Please wait");

        FileUtil.DeleteFileOrDirectory("Assets/Scripts/UnityPurchasing/generated");
        FileUtil.DeleteFileOrDirectory("Assets/Scripts/UnityPurchasing");
        
        AssetDatabase.Refresh();

        Client.Remove(packageToImport);

        if (CodePackage == 1)
        {
            AddToPlatform("UNITY_ADS");
        }
        if (CodePackage == 2)
        {
            AddToPlatform("UNITY_In_APP");
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
    }
    private void AddToPlatform(string directive)
    {
#if UNITY_2023_1_OR_NEWER
            string textToWriteAndroid = PlayerSettings.GetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Android);
            string textToWriteiOS = PlayerSettings.GetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.iOS);
#else
        string textToWriteAndroid = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
        string textToWriteiOS = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS);
#endif
        if (!Install)
        {
            if (textToWriteAndroid.Contains(directive))
            {
                textToWriteAndroid = textToWriteAndroid.Replace(directive, "");
            }
            if (textToWriteiOS.Contains(directive))
            {
                textToWriteiOS = textToWriteiOS.Replace(directive, "");
            }
        }
        else
        {
            if (directive != "UNITY_ADS" && !textToWriteAndroid.Contains(directive))
            {
                if (textToWriteAndroid == "")
                {
                    textToWriteAndroid += directive;
                }
                else
                {
                    textToWriteAndroid += "," + directive;
                }
            }

            if (directive != "UNITY_In_APP" && directive != "UNITY_ADS" && !textToWriteiOS.Contains(directive))
            {
                if (textToWriteiOS == "")
                {
                    textToWriteiOS += directive;
                }
                else
                {
                    textToWriteiOS += "," + directive;
                }
            }

        }
#if UNITY_2023_1_OR_NEWER
        PlayerSettings.SetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.FromBuildTargetGroup(BuildTargetGroup.Android), textToWriteAndroid);
        PlayerSettings.SetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.FromBuildTargetGroup(BuildTargetGroup.iOS), textToWriteiOS);
#else
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, textToWriteAndroid);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, textToWriteiOS);
#endif
    }

}



