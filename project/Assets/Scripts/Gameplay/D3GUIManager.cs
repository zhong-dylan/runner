using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

[System.Serializable]
public class D3GUIManager : MonoBehaviour
{

	public enum ButtonType
	{
		pause, resume, exit, restart, sumExit
	}


	[System.Serializable]
	public class RectGroup
	{
		public string name;
	}

	[System.Serializable]
	public class GroupGUITexture
	{
		public string name;
		public D3Item.TypeItem itemType;
		public Image guiTexture;
		public Text guiText;
    }

	[System.Serializable]
	public class GroupGUIButton
	{
		public string name;
		public ButtonType buttonType;
		public Button button;
	}

	public List<GroupGUITexture> itemStateSet = new List<GroupGUITexture>();
	public List<GroupGUIButton> menuButtonSet = new List<GroupGUIButton>();
	private D3CalOnGUI calOnGUI;
	public GameObject PauseGui;
	public GameObject GameOverGui;
	public GameObject InGameGui;
	public GameObject WinGameGui;
	public GameObject CoinTObj;
	public GameObject LifeTObj;
	public GameObject SettingsGUI;
    public GameObject BestScoreGUI;
    public GameObject HeroGui;
    public GameObject RewardWindow;
    public GameObject PanelRandomReward;

    public Text BestScoreText;

    public Text PauseScore;
	public Text PauseToWinScore;
	public Text PauseBestScore;

	public Text GameOverScore;
	public Text GameToWinScore;
	public Text GameOverBestScore;

	public Text TextCoinCollect;
	public Text Textdistance;
    public Text LevelText;
    public Text GameOverLevelText;
    public Text WinLevelText;
    public Text PauseLevelText;

    public Text WinScore;
    public Text WinDistance;
    public Text WinTextBestScore;

	public Text CoinT;
	public Text LifeT;
	public string TextScore = "Distance: ";
	public string TextToWinInfo = "To Win: ";
	public string TextBestScore = "Best Distance: ";

	public GameObject ButtonToNewLevel;

	public static D3GUIManager instance;

	public D3SoundManager SoundManager;

	public Button ButtonRevival;
	public Button ButtonHoverBoard;
	public Text CantHoverBoard;
	public GameObject BGLoading;


	public GameObject StartOFF1;
	public GameObject StartON1;
	public GameObject StartOFF2;
	public GameObject StartON2;
	public GameObject StartOFF3;
	public GameObject StartON3;
	public GameObject StartWinON1;
	public GameObject StartWinON2;
	public GameObject StartWinON3;
    public GameObject StartWinOFF1;
    public GameObject StartWinOFF2;
    public GameObject StartWinOFF3;

    public Slider LoadingBar;
	public Text LoadingBarText;

	public Slider musicVolumeSlider = null;

	public Slider SFXVolumeSlider = null;

	int GetStart1;
	int Getstart2;
	int Getstart3;

    public Text PriceTextSprintTime;
    public Slider ScrollbarAddTimeSprintTime;
    public TextMeshProUGUI TextAddTimeSprintTime;
    public Button ButtonSprintTime;
    public Text ButtonTextSprintTime;


    public Text PriceTextSpecialTime;
    public Slider ScrollbarAddTimeSpecialTime;
    public TextMeshProUGUI TextAddTimeSpecialTime;
    public Button ButtonSpecialTime;
    public Text ButtonTextSpecialTime;


    public Text PriceTextMultiplyTime;
    public Slider ScrollbarAddTimeMultiplyTime;
    public TextMeshProUGUI TextAddTimeMultiplyTime;
    public Button ButtonMultiplyTime;
    public Text ButtonTextMultiplyTime;


    public Text PriceTextMagnetTime;
    public Slider ScrollbarAddTimeMagnetTime;
    public TextMeshProUGUI TextAddTimeMagnetTime;
    public Button ButtonMagnetTime;
    public Text ButtonTextMagnetTime;


    public Text PriceTextShieldTime;
    public Slider ScrollbarAddTimeShieldTime;
    public TextMeshProUGUI TextAddTimeShieldTime;
    public Button ButtonShieldTime;
    public Text ButtonTextShieldTime;

    public int MultiplyPriceWhenPurchasing = 2;
    public Sprite ImageButtonBuy;
    public Sprite ImageButtonNoCoin;
    public Text NamePlayerText;
    public string BuyText = "UPGRADE";
    public string NOCoinText = "NO COIN";
    public string FinishUpgradeText = "MAX";


    private readonly string _appearTrigger = "Appear";
	private readonly string _disappearTrigger = "Disappear";

	public GameObject PanelFade;
	public float TimeFade = 1;

    public bool KeyInput = true;
    public bool TouchInput = true;
    public bool AutoRun = true;
    public bool KeyInputContinueRun = true;
    public bool TouchInputContinueRun = true;
    public KeyCode KeyCodeContinueRun = KeyCode.Space;
    public KeyCode KeyCodeUP = KeyCode.W;
    public KeyCode KeyCodeLeft = KeyCode.A;
    public KeyCode KeyCodeRight = KeyCode.D;
    public KeyCode KeyCodeDown = KeyCode.S;

