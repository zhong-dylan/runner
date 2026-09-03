using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(D3TitleCharacter))]

public class D3TitleCharacterEditor : Editor
{

    Vector2 scrollPos;

    D3TitleCharacter TitleSceneTC;

    void OnEnable()
    {
        TitleSceneTC = target as D3TitleCharacter;

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


        if (TitleSceneTC)
        {
            GUILayout.Label("UI Mánager", EditorStyles.boldLabel);

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.BeginVertical("GroupBox");

            GUILayout.Space(10f);
            GUILayout.Label("Components necessary to function: \n(check here that everything is assigned).", EditorStyles.boldLabel);
            GUILayout.Space(10f);

            EditorGUILayout.TextArea("It is recommended not to leave any field empty, if you are not going to use an element, just deactivate it.", GUI.skin.GetStyle("HelpBox"));
            GUILayout.Space(10f);


            GUILayout.BeginVertical("GroupBox");
            GUILayout.Space(10f);


            TitleSceneTC.Scene = EditorGUILayout.ObjectField("Scene: ", TitleSceneTC.Scene, typeof(SceneAsset), true) as SceneAsset;

            if (TitleSceneTC.Scene != null)
            {
                TitleSceneTC.GameScene = TitleSceneTC.Scene.name;
            }
            

            GUILayout.Space(10f);

            TitleSceneTC.GameVersionInfo = EditorGUILayout.TextField("Game Version Info: ", TitleSceneTC.GameVersionInfo);

            GUILayout.Space(10f);

            TitleSceneTC.coinText = EditorGUILayout.ObjectField("Coin Text Object: ", TitleSceneTC.coinText, typeof(Text), true) as Text;

            GUILayout.Space(10f);

            TitleSceneTC.LifeText = EditorGUILayout.ObjectField("Life Text Object: ", TitleSceneTC.LifeText, typeof(Text), true) as Text;

            GUILayout.Space(10f);

            TitleSceneTC.GameVersionText = EditorGUILayout.ObjectField("Game Version Text: ", TitleSceneTC.GameVersionText, typeof(Text), true) as Text;

            GUILayout.Space(10f);

            TitleSceneTC.HoverBoardText = EditorGUILayout.ObjectField("HoverBoard Text  Object: ", TitleSceneTC.HoverBoardText, typeof(Text), true) as Text;

            GUILayout.Space(10f);

            TitleSceneTC.BestScoreText = EditorGUILayout.ObjectField("Best Score Text Object: ", TitleSceneTC.BestScoreText, typeof(Text), true) as Text;

            GUILayout.Space(10f);

            TitleSceneTC.BtnPlay = EditorGUILayout.ObjectField("Button Play: ", TitleSceneTC.BtnPlay, typeof(Button), true) as Button;

            GUILayout.Space(10f);

            TitleSceneTC.BtnShop = EditorGUILayout.ObjectField("Button Shop: ", TitleSceneTC.BtnShop, typeof(Button), true) as Button;
           
            GUILayout.Space(10f);

            TitleSceneTC.TitleGUI = EditorGUILayout.ObjectField("Title Window: ", TitleSceneTC.TitleGUI, typeof(GameObject), true) as GameObject;

            GUILayout.Space(10f);

            TitleSceneTC.TextObject = EditorGUILayout.ObjectField("Texts Objects: ", TitleSceneTC.TextObject, typeof(GameObject), true) as GameObject;

            GUILayout.Space(10f);

            TitleSceneTC.SettingGUI = EditorGUILayout.ObjectField("Setting Window: ", TitleSceneTC.SettingGUI, typeof(GameObject), true) as GameObject;

            GUILayout.Space(10f);

            TitleSceneTC.NoLifeGUI = EditorGUILayout.ObjectField("No Life Window: ", TitleSceneTC.NoLifeGUI, typeof(GameObject), true) as GameObject;

            GUILayout.Space(10f);

            TitleSceneTC.ShopGui = EditorGUILayout.ObjectField("Shop Window: ", TitleSceneTC.ShopGui, typeof(GameObject), true) as GameObject;

            GUILayout.Space(10f);

            TitleSceneTC.HeroController = EditorGUILayout.ObjectField("Hero Window: ", TitleSceneTC.HeroController, typeof(GameObject), true) as GameObject;

            GUILayout.Space(10f);

            TitleSceneTC.RewardWindow = EditorGUILayout.ObjectField("Reward Window: ", TitleSceneTC.RewardWindow, typeof(GameObject), true) as GameObject;

            GUILayout.Space(10f);

            TitleSceneTC.WindowLevelSystem = EditorGUILayout.ObjectField("Level System Window: ", TitleSceneTC.WindowLevelSystem, typeof(D3LevelSystem), true) as D3LevelSystem;

            GUILayout.Space(10f);

            TitleSceneTC.LevelSystemManager = EditorGUILayout.ObjectField("Level System Manager: ", TitleSceneTC.LevelSystemManager, typeof(D3LevelSystemManager), true) as D3LevelSystemManager;

            GUILayout.Space(10f);

            GUILayout.EndVertical();
            

            GUILayout.Space(10f);

            GUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }
        if (EditorGUI.EndChangeCheck())
        {
            if (TitleSceneTC.Scene != null)
            {
                TitleSceneTC.GameScene = TitleSceneTC.Scene.name;
            }
        }

    }


}

