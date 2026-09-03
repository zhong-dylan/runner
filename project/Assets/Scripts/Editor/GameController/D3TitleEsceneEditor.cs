using TMPro;
using UnityEditor;
using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using static D3ADSTypeReward;
using static D3ItemShop;
using static D3SoundManager;

#if UNITY_ADS && ENABLE_UNITY_ADS
using UnityEngine.Advertisements;
#endif

[CustomEditor(typeof(D3TitleScene))]

public class D3TitleEsceneEditor : Editor
{

    //TitleScene
    private string[] m_tabs = { "Game Config", "Level System + Scene Manager", "Audio Manager", "UI Manager", "Shop Manager + In App Purchasing", "ADS Manager", "Contact" };
    private int m_tabsSelected = -1;

    D3TitleScene TitleScene;
    D3TitleCharacter TitleSceneTC;
    SerializedObject SerializedTitleScene, SerializedADSManager, SerializedLevelSystem;
    Vector2 AdsScrollPos, LevelSystenScrollPos;
    D3SoundManager GameS;
    D3ShopCharacter GameShop;
    D3HeroController itemTarget;
    D3ADSManager AdsManager;
    D3RewardWindow RewardWindow;
    SerializedObject GameSS, GameShopS;
    D3LevelSystem WindowLevelSystem;
    D3LevelSystemManager LevelSystem;

    int selected = 0;

    string[] options = new string[]
    {
      "None","Title Window", "Settings Window", "No Life Window","Shop Window","Hero Window","Reward Window",
    };

    int SelectedFilter = 0;

    string[] OptionsFilter = new string[]
    {
      "All Items","Virtual Money", "Real Money", "None",
    };