    public GameObject ImgInfoToRun;
    void Start()
    {
        instance = this;
        Canvas canvas = GetComponentInChildren<Canvas>().rootCanvas;
        canvas.worldCamera = Camera.main;
        if (SoundManager == null)
        {
            SoundManager = FindAnyObjectByType<D3SoundManager>();
        }
        if (D3LevelSystemManager.Instance)
        {
            if (D3LevelSystemManager.Instance.EnabledLevelSystem)
            {
                int newlevel = (D3LevelSystemManager.Instance.CurrentLevel + 1);
                if (LevelText != null)
                {
                    LevelText.gameObject.SetActive(true);
                    LevelText.text = "Level " + newlevel;
                }
                if (GameOverLevelText != null)
                {
                    GameOverLevelText.gameObject.SetActive(true);
                    GameOverLevelText.text = "Level " + newlevel;
                }
                if (PauseLevelText != null)
                {
                    PauseLevelText.gameObject.SetActive(true);
                    PauseLevelText.text = "Level " + newlevel;
                }
                if (WinLevelText != null)
                {
                    WinLevelText.gameObject.SetActive(true);
                    WinLevelText.text = "Level " + newlevel;
                }

                if (newlevel < D3LevelSystemManager.Instance.LevelSystenData.Count)
                {
                    ButtonToNewLevel.SetActive(true);
                }
                else
                {
                    ButtonToNewLevel.SetActive(false);
                }
            }
            else
            {

                ButtonToNewLevel.SetActive(false);
                Debug.Log("LevelSysten no habilitado");
                if (LevelText != null)
                {
                    LevelText.gameObject.SetActive(false);
                }
                if (GameOverLevelText != null)
                {
                    GameOverLevelText.gameObject.SetActive(false);
                }
                if (PauseLevelText != null)
                {
                    PauseLevelText.gameObject.SetActive(false);
                }
                if (WinLevelText != null)
                {
                    WinLevelText.gameObject.SetActive(false);
                }
            }

        }
        else {

            ButtonToNewLevel.SetActive(false);
            if (LevelText != null)
            {
                LevelText.gameObject.SetActive(false);
            }
            if (GameOverLevelText != null)
            {
                GameOverLevelText.gameObject.SetActive(false);
            }
            if (PauseLevelText != null)
            {
                PauseLevelText.gameObject.SetActive(false);
            }
            if (WinLevelText != null)
            {
                WinLevelText.gameObject.SetActive(false);
            }
        }


        for (int i = 0; i < menuButtonSet.Count; i++)
        {
            menuButtonSet[i].button.transform.gameObject.SetActive(false);
        }

        for (int i = 0; i < itemStateSet.Count; i++)
        {
            itemStateSet[i].guiTexture.transform.gameObject.SetActive(false);
            itemStateSet[i].guiText.transform.gameObject.SetActive(false);
        }
        if (PanelFade != null)
        {
            PanelFade.GetComponent<Image>().enabled = false;
            PanelFade.SetActive(false);
        }
        if (BGLoading != null)
        {
            BGLoading.SetActive(true);
        }
        InGameGui.SetActive(false);
        PauseGui.SetActive(false);
        GameOverGui.SetActive(false);
        WinGameGui.SetActive(false);
        CoinTObj.SetActive(false);
        LifeTObj.SetActive(false);
        SettingsGUI.SetActive(false);
        BestScoreGUI.SetActive(false);
        HeroGui.SetActive(false);
        RestartStart();
        ButtonHoverBoard.gameObject.SetActive(false);
        calOnGUI = GetComponent<D3CalOnGUI>();
        UpdateText();
        instance = this;
#if UNITY_ADS && ENABLE_UNITY_ADS
        if (D3ADSManager.D3AdsManager && D3ADSManager.D3AdsManager.ADSReady)
        {
            if (D3GameController.instace.EnableADSOnScene)
            {
                if (D3GameController.instace.UseBanner)
                {
                    D3ADSManager.D3AdsManager.RequestBanner();
                }
                if (D3GameController.instace.EnableInterstitialADSOnWim || D3GameController.instace.EnableInterstitialADSOnGameOver)
                {
                    D3ADSManager.D3AdsManager.LoadInstertialADS();
                }
            }

            if (D3GameController.instace.EnableRewardedADOnScene)
            {
                if (PanelRandomReward.GetComponent<D3PanelRewardADS>() && D3GameController.instace.ListRandomRewardedAD.Count > 0)
                {
                    D3ADSManager.D3AdsManager.LoadRewardedUnityVideo();
                    PanelRandomReward.GetComponent<D3PanelRewardADS>().EnabledPanel();
                    PanelRandomReward.SetActive(true);
                }

            }
            if (!D3GameController.instace.EnableRewardedADOnScene)
            {
                if (PanelRandomReward.GetComponent<D3PanelRewardADS>())
                {
                    PanelRandomReward.GetComponent<D3PanelRewardADS>().DisablePanel();
                    PanelRandomReward.SetActive(false);
                }

            }
        }
        else {
            Debug.LogWarning("Attention: for the advertising settings to work correctly, start from the main scene (Title Scene)");
        }
#endif
    }


    public void UpdateText()
    {
        NamePlayerText.text = D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].Name;

