using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(D3GUIManager))]
public class D3GuiManagerEditor : Editor
{
    Vector2 scrollPos;

    D3GUIManager GuiComponent;
    D3SoundManager GameS;

    void OnEnable()
    {
        GuiComponent = target as D3GUIManager;
        GameS = FindObjectOfType<D3SoundManager>();

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
        GUILayout.Label("This Script is Controlled by Infinity Runner Engine.\nTo Edit Go to Unity Menu:\nDenvzla Estudio/3D Infinity Runner Engine/Welcome Window", EditorStyles.boldLabel);
        GUILayout.Space(10f);
        GUILayout.EndVertical();

        if (GuiComponent)
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
            GuiComponent.SoundManager = EditorGUILayout.ObjectField("Sound Manager: ", GuiComponent.SoundManager, typeof(D3SoundManager), true) as D3SoundManager;
            if (GameS != null && GuiComponent.SoundManager == null)
            {
                GuiComponent.SoundManager = GameS;
            }
            GUILayout.Space(10f);
            GuiComponent.musicVolumeSlider = EditorGUILayout.ObjectField("Music Volume Slider: ", GuiComponent.musicVolumeSlider, typeof(Slider), true) as Slider;
            GUILayout.Space(10f);
            GuiComponent.SFXVolumeSlider = EditorGUILayout.ObjectField("SFX Volume Slider: ", GuiComponent.SFXVolumeSlider, typeof(Slider), true) as Slider;
            GUILayout.Space(10f);
            GUILayout.EndVertical();

            GUILayout.BeginVertical("GroupBox");
            GUILayout.Label("Windows: ", EditorStyles.boldLabel);
            GUILayout.Space(10f);
            GuiComponent.PauseGui = EditorGUILayout.ObjectField("Pause: ", GuiComponent.PauseGui, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GuiComponent.GameOverGui = EditorGUILayout.ObjectField("GameOver: ", GuiComponent.GameOverGui, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GuiComponent.InGameGui = EditorGUILayout.ObjectField("In Game: ", GuiComponent.InGameGui, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GuiComponent.SettingsGUI = EditorGUILayout.ObjectField("Settings: ", GuiComponent.SettingsGUI, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GuiComponent.BestScoreGUI = EditorGUILayout.ObjectField("Best Score: ", GuiComponent.BestScoreGUI, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GuiComponent.HeroGui = EditorGUILayout.ObjectField("Panel Hero: ", GuiComponent.HeroGui, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GuiComponent.CoinTObj = EditorGUILayout.ObjectField("Panel Coin: ", GuiComponent.CoinTObj, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GuiComponent.LifeTObj = EditorGUILayout.ObjectField("Panel Life: ", GuiComponent.LifeTObj, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GuiComponent.ButtonToNewLevel = EditorGUILayout.ObjectField("Button To New Level: ", GuiComponent.ButtonToNewLevel, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GuiComponent.ButtonRevival = EditorGUILayout.ObjectField("Button Revival: ", GuiComponent.ButtonRevival, typeof(Button), true) as Button;
            GUILayout.Space(10f);
            GuiComponent.ButtonHoverBoard = EditorGUILayout.ObjectField("Button HoverBoard: ", GuiComponent.ButtonHoverBoard, typeof(Button), true) as Button;
            GUILayout.Space(10f);
            GuiComponent.RewardWindow = EditorGUILayout.ObjectField("Reward Window: ", GuiComponent.RewardWindow, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GuiComponent.PanelRandomReward = EditorGUILayout.ObjectField("Panel Random Reward: ", GuiComponent.PanelRandomReward, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GUILayout.EndVertical();


            GUILayout.BeginVertical("GroupBox");
            GUILayout.Space(10f);
            GUILayout.Label("Text Objects: ", EditorStyles.boldLabel);
            GUILayout.Space(10f);

            GuiComponent.PauseScore = EditorGUILayout.ObjectField("(Pause) Score: ", GuiComponent.PauseScore, typeof(Text), true) as Text;
            GUILayout.Space(10f);
            GuiComponent.PauseToWinScore = EditorGUILayout.ObjectField("(Pause) To Win Score: ", GuiComponent.PauseToWinScore, typeof(Text), true) as Text;
            GUILayout.Space(10f);
            GuiComponent.PauseBestScore = EditorGUILayout.ObjectField("(Pause) Best Score: ", GuiComponent.PauseBestScore, typeof(Text), true) as Text;
            GUILayout.Space(10f);
            GuiComponent.PauseLevelText = EditorGUILayout.ObjectField("(Pause) Level Text: ", GuiComponent.PauseLevelText, typeof(Text), true) as Text;
            GUILayout.Space(20f);

            GuiComponent.GameOverScore = EditorGUILayout.ObjectField("(GameOver) Score: ", GuiComponent.GameOverScore, typeof(Text), true) as Text;
            GUILayout.Space(10f);
            GuiComponent.GameToWinScore = EditorGUILayout.ObjectField("(GameOver) To Win Score: ", GuiComponent.GameToWinScore, typeof(Text), true) as Text;
            GUILayout.Space(10f);
            GuiComponent.GameOverBestScore = EditorGUILayout.ObjectField("(GameOver) Best Score: ", GuiComponent.GameOverBestScore, typeof(Text), true) as Text;
            GUILayout.Space(10f);
            GuiComponent.GameOverLevelText = EditorGUILayout.ObjectField("(GameOver) Level Text: ", GuiComponent.GameOverLevelText, typeof(Text), true) as Text;
            GUILayout.Space(20f);

            GuiComponent.TextCoinCollect = EditorGUILayout.ObjectField("(InGame) Coin Collect: ", GuiComponent.TextCoinCollect, typeof(Text), true) as Text;
            GUILayout.Space(10f);
            GuiComponent.Textdistance = EditorGUILayout.ObjectField("(InGame) Distance: ", GuiComponent.Textdistance, typeof(Text), true) as Text;
            GUILayout.Space(10f);
            GuiComponent.CantHoverBoard = EditorGUILayout.ObjectField("(InGame) Cant HoverBoard: ", GuiComponent.CantHoverBoard, typeof(Text), true) as Text;
            GUILayout.Space(10f);
            GuiComponent.LevelText = EditorGUILayout.ObjectField("(InGame) Level Text: ", GuiComponent.LevelText, typeof(Text), true) as Text;
            GUILayout.Space(10f);
            GuiComponent.ImgInfoToRun = EditorGUILayout.ObjectField("(InGame) Img Info To Run: ", GuiComponent.ImgInfoToRun, typeof(GameObject), true) as GameObject;
            GUILayout.Space(20f);


            GuiComponent.BestScoreText = EditorGUILayout.ObjectField("(BestScore) Best Score: ", GuiComponent.BestScoreText, typeof(Text), true) as Text;
            GUILayout.Space(20f);

            GuiComponent.WinScore = EditorGUILayout.ObjectField("(Win) Score: ", GuiComponent.WinScore, typeof(Text), true) as Text;
            GUILayout.Space(10f);
            GuiComponent.WinDistance = EditorGUILayout.ObjectField("(Win) Text Distance: ", GuiComponent.WinDistance, typeof(Text), true) as Text;
            GUILayout.Space(10f);
            GuiComponent.WinTextBestScore = EditorGUILayout.ObjectField("(Win) Best Score: ", GuiComponent.WinTextBestScore, typeof(Text), true) as Text;
            GUILayout.Space(10f);
            GuiComponent.WinLevelText = EditorGUILayout.ObjectField("(Win) Level Text: ", GuiComponent.WinLevelText, typeof(Text), true) as Text;

            GUILayout.Space(20f);


            GUILayout.EndVertical();



            GUILayout.BeginVertical("GroupBox");
            GUILayout.Space(10f);

            GUILayout.Label("Information Text: ", EditorStyles.boldLabel);
            GUILayout.Space(10f);

            GuiComponent.TextScore = EditorGUILayout.TextField("Text Score: ", GuiComponent.TextScore);
            GUILayout.Space(10f);
            GuiComponent.TextToWinInfo = EditorGUILayout.TextField("Text To Win: ", GuiComponent.TextToWinInfo);
            GUILayout.Space(10f);
            GuiComponent.TextBestScore = EditorGUILayout.TextField("Text Best Score: ", GuiComponent.TextBestScore);
            GUILayout.Space(10f);

            GUILayout.EndVertical();


            GUILayout.BeginVertical("GroupBox");
            GUILayout.Space(10f);

            GUILayout.Label("Loading Bar: ", EditorStyles.boldLabel);
            GUILayout.Space(10f);

            GuiComponent.BGLoading = EditorGUILayout.ObjectField("BG Loading: ", GuiComponent.BGLoading, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GuiComponent.LoadingBar = EditorGUILayout.ObjectField("Loading Bar: ", GuiComponent.LoadingBar, typeof(Slider), true) as Slider;
            GUILayout.Space(10f);
            GuiComponent.LoadingBarText = EditorGUILayout.ObjectField("Loading Bar Text: ", GuiComponent.LoadingBarText, typeof(Text), true) as Text;
            GUILayout.Space(10f);
            GuiComponent.PanelFade = EditorGUILayout.ObjectField("Panel Fade: ", GuiComponent.PanelFade, typeof(GameObject), true) as GameObject;
            GUILayout.Space(20f);

            GUILayout.EndVertical();


            GUILayout.BeginVertical("GroupBox");
            GUILayout.Space(10f);

            GUILayout.Label("Start System: ", EditorStyles.boldLabel);
            GUILayout.Space(10f);
            GuiComponent.StartWinON1 = EditorGUILayout.ObjectField("Start Win ON 1: ", GuiComponent.StartWinON1, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GuiComponent.StartWinOFF1 = EditorGUILayout.ObjectField("Start Win OFF 1: ", GuiComponent.StartWinOFF1, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GuiComponent.StartOFF1 = EditorGUILayout.ObjectField("Start OFF 1: ", GuiComponent.StartOFF1, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GuiComponent.StartON1 = EditorGUILayout.ObjectField("Start ON 1: ", GuiComponent.StartON1, typeof(GameObject), true) as GameObject;
            GUILayout.Space(20f);

            GuiComponent.StartWinON2 = EditorGUILayout.ObjectField("Start Win ON 2: ", GuiComponent.StartWinON2, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GuiComponent.StartWinOFF2 = EditorGUILayout.ObjectField("Start Win OFF 2: ", GuiComponent.StartWinOFF2, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GuiComponent.StartOFF2 = EditorGUILayout.ObjectField("Start OFF 2: ", GuiComponent.StartOFF2, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GuiComponent.StartON2 = EditorGUILayout.ObjectField("Start ON 2: ", GuiComponent.StartON2, typeof(GameObject), true) as GameObject;
            GUILayout.Space(20f);

            GuiComponent.StartWinON3 = EditorGUILayout.ObjectField("Start Win ON 3: ", GuiComponent.StartWinON3, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GuiComponent.StartWinOFF3 = EditorGUILayout.ObjectField("Start Win OFF 3: ", GuiComponent.StartWinOFF3, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GuiComponent.StartOFF3 = EditorGUILayout.ObjectField("Start OFF 3: ", GuiComponent.StartOFF3, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GuiComponent.StartON3 = EditorGUILayout.ObjectField("Start ON 3: ", GuiComponent.StartON3, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);

            GUILayout.EndVertical();

            GUILayout.Space(10f);
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("Basic", style);

            GUILayout.Space(10f);
            GuiComponent.NamePlayerText = EditorGUILayout.ObjectField("Player Name Text: ", GuiComponent.NamePlayerText, typeof(Text), true) as Text;

            GUILayout.Space(10f);
            GuiComponent.ImageButtonBuy = EditorGUILayout.ObjectField("Image Button Buy: ", GuiComponent.ImageButtonBuy, typeof(Sprite), true) as Sprite;

            GUILayout.Space(10f);
            GuiComponent.ImageButtonNoCoin = EditorGUILayout.ObjectField("Image Button No Coin: ", GuiComponent.ImageButtonNoCoin, typeof(Sprite), true) as Sprite;

            GUILayout.Space(10f);
            GuiComponent.BuyText = EditorGUILayout.TextField("Buy Text: ", GuiComponent.BuyText);

            GUILayout.Space(10f);
            GuiComponent.NOCoinText = EditorGUILayout.TextField("NO Coin Text: ", GuiComponent.NOCoinText);

            GUILayout.Space(10f);
            GuiComponent.MultiplyPriceWhenPurchasing = EditorGUILayout.IntField("Cant Multiply Price When Purchasing :", GuiComponent.MultiplyPriceWhenPurchasing);

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
            GuiComponent.PriceTextSprintTime = EditorGUILayout.ObjectField("Object Text Price: ", GuiComponent.PriceTextSprintTime, typeof(Text), true) as Text;
            GUILayout.Space(10f);
            GuiComponent.ScrollbarAddTimeSprintTime = EditorGUILayout.ObjectField("Object Slider: ", GuiComponent.ScrollbarAddTimeSprintTime, typeof(Slider), true) as Slider;
            GUILayout.Space(10f);
            GuiComponent.TextAddTimeSprintTime = EditorGUILayout.ObjectField("Object Text Add Time: ", GuiComponent.TextAddTimeSprintTime, typeof(TextMeshProUGUI), true) as TextMeshProUGUI;
            GUILayout.Space(10f);
            GuiComponent.ButtonSprintTime = EditorGUILayout.ObjectField("Object Button: ", GuiComponent.ButtonSprintTime, typeof(Button), true) as Button;
            GUILayout.Space(10f);
            GuiComponent.ButtonTextSprintTime = EditorGUILayout.ObjectField("Object Text Button: ", GuiComponent.ButtonTextSprintTime, typeof(Text), true) as Text;
            GUILayout.Space(10f);
            GUILayout.EndVertical();

            GUILayout.Space(10f);
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("Special", style);
            GUILayout.Space(10f);
            GuiComponent.PriceTextSpecialTime = EditorGUILayout.ObjectField("Object Text Price: ", GuiComponent.PriceTextSpecialTime, typeof(Text), true) as Text;
            GUILayout.Space(10f);
            GuiComponent.ScrollbarAddTimeSpecialTime = EditorGUILayout.ObjectField("Object Slider: ", GuiComponent.ScrollbarAddTimeSpecialTime, typeof(Slider), true) as Slider;
            GUILayout.Space(10f);
            GuiComponent.TextAddTimeSpecialTime = EditorGUILayout.ObjectField("Object Text Add Time: ", GuiComponent.TextAddTimeSpecialTime, typeof(TextMeshProUGUI), true) as TextMeshProUGUI;
            GUILayout.Space(10f);
            GuiComponent.ButtonSpecialTime = EditorGUILayout.ObjectField("Object Button: ", GuiComponent.ButtonSpecialTime, typeof(Button), true) as Button;
            GUILayout.Space(10f);
            GuiComponent.ButtonTextSpecialTime = EditorGUILayout.ObjectField("Object Text Button: ", GuiComponent.ButtonTextSpecialTime, typeof(Text), true) as Text;
            GUILayout.Space(10f);
            GUILayout.EndVertical();

            GUILayout.Space(10f);
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("Multiply", style);
            GUILayout.Space(10f);
            GuiComponent.PriceTextMultiplyTime = EditorGUILayout.ObjectField("Object Text Price: ", GuiComponent.PriceTextMultiplyTime, typeof(Text), true) as Text;
            GUILayout.Space(10f);
            GuiComponent.ScrollbarAddTimeMultiplyTime = EditorGUILayout.ObjectField("Object Slider: ", GuiComponent.ScrollbarAddTimeMultiplyTime, typeof(Slider), true) as Slider;
            GUILayout.Space(10f);
            GuiComponent.TextAddTimeMultiplyTime = EditorGUILayout.ObjectField("Object Text Add Time: ", GuiComponent.TextAddTimeMultiplyTime, typeof(TextMeshProUGUI), true) as TextMeshProUGUI;
            GUILayout.Space(10f);
            GuiComponent.ButtonMultiplyTime = EditorGUILayout.ObjectField("Object Button: ", GuiComponent.ButtonMultiplyTime, typeof(Button), true) as Button;
            GUILayout.Space(10f);
            GuiComponent.ButtonTextMultiplyTime = EditorGUILayout.ObjectField("Object Text Button: ", GuiComponent.ButtonTextMultiplyTime, typeof(Text), true) as Text;
            GUILayout.Space(10f);
            GUILayout.EndVertical();

            GUILayout.Space(10f);
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("Magnet", style);
            GUILayout.Space(10f);
            GuiComponent.PriceTextMagnetTime = EditorGUILayout.ObjectField("Object Text Price: ", GuiComponent.PriceTextMagnetTime, typeof(Text), true) as Text;
            GUILayout.Space(10f);
            GuiComponent.ScrollbarAddTimeMagnetTime = EditorGUILayout.ObjectField("Object Slider: ", GuiComponent.ScrollbarAddTimeMagnetTime, typeof(Slider), true) as Slider;
            GUILayout.Space(10f);
            GuiComponent.TextAddTimeMagnetTime = EditorGUILayout.ObjectField("Object Text Add Time: ", GuiComponent.TextAddTimeMagnetTime, typeof(TextMeshProUGUI), true) as TextMeshProUGUI;
            GUILayout.Space(10f);
            GuiComponent.ButtonMagnetTime = EditorGUILayout.ObjectField("Object Button: ", GuiComponent.ButtonMagnetTime, typeof(Button), true) as Button;
            GUILayout.Space(10f);
            GuiComponent.ButtonTextMagnetTime = EditorGUILayout.ObjectField("Object Text Button: ", GuiComponent.ButtonTextMagnetTime, typeof(Text), true) as Text;
            GUILayout.Space(10f);
            GUILayout.EndVertical();

            GUILayout.Space(10f);
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("Shield", style);
            GUILayout.Space(10f);
            GuiComponent.PriceTextShieldTime = EditorGUILayout.ObjectField("Object Text Price: ", GuiComponent.PriceTextShieldTime, typeof(Text), true) as Text;
            GUILayout.Space(10f);
            GuiComponent.ScrollbarAddTimeShieldTime = EditorGUILayout.ObjectField("Object Slider: ", GuiComponent.ScrollbarAddTimeShieldTime, typeof(Slider), true) as Slider;
            GUILayout.Space(10f);
            GuiComponent.TextAddTimeShieldTime = EditorGUILayout.ObjectField("Object Text Add Time: ", GuiComponent.TextAddTimeShieldTime, typeof(TextMeshProUGUI), true) as TextMeshProUGUI;
            GUILayout.Space(10f);
            GuiComponent.ButtonShieldTime = EditorGUILayout.ObjectField("Object Button: ", GuiComponent.ButtonShieldTime, typeof(Button), true) as Button;
            GUILayout.Space(10f);
            GuiComponent.ButtonTextShieldTime = EditorGUILayout.ObjectField("Object Text Button: ", GuiComponent.ButtonTextShieldTime, typeof(Text), true) as Text;
            GUILayout.Space(10f);
            GUILayout.EndVertical();

            GUILayout.EndVertical();


            GUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            if (EditorGUI.EndChangeCheck())
            {
                if (!Application.isPlaying)
                {
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                    EditorUtility.SetDirty(GuiComponent);
                    PrefabUtility.RecordPrefabInstancePropertyModifications(GuiComponent);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }

            }
        }

    }
}