    void OnEnable()
    {
        TitleScene = target as D3TitleScene;

        if (TitleScene)
        {
            if (FindObjectOfType<D3TitleCharacter>())
            {
                TitleSceneTC = FindObjectOfType<D3TitleCharacter>();
                SerializedTitleScene = new SerializedObject(TitleSceneTC);

                if (TitleSceneTC)
                {
                    if (TitleSceneTC.RewardWindow)
                    {
                        if (TitleSceneTC.RewardWindow.GetComponent<D3RewardWindow>())
                        {
                            RewardWindow = TitleSceneTC.RewardWindow.GetComponent<D3RewardWindow>();
                        } else
                        {
                            Debug.LogWarning("<b>ERROR</b> D3RewardWindow Script not found in the scene, it is recommended to delete the scene and create a new one or review the demo scene and compare the objects.</b>");

                        }
                    }

                    if (TitleSceneTC.HeroController)
                    {
                        if (TitleSceneTC.HeroController.GetComponent<D3HeroController>())
                        {
                            itemTarget = TitleSceneTC.HeroController.GetComponent<D3HeroController>();
                        }else
                        {
                            Debug.LogWarning("<b>ERROR</b> D3HeroController is not installed in this scene, click on the ADS manager tab of 3DRunnerEditor for more information..</b>");
                        }
                    }

                    if (TitleSceneTC.LevelSystemManager)
                    {
                        LevelSystem = TitleSceneTC.LevelSystemManager;
                        SerializedLevelSystem = new SerializedObject(LevelSystem);
                    }
                    if (TitleSceneTC.WindowLevelSystem)
                    { 
                        WindowLevelSystem = TitleSceneTC.WindowLevelSystem;
                    }

                }

            }
            else {
                Debug.LogWarning("<b>ERROR</b> D3TitleCharacter Script not found in the scene, it is recommended to delete the scene and create a new one or review the demo scene and compare the objects.</b>");
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

            if (FindObjectOfType<D3ShopCharacter>())
            {
                GameShop = FindObjectOfType<D3ShopCharacter>();
                GameShopS = new SerializedObject(GameShop);
            }
            else
            {
                Debug.LogWarning("<b>ERROR</b> D3ShopCharacter Script not found in the scene, it is recommended to delete the scene and create a new one or review the demo scene and compare the objects.</b>");
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
        EditorGUI.BeginChangeCheck();
        var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true), GUILayout.Height(170f));
        Texture2D m_Logo = (Texture2D)Resources.Load("Img/D3Icon", typeof(Texture2D));
        GUILayout.Label(m_Logo, style, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        GUILayout.EndVertical();

        if (TitleScene)
        {
            GUILayout.Space(10f);
            if (GUILayout.Button("Online Manual"))
            {
                Application.OpenURL("https://denvzla-estudio.gitbook.io/infinite-runner-engine-3d/");
            }
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));

            GUILayout.Label("Title Scene Editor", style);
            GUILayout.EndVertical();
            if (!Application.isPlaying)
            {
                TabManager();
            }
            else {
                GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));

                GUILayout.Label("Not available in play mode", style);
                GUILayout.EndVertical();
            }
        }
        if (EditorGUI.EndChangeCheck())
        {
            if (!Application.isPlaying)
            {
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
           
        }

    }

    void TabManager()
    {
        EditorGUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
        m_tabsSelected = GUILayout.SelectionGrid(m_tabsSelected, m_tabs, 2);
        EditorGUILayout.EndVertical();
        if (m_tabsSelected >= 0 && m_tabsSelected < m_tabs.Length)
        {
            switch (m_tabs[m_tabsSelected])
            {
                case "Game Config":
                    GameManager();
                    break;
                case "Level System + Scene Manager":
                    LevelSystemManager();
                    break;
                case "Audio Manager":
                    SoundManager();
                    break;
                case "UI Manager":
                    UIManager();
                    break;
                case "Contact":
                    Contact();
                    break;
                case "Shop Manager + In App Purchasing":
                    ShopManager();
                    break;
                case "ADS Manager":
                    ADSManager();
                    break;
            }

        }
    }
    void GameManager()
    {
        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
        EditorGUI.BeginChangeCheck();

        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
        GUILayout.Space(5f);
        GUILayout.Label("Level System", EditorStyles.boldLabel);

        GUILayout.Space(10f);

        EditorGUILayout.TextArea("Life when starting the application for the first time: ", GUI.skin.GetStyle("HelpBox"));

        TitleSceneTC.lifeInitial = EditorGUILayout.IntField("Life for the First Time: ", TitleSceneTC.lifeInitial);
        GUILayout.Space(10f);


        GUILayout.EndVertical();


        if (EditorGUI.EndChangeCheck())
        {
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
        GUILayout.EndVertical();
    }
    void SoundManager()
    {
        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
        EditorGUI.BeginChangeCheck();


        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
        GUILayout.Space(5f);
        GUILayout.Label("Audio System", EditorStyles.boldLabel);
        GUILayout.Space(10f);


        GameS.bgmSound = EditorGUILayout.ObjectField("Audio Source: ", GameS.bgmSound, typeof(AudioSource), true) as AudioSource;
        GUILayout.Space(10f);

        GameS.SFXSound = EditorGUILayout.ObjectField("SFX Source: ", GameS.SFXSound, typeof(AudioSource), true) as AudioSource;
        GUILayout.Space(10f);

        GameS.BGSound = EditorGUILayout.ObjectField("BG Music: ", GameS.BGSound, typeof(AudioClip), true) as AudioClip;

        GUILayout.Space(10f);

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
        if (GameS.sound_List.Count > 1)
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
                        if (GameS.sound_List[i].soundName != "Button")
                        {
                            GameS.sound_List[i].soundName = "Button";
                        }
                    }
                    if (i > 0)
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

        GameSS.ApplyModifiedProperties();
        GUILayout.EndVertical();


        if (EditorGUI.EndChangeCheck())
        {
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
        GUILayout.EndVertical();

    }
    void UIManager()
    {
        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
        EditorGUI.BeginChangeCheck();


        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
        GUILayout.Space(5f);
        GUILayout.Label("UI Manager", EditorStyles.boldLabel);
        GUILayout.Space(10f);

        selected = EditorGUILayout.Popup("Select Window to Edit: ", selected, options);


        switch (selected)
        {
            case 0:
                WindowsValidation();
                break;

            case 1:
                WindowsValidation();
                if(TitleSceneTC.TitleGUI && TitleSceneTC.TextObject) 
                {
                    TitleSceneTC.TitleGUI.SetActive(true);
                    TitleSceneTC.TextObject.SetActive(true);

                    GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                    GUILayout.Label("Title Window", EditorStyles.boldLabel);
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


                    GUILayout.EndVertical();
                }
                
                break;
            case 2:
                WindowsValidation();
                if (TitleSceneTC.SettingGUI)
                {
                    TitleSceneTC.SettingGUI.SetActive(true);
                    if (GameS)
                    {
                        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                        GUILayout.Label("Settings Window", EditorStyles.boldLabel);
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
                    
                }

                break;
            case 3:
                WindowsValidation();
                if (TitleSceneTC.NoLifeGUI)
                {
                    TitleSceneTC.NoLifeGUI.SetActive(true);
                    GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                    GUILayout.Label("No Life Window", EditorStyles.boldLabel);
                    GUILayout.Space(10f);

                    EditorGUILayout.TextArea("This window contains an example script to add life or hoverboard with ads or purchase.", GUI.skin.GetStyle("HelpBox"));

                    GUILayout.EndVertical();
                }

                break;
            case 4:
                WindowsValidation();
                if (TitleSceneTC.ShopGui)
                {

                    TitleSceneTC.ShopGui.SetActive(true);
                    if (GameShop)
                    {
                        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                        GUILayout.Label("Shop Window", EditorStyles.boldLabel);
                        GUILayout.Space(10f);

                        GameShop.m_ContentHero = EditorGUILayout.ObjectField("Content Hero : ", GameShop.m_ContentHero, typeof(Transform), true) as Transform;

                        GUILayout.Space(10f);

                        GameShop.m_ContentHoverboard = EditorGUILayout.ObjectField("Content Hoverboard : ", GameShop.m_ContentHoverboard, typeof(Transform), true) as Transform;

                        GUILayout.Space(10f);

                        GameShop.m_ContentItems = EditorGUILayout.ObjectField("Content Items : ", GameShop.m_ContentItems, typeof(Transform), true) as Transform;

                        GUILayout.Space(10f);

                        GameShop.m_ContentNonConsumable = EditorGUILayout.ObjectField("Content Non Consumable : ", GameShop.m_ContentNonConsumable, typeof(Transform), true) as Transform;


                        GUILayout.EndVertical();
                    }
                    else { 
                    OnEnable();
                    }
                    

                }

                break;
            case 5:
                WindowsValidation();
                if (TitleSceneTC.HeroController)
                {
                    TitleSceneTC.HeroController.SetActive(true);

                    if (itemTarget)
                    {
                        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                        GUILayout.Label("Hero Upgrades Editor", EditorStyles.boldLabel);
                        GUILayout.Space(10f);

                        GUILayout.Space(10f);
                        GUILayout.Label("Basic", EditorStyles.boldLabel);

                        GUILayout.Space(10f);
                        itemTarget.ShopContoller = EditorGUILayout.ObjectField("Component Shop Contoller: ", itemTarget.ShopContoller, typeof(D3ShopCharacter), true) as D3ShopCharacter;

                        GUILayout.Space(10f);
                        itemTarget.NamePlayerText = EditorGUILayout.ObjectField("Player Name Text: ", itemTarget.NamePlayerText, typeof(Text), true) as Text;

                        GUILayout.Space(10f);
                        itemTarget.ImageButtonBuy = EditorGUILayout.ObjectField("Image Button Buy: ", itemTarget.ImageButtonBuy, typeof(Sprite), true) as Sprite;

                        GUILayout.Space(10f);
                        itemTarget.ImageButtonNoCoin = EditorGUILayout.ObjectField("Image Button No Coin: ", itemTarget.ImageButtonNoCoin, typeof(Sprite), true) as Sprite;

                        GUILayout.Space(10f);
                        itemTarget.BuyText = EditorGUILayout.TextField("Buy Text: ", itemTarget.BuyText);

                        GUILayout.Space(10f);
                        itemTarget.NOCoinText = EditorGUILayout.TextField("NO Coin Text: ", itemTarget.NOCoinText);

                        GUILayout.Space(10f);
                        itemTarget.MultiplyPriceWhenPurchasing = EditorGUILayout.IntField("Cant Multiply Price When Purchasing :", itemTarget.MultiplyPriceWhenPurchasing);

                        GUILayout.Space(10f);

                        GUILayout.Space(10f);
                        GUILayout.Label("Upgrades", EditorStyles.boldLabel);


                        GUILayout.Space(10f);
                        GUILayout.Label("Sprint", EditorStyles.boldLabel);
                        GUILayout.Space(10f);
                        itemTarget.PriceTextSprintTime = EditorGUILayout.ObjectField("Object Text Price: ", itemTarget.PriceTextSprintTime, typeof(Text), true) as Text;
                        GUILayout.Space(10f);
                        itemTarget.ScrollbarAddTimeSprintTime = EditorGUILayout.ObjectField("Object Slider: ", itemTarget.ScrollbarAddTimeSprintTime, typeof(Slider), true) as Slider;
                        GUILayout.Space(10f);
                        itemTarget.TextAddTimeSprintTime = EditorGUILayout.ObjectField("Object Text Add Time: ", itemTarget.TextAddTimeSprintTime, typeof(TextMeshProUGUI), true) as TextMeshProUGUI;
                        GUILayout.Space(10f);
                        itemTarget.ButtonSprintTime = EditorGUILayout.ObjectField("Object Button: ", itemTarget.ButtonSprintTime, typeof(Button), true) as Button;
                        GUILayout.Space(10f);
                        itemTarget.ButtonTextSprintTime = EditorGUILayout.ObjectField("Object Text Button: ", itemTarget.ButtonTextSprintTime, typeof(Text), true) as Text;
                        GUILayout.Space(10f);

                        GUILayout.Space(10f);
                        GUILayout.Label("Special", EditorStyles.boldLabel);
                        GUILayout.Space(10f);
                        itemTarget.PriceTextSpecialTime = EditorGUILayout.ObjectField("Object Text Price: ", itemTarget.PriceTextSpecialTime, typeof(Text), true) as Text;
                        GUILayout.Space(10f);
                        itemTarget.ScrollbarAddTimeSpecialTime = EditorGUILayout.ObjectField("Object Slider: ", itemTarget.ScrollbarAddTimeSpecialTime, typeof(Slider), true) as Slider;
                        GUILayout.Space(10f);
                        itemTarget.TextAddTimeSpecialTime = EditorGUILayout.ObjectField("Object Text Add Time: ", itemTarget.TextAddTimeSpecialTime, typeof(TextMeshProUGUI), true) as TextMeshProUGUI;
                        GUILayout.Space(10f);
                        itemTarget.ButtonSpecialTime = EditorGUILayout.ObjectField("Object Button: ", itemTarget.ButtonSpecialTime, typeof(Button), true) as Button;
                        GUILayout.Space(10f);
                        itemTarget.ButtonTextSpecialTime = EditorGUILayout.ObjectField("Object Text Button: ", itemTarget.ButtonTextSpecialTime, typeof(Text), true) as Text;
                        GUILayout.Space(10f);

                        GUILayout.Space(10f);
                        GUILayout.Label("Multiply", EditorStyles.boldLabel);
                        GUILayout.Space(10f);
                        itemTarget.PriceTextMultiplyTime = EditorGUILayout.ObjectField("Object Text Price: ", itemTarget.PriceTextMultiplyTime, typeof(Text), true) as Text;
                        GUILayout.Space(10f);
                        itemTarget.ScrollbarAddTimeMultiplyTime = EditorGUILayout.ObjectField("Object Slider: ", itemTarget.ScrollbarAddTimeMultiplyTime, typeof(Slider), true) as Slider;
                        GUILayout.Space(10f);
                        itemTarget.TextAddTimeMultiplyTime = EditorGUILayout.ObjectField("Object Text Add Time: ", itemTarget.TextAddTimeMultiplyTime, typeof(TextMeshProUGUI), true) as TextMeshProUGUI;
                        GUILayout.Space(10f);
                        itemTarget.ButtonMultiplyTime = EditorGUILayout.ObjectField("Object Button: ", itemTarget.ButtonMultiplyTime, typeof(Button), true) as Button;
                        GUILayout.Space(10f);
                        itemTarget.ButtonTextMultiplyTime = EditorGUILayout.ObjectField("Object Text Button: ", itemTarget.ButtonTextMultiplyTime, typeof(Text), true) as Text;
                        GUILayout.Space(10f);

                        GUILayout.Space(10f);
                        GUILayout.Label("Magnet", EditorStyles.boldLabel);
                        GUILayout.Space(10f);
                        itemTarget.PriceTextMagnetTime = EditorGUILayout.ObjectField("Object Text Price: ", itemTarget.PriceTextMagnetTime, typeof(Text), true) as Text;
                        GUILayout.Space(10f);
                        itemTarget.ScrollbarAddTimeMagnetTime = EditorGUILayout.ObjectField("Object Slider: ", itemTarget.ScrollbarAddTimeMagnetTime, typeof(Slider), true) as Slider;
                        GUILayout.Space(10f);
                        itemTarget.TextAddTimeMagnetTime = EditorGUILayout.ObjectField("Object Text Add Time: ", itemTarget.TextAddTimeMagnetTime, typeof(TextMeshProUGUI), true) as TextMeshProUGUI;
                        GUILayout.Space(10f);
                        itemTarget.ButtonMagnetTime = EditorGUILayout.ObjectField("Object Button: ", itemTarget.ButtonMagnetTime, typeof(Button), true) as Button;
                        GUILayout.Space(10f);
                        itemTarget.ButtonTextMagnetTime = EditorGUILayout.ObjectField("Object Text Button: ", itemTarget.ButtonTextMagnetTime, typeof(Text), true) as Text;
                        GUILayout.Space(10f);

                        GUILayout.Space(10f);
                        GUILayout.Label("Shield", EditorStyles.boldLabel);
                        GUILayout.Space(10f);
                        itemTarget.PriceTextShieldTime = EditorGUILayout.ObjectField("Object Text Price: ", itemTarget.PriceTextShieldTime, typeof(Text), true) as Text;
                        GUILayout.Space(10f);
                        itemTarget.ScrollbarAddTimeShieldTime = EditorGUILayout.ObjectField("Object Slider: ", itemTarget.ScrollbarAddTimeShieldTime, typeof(Slider), true) as Slider;
                        GUILayout.Space(10f);
                        itemTarget.TextAddTimeShieldTime = EditorGUILayout.ObjectField("Object Text Add Time: ", itemTarget.TextAddTimeShieldTime, typeof(TextMeshProUGUI), true) as TextMeshProUGUI;
                        GUILayout.Space(10f);
                        itemTarget.ButtonShieldTime = EditorGUILayout.ObjectField("Object Button: ", itemTarget.ButtonShieldTime, typeof(Button), true) as Button;
                        GUILayout.Space(10f);
                        itemTarget.ButtonTextShieldTime = EditorGUILayout.ObjectField("Object Text Button: ", itemTarget.ButtonTextShieldTime, typeof(Text), true) as Text;
                        GUILayout.Space(10f);


                        GUILayout.EndVertical();
                    }else
                    {
                        OnEnable();
                    }

                }
                

                break;
            case 6:
                WindowsValidation();
                if (TitleSceneTC.RewardWindow)
                {
                    TitleSceneTC.RewardWindow.SetActive(true);
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
                    else {
                        OnEnable();
                    }


                    GUILayout.Space(10f);
                    GUILayout.EndVertical();
                }
                break;

        }
        GUILayout.EndVertical();

        if (EditorGUI.EndChangeCheck())
        {
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
        GUILayout.EndVertical();
    }
    void WindowsValidation()
    {
        ///1
        ///
        if (TitleSceneTC.TitleGUI)
        {
            TitleSceneTC.TitleGUI.SetActive(false);
        }
        else {
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("ERROR: window not found, please assign:", EditorStyles.boldLabel);
            TitleSceneTC.TitleGUI = EditorGUILayout.ObjectField("Title Window: ", TitleSceneTC.TitleGUI, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GUILayout.EndVertical();
        }
        ////2
        ///
        if (TitleSceneTC.SettingGUI)
        {
            TitleSceneTC.SettingGUI.SetActive(false);
        }
        else
        {
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("ERROR: window not found, please assign:", EditorStyles.boldLabel);
            TitleSceneTC.SettingGUI = EditorGUILayout.ObjectField("Setting Window: ", TitleSceneTC.SettingGUI, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GUILayout.EndVertical();
        }
        ///3
        ///
        if (TitleSceneTC.NoLifeGUI)
        {
            TitleSceneTC.NoLifeGUI.SetActive(false);
        }
        else
        {
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("ERROR: window not found, please assign:", EditorStyles.boldLabel);
            TitleSceneTC.NoLifeGUI = EditorGUILayout.ObjectField("No Life Window: ", TitleSceneTC.NoLifeGUI, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GUILayout.EndVertical();
        }
        ////4
        ///
        if (TitleSceneTC.TextObject)
        {
            TitleSceneTC.TextObject.SetActive(false);
        }
        else
        {
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("ERROR: window not found, please assign:", EditorStyles.boldLabel);
            TitleSceneTC.TextObject = EditorGUILayout.ObjectField("Texts Objects: ", TitleSceneTC.TextObject, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GUILayout.EndVertical();
        }
        ///5
        ///
        if (TitleSceneTC.HeroController)
        {
            TitleSceneTC.HeroController.SetActive(false);
        }
        else
        {
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("ERROR: window not found, please assign:", EditorStyles.boldLabel);
            TitleSceneTC.HeroController = EditorGUILayout.ObjectField("Hero Window: ", TitleSceneTC.HeroController, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GUILayout.EndVertical();
        }
        ////6
        ///
        if (TitleSceneTC.ShopGui)
        {
            TitleSceneTC.ShopGui.SetActive(false);
        }
        else
        {
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("ERROR: window not found, please assign:", EditorStyles.boldLabel);
            TitleSceneTC.ShopGui = EditorGUILayout.ObjectField("Shop Window: ", TitleSceneTC.ShopGui, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GUILayout.EndVertical();
        }
        ///7
        ///
        if (TitleSceneTC.HeroController)
        {
            TitleSceneTC.HeroController.SetActive(false);
        }
        else
        {
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("ERROR: window not found, please assign:", EditorStyles.boldLabel);
            TitleSceneTC.HeroController = EditorGUILayout.ObjectField("Hero Window: ", TitleSceneTC.HeroController, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GUILayout.EndVertical();
        }
        ///8
        ///
        if (TitleSceneTC.RewardWindow)
        {
            TitleSceneTC.RewardWindow.SetActive(false);
        }
        else
        {
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("ERROR: window not found, please assign:", EditorStyles.boldLabel);
            TitleSceneTC.RewardWindow = EditorGUILayout.ObjectField("Reward Window: ", TitleSceneTC.RewardWindow, typeof(GameObject), true) as GameObject;
            GUILayout.Space(10f);
            GUILayout.EndVertical();
        }

    }
    void ShopManager()
    {
        EditorGUI.BeginChangeCheck();
        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
        GUILayout.Label("Shop Window", EditorStyles.boldLabel);
        GUILayout.Space(10f);

        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));

#if UNITY_ANDROID
        GUILayout.Label("Current Platform: " + "ANDROID");
#endif
#if UNITY_IOS
						GUILayout.Label("Current Platform: " + "iOS");
#endif
#if UNITY_STANDALONE_OSX
                        GUILayout.Label("Current Platform: " + "macOS");
#endif
#if UNITY_STANDALONE_WIN
						GUILayout.Label("Current Platform: " + "Windows");
#endif
        GUILayout.EndVertical();
        GUILayout.Space(10f);


        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
        GUILayout.Space(10f);

        GameShop.EnableInAppPurchasing = GUILayout.Toggle(GameShop.EnableInAppPurchasing, " Enable In App Purchasing");
        EditorGUILayout.TextArea("Before setting up the plugin enable/disabled In-App Purchasing from Unity Services, \r\n Go to Unity Menu>>Services>>In-App Purchasing>>Configure", GUI.skin.GetStyle("HelpBox"));

        GUILayout.Space(10f);

        GUILayout.EndVertical();

        if (GameShop.EnableInAppPurchasing)
        {
#if UNITY_In_APP && ENABLE_UNITY_IAP
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Label("Revenue Validation", EditorStyles.boldLabel);
            EditorGUILayout.TextArea("Receipt validation helps you prevent users from accessing content they have not purchased. \r\n To enable it check the Use Receipt Validation checkbox.", GUI.skin.GetStyle("HelpBox"));

            EditorGUILayout.TextArea("To setup validation key:\r\na. Go to Unity Menu>>Services>>In-App Purchasing>>Configure> IAP Receipt Validation Obfuscator\r\nb. Paste your GooglePlay public key (from the application’s Google Play Developer Console’s\r\nServices & APIs page).\r\nc. Click Obfuscate Google Play Licence Key.", GUI.skin.GetStyle("HelpBox"));

            GUILayout.Space(10f);

            GameShop.useReceiptValidation = GUILayout.Toggle(GameShop.useReceiptValidation, " Enable Receip Validation");
            GUILayout.Space(10f);
            GameShop.debug = GUILayout.Toggle(GameShop.debug, " Debug");

            GUILayout.EndVertical();

            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Label("Select your platforms:", EditorStyles.boldLabel);


            if (!GameShop.useForAmazon)
            {
                GUILayout.Space(10f);
                GameShop.useForGooglePlay = GUILayout.Toggle(GameShop.useForGooglePlay, " Google Play");

                if (GameShop.useForGooglePlay)
                {
                    GameShop.useForAmazon = false;
                }
            }

            if (!GameShop.useForGooglePlay)
            {
                GUILayout.Space(10f);
                GameShop.useForGooglePlay = false;
                GameShop.useForAmazon = GUILayout.Toggle(GameShop.useForAmazon, " Amazon");
            }



            GUILayout.Space(10f);

            GameShop.useForWindows = GUILayout.Toggle(GameShop.useForWindows, " Windows");

            GUILayout.EndVertical();

#endif

#if !UNITY_In_APP || !ENABLE_UNITY_IAP
            Color defaultColor = GUI.color;
            GUI.color = Color.red;
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUI.color = defaultColor;
            GUILayout.Label("Unity In-App Purchasing SDK not found");
                GUILayout.Space(10);
                if (GUILayout.Button("Import Unity In-App Purchasing Package"))
                {
                    const int width = 600;
                    const int height = 150;

                    var x = (Screen.currentResolution.width - width) / 2;
                    var y = (Screen.currentResolution.height - height) / 2;

                    WindowPackageUnityInstallTitle window = (WindowPackageUnityInstallTitle)EditorWindow.GetWindow(typeof(WindowPackageUnityInstallTitle), false, "Import Unity In-App Purchasing Package");
                    window.position = new Rect(x, y, width, height);
                    window.CodePackage = 2;
                    window.packageToImport = "com.unity.purchasing";
                    window.Install = true;
                    window.Show();

                }
                GUILayout.Label("Attention, after installing the SDK, restart Unity so that the changes are detected.");
                
                GUILayout.EndVertical();
#endif
        }

        if (!GameShop.EnableInAppPurchasing)
        {
#if UNITY_In_APP && ENABLE_UNITY_IAP
            Color defaultColor = GUI.color;
            GUI.color = Color.red;
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUI.color = defaultColor;
            if (GUILayout.Button("Uninstall Unity In App Package"))
            {
                const int width = 600;
                const int height = 150;

                var x = (Screen.currentResolution.width - width) / 2;
                var y = (Screen.currentResolution.height - height) / 2;

                WindowPackageUnityInstallGame window = (WindowPackageUnityInstallGame)EditorWindow.GetWindow(typeof(WindowPackageUnityInstallGame), false, "Uninstall Unity in App Package");
                window.position = new Rect(x, y, width, height);
                window.CodePackage = 2;
                window.packageToImport = "com.unity.purchasing";
                window.Install = false;
                window.Show();

            }
            GUILayout.EndVertical();
#endif
        }

        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
        if (GUILayout.Button("Add Item Shop"))
        {
            GameShop.ListItemShop.Add(new D3ItemShop());
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        }
        GUILayout.EndVertical();

        if (GameShop.EnableInAppPurchasing)
        {
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            SelectedFilter = EditorGUILayout.Popup("Show Items by: ", SelectedFilter, OptionsFilter);
            GUILayout.EndVertical();
        }

        ListOfItems();

        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));

        GUILayout.Space(10f);
        GameShop.TempleteListItem = EditorGUILayout.ObjectField("Templete List Item : ", GameShop.TempleteListItem, typeof(D3ListItemShop), true) as D3ListItemShop;
        EditorGUILayout.TextArea("This template is the one used to instantiate the items", GUI.skin.GetStyle("HelpBox"));

        GUILayout.Space(10f);
        GameShop.TempleteListItemNonConsumable = EditorGUILayout.ObjectField("Templete List Non Consumable: ", GameShop.TempleteListItemNonConsumable, typeof(D3ListItemShop), true) as D3ListItemShop;
        EditorGUILayout.TextArea("This template is the one used to instantiate the items", GUI.skin.GetStyle("HelpBox"));

        GUILayout.Space(10f);
        GameShop.m_ContentHero = EditorGUILayout.ObjectField("Content Hero : ", GameShop.m_ContentHero, typeof(Transform), true) as Transform;

        GUILayout.Space(10f);

        GameShop.m_ContentHoverboard = EditorGUILayout.ObjectField("Content Hoverboard : ", GameShop.m_ContentHoverboard, typeof(Transform), true) as Transform;

        GUILayout.Space(10f);

        GameShop.m_ContentItems = EditorGUILayout.ObjectField("Content Items : ", GameShop.m_ContentItems, typeof(Transform), true) as Transform;

        GUILayout.Space(10f);

        GameShop.m_ContentNonConsumable = EditorGUILayout.ObjectField("Content Non Consumable : ", GameShop.m_ContentNonConsumable, typeof(Transform), true) as Transform;

        GUILayout.Space(10f);

        GameShop.PlayerView = EditorGUILayout.ObjectField("Player View : ", GameShop.PlayerView, typeof(GameObject), true) as GameObject;

        GUILayout.Space(10f);

        GameShop.ScrollbarContentHero = EditorGUILayout.ObjectField("Scrollbar Content Hero : ", GameShop.ScrollbarContentHero, typeof(Scrollbar), true) as Scrollbar;

        GUILayout.Space(10f);

        GameShop.ScrollbarContentHoverboard = EditorGUILayout.ObjectField("Scrollbar Content Hoverboard: ", GameShop.ScrollbarContentHoverboard, typeof(Scrollbar), true) as Scrollbar;

        GUILayout.Space(10f);

        GameShop.ScrollbarContentItems = EditorGUILayout.ObjectField("Scrollbar Content Items : ", GameShop.ScrollbarContentItems, typeof(Scrollbar), true) as Scrollbar;


        GUILayout.EndVertical();

        GUILayout.EndVertical();
        if (EditorGUI.EndChangeCheck())
        {
#if UNITY_In_APP && ENABLE_UNITY_IAP
            if (GameShop.useForGooglePlay)
            {
                UnityEditor.Purchasing.UnityPurchasingEditor.TargetAndroidStore(UnityEngine.Purchasing.AppStore.GooglePlay);
            }
            if (GameShop.useForAmazon)
            {
                UnityEditor.Purchasing.UnityPurchasingEditor.TargetAndroidStore(UnityEngine.Purchasing.AppStore.AmazonAppStore);
            }
#endif

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

            for (int i = 0; GameShop.ListItemShop.Count > i; i++)
            {
                if (GameShop.ListItemShop[i].TypeItem == D3ItemShop.TypeItemShop.Character)
                {
                    if (GameShop.ListItemShop[i].OnlyForCharacterTypeAddPrefab != null)
                    {
                        GameShop.ListItemShop[i].OnlyForCharacterTypeAddPrefab.Name = GameShop.ListItemShop[i].Name;
                        EditorUtility.SetDirty(GameShop.ListItemShop[i].OnlyForCharacterTypeAddPrefab);
                        PrefabUtility.RecordPrefabInstancePropertyModifications(GameShop.ListItemShop[i].OnlyForCharacterTypeAddPrefab);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                }
            }
        }

    }

    void ListOfItems()
    {
        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
        GameShopS.Update();
        if (GameShop.EnableInAppPurchasing)
        {
            switch (SelectedFilter)
            {

                case 0:
                    GUILayout.Label("List of All Items", EditorStyles.boldLabel);
                    break;
                case 1:
                    GUILayout.Label("List of items With Virtual Money", EditorStyles.boldLabel);
                    break;
                case 2:
                    GUILayout.Label("List of items With Real Money", EditorStyles.boldLabel);
                    break;
                case 3:
                    GUILayout.Label("List of Undefined Items", EditorStyles.boldLabel);
                    break;

            }
        }
        else {
            GUILayout.Label("List of items With Virtual Money", EditorStyles.boldLabel);
        }
#if UNITY_In_APP && ENABLE_UNITY_IAP
                if (GameShop.EnableInAppPurchasing)
        {
            if (!GameShop.useForGooglePlay && !GameShop.useForAmazon && !GameShop.useForWindows)
            {
                Color defaultColor = GUI.color;
                GUI.color = Color.red;
                GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                GUI.color = defaultColor;
                GUILayout.Label("Select your Platforms: (Google Play, Amazon, Windows) ", EditorStyles.boldLabel);
                GUILayout.EndVertical();

            }
        }
#endif
       
        if (GameShop.ListItemShop.Count > 0)
        {

            for (int i = 0; i < GameShop.ListItemShop.Count; i++)
            {
                if (GameShop.EnableInAppPurchasing)
                {
                    switch (SelectedFilter)
                    {
                        case 0:
                            if (GameShop.ListItemShop[i] != null)
                            {
                                GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                                GUILayout.Label("Item Shop Name: " + GameShop.ListItemShop[i].Name);

                                GUILayout.Space(10);

                                GameShop.ListItemShop[i].Name = EditorGUILayout.TextField("Item Name: ", GameShop.ListItemShop[i].Name);

                                if (GameShop.EnableInAppPurchasing)
                                {
                                    GameShop.ListItemShop[i].ProductType = (D3ProductType)EditorGUILayout.EnumPopup("Product Item Shop: ", GameShop.ListItemShop[i].ProductType);
                                }
                                else
                                {
                                    GUILayout.Label("Item With In-Game Currency");
                                    GameShop.ListItemShop[i].ProductType = D3ProductType.VirtualMoney;
                                }

                                if (GameShop.ListItemShop[i].ProductType == D3ProductType.RealMoneyConsumable || GameShop.ListItemShop[i].ProductType == D3ProductType.VirtualMoney)
                                {
                                    GameShop.ListItemShop[i].TypeItem = (TypeItemShop)EditorGUILayout.EnumPopup("Type Item Shop: ", GameShop.ListItemShop[i].TypeItem);

                                }

                                if (GameShop.ListItemShop[i].ProductType == D3ProductType.RealMoneyNoADS)
                                {
                                    GameShop.ListItemShop[i].TypeItem = TypeItemShop.None;
                                }

                                if (GameShop.ListItemShop[i].ProductType == D3ProductType.RealMoneyNoADS || GameShop.ListItemShop[i].ProductType == D3ProductType.RealMoneyConsumable)
                                {
                                    GameShop.ListItemShop[i].ProductCode = EditorGUILayout.TextField("Product Code ID: ", GameShop.ListItemShop[i].ProductCode);
                                }

                                if (GameShop.ListItemShop[i].TypeItem == TypeItemShop.Character)
                                {
                                    GameShop.ListItemShop[i].ID = EditorGUILayout.IntField("Player ID: ", GameShop.ListItemShop[i].ID);
                                    EditorGUILayout.TextArea("The ID must be the same as the character list position in the game scene, in the Game Manager tab", GUI.skin.GetStyle("HelpBox"));

                                    GameShop.ListItemShop[i].OnlyForCharacterTypeAddPrefab = EditorGUILayout.ObjectField("ADD Character Prefab: ", GameShop.ListItemShop[i].OnlyForCharacterTypeAddPrefab, typeof(D3Controller), true) as D3Controller;
                                    EditorGUILayout.TextArea("Drag and drop the prefab or gameobject of the created character here", GUI.skin.GetStyle("HelpBox"));


                                }
                                if (GameShop.ListItemShop[i].TypeItem == TypeItemShop.HoverBoard)
                                {
                                    GameShop.ListItemShop[i].ID = EditorGUILayout.IntField("HoverBike ID: ", GameShop.ListItemShop[i].ID);
                                    EditorGUILayout.TextArea("The HoverBike list is not random, the position in the list of each element is the purchase ID, example position 1, ID in the Purchase 1 store. Do not forget. For more information check the sample characters.", GUI.skin.GetStyle("HelpBox"));
                                }

                                if (GameShop.ListItemShop[i].TypeItem == TypeItemShop.Life || GameShop.ListItemShop[i].TypeItem == TypeItemShop.HoverboardKeyUse)
                                {
                                    GameShop.ListItemShop[i].Cant = EditorGUILayout.IntField("Reward Value: ", GameShop.ListItemShop[i].Cant);
                                }

                                if (GameShop.ListItemShop[i].ProductType == D3ProductType.VirtualMoney)
                                {
                                    GameShop.ListItemShop[i].Price = EditorGUILayout.FloatField("Price (Virtual Money): ", GameShop.ListItemShop[i].Price);
                                }

                                if (GameShop.ListItemShop[i].ProductType == D3ProductType.RealMoneyNoADS || GameShop.ListItemShop[i].ProductType == D3ProductType.RealMoneyConsumable)
                                {
                                    GameShop.ListItemShop[i].Price = EditorGUILayout.FloatField("Price (Real Money): ", GameShop.ListItemShop[i].Price);
                                }

                                GameShop.ListItemShop[i].Image = EditorGUILayout.ObjectField("Image: ", GameShop.ListItemShop[i].Image, typeof(Sprite), true) as Sprite;

                                GUILayout.Space(10);

                                if (GUILayout.Button("Remove Item"))
                                {
                                    GameShop.ListItemShop.RemoveAt(i);
                                    EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                                }
                                GUILayout.EndVertical();
                            }
                            break;
                        case 1:
                            if (GameShop.ListItemShop[i] != null && GameShop.ListItemShop[i].ProductType == D3ProductType.VirtualMoney)
                            {
                                GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                                GUILayout.Label("Item Shop Name: " + GameShop.ListItemShop[i].Name);

                                GUILayout.Space(10);

                                GameShop.ListItemShop[i].Name = EditorGUILayout.TextField("Item Name: ", GameShop.ListItemShop[i].Name);

                                if (GameShop.EnableInAppPurchasing)
                                {
                                    GameShop.ListItemShop[i].ProductType = (D3ProductType)EditorGUILayout.EnumPopup("Product Item Shop: ", GameShop.ListItemShop[i].ProductType);
                                }
                                else
                                {
                                    GUILayout.Label("Item With In-Game Currency");
                                    GameShop.ListItemShop[i].ProductType = D3ProductType.VirtualMoney;
                                }

                                if (GameShop.ListItemShop[i].ProductType == D3ProductType.RealMoneyConsumable || GameShop.ListItemShop[i].ProductType == D3ProductType.VirtualMoney)
                                {
                                    GameShop.ListItemShop[i].TypeItem = (TypeItemShop)EditorGUILayout.EnumPopup("Type Item Shop: ", GameShop.ListItemShop[i].TypeItem);

                                }

                                if (GameShop.ListItemShop[i].ProductType == D3ProductType.RealMoneyNoADS)
                                {
                                    GameShop.ListItemShop[i].TypeItem = TypeItemShop.None;
                                }

                                if (GameShop.ListItemShop[i].ProductType == D3ProductType.RealMoneyNoADS || GameShop.ListItemShop[i].ProductType == D3ProductType.RealMoneyConsumable)
                                {
                                    GameShop.ListItemShop[i].ProductCode = EditorGUILayout.TextField("Product Code ID: ", GameShop.ListItemShop[i].ProductCode);
                                }

                                if (GameShop.ListItemShop[i].TypeItem == TypeItemShop.Character)
                                {
                                    GameShop.ListItemShop[i].ID = EditorGUILayout.IntField("Player ID: ", GameShop.ListItemShop[i].ID);
                                    EditorGUILayout.TextArea("The ID must be the same as the character list position in the game scene, in the Game Manager tab", GUI.skin.GetStyle("HelpBox"));

                                    GameShop.ListItemShop[i].OnlyForCharacterTypeAddPrefab = EditorGUILayout.ObjectField("ADD Character Prefab: ", GameShop.ListItemShop[i].OnlyForCharacterTypeAddPrefab, typeof(D3Controller), true) as D3Controller;
                                    EditorGUILayout.TextArea("Drag and drop the prefab or gameobject of the created character here", GUI.skin.GetStyle("HelpBox"));


                                }
                                if (GameShop.ListItemShop[i].TypeItem == TypeItemShop.HoverBoard)
                                {
                                    GameShop.ListItemShop[i].ID = EditorGUILayout.IntField("HoverBike ID: ", GameShop.ListItemShop[i].ID);
                                    EditorGUILayout.TextArea("The HoverBike list is not random, the position in the list of each element is the purchase ID, example position 1, ID in the Purchase 1 store. Do not forget. For more information check the sample characters.", GUI.skin.GetStyle("HelpBox"));
                                }

                                if (GameShop.ListItemShop[i].TypeItem == TypeItemShop.Life || GameShop.ListItemShop[i].TypeItem == TypeItemShop.HoverboardKeyUse)
                                {
                                    GameShop.ListItemShop[i].Cant = EditorGUILayout.IntField("Reward Value: ", GameShop.ListItemShop[i].Cant);
                                }

                                GameShop.ListItemShop[i].Price = EditorGUILayout.FloatField("Price (Virtual Coin", GameShop.ListItemShop[i].Price);


                                GameShop.ListItemShop[i].Image = EditorGUILayout.ObjectField("Image: ", GameShop.ListItemShop[i].Image, typeof(Sprite), true) as Sprite;

                                GUILayout.Space(10);

                                if (GUILayout.Button("Remove Item"))
                                {
                                    GameShop.ListItemShop.RemoveAt(i);
                                    EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                                }
                                GUILayout.EndVertical();
                            }
                            break;
                        case 2:
                            if (GameShop.ListItemShop[i] != null && GameShop.ListItemShop[i].ProductType == D3ProductType.RealMoneyConsumable || GameShop.ListItemShop[i].ProductType == D3ProductType.RealMoneyNoADS)
                            {
                                GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                                GUILayout.Label("Item Shop Name: " + GameShop.ListItemShop[i].Name);

                                GUILayout.Space(10);

                                GameShop.ListItemShop[i].Name = EditorGUILayout.TextField("Item Name: ", GameShop.ListItemShop[i].Name);

                                if (GameShop.EnableInAppPurchasing)
                                {
                                    GameShop.ListItemShop[i].ProductType = (D3ProductType)EditorGUILayout.EnumPopup("Product Item Shop: ", GameShop.ListItemShop[i].ProductType);
                                }
                                else
                                {
                                    GUILayout.Label("Item With In-Game Currency");
                                    GameShop.ListItemShop[i].ProductType = D3ProductType.VirtualMoney;
                                }

                                if (GameShop.ListItemShop[i].ProductType == D3ProductType.RealMoneyConsumable || GameShop.ListItemShop[i].ProductType == D3ProductType.VirtualMoney)
                                {
                                    GameShop.ListItemShop[i].TypeItem = (TypeItemShop)EditorGUILayout.EnumPopup("Type Item Shop: ", GameShop.ListItemShop[i].TypeItem);

                                }

                                if (GameShop.ListItemShop[i].ProductType == D3ProductType.RealMoneyNoADS)
                                {
                                    GameShop.ListItemShop[i].TypeItem = TypeItemShop.None;
                                }

                                if (GameShop.ListItemShop[i].ProductType == D3ProductType.RealMoneyNoADS || GameShop.ListItemShop[i].ProductType == D3ProductType.RealMoneyConsumable)
                                {
                                    GameShop.ListItemShop[i].ProductCode = EditorGUILayout.TextField("Product Code ID: ", GameShop.ListItemShop[i].ProductCode);
                                }

                                if (GameShop.ListItemShop[i].TypeItem == TypeItemShop.Character)
                                {
                                    GameShop.ListItemShop[i].ID = EditorGUILayout.IntField("Player ID: ", GameShop.ListItemShop[i].ID);
                                    EditorGUILayout.TextArea("The ID must be the same as the character list position in the game scene, in the Game Manager tab", GUI.skin.GetStyle("HelpBox"));

                                    GameShop.ListItemShop[i].OnlyForCharacterTypeAddPrefab = EditorGUILayout.ObjectField("ADD Character Prefab: ", GameShop.ListItemShop[i].OnlyForCharacterTypeAddPrefab, typeof(D3Controller), true) as D3Controller;
                                    EditorGUILayout.TextArea("Drag and drop the prefab or gameobject of the created character here", GUI.skin.GetStyle("HelpBox"));


                                }
                                if (GameShop.ListItemShop[i].TypeItem == TypeItemShop.HoverBoard)
                                {
                                    GameShop.ListItemShop[i].ID = EditorGUILayout.IntField("HoverBike ID: ", GameShop.ListItemShop[i].ID);
                                    EditorGUILayout.TextArea("The HoverBike list is not random, the position in the list of each element is the purchase ID, example position 1, ID in the Purchase 1 store. Do not forget. For more information check the sample characters.", GUI.skin.GetStyle("HelpBox"));
                                }

                                if (GameShop.ListItemShop[i].TypeItem == TypeItemShop.Life || GameShop.ListItemShop[i].TypeItem == TypeItemShop.HoverboardKeyUse)
                                {
                                    GameShop.ListItemShop[i].Cant = EditorGUILayout.IntField("Reward Value: ", GameShop.ListItemShop[i].Cant);
                                }

                                GameShop.ListItemShop[i].Price = EditorGUILayout.FloatField("Price (Virtual Coin", GameShop.ListItemShop[i].Price);


                                GameShop.ListItemShop[i].Image = EditorGUILayout.ObjectField("Image: ", GameShop.ListItemShop[i].Image, typeof(Sprite), true) as Sprite;

                                GUILayout.Space(10);

                                if (GUILayout.Button("Remove Item"))
                                {
                                    GameShop.ListItemShop.RemoveAt(i);
                                    EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                                }
                                GUILayout.EndVertical();
                            }
                            break;
                        case 3:
                            if (GameShop.ListItemShop[i] != null && GameShop.ListItemShop[i].TypeItem == TypeItemShop.None)
                            {
                                GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                                GUILayout.Label("Item Shop Name: " + GameShop.ListItemShop[i].Name);

                                GUILayout.Space(10);

                                GameShop.ListItemShop[i].Name = EditorGUILayout.TextField("Item Name: ", GameShop.ListItemShop[i].Name);

                                if (GameShop.EnableInAppPurchasing)
                                {
                                    GameShop.ListItemShop[i].ProductType = (D3ProductType)EditorGUILayout.EnumPopup("Product Item Shop: ", GameShop.ListItemShop[i].ProductType);
                                }
                                else
                                {
                                    GUILayout.Label("Item With In-Game Currency");
                                    GameShop.ListItemShop[i].ProductType = D3ProductType.VirtualMoney;
                                }

                                if (GameShop.ListItemShop[i].ProductType == D3ProductType.RealMoneyConsumable || GameShop.ListItemShop[i].ProductType == D3ProductType.VirtualMoney)
                                {
                                    GameShop.ListItemShop[i].TypeItem = (TypeItemShop)EditorGUILayout.EnumPopup("Type Item Shop: ", GameShop.ListItemShop[i].TypeItem);

                                }

                                if (GameShop.ListItemShop[i].ProductType == D3ProductType.RealMoneyNoADS)
                                {
                                    GameShop.ListItemShop[i].TypeItem = TypeItemShop.None;
                                }

                                if (GameShop.ListItemShop[i].ProductType == D3ProductType.RealMoneyNoADS && GameShop.ListItemShop[i].ProductType == D3ProductType.RealMoneyConsumable)
                                {
                                    GameShop.ListItemShop[i].ProductCode = EditorGUILayout.TextField("Product Code ID: ", GameShop.ListItemShop[i].ProductCode);
                                }

                                if (GameShop.ListItemShop[i].TypeItem == TypeItemShop.Character)
                                {
                                    GameShop.ListItemShop[i].ID = EditorGUILayout.IntField("Player ID: ", GameShop.ListItemShop[i].ID);
                                    EditorGUILayout.TextArea("The ID must be the same as the character list position in the game scene, in the Game Manager tab", GUI.skin.GetStyle("HelpBox"));

                                    GameShop.ListItemShop[i].OnlyForCharacterTypeAddPrefab = EditorGUILayout.ObjectField("ADD Character Prefab: ", GameShop.ListItemShop[i].OnlyForCharacterTypeAddPrefab, typeof(D3Controller), true) as D3Controller;
                                    EditorGUILayout.TextArea("Drag and drop the prefab or gameobject of the created character here", GUI.skin.GetStyle("HelpBox"));


                                }
                                if (GameShop.ListItemShop[i].TypeItem == TypeItemShop.HoverBoard)
                                {
                                    GameShop.ListItemShop[i].ID = EditorGUILayout.IntField("HoverBike ID: ", GameShop.ListItemShop[i].ID);
                                    EditorGUILayout.TextArea("The HoverBike list is not random, the position in the list of each element is the purchase ID, example position 1, ID in the Purchase 1 store. Do not forget. For more information check the sample characters.", GUI.skin.GetStyle("HelpBox"));
                                }

                                if (GameShop.ListItemShop[i].TypeItem == TypeItemShop.Life || GameShop.ListItemShop[i].TypeItem == TypeItemShop.HoverboardKeyUse)
                                {
                                    GameShop.ListItemShop[i].Cant = EditorGUILayout.IntField("Reward Value: ", GameShop.ListItemShop[i].Cant);
                                }

                                GameShop.ListItemShop[i].Price = EditorGUILayout.FloatField("Price (Virtual Coin", GameShop.ListItemShop[i].Price);


                                GameShop.ListItemShop[i].Image = EditorGUILayout.ObjectField("Image: ", GameShop.ListItemShop[i].Image, typeof(Sprite), true) as Sprite;

                                GUILayout.Space(10);

                                if (GUILayout.Button("Remove Item"))
                                {
                                    GameShop.ListItemShop.RemoveAt(i);
                                    EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                                }
                                GUILayout.EndVertical();
                            }
                            break;
                    }
                }
                else {
                    if (GameShop.ListItemShop[i] != null && GameShop.ListItemShop[i].ProductType == D3ProductType.VirtualMoney)
                    {
                        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                        GUILayout.Label("Item Shop Name: " + GameShop.ListItemShop[i].Name);

                        GUILayout.Space(10);

                        GameShop.ListItemShop[i].Name = EditorGUILayout.TextField("Item Name: ", GameShop.ListItemShop[i].Name);

                        if (GameShop.EnableInAppPurchasing)
                        {
                            GameShop.ListItemShop[i].ProductType = (D3ProductType)EditorGUILayout.EnumPopup("Product Item Shop: ", GameShop.ListItemShop[i].ProductType);
                        }
                        else
                        {
                            GUILayout.Label("Item With In-Game Currency");
                            GameShop.ListItemShop[i].ProductType = D3ProductType.VirtualMoney;
                        }

                        if (GameShop.ListItemShop[i].ProductType == D3ProductType.RealMoneyConsumable || GameShop.ListItemShop[i].ProductType == D3ProductType.VirtualMoney)
                        {
                            GameShop.ListItemShop[i].TypeItem = (TypeItemShop)EditorGUILayout.EnumPopup("Type Item Shop: ", GameShop.ListItemShop[i].TypeItem);

                        }

                        if (GameShop.ListItemShop[i].ProductType == D3ProductType.RealMoneyNoADS)
                        {
                            GameShop.ListItemShop[i].TypeItem = TypeItemShop.None;
                        }

                        if (GameShop.ListItemShop[i].ProductType == D3ProductType.RealMoneyNoADS || GameShop.ListItemShop[i].ProductType == D3ProductType.RealMoneyConsumable)
                        {
                            GameShop.ListItemShop[i].ProductCode = EditorGUILayout.TextField("Product Code ID: ", GameShop.ListItemShop[i].ProductCode);
                        }

                        if (GameShop.ListItemShop[i].TypeItem == TypeItemShop.Character)
                        {
                            GameShop.ListItemShop[i].ID = EditorGUILayout.IntField("Player ID: ", GameShop.ListItemShop[i].ID);
                            EditorGUILayout.TextArea("The ID must be the same as the character list position in the game scene, in the Game Manager tab", GUI.skin.GetStyle("HelpBox"));

                            GameShop.ListItemShop[i].OnlyForCharacterTypeAddPrefab = EditorGUILayout.ObjectField("ADD Character Prefab: ", GameShop.ListItemShop[i].OnlyForCharacterTypeAddPrefab, typeof(D3Controller), true) as D3Controller;
                            EditorGUILayout.TextArea("Drag and drop the prefab or gameobject of the created character here", GUI.skin.GetStyle("HelpBox"));


                        }
                        if (GameShop.ListItemShop[i].TypeItem == TypeItemShop.HoverBoard)
                        {
                            GameShop.ListItemShop[i].ID = EditorGUILayout.IntField("HoverBike ID: ", GameShop.ListItemShop[i].ID);
                            EditorGUILayout.TextArea("The HoverBike list is not random, the position in the list of each element is the purchase ID, example position 1, ID in the Purchase 1 store. Do not forget. For more information check the sample characters.", GUI.skin.GetStyle("HelpBox"));
                        }

                        if (GameShop.ListItemShop[i].TypeItem == TypeItemShop.Life || GameShop.ListItemShop[i].TypeItem == TypeItemShop.HoverboardKeyUse)
                        {
                            GameShop.ListItemShop[i].Cant = EditorGUILayout.IntField("Reward Value: ", GameShop.ListItemShop[i].Cant);
                        }

                        GameShop.ListItemShop[i].Price = EditorGUILayout.FloatField("Price (Virtual Coin", GameShop.ListItemShop[i].Price);


                        GameShop.ListItemShop[i].Image = EditorGUILayout.ObjectField("Image: ", GameShop.ListItemShop[i].Image, typeof(Sprite), true) as Sprite;

                        GUILayout.Space(10);

                        if (GUILayout.Button("Remove Item"))
                        {
                            GameShop.ListItemShop.RemoveAt(i);
                            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                        }
                        GUILayout.EndVertical();
                    }

                }


            }
            GUILayout.Space(10);
            if (GUILayout.Button("Add Item Shop"))
            {
                GameShop.ListItemShop.Add(new D3ItemShop());
                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
            }
            GUILayout.Space(10);

        }

        GameShopS.ApplyModifiedProperties();

        GUILayout.EndVertical();
    }

    void ADSManager() 
    {
#if UNITY_ANDROID || UNITY_IOS
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
                ASS.transform.SetParent(TitleScene.gameObject.transform);
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

                    WindowPackageUnityInstallTitle window = (WindowPackageUnityInstallTitle)EditorWindow.GetWindow(typeof(WindowPackageUnityInstallTitle), false, "Import Unity Ads Package");
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

                TitleSceneTC.EnableADSOnScene = GUILayout.Toggle(TitleSceneTC.EnableADSOnScene, " Enable ADS On Scene");

                AdsManager.EnableTESTMODE = GUILayout.Toggle(AdsManager.EnableTESTMODE, " Enable Test Mode");

                AdsManager.TitleScene = TitleSceneTC;

                GUILayout.EndVertical();

                if (TitleSceneTC.EnableADSOnScene)
                {
                    AdsScrollPos = EditorGUILayout.BeginScrollView(AdsScrollPos, GUILayout.ExpandWidth(true), GUILayout.Height(600));
                    
                    GUILayout.Space(10f);
                    SerializedADSManager.Update();

                    GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                    GUILayout.Space(10);

                    GUILayout.Label("Banner ADS");
                    GUILayout.Space(10);
                    TitleSceneTC.UseBanner = GUILayout.Toggle(TitleSceneTC.UseBanner, " Use Banner");

                    if (TitleSceneTC.UseBanner)
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
                    SerializedTitleScene.Update();
                    GUILayout.Label("Rewarded Video ADS");
                    TitleSceneTC.EnableRewardedADOnScene = GUILayout.Toggle(TitleSceneTC.EnableRewardedADOnScene, " Use Rewarded AD Buttons");

                    if (TitleSceneTC.EnableRewardedADOnScene)
                    {
                        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                        GUILayout.Label("Rewarded Window");
                     
                        GUILayout.Space(10);

                        TitleSceneTC.EnableRewardedWindow = GUILayout.Toggle(TitleSceneTC.EnableRewardedWindow, " Enable Rewarded Window");
                        
                        GUILayout.Space(10);

                        EditorGUILayout.TextArea("This window is used to show the player the amount of reward obtained.", GUI.skin.GetStyle("HelpBox"));

                        GUILayout.Space(10);
                        if (TitleSceneTC.EnableRewardedWindow)
                        {
                            TitleSceneTC.RewardWindow = EditorGUILayout.ObjectField("Reward Window: ", TitleSceneTC.RewardWindow, typeof(GameObject), true) as GameObject;
                            if (TitleSceneTC.RewardWindow)
                            {
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
                            }
                            GUILayout.Space(10);
                        }

                        GUILayout.Space(10);
                        GUILayout.EndVertical();


                        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                        GUILayout.Label("Rewarded AD Buttons");
                        if (GUILayout.Button("Add Reward button"))
                        {
                            TitleSceneTC.ListRewardedADButtons.Add(new D3ADSTypeReward());
                            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                        }
                        GUILayout.Space(10);
                        if (TitleSceneTC.ListRewardedADButtons.Count > 0)
                        {
                            for (int i = 0; i < TitleSceneTC.ListRewardedADButtons.Count; i++)
                            {
                                if (TitleSceneTC.ListRewardedADButtons[i] != null)
                                {
                                    GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                                    
                                    GUILayout.Space(10);
                                    GUILayout.Label("AD Button ID: "+i);
                                    GUILayout.Space(10);

                                    TitleSceneTC.ListRewardedADButtons[i].IdButton = i;
                                    GUILayout.Space(10);

                                    TitleSceneTC.ListRewardedADButtons[i].showAdButton = EditorGUILayout.ObjectField("Ad Button: ", TitleSceneTC.ListRewardedADButtons[i].showAdButton, typeof(Button), true) as Button;

                                    GUILayout.Space(10);

                                    TitleSceneTC.ListRewardedADButtons[i].TypeReward = (D3TypeReward)EditorGUILayout.EnumPopup("Type Reward: ", TitleSceneTC.ListRewardedADButtons[i].TypeReward);

                                    GUILayout.Space(10);

                                    TitleSceneTC.ListRewardedADButtons[i].CantReward = EditorGUILayout.IntField("Reward Value: ", TitleSceneTC.ListRewardedADButtons[i].CantReward);

                                    GUILayout.Space(10);

                                    TitleSceneTC.ListRewardedADButtons[i].ImgReward = EditorGUILayout.ObjectField("Img Reward: ", TitleSceneTC.ListRewardedADButtons[i].ImgReward, typeof(Sprite), true) as Sprite;

                                    GUILayout.Space(10);

                                    if (GUILayout.Button("Remove Reward button"))
                                    {
                                        TitleSceneTC.ListRewardedADButtons.RemoveAt(i);
                                        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                                    }

                                    GUILayout.Space(10);

                                    GUILayout.EndVertical();

                                    
                                }
                            }

                            if (GUILayout.Button("Add Reward button"))
                            {
                                TitleSceneTC.ListRewardedADButtons.Add(new D3ADSTypeReward());
                                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                            }
                        }
                        GUILayout.EndVertical();
                        GUILayout.Space(10);

                        GUILayout.Label("Rewarded Events");
                        GUILayout.Space(10);

                        SerializedProperty prop5 = SerializedADSManager.FindProperty("ShowCompleteUnityRewardedVideo");
                        if (prop5 != null)
                            EditorGUILayout.PropertyField(prop5, true);
                        GUILayout.Space(10f);

                        SerializedProperty prop6 = SerializedADSManager.FindProperty("SkippedUnityRewardedVideo");
                        if (prop6 != null)
                            EditorGUILayout.PropertyField(prop6, true);
                        GUILayout.Space(10f);

                    }

                    GUILayout.Space(10);
                    GUILayout.EndVertical();



                    GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                    GUILayout.Label("Interstitial ADS");
                    TitleSceneTC.EnableInterstitialADSOnScene = GUILayout.Toggle(TitleSceneTC.EnableInterstitialADSOnScene, " Use Interstitial ADS");
                    GUILayout.Space(10f);
                    EditorGUILayout.TextArea("Enable interstitial ads before loading next scene", GUI.skin.GetStyle("HelpBox"));


                    if (TitleSceneTC.EnableInterstitialADSOnScene)
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
                    SerializedTitleScene.ApplyModifiedProperties();

                    EditorGUILayout.EndScrollView();
                }
                
                GUILayout.Space(10);

                

#endif

                GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                if (GUILayout.Button("Disable Unity ADS"))
                {
                    AdsManager.EnableUnityADS = false;
                    TitleSceneTC.EnableInterstitialADSOnScene = false;
                    TitleSceneTC.EnableRewardedADOnScene = false;
                    TitleSceneTC.UseBanner = false;
                    TitleSceneTC.EnableADSOnScene = false;

#if UNITY_ADS && ENABLE_UNITY_ADS
                    const int width = 600;
                    const int height = 150;

                    var x = (Screen.currentResolution.width - width) / 2;
                    var y = (Screen.currentResolution.height - height) / 2;

                    WindowPackageUnityInstallTitle window = (WindowPackageUnityInstallTitle)EditorWindow.GetWindow(typeof(WindowPackageUnityInstallTitle), false, "Uninstall Unity Ads Package");
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
#endif
#if !UNITY_ANDROID && !UNITY_IOS
        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
        GUILayout.Space(10f);
        EditorGUILayout.TextArea("ADS Only Available for Android and IOS", GUI.skin.GetStyle("HelpBox"));
        GUILayout.Space(10f);
        GUILayout.EndVertical();

#endif
    }

    void LevelSystemManager()
    {
        if (!Application.isPlaying)
        {
            EditorGUI.BeginChangeCheck();
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            TitleSceneTC.EnabledLevelSystem = GUILayout.Toggle(TitleSceneTC.EnabledLevelSystem, " Enabled Level System");
            GUILayout.Space(10f);
            GUILayout.EndVertical();
            if (TitleSceneTC.EnabledLevelSystem)
            {
                GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                GUILayout.Space(5f);
                GUILayout.Label("Level Manager", EditorStyles.boldLabel);
                TitleSceneTC.LevelSystemManager = EditorGUILayout.ObjectField("Level System Manager: ", TitleSceneTC.LevelSystemManager, typeof(D3LevelSystemManager), true) as D3LevelSystemManager;
                GUILayout.Space(5f);
                if (LevelSystem)
                {
                    TitleSceneTC.WindowLevelSystem.TemplateLevelPrefab = EditorGUILayout.ObjectField("Template Level Prefab: ", TitleSceneTC.WindowLevelSystem.TemplateLevelPrefab, typeof(GameObject), true) as GameObject;
                    GUILayout.Space(5f);
                    TitleSceneTC.WindowLevelSystem.RootCanvasLevelsParent = EditorGUILayout.ObjectField("Root Canvas Levels: ", TitleSceneTC.WindowLevelSystem.RootCanvasLevelsParent, typeof(Transform), true) as Transform;
                    GUILayout.Space(5f);
                }
                GUILayout.EndVertical();
                if (!LevelSystem)
                {
                    Color defaultColor = GUI.color;
                    GUI.color = Color.red;
                    GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                    GUI.color = defaultColor;
                    if (GUILayout.Button("Level Manager not found, Click to add"))
                    {
                        GameObject LevelsystemManager = Instantiate(Resources.Load("Game/LevelSystemManager")) as GameObject;
                        LevelsystemManager.name = "Level System Manager";
                        LevelsystemManager.transform.position = new Vector3(0f, 0f, 0f);
                        LevelsystemManager.transform.SetParent(TitleScene.gameObject.transform);
                        TitleSceneTC.LevelSystemManager = LevelsystemManager.GetComponent<D3LevelSystemManager>();
                        TitleSceneTC.LevelSystemManager.EnabledLevelSystem = true;

                        GameObject windowlevelsystem = Instantiate(Resources.Load("Game/WindowLevelSystem")) as GameObject;
                        windowlevelsystem.name = "Windows Level System";
                        Canvas Canvas = TitleScene.gameObject.GetComponentInChildren<Canvas>().rootCanvas;
                        windowlevelsystem.transform.SetParent(Canvas.gameObject.transform);
                        RectTransform rectTransform = windowlevelsystem.GetComponent<RectTransform>();
                        rectTransform.offsetMin = new Vector2(0, 0);
                        rectTransform.offsetMax = new Vector2(0, 0);
                        rectTransform.localScale = new Vector3(1, 1, 1);
                        TitleSceneTC.WindowLevelSystem = windowlevelsystem.GetComponent<D3LevelSystem>();
                        windowlevelsystem.gameObject.SetActive(false);
                        OnEnable();

                        Debug.Log("Level Manager Prefabs Created!");
                    }
                    GUILayout.EndVertical();
                }

                if (LevelSystem)
                {
                    GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                    GUILayout.Space(5f);
                    GUILayout.Label("Level System", EditorStyles.boldLabel);

                    EditorGUILayout.TextArea("Any changes made to the level system are automatically saved and the values ​​are reset to prevent errors.", GUI.skin.GetStyle("HelpBox"));

                    GUILayout.Space(5f);

                    SerializedLevelSystem.Update();

                    GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                    GUILayout.Space(5f);

                    GUILayout.Label("Save Data Name: ");

                    TitleSceneTC.LevelSystemManager.LevelDataKey = GUILayout.TextField(TitleSceneTC.LevelSystemManager.LevelDataKey, 50); 

                    GUILayout.Space(5f);
                    GUILayout.EndVertical();
                    GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                    if (GUILayout.Button("Delete Save Unlocked Levels"))
                    {
                        TitleSceneTC.LevelSystemManager.ClearMemory();
                        TitleSceneTC.LevelSystemManager.SaveToMemory();
                        Debug.Log("Delete Complete!");
                    }
                    GUILayout.EndVertical();

                    if (TitleSceneTC.WindowLevelSystem.TemplateLevelPrefab != null)
                    {
                        if (GUILayout.Button("Add New Level"))
                        {
                            TitleSceneTC.LevelSystemManager.LevelSystenData.Add(new D3LevelSystemData());
                            SerializedLevelSystem.ApplyModifiedProperties();
                            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                        }

                    }
                    else
                    {
                        Color defaultColor = GUI.color;
                        GUI.color = Color.red;
                        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                        GUI.color = defaultColor;
                        if (GUILayout.Button("Template Level Object not found, Click to add"))
                        {
                            GameObject ASS = Resources.Load("Game/TemplateLevelObject") as GameObject;

                            TitleSceneTC.WindowLevelSystem.TemplateLevelPrefab = ASS;

                            Debug.Log("Template Level Object Assign!");
                        }
                        GUILayout.EndVertical();
                    }

                    SerializedLevelSystem.ApplyModifiedProperties();

                    GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                    LevelSystenScrollPos = EditorGUILayout.BeginScrollView(LevelSystenScrollPos, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    for (int i = 0; i < TitleSceneTC.LevelSystemManager.LevelSystenData.Count; i++)
                    {
                        EditorGUI.BeginChangeCheck();
                        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));

                        EditorGUILayout.LabelField("Level: ", (i + 1).ToString());
                        TitleSceneTC.LevelSystemManager.LevelSystenData[i].IDLevel = i + 1;
                        TitleSceneTC.LevelSystemManager.LevelSystenData[i].LevelLocked = EditorGUILayout.Toggle("Locked: ", TitleSceneTC.LevelSystemManager.LevelSystenData[i].LevelLocked);
                        TitleSceneTC.LevelSystemManager.LevelSystenData[i].LevelRating = EditorGUILayout.IntField("Rating: ", TitleSceneTC.LevelSystemManager.LevelSystenData[i].LevelRating);
                        TitleSceneTC.LevelSystemManager.LevelSystenData[i].Scene = EditorGUILayout.ObjectField("Scene: ", TitleSceneTC.LevelSystemManager.LevelSystenData[i].Scene, typeof(SceneAsset), true) as SceneAsset;
                        if (TitleSceneTC.LevelSystemManager.LevelSystenData[i].Scene != null)
                        {
                            TitleSceneTC.LevelSystemManager.LevelSystenData[i].LevelScene = TitleSceneTC.LevelSystemManager.LevelSystenData[i].Scene.name;
                        }
                        if (GUILayout.Button("Remove Level", GUILayout.Width(100)))
                        {
                            TitleSceneTC.LevelSystemManager.LevelSystenData.RemoveAt(i);
                            SerializedLevelSystem.ApplyModifiedProperties();
                            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                        }
                        GUILayout.EndVertical();
                        if (EditorGUI.EndChangeCheck())
                        {
                            if (i < TitleSceneTC.LevelSystemManager.LevelSystenData.Count)
                            {
                                if (TitleSceneTC.LevelSystemManager.LevelSystenData[i].Scene != null)
                                {
                                    TitleSceneTC.LevelSystemManager.LevelSystenData[i].LevelScene = TitleSceneTC.LevelSystemManager.LevelSystenData[i].Scene.name;
                                }
                            }
                            TitleSceneTC.LevelSystemManager.SaveToMemory();
                        }
                    }
                    EditorGUILayout.EndScrollView();
                    GUILayout.EndVertical();


                    GUILayout.EndVertical();


                }
            }

            if (!TitleSceneTC.EnabledLevelSystem)
            {
                if (TitleSceneTC.LevelSystemManager != null)
                {
                    DestroyImmediate(TitleSceneTC.LevelSystemManager.gameObject);
                }
                if (TitleSceneTC.WindowLevelSystem != null)
                {
                    DestroyImmediate(TitleSceneTC.WindowLevelSystem.gameObject);
                }
                GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                GUILayout.Space(10f);
                GUILayout.Label("First Scene To Load: ", EditorStyles.boldLabel);
                GUILayout.Space(10f);
                TitleSceneTC.Scene = EditorGUILayout.ObjectField("Scene: ", TitleSceneTC.Scene, typeof(SceneAsset), true) as SceneAsset;
                if (TitleSceneTC.Scene != null)
                {
                    TitleSceneTC.GameScene = TitleSceneTC.Scene.name;
                }
                GUILayout.Space(10f);
                GUILayout.EndVertical();
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
}
class WindowPackageUnityInstallTitle : EditorWindow
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

        if (CodePackage == 2)
        {
            Debug.Log("Unity In APP installation started. Please wait");
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