        #region Sprint Time
        if (D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentAddSprintTime < D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddSprintTimeMax)
        {
            ScrollbarAddTimeSprintTime.maxValue = D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddSprintTimeMax;

            if (!PlayerPrefs.HasKey(D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].gameObject.name + "SprintTime"))
            {
                ScrollbarAddTimeSprintTime.value = D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentAddSprintTime;
                TextAddTimeSprintTime.text = D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentAddSprintTime + "/" + D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddSprintTimeMax;
            }
            else
            {
                ScrollbarAddTimeSprintTime.value = PlayerPrefs.GetFloat(D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].gameObject.name + "SprintTime");
                TextAddTimeSprintTime.text = PlayerPrefs.GetFloat(D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].gameObject.name + "SprintTime") + "/" + D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddSprintTimeMax;
            }

            if (D3GameAttribute.gameAttribute.Totalcoin >= D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentPriceSprintTime)
            {
                if (ButtonSprintTime.image.sprite != ImageButtonBuy)
                {
                    ButtonSprintTime.image.sprite = ImageButtonBuy;
                }
                ButtonSprintTime.interactable = true;
                ButtonTextSprintTime.text = BuyText;
            }
            else
            {
                if (ButtonSprintTime.image.sprite != ImageButtonNoCoin)
                {
                    ButtonSprintTime.image.sprite = ImageButtonNoCoin;
                }
                ButtonSprintTime.interactable = false;
                ButtonTextSprintTime.text = NOCoinText;
            }

            PriceTextSprintTime.text = D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentPriceSprintTime.ToString();
        }
        else
        {
            ButtonSprintTime.interactable = false;
            ButtonTextSprintTime.text = FinishUpgradeText;
            ScrollbarAddTimeSprintTime.maxValue = D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddSprintTimeMax;
            ScrollbarAddTimeSprintTime.value = D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentAddSprintTime;
            ButtonSprintTime.image.sprite = ImageButtonNoCoin;
            TextAddTimeSprintTime.text = PlayerPrefs.GetFloat(D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].gameObject.name + "SprintTime") + "/" + D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddSprintTimeMax;
            PriceTextSprintTime.text = FinishUpgradeText.ToString();
        }

        #endregion

        #region SpecialTime
        if (D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentAddSpecialTime < D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddSpecialTimeMax)
        {
            ScrollbarAddTimeSpecialTime.maxValue = D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddSpecialTimeMax;

            if (!PlayerPrefs.HasKey(D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].gameObject.name + "SpecialTime"))
            {
                ScrollbarAddTimeSpecialTime.value = D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentAddSpecialTime;
                TextAddTimeSpecialTime.text = D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentAddSpecialTime + "/" + D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddSpecialTimeMax;
            }
            else
            {
                ScrollbarAddTimeSpecialTime.value = PlayerPrefs.GetFloat(D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].gameObject.name + "SpecialTime");
                TextAddTimeSpecialTime.text = PlayerPrefs.GetFloat(D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].gameObject.name + "SpecialTime") + "/" + D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddSpecialTimeMax;
            }

            if (D3GameAttribute.gameAttribute.Totalcoin >= D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentPriceSpecialTime)
            {
                if (ButtonSpecialTime.image.sprite != ImageButtonBuy)
                {
                    ButtonSpecialTime.image.sprite = ImageButtonBuy;
                }
                ButtonSpecialTime.interactable = true;
                ButtonTextSpecialTime.text = BuyText;
            }
            else
            {
                if (ButtonSpecialTime.image.sprite != ImageButtonNoCoin)
                {
                    ButtonSpecialTime.image.sprite = ImageButtonNoCoin;
                }
                ButtonSpecialTime.interactable = false;
                ButtonTextSpecialTime.text = NOCoinText;
            }
            PriceTextSpecialTime.text = D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentPriceSpecialTime.ToString();

        }
        else
        {
            ButtonSpecialTime.interactable = false;
            ButtonTextSpecialTime.text = FinishUpgradeText;
            ScrollbarAddTimeSpecialTime.maxValue = D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddSpecialTimeMax;
            ScrollbarAddTimeSpecialTime.value = D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentAddSpecialTime;
            ButtonSpecialTime.image.sprite = ImageButtonNoCoin;
            TextAddTimeSpecialTime.text = PlayerPrefs.GetFloat(D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].gameObject.name + "SpecialTime") + "/" + D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddSpecialTimeMax;
            PriceTextSpecialTime.text = FinishUpgradeText.ToString();


        }


        #endregion

        #region MultiplyTime
        if (D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentAddMultiplyTime < D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddMultiplyTimeMax)
        {
            ScrollbarAddTimeMultiplyTime.maxValue = D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddMultiplyTimeMax;

            if (!PlayerPrefs.HasKey(D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].gameObject.name + "MultiplyTime"))
            {
                ScrollbarAddTimeMultiplyTime.value = D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentAddMultiplyTime;
                TextAddTimeMultiplyTime.text = D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentAddMultiplyTime + "/" + D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddMultiplyTimeMax;
            }
            else
            {
                ScrollbarAddTimeMultiplyTime.value = PlayerPrefs.GetFloat(D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].gameObject.name + "MultiplyTime");
                TextAddTimeMultiplyTime.text = PlayerPrefs.GetFloat(D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].gameObject.name + "MultiplyTime") + "/" + D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddMultiplyTimeMax;
            }

            if (D3GameAttribute.gameAttribute.Totalcoin >= D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentPriceMultiplyTime)
            {
                if (ButtonMultiplyTime.image.sprite != ImageButtonBuy)
                {
                    ButtonMultiplyTime.image.sprite = ImageButtonBuy;
                }
                ButtonMultiplyTime.interactable = true;
                ButtonTextMultiplyTime.text = BuyText;
            }
            else
            {
                if (ButtonMultiplyTime.image.sprite != ImageButtonNoCoin)
                {
                    ButtonMultiplyTime.image.sprite = ImageButtonNoCoin;
                }
                ButtonMultiplyTime.interactable = false;
                ButtonTextMultiplyTime.text = NOCoinText;
            }

            PriceTextMultiplyTime.text = D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentPriceMultiplyTime.ToString();
        }
        else
        {
            ButtonMultiplyTime.interactable = false;
            ButtonTextMultiplyTime.text = FinishUpgradeText;
            ScrollbarAddTimeMultiplyTime.maxValue = D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddMultiplyTimeMax;
            ScrollbarAddTimeMultiplyTime.value = D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentAddMultiplyTime;
            ButtonMultiplyTime.image.sprite = ImageButtonNoCoin;
            TextAddTimeMultiplyTime.text = PlayerPrefs.GetFloat(D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].gameObject.name + "MultiplyTime") + "/" + D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddMultiplyTimeMax;
            PriceTextMultiplyTime.text = FinishUpgradeText.ToString();


        }

        #endregion

        #region MagnetTime
        if (D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentAddMagnetTime < D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddMagnetTimeMax)
        {
            ScrollbarAddTimeMagnetTime.maxValue = D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddMagnetTimeMax;

            if (!PlayerPrefs.HasKey(D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].gameObject.name + "MagnetTime"))
            {
                ScrollbarAddTimeMagnetTime.value = D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentAddMagnetTime;
                TextAddTimeMagnetTime.text = D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentAddMagnetTime + "/" + D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddMagnetTimeMax;
            }
            else
            {
                ScrollbarAddTimeMagnetTime.value = PlayerPrefs.GetFloat(D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].gameObject.name + "MagnetTime");
                TextAddTimeMagnetTime.text = PlayerPrefs.GetFloat(D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].gameObject.name + "MagnetTime") + "/" + D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddMagnetTimeMax;
            }

            if (D3GameAttribute.gameAttribute.Totalcoin >= D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentPriceMagnetTime)
            {
                if (ButtonMagnetTime.image.sprite != ImageButtonBuy)
                {
                    ButtonMagnetTime.image.sprite = ImageButtonBuy;
                }
                ButtonMagnetTime.interactable = true;
                ButtonTextMagnetTime.text = BuyText;
            }
            else
            {
                if (ButtonMagnetTime.image.sprite != ImageButtonNoCoin)
                {
                    ButtonMagnetTime.image.sprite = ImageButtonNoCoin;
                }
                ButtonMagnetTime.interactable = false;
                ButtonTextMagnetTime.text = NOCoinText;
            }

            PriceTextMagnetTime.text = D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentPriceMagnetTime.ToString();
        }
        else
        {
            ButtonMagnetTime.interactable = false;
            ButtonTextMagnetTime.text = FinishUpgradeText;
            ScrollbarAddTimeMagnetTime.maxValue = D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddMagnetTimeMax;
            ScrollbarAddTimeMagnetTime.value = D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentAddMagnetTime;
            ButtonMagnetTime.image.sprite = ImageButtonNoCoin;
            TextAddTimeMagnetTime.text = PlayerPrefs.GetFloat(D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].gameObject.name + "MagnetTime") + "/" + D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddMagnetTimeMax;
            PriceTextMagnetTime.text = FinishUpgradeText.ToString();


        }

        #endregion

        #region ShieldTime
        if (D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentAddShieldTime < D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddShieldTimeMax)
        {
            ScrollbarAddTimeShieldTime.maxValue = D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddShieldTimeMax;

            if (!PlayerPrefs.HasKey(D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].gameObject.name + "ShieldTime"))
            {
                ScrollbarAddTimeShieldTime.value = D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentAddShieldTime;
                TextAddTimeShieldTime.text = D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddShieldTime + "/" + D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddShieldTimeMax;
            }
            else
            {
                ScrollbarAddTimeShieldTime.value = PlayerPrefs.GetFloat(D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].gameObject.name + "ShieldTime");
                TextAddTimeShieldTime.text = PlayerPrefs.GetFloat(D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].gameObject.name + "ShieldTime") + "/" + D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddShieldTimeMax;
            }

            if (D3GameAttribute.gameAttribute.Totalcoin >= D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentPriceShieldTime)
            {
                if (ButtonShieldTime.image.sprite != ImageButtonBuy)
                {
                    ButtonShieldTime.image.sprite = ImageButtonBuy;
                }
                ButtonShieldTime.interactable = true;
                ButtonTextShieldTime.text = BuyText;
            }
            else
            {
                if (ButtonShieldTime.image.sprite != ImageButtonNoCoin)
                {
                    ButtonShieldTime.image.sprite = ImageButtonNoCoin;
                }
                ButtonShieldTime.interactable = false;
                ButtonTextShieldTime.text = NOCoinText;
            }

            PriceTextShieldTime.text = D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentPriceShieldTime.ToString();
        }
        else
        {
            ButtonShieldTime.interactable = false;
            ButtonTextShieldTime.text = FinishUpgradeText;
            ScrollbarAddTimeShieldTime.maxValue = D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddShieldTimeMax;
            ScrollbarAddTimeShieldTime.value = D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentAddShieldTime;
            ButtonShieldTime.image.sprite = ImageButtonNoCoin;
            TextAddTimeShieldTime.text = PlayerPrefs.GetFloat(D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].gameObject.name + "ShieldTime") + "/" + D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddShieldTimeMax;
            PriceTextShieldTime.text = FinishUpgradeText.ToString();


        }

        #endregion


    }


    public void BuySprintTime()
    {
        if (D3SoundManager.instance != null)
            D3SoundManager.instance.PlayingSound("Button");

        if (D3GameAttribute.gameAttribute.Totalcoin >= D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentPriceSprintTime)
        {
            int Coinsave = PlayerPrefs.GetInt("Coin") - D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentPriceSprintTime;
            D3GameData.SaveCoin(Coinsave);

            D3GameAttribute.gameAttribute.Totalcoin = D3GameData.LoadCoin();

            D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddSprintTimeVoid(MultiplyPriceWhenPurchasing);

            UpdateText();

        }else
        {
            UpdateText();
        }


    }


    public void BuySpecialTime()
    {
        if (D3SoundManager.instance != null)
            D3SoundManager.instance.PlayingSound("Button");

        if (D3GameAttribute.gameAttribute.Totalcoin >= D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentPriceSpecialTime)
        {
            int Coinsave = PlayerPrefs.GetInt("Coin") - D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentPriceSpecialTime;
            D3GameData.SaveCoin(Coinsave);

            D3GameAttribute.gameAttribute.Totalcoin = D3GameData.LoadCoin();

            D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddSpecialTimeVoid(MultiplyPriceWhenPurchasing);

            UpdateText();

        } else
        {
            UpdateText();
        }

    }


    public void BuyMultiplyTime()
    {
        if (D3SoundManager.instance != null)
            D3SoundManager.instance.PlayingSound("Button");

        if (D3GameAttribute.gameAttribute.Totalcoin >= D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentPriceMultiplyTime)
        {
            int Coinsave = PlayerPrefs.GetInt("Coin") - D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentPriceMultiplyTime;
            D3GameData.SaveCoin(Coinsave);

            D3GameAttribute.gameAttribute.Totalcoin = D3GameData.LoadCoin();

            D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddMultiplyTimeVoid(MultiplyPriceWhenPurchasing);

            UpdateText();

        }else
        {
            UpdateText();
        }

    }

    public void BuyMagnetTime()
    {
        if (D3SoundManager.instance != null)
            D3SoundManager.instance.PlayingSound("Button");

        if (D3GameAttribute.gameAttribute.Totalcoin >= D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentPriceMagnetTime)
        {
            int Coinsave = PlayerPrefs.GetInt("Coin") - D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentPriceMagnetTime;
            D3GameData.SaveCoin(Coinsave);

            D3GameAttribute.gameAttribute.Totalcoin = D3GameData.LoadCoin();

            D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddMagnetTimeVoid(MultiplyPriceWhenPurchasing);

            UpdateText();

        }else
        {
            UpdateText();
        }

    }

    public void BuyShieldTime()
    {
        if (D3SoundManager.instance != null)
            D3SoundManager.instance.PlayingSound("Button");

        if (D3GameAttribute.gameAttribute.Totalcoin >= D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentPriceShieldTime)
        {
            int Coinsave = PlayerPrefs.GetInt("Coin") - D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].CurrentPriceShieldTime;
            D3GameData.SaveCoin(Coinsave);

            D3GameAttribute.gameAttribute.Totalcoin = D3GameData.LoadCoin();

            D3GameController.instace.playerPref[D3GameController.instace.selectPlayer].AddShieldTimeVoid(MultiplyPriceWhenPurchasing);

            UpdateText();

        }else
        {
            UpdateText();
        }

    }


    private void AppearWindow(GameObject window)
	{
		if (window == null)
		{
			return;
		}

		var anim = window.GetComponent<Animator>();

		if (anim == null)
		{
			return;
		}

		anim.SetTrigger(_appearTrigger);
	}

	private void DisappearWindow(GameObject window)
	{
		if (window == null)
		{
			return;
		}

		var anim = window.GetComponent<Animator>();

		if (anim == null)
		{
			return;
		}
		anim.SetTrigger(_disappearTrigger);
	}

	void Update()
	{
		if (Application.isPlaying == false)
		{
			if (calOnGUI == null)
			{
				calOnGUI = GetComponent<D3CalOnGUI>();
			}
		}
		else
		{
			if (D3PatternSystem.instance.loadingComplete == true)
			{
				InGameGui.SetActive(true);
				CoinTObj.SetActive(true);
				LifeTObj.SetActive(true);
				if (D3GameAttribute.gameAttribute.UseStartSystem)
				{
					if ((int)D3GameAttribute.gameAttribute.distance > GetStart1)
					{
						if (!StartON1.activeSelf)
							StartON1.SetActive(true);
					}
					if ((int)D3GameAttribute.gameAttribute.distance > Getstart2)
					{
						if (!StartON2.activeSelf)
							StartON2.SetActive(true);
					}
					if ((int)D3GameAttribute.gameAttribute.distance > Getstart3)
					{
						if (!StartON3.activeSelf)
						{
                            StartON3.SetActive(true);
                        }
						if (D3GameController.instace && D3GameAttribute.gameAttribute)
						{
							if (D3GameAttribute.gameAttribute.isPlaying)
							{
								D3GameAttribute.gameAttribute.isPlaying = false;

                                pauseInSpecial();

                                D3GameController.instace.StartCoroutine(D3GameController.instace.ResetGame());

								D3Controller.instace.GetComponent<Animator>().PlayInFixedTime("WinGameUI");
								D3EnemyController.instance.GetComponent<Animator>().PlayInFixedTime("Idle");
                            }

                        }

                    }

					if ((int)D3GameAttribute.gameAttribute.distance < GetStart1)
					{
                        Textdistance.text = (int)D3GameAttribute.gameAttribute.distance +" / " +D3GameAttribute.gameAttribute.ScoreToWin.ToString();
					}
					if (StartON1.activeSelf)
					{
                        Textdistance.text = (int)D3GameAttribute.gameAttribute.distance + " / " + Getstart2.ToString(); 
					}
					if (StartON2.activeSelf)
					{
                        Textdistance.text = (int)D3GameAttribute.gameAttribute.distance + " / " + Getstart3.ToString();
					}

				}
				else {
                    if (D3GameAttribute.gameAttribute.UseRandomScore)
                    {
                        Textdistance.text = (int)D3GameAttribute.gameAttribute.distance + " / " + D3GameAttribute.gameAttribute.ScoreToWin.ToString();
                    }
                    if (!D3GameAttribute.gameAttribute.UseRandomScore)
                    {
                        Textdistance.text = TextScore + (int)D3GameAttribute.gameAttribute.distance;
                    }
                }


				ButtonHoverBoard.gameObject.SetActive(true);
				CoinT.text = D3GameAttribute.gameAttribute.Totalcoin.ToString();
				LifeT.text = D3GameAttribute.gameAttribute.lifeSave.ToString();
				TextCoinCollect.text = "" + D3GameAttribute.gameAttribute.coin;


				if (D3Controller.instace != null)
				{
					CantHoverBoard.text = D3GameAttribute.gameAttribute.HoveBoard.ToString();

					if (D3GameAttribute.gameAttribute.HoveBoard > 0 || !D3Controller.instace.IsHoverboard || !D3Controller.instace.isSprint)
					{
						if (!ButtonHoverBoard.interactable)
						{
							ButtonHoverBoard.interactable = true;
						}


						for (int i = 0; menuButtonSet.Count > i; i++)
						{
							if (menuButtonSet[i].buttonType == ButtonType.pause)
							{
								if (!menuButtonSet[i].button.interactable)
								{
									menuButtonSet[i].button.interactable = true;
								}

							}
						}
					}

					if (D3GameAttribute.gameAttribute.HoveBoard <= 0 || D3Controller.instace.IsHoverboard || D3Controller.instace.isSprint || D3Controller.instace.IsSpecial)
					{
						if (ButtonHoverBoard.interactable)
						{
							ButtonHoverBoard.interactable = false;
						}
					}

					if (D3Controller.instace.isSprint)
					{
						for (int i = 0; menuButtonSet.Count > i; i++)
						{
							if (menuButtonSet[i].buttonType == ButtonType.pause)
							{
								if (menuButtonSet[i].button.interactable)
								{
									menuButtonSet[i].button.interactable = false;
								}
							}
						}
					}

					for (int i = 0; i < itemStateSet.Count; i++)
					{
						if (itemStateSet[i].itemType == D3Item.TypeItem.ItemJump)
						{
							if (D3Controller.instace.timeJump > 0)
							{
								itemStateSet[i].guiTexture.transform.gameObject.SetActive(true);
								itemStateSet[i].guiText.transform.gameObject.SetActive(true);
								ShowGUI(i, D3Controller.instace.timeJump);
							}
							else
							{
								itemStateSet[i].guiTexture.transform.gameObject.SetActive(false);
								itemStateSet[i].guiText.transform.gameObject.SetActive(false);
							}
						}

						if (itemStateSet[i].itemType == D3Item.TypeItem.ItemMagnet)
						{
							if (D3Controller.instace.timeMagnet > 0)
							{
								itemStateSet[i].guiTexture.transform.gameObject.SetActive(true);
								itemStateSet[i].guiText.transform.gameObject.SetActive(true);
								ShowGUI(i, D3Controller.instace.timeMagnet);
							}
							else
							{
								itemStateSet[i].guiTexture.transform.gameObject.SetActive(false);
								itemStateSet[i].guiText.transform.gameObject.SetActive(false);
							}
						}

						if (itemStateSet[i].itemType == D3Item.TypeItem.ItemMultiply)
						{
							if (D3Controller.instace.timeMultiply > 0)
							{
								itemStateSet[i].guiTexture.transform.gameObject.SetActive(true);
								itemStateSet[i].guiText.transform.gameObject.SetActive(true);
								ShowGUI(i, D3Controller.instace.timeMultiply);
							}
							else
							{
								itemStateSet[i].guiTexture.transform.gameObject.SetActive(false);
								itemStateSet[i].guiText.transform.gameObject.SetActive(false);
							}
						}

						if (itemStateSet[i].itemType == D3Item.TypeItem.ItemSprint)
						{
							if (D3Controller.instace.timeSprint > 0)
							{
								itemStateSet[i].guiTexture.transform.gameObject.SetActive(true);
								itemStateSet[i].guiText.transform.gameObject.SetActive(true);
								ShowGUI(i, D3Controller.instace.timeSprint);
							}
							else
							{
								itemStateSet[i].guiTexture.transform.gameObject.SetActive(false);
								itemStateSet[i].guiText.transform.gameObject.SetActive(false);
							}
						}
						if (itemStateSet[i].itemType == D3Item.TypeItem.ItemShield)
						{
							if (D3Controller.instace._ImmortalityTime > 0)
							{
								itemStateSet[i].guiTexture.transform.gameObject.SetActive(true);
								itemStateSet[i].guiText.transform.gameObject.SetActive(true);
								ShowGUI(i, D3Controller.instace._ImmortalityTime);
							}
							else
							{
								itemStateSet[i].guiTexture.transform.gameObject.SetActive(false);
								itemStateSet[i].guiText.transform.gameObject.SetActive(false);
							}
						}
                        if (itemStateSet[i].itemType == D3Item.TypeItem.ItemSpecial)
                        {
                            if (D3Controller.instace.timeSpecial > 0)
                            {
                                itemStateSet[i].guiTexture.transform.gameObject.SetActive(true);
                                itemStateSet[i].guiText.transform.gameObject.SetActive(true);
                                ShowGUI(i, D3Controller.instace.timeSpecial);
                            }
                            else
                            {
                                itemStateSet[i].guiTexture.transform.gameObject.SetActive(false);
                                itemStateSet[i].guiText.transform.gameObject.SetActive(false);
                            }
                        }
                    }
				}

				for (int i = 0; i < menuButtonSet.Count; i++)
				{
					CheckTypeButtonActive(i);

				}
			}
			else
			{
				TextCoinCollect.text = "";
				Textdistance.text = "";
			}
		}

	}

	void RestartStart()
	{
        if (StartON1 != null)
        {
            StartON1.SetActive(false);
        }
        if (StartON2 != null)
        {
            StartON2.SetActive(false);
        }
        if (StartON3 != null)
        {
            StartON3.SetActive(false);
        }
        if (StartOFF1 != null)
        {
            StartOFF1.SetActive(false);
        }
        if (StartOFF2 != null)
        {
            StartOFF2.SetActive(false);
        }
        if (StartOFF3 != null)
        {
            StartOFF3.SetActive(false);
        }
        if (D3GameAttribute.gameAttribute.UseStartSystem)
        {
            GetStart1 = D3GameAttribute.gameAttribute.ScoreToWin;
            Getstart2 = D3GameAttribute.gameAttribute.ScoreToWin * 2;
            Getstart3 = D3GameAttribute.gameAttribute.ScoreToWin * 4;

            if (StartOFF1 != null)
            {
                StartOFF1.SetActive(true);
            }
            if (StartOFF2 != null)
            {
                StartOFF2.SetActive(true);
            }
            if (StartOFF3 != null)
            {
                StartOFF3.SetActive(true);
            }

        }
    }

	public void ActivateHoverBoard()
	{
		if (D3GameAttribute.gameAttribute.HoveBoard >= 1)
		{
			if (SoundManager != null)
				SoundManager.PlayingSound("Button");
			int HoveBoard = (int)D3GameAttribute.gameAttribute.HoveBoard - 1;
			D3GameData.SaveHoveBoard(HoveBoard);
			D3GameAttribute.gameAttribute.HoveBoard = D3GameData.LoadHoveBoard();
			D3Controller.instace.ActivateHoverboard();

		}

	}


	//Check hide/active button
	private void CheckTypeButtonActive(int i)
	{
		if (menuButtonSet[i].buttonType == ButtonType.exit)
		{
			if (D3GameAttribute.gameAttribute.pause == true)
			{
				menuButtonSet[i].button.transform.gameObject.SetActive(true);
			}
			else
			{
				menuButtonSet[i].button.transform.gameObject.SetActive(false);

			}
		}

		if (menuButtonSet[i].buttonType == ButtonType.pause)
		{
			if (D3GameAttribute.gameAttribute.pause == false)
			{
				menuButtonSet[i].button.transform.gameObject.SetActive(true);
			}
			else
			{
				menuButtonSet[i].button.transform.gameObject.SetActive(false);
			}
		}

		if (menuButtonSet[i].buttonType == ButtonType.restart)
		{
			if (D3GameAttribute.gameAttribute.pause == true)
			{
				menuButtonSet[i].button.transform.gameObject.SetActive(true);
			}
			else
			{
				menuButtonSet[i].button.transform.gameObject.SetActive(false);
			}
		}

		if (menuButtonSet[i].buttonType == ButtonType.resume)
		{
			if (D3GameAttribute.gameAttribute.pause == true)
			{
				menuButtonSet[i].button.transform.gameObject.SetActive(true);
			}
			else
			{
				menuButtonSet[i].button.transform.gameObject.SetActive(false);
			}
		}

		if (menuButtonSet[i].buttonType == ButtonType.sumExit)
		{
			if (D3GameAttribute.gameAttribute.life <= 0 && D3GameAttribute.gameAttribute.showSumGUI == true)
			{
				menuButtonSet[i].button.transform.gameObject.SetActive(true);
			}
			else
			{
				menuButtonSet[i].button.transform.gameObject.SetActive(false);
			}
		}
	}

	//This method use to input command in button

	//exit button back to title
	public void exit()
	{
		if (SoundManager != null)
			SoundManager.PlayingSound("Button");
#if UNITY_ADS && ENABLE_UNITY_ADS
        if (D3GameController.instace.EnableADSOnScene)
        {
            if (D3GameController.instace.UseBanner)
            {
                if (D3ADSManager.D3AdsManager != null)
                {
                    D3ADSManager.D3AdsManager.DestroyBanner();

                }
            }

            D3AddressablesManager.LoadSceneByName(D3GameAttribute.gameAttribute.SceneTitleToload);

        }
        else
        {
            D3AddressablesManager.LoadSceneByName(D3GameAttribute.gameAttribute.SceneTitleToload);
        }

#endif
#if !UNITY_ADS || !ENABLE_UNITY_ADS
           D3AddressablesManager.LoadSceneByName(D3GameAttribute.gameAttribute.SceneTitleToload);
#endif
    }

    //pause button
    public void pause()
	{
		if (SoundManager != null)
			SoundManager.PlayingSound("Button");

        if (D3GameController.instace.SpawnPlayerOnDead)
        {
            D3GameController.instace.PlayerCam.GetComponent<Animator>().PlayInFixedTime("PauseUI");
            D3GameController.instace.PlayerCam.GetComponent<Animator>().SetBool("IsUIManger", true);
        }

        if (D3GameAttribute.gameAttribute.life > 0 && D3GameAttribute.gameAttribute != null)
		{
			D3Controller.instace.PauseOn();
			D3GameAttribute.gameAttribute.Pause(true);
			InGameGui.SetActive(false);
			GameOverGui.SetActive(false);
			WinGameGui.SetActive(false);
			PauseGui.SetActive(true);
			SettingsGUI.SetActive(false);
			AppearWindow(PauseGui);
			PauseScore.text = TextScore + (int)D3GameAttribute.gameAttribute.distance;
            if (D3GameAttribute.gameAttribute.UseRandomScore)
            {
                PauseToWinScore.gameObject.SetActive(true);
                PauseToWinScore.text = TextToWinInfo + D3GameAttribute.gameAttribute.ScoreToWin;
            }
            if (!D3GameAttribute.gameAttribute.UseRandomScore)
            {
                PauseToWinScore.gameObject.SetActive(false);
            }

            PauseBestScore.text = TextBestScore + D3GameAttribute.gameAttribute.BestScore;
		}
	}

    public void pauseInSpecial()
    {
        if (D3GameAttribute.gameAttribute.life > 0 && D3GameAttribute.gameAttribute != null)
        {
            D3GameAttribute.gameAttribute.Pause(true);
        }
    }

    public void resumeInSpecial()
    {
        D3Controller.instace.PauseOff();
        D3GameAttribute.gameAttribute.Pause(false);
        
    }

    //restart button
    public void restart()
	{
        if (SoundManager != null)
			SoundManager.PlayingSound("Button");
        Fade_In();
        RestartStart();
        if (D3Controller.instace)
        {

            D3Controller.instace.ControlSpecial = true;
            D3Controller.instace.PauseOn();
            
        }
        
		PauseGui.SetActive(false);
		DisappearWindow(PauseGui);
        D3AddressablesManager.LoadSceneByName(SceneManager.GetActiveScene().name);
    }

	//resume button
	public void resume()
	{
		if (SoundManager != null)
			SoundManager.PlayingSound("Button");
		D3Controller.instace.PauseOff();
		D3GameAttribute.gameAttribute.Pause(false);
		PauseGui.SetActive(false);
		DisappearWindow(PauseGui);
		GameOverGui.SetActive(false);
		WinGameGui.SetActive(false);
		InGameGui.SetActive(true);
		SettingsGUI.SetActive(false);
		AppearWindow(InGameGui);
	}

	//exit button in summary screen
	public void sumExit()
	{
		if (SoundManager != null)
			SoundManager.PlayingSound("Button");
#if UNITY_ADS && ENABLE_UNITY_ADS
        if (D3GameController.instace.EnableADSOnScene)
        {
            if (D3GameController.instace.UseBanner)
            {
                if (D3ADSManager.D3AdsManager != null)
                {
                    D3ADSManager.D3AdsManager.DestroyBanner();

                }
            }
            D3AddressablesManager.LoadSceneByName(D3GameAttribute.gameAttribute.SceneTitleToload);

        }
        else
        {
            D3AddressablesManager.LoadSceneByName(D3GameAttribute.gameAttribute.SceneTitleToload);
        }

#endif
#if !UNITY_ADS || !ENABLE_UNITY_ADS
           D3AddressablesManager.LoadSceneByName(D3GameAttribute.gameAttribute.SceneTitleToload);
#endif
    }

    private void ShowGUI(int i, float time)
	{
		if (itemStateSet[i].guiText != null)
		{
			itemStateSet[i].guiText.text = time.ToString("0") + "s";
		}
	}

	public void Gameover()
	{
        D3GameAttribute.gameAttribute.Pause(true);
        if ((int)D3GameAttribute.gameAttribute.distance >= D3GameAttribute.gameAttribute.ScoreToWin)
		{
			int BestScore = D3GameData.LoadBestScore();
           
			if ((int)D3GameAttribute.gameAttribute.distance > BestScore)
			{
				D3GameData.SaveBestScore((int)D3GameAttribute.gameAttribute.distance);
				WindowBestScore();
			}
			else {
                WinGame();
            }
			
		}
		if ((int)D3GameAttribute.gameAttribute.distance < D3GameAttribute.gameAttribute.ScoreToWin)
		{
			NoWinGame();
		}

	}

	public void WindowBestScore()
	{
        InGameGui.SetActive(false);
        PauseGui.SetActive(false);
        WinGameGui.SetActive(false);
        GameOverGui.SetActive(false);
        SettingsGUI.SetActive(false);
        HeroGui.SetActive(false);
        BestScoreGUI.SetActive(true);

		if (D3GameController.instace.SpawnPlayerOnDead)
		{
            D3GameController.instace.PlayerCam.GetComponent<Animator>().PlayInFixedTime("BestScoreUI");
            D3GameController.instace.PlayerCam.GetComponent<Animator>().SetBool("IsUIManger", true);
		}


        BestScoreText.text = TextBestScore + (int)D3GameData.LoadBestScore();
        AppearWindow(BestScoreGUI);
    }

	
    public void CloseWindowBestScore()
    {
        DisappearWindow(BestScoreGUI);
        BestScoreGUI.SetActive(false);
        WinGame();
    }

    public void WinGame()
	{
        if (!D3GameAttribute.gameAttribute.UseRandomScore && !D3GameAttribute.gameAttribute.UseSpecificScore)
        {
            NoWinGame();

        }
        if (D3GameAttribute.gameAttribute.UseRandomScore || D3GameAttribute.gameAttribute.UseSpecificScore)
        {
            int NewScore = (int)D3GameAttribute.gameAttribute.Totalcoin + (int)D3GameAttribute.gameAttribute.coin;
            D3GameData.SaveCoin(NewScore);
            InGameGui.SetActive(false);
            PauseGui.SetActive(false);
            GameOverGui.SetActive(false);
            HeroGui.SetActive(false);
            WinGameGui.SetActive(true);
            SettingsGUI.SetActive(false);



            if (D3GameController.instace.SpawnPlayerOnDead)
            {
                D3GameController.instace.PlayerCam.GetComponent<Animator>().PlayInFixedTime("WinGameUI");
                D3GameController.instace.PlayerCam.GetComponent<Animator>().SetBool("IsUIManger", true);
            }

            if (D3GameAttribute.gameAttribute.UseStartSystem)
            {
                if ((int)D3GameAttribute.gameAttribute.distance > GetStart1)
                {
                    if (!StartWinON1.activeSelf)
                    {
                        if (SoundManager != null)
                            SoundManager.PlayingSound("GetItem");
                        StartWinON1.SetActive(true);

                    }
                    StartWinOFF1.SetActive(true);
                }
                else
                {
                    if (StartWinON1.activeSelf)
                        StartWinON1.SetActive(false);
                    StartWinOFF1.SetActive(false);
                }
                if ((int)D3GameAttribute.gameAttribute.distance > Getstart2)
                {
                    if (!StartWinON2.activeSelf)
                    {
                        StartWinON2.SetActive(true);
                    }
                    StartWinOFF2.SetActive(true);

                }
                else
                {
                    if (StartWinON2.activeSelf)
                        StartWinON2.SetActive(false);
                    StartWinOFF2.SetActive(false);
                }
                if ((int)D3GameAttribute.gameAttribute.distance > Getstart3)
                {
                    if (!StartWinON3.activeSelf)
                    {
                        StartWinON3.SetActive(true);
                    }
                    StartWinOFF3.SetActive(true);

                }
                else
                {
                    if (StartWinON3.activeSelf)
                        StartWinON3.SetActive(false);
                    StartWinOFF3.SetActive(false);
                }
            }
            else
            {
                if (StartWinON1.activeSelf)
                    StartWinON1.SetActive(false);
                if (StartWinON2.activeSelf)
                    StartWinON2.SetActive(false);
                if (StartWinON3.activeSelf)
                    StartWinON3.SetActive(false);
                if (StartWinON1.activeSelf)
                    StartWinOFF1.SetActive(false);
                if (StartWinOFF2.activeSelf)
                    StartWinOFF2.SetActive(false);
                if (StartWinOFF3.activeSelf)
                    StartWinOFF3.SetActive(false);

            }
            AppearWindow(WinGameGui);
#if UNITY_ADS && ENABLE_UNITY_ADS
            if (D3GameController.instace.EnableInterstitialADSOnWim)
            {
                if (D3ADSManager.D3AdsManager.EnableUnityADS)
                {
                    D3ADSManager.D3AdsManager.ShowUnityInterstitialADS();
                }

            }
#endif
            WinScore.text = D3GameAttribute.gameAttribute.coin.ToString();


            WinDistance.text = TextScore + (int)D3GameAttribute.gameAttribute.distance;
            WinTextBestScore.text = TextBestScore + (int)D3GameData.LoadBestScore();

            if ((int)D3GameAttribute.gameAttribute.distance > GetStart1 && (int)D3GameAttribute.gameAttribute.distance < Getstart2)
            {
                if (D3LevelSystemManager.Instance)
                {
                    if (D3LevelSystemManager.Instance.EnabledLevelSystem) 
                        D3LevelSystemManager.Instance.CompletedLevel(D3LevelSystemManager.Instance.CurrentLevel, 1);
                }
            }
            if ((int)D3GameAttribute.gameAttribute.distance > Getstart2 && (int)D3GameAttribute.gameAttribute.distance < Getstart3)
            {
                if (D3LevelSystemManager.Instance)
                {
                    if (D3LevelSystemManager.Instance.EnabledLevelSystem)
                        D3LevelSystemManager.Instance.CompletedLevel(D3LevelSystemManager.Instance.CurrentLevel, 2);
                }
            }
            if ((int)D3GameAttribute.gameAttribute.distance > Getstart3)
            {
                if (D3LevelSystemManager.Instance)
                {
                    if (D3LevelSystemManager.Instance.EnabledLevelSystem)
                        D3LevelSystemManager.Instance.CompletedLevel(D3LevelSystemManager.Instance.CurrentLevel, 3);
                }
            }

        }
       
	}

	public void OpenSettings()
	{
		if (SoundManager != null)
		{
			SoundManager.PlayingSound("Button");
			loadVolumen();
            HeroGui.SetActive(false);
            SettingsGUI.SetActive(true);
			AppearWindow(SettingsGUI);
		}
	}

	public void loadVolumen()
	{
		SoundManager.SFXSound.volume = SoundManager.SFXVolume;
		SoundManager.bgmSound.volume = SoundManager.musicVolume;
		if (SFXVolumeSlider != null)
			SFXVolumeSlider.value = SoundManager.SFXVolume;
		if (musicVolumeSlider != null)
			musicVolumeSlider.value = SoundManager.musicVolume;
	}

	public void OnSFXVolumeSliderUpdated()
	{
		if (SFXVolumeSlider != null && SoundManager != null)
		{
			SoundManager.SFXVolume = SFXVolumeSlider.value;
			SoundManager.SFXSound.volume = SoundManager.SFXVolume;
		}

	}

	public void OnMusicVolumeSliderUpdated()
	{
		if (musicVolumeSlider != null && SoundManager != null)
		{
			SoundManager.musicVolume = musicVolumeSlider.value;
			SoundManager.bgmSound.volume = SoundManager.musicVolume;
		}

	}

	public void CloseSettingsAndSaveChangues(bool Savechangue)
	{
		if (SoundManager != null)
		{
			if (Savechangue)
			{
				SoundManager.OnAudioDataUpdated();
			}
			SoundManager.PlayingSound("Button");
		}

		SettingsGUI.SetActive(false);
		DisappearWindow(SettingsGUI);
	}
	public void NoWinGame()
	{
		InGameGui.SetActive(false);
		PauseGui.SetActive(false);
		WinGameGui.SetActive(false);
		GameOverGui.SetActive(true);
        HeroGui.SetActive(false);
        SettingsGUI.SetActive(false);
		AppearWindow(GameOverGui);
#if UNITY_ADS && ENABLE_UNITY_ADS
        if (D3GameController.instace.EnableInterstitialADSOnGameOver)
        {
            if (D3ADSManager.D3AdsManager.EnableUnityADS)
            {
                D3ADSManager.D3AdsManager.ShowUnityInterstitialADS();
            }
        }
#endif


        if (D3GameController.instace.SpawnPlayerOnDead)
		{
            D3GameController.instace.PlayerCam.GetComponent<Animator>().PlayInFixedTime("GameOverUI");
            D3GameController.instace.PlayerCam.GetComponent<Animator>().SetBool("IsUIManger", true);
        }

        if (D3GameAttribute.gameAttribute.lifeSave > 0)
		{
			int LifeT = (int)D3GameAttribute.gameAttribute.lifeSave - 1;
			D3GameData.SaveLife(LifeT);
			D3GameAttribute.gameAttribute.lifeSave = D3GameData.LoadLife();
		}
		if (D3GameAttribute.gameAttribute.lifeSave >= 1)
		{
			ButtonRevival.interactable = true;
		}
		if (D3GameAttribute.gameAttribute.lifeSave <= 0)
		{
			ButtonRevival.interactable = false;
		}
		GameOverScore.text = TextScore + (int)D3GameAttribute.gameAttribute.distance;

        if (D3GameAttribute.gameAttribute.UseRandomScore)
        {
            GameToWinScore.gameObject.SetActive(true);
            GameToWinScore.text = TextToWinInfo + D3GameAttribute.gameAttribute.ScoreToWin;
        }
        if (!D3GameAttribute.gameAttribute.UseRandomScore)
        {
            GameToWinScore.gameObject.SetActive(false);
        }

		GameOverBestScore.text = TextBestScore + D3GameAttribute.gameAttribute.BestScore;
	}

    public void OpenHeroWindow()
    {
        if (D3SoundManager.instance != null)
            D3SoundManager.instance.PlayingSound("Button");
        D3SoundManager.instance.loadVolumen();
        HeroGui.SetActive(true);
		UpdateText();
        AppearWindow(HeroGui);
    }

    public void OpenRewardWindow()
    {
        if (D3SoundManager.instance != null)
            D3SoundManager.instance.PlayingSound("Button");
        RewardWindow.SetActive(true);

        AppearWindow(RewardWindow);
    }
    public void CloseRewardWindow()
    {
        if (D3SoundManager.instance != null)
            D3SoundManager.instance.PlayingSound("Button");

        RewardWindow.SetActive(false);
        DisappearWindow(RewardWindow);
    }
    public void CloseHeroWindow()
    {
        if (D3SoundManager.instance != null)
            D3SoundManager.instance.PlayingSound("Button");
        UpdateText();
        HeroGui.SetActive(false);
        DisappearWindow(HeroGui);
    }

    public void Reset()
	{
		for (int i = 0; i < menuButtonSet.Count; i++)
		{
			menuButtonSet[i].button.transform.gameObject.SetActive(false);
		}

		for (int i = 0; i < itemStateSet.Count; i++)
		{
			itemStateSet[i].guiTexture.transform.gameObject.SetActive(false);
			itemStateSet[i].guiText.transform.gameObject.SetActive(false);
		}

		D3GameAttribute.gameAttribute.showSumGUI = false;
        D3CameraFollow.instace.Reset();
    }

	public void NewLevel()
	{
        if (SoundManager != null)
			SoundManager.PlayingSound("Button");
#if UNITY_ADS && ENABLE_UNITY_ADS
        if (D3GameController.instace.EnableADSOnScene)
        {
            if (D3GameController.instace.UseBanner)
            {
                if (D3ADSManager.D3AdsManager != null)
                {
                    if (D3ADSManager.D3AdsManager.EnableUnityADS)
                    {
                        D3ADSManager.D3AdsManager.DestroyBanner();
                    }

                }
            }
            if (D3LevelSystemManager.Instance)
            {
                D3LevelSystemManager.Instance.LoadNewLevel();
            }
            else {
                Debug.LogError("Scene is not assigned to the Level");
                return;
            }

        }
        else
        {
            if (D3LevelSystemManager.Instance)
            {
                D3LevelSystemManager.Instance.LoadNewLevel();
            }
            else
            {
                Debug.LogError("Scene is not assigned to the Level");
                return;
            }
        }

#endif
#if !UNITY_ADS || !ENABLE_UNITY_ADS
        if (D3LevelSystemManager.Instance)
        {
            D3LevelSystemManager.Instance.LoadNewLevel();
        }
        else
        {
            Debug.LogError("Scene is not assigned to the Level");
            return;
        }
#endif

    }


    public void ClickOnSaveMeButtonUsingCoin()
	{
		if (D3GameAttribute.gameAttribute.lifeSave >= 1)
		{
			if (SoundManager != null)
				SoundManager.PlayingSound("Button");
			int LifeT = (int)D3GameAttribute.gameAttribute.lifeSave - 1;
            D3Controller.instace.ControlSpecial = true;
            D3GameData.SaveLife(LifeT);
			Revival();
		}
	}

	public void Revival()
	{
        Fade_In();
        if (D3Controller.instace)
        {
            D3Controller.instace.PauseOn();
        }
		if (GameOverGui.activeInHierarchy)
		{
            DisappearWindow(GameOverGui);
            GameOverGui.SetActive(false);
        }
		SettingsGUI.SetActive(false);
		InGameGui.SetActive(true);
		AppearWindow(InGameGui);
		D3GameAttribute.gameAttribute.Revival();

        D3Controller.instace.Revive();
    }


	public void Fade_In()
	{
		
        PanelFade.SetActive(true);
        PanelFade.GetComponent<Image>().enabled = true;
        StartCoroutine(FadeIn());
    }
	public void Fade_Out()
	{
        PanelFade.SetActive(true);
        PanelFade.GetComponent<Image>().enabled = true;
        StartCoroutine(FadeOut());

    }

	IEnumerator FadeIn()
	{
        yield return new WaitForSeconds(TimeFade);

    }

    IEnumerator FadeOut()
    {
        float i = 1;
        while (i >= 1)
        {
            PanelFade.GetComponent<Image>().color = new Color(255f, 255f, 255f, i);
            i -= 0.01f;
            yield return new WaitForSeconds(TimeFade);
        }
        PanelFade.GetComponent<Image>().enabled = false;
        PanelFade.SetActive(false);
		if (D3Controller.instace)
		{
            D3Controller.instace.ColActivate=true;

        }
        resumeInSpecial();
        if (D3EnemyController.instance)
        {
            D3EnemyController.instance.statusPlay = true;
            D3EnemyController.instance.ReStart();

        }
    }



}

