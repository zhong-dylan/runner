using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;


#if UNITY_ADS && ENABLE_UNITY_ADS
using UnityEngine.Advertisements;
#endif

public class D3ADSManager : MonoBehaviour
#if UNITY_ADS && ENABLE_UNITY_ADS
	, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
#endif
{
    public static D3ADSManager D3AdsManager;


    public string UNITY_ADSID_ANDROID = "5574755";
    public string UNITY_ADSID_IOS = "5574754";

#if UNITY_ADS && ENABLE_UNITY_ADS
	public string _adVideoUnitId = null; // This will remain null for unsupported platforms
    public string _AdVideoUnitName = "Rewarded_Android";
    public string _adInterstitialUnitId = null; // This will remain null for unsupported platforms
    public string _AdInterstitiaUnitName = "Interstitial_Android";
    public string _adBannerUnitId = null; // This will remain null for unsupported platforms
    public string _AdBannerUnitName = "Banner_Android";
    public string _gameId;
	public BannerPosition _bannerPosition;
#endif

    public bool EnableUnityADS = true;
    public bool EnableTESTMODE = true;

    public UnityEvent ShowCompleteUnityBanner;
    public UnityEvent BannerDestroyOrHide;
    public UnityEvent ShowCompleteUnityRewardedVideo;
    public UnityEvent SkippedUnityRewardedVideo;

    public UnityEvent ShowCompleteUnityInterstitial;
    public UnityEvent SkippedUnityinterstitial;

    public D3TitleCharacter TitleScene;
    public D3GameController GameScene;

    public int IDButton = 0;

    public bool ADSReady = false;
    public bool ADSInsterstitialReady = false;
    public bool ADSRewardReady = false;

    public int Unlocked = 0;

    public enum D3TypeADS
    {
        Interstitial, VideoRewarded
    }

    #region Initialize Ads

    public void InitializeAds()
    {
#if UNITY_ADS && ENABLE_UNITY_ADS
        Unlocked = PlayerPrefs.GetInt("EnableUnityADS");
        if (Unlocked == 0)
        {
            EnableUnityADS = true;
        }
        if (Unlocked == 1)
        {
            EnableUnityADS = false;
        }

        _gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
                ? UNITY_ADSID_IOS
                : UNITY_ADSID_ANDROID;

        Advertisement.Initialize(_gameId, EnableTESTMODE, this);
        ADSReady = false;
        ADSRewardReady = false;
#endif
    }
    void Start()
    {
        if (D3AdsManager == null)
        {
            D3AdsManager = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
            InitializeAds();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region Example ADD Item, Coin , Hoverboard

    public void AddLife(int cant)
    {
        if (D3SoundManager.instance != null)
            D3SoundManager.instance.PlayingSound("Button");

        if (TitleScene)
        {
            int lifesave = PlayerPrefs.GetInt("Life") + cant;
            D3GameData.SaveLife(lifesave);
            D3TitleCharacter.instance.UpdateText();
        }
        if (GameScene)
        {
            int LifeT = D3GameAttribute.gameAttribute.lifeSave + cant;
            D3GameData.SaveLife(LifeT);
            D3GameAttribute.gameAttribute.lifeSave = D3GameData.LoadLife();
        }
        
    }

    public void ADDCoin(int cant)
    {

        if (D3SoundManager.instance != null)
            D3SoundManager.instance.PlayingSound("Button");

        
        if (TitleScene)
        {
            int Coinsave = PlayerPrefs.GetInt("Coin") + cant;
            D3GameData.SaveCoin(Coinsave);
            D3TitleCharacter.instance.UpdateText();
        }
        if (GameScene)
        {
            D3GameAttribute.gameAttribute.coin += cant;

        }

    }


    public void AddHoverBoard(int cant)
    {
        if (D3SoundManager.instance != null)
            D3SoundManager.instance.PlayingSound("Button");

        if (TitleScene)
        {
            int Hoversave = PlayerPrefs.GetInt("HoveBoard") + cant;
            D3GameData.SaveHoveBoard(Hoversave);
            D3TitleCharacter.instance.UpdateText();
        }
        if (GameScene)
        {
            int Hoversave = PlayerPrefs.GetInt("HoveBoard") + cant;
            D3GameData.SaveHoveBoard(Hoversave);
            D3GameAttribute.gameAttribute.HoveBoard = D3GameData.LoadHoveBoard();
        }
    }

    #endregion
    private void Update()
    {
        if (GameScene == null)
        {
            if (FindObjectOfType<D3GameController>())
            {
                GameScene = FindObjectOfType<D3GameController>();
            }
        }
        if (TitleScene == null)
        {
            if (FindObjectOfType<D3TitleCharacter>())
            {
                TitleScene = FindObjectOfType<D3TitleCharacter>();
            }
        }

        if (GameScene != null)
        {
            if (GameScene.EnableADSOnScene && !EnableUnityADS)
            {
                GameScene.EnableADSOnScene = false;
                if (GameScene.UseBanner)
                {
#if UNITY_ADS && ENABLE_UNITY_ADS
                    DestroyBanner();
#endif
                }
            }

        }
        if (TitleScene != null)
        {
            if (TitleScene.EnableADSOnScene && !EnableUnityADS)
            {
                TitleScene.EnableADSOnScene = false;
                if (TitleScene.UseBanner)
                {
#if UNITY_ADS && ENABLE_UNITY_ADS
                    DestroyBanner();
#endif
                }
            }
        }
    }

    public void OnUnityNoADS(int Id)
    {
        IDButton = Id;
        if (TitleScene != null)
        {
            if (TitleScene.ListRewardedADButtons.Count > 0)
            {
                for (int i = 0; TitleScene.ListRewardedADButtons.Count > i; i++)
                {
                    if (TitleScene.ListRewardedADButtons[i].showAdButton != null)
                    {
                        TitleScene.ListRewardedADButtons[i].showAdButton.interactable = false;
                    }
                    if (i == IDButton)
                    {
                        if (TitleScene.ListRewardedADButtons[i].TypeReward == D3ADSTypeReward.D3TypeReward.None)
                        {
                            Debug.Log("Unity Ads Rewarded NONE");
                        }
                        else
                        {
                            if (TitleScene.ListRewardedADButtons[i].TypeReward == D3ADSTypeReward.D3TypeReward.Life)
                            {
                                if (TitleScene.EnableRewardedWindow)
                                {
                                    if (TitleScene.RewardWindow)
                                    {
                                        if (TitleScene.RewardWindow.GetComponent<D3RewardWindow>())
                                        {
                                            if (TitleScene.RewardWindow.GetComponent<D3RewardWindow>().TextReward != null)
                                            {
                                                TitleScene.RewardWindow.GetComponent<D3RewardWindow>().TextReward.text = "Life + " + TitleScene.ListRewardedADButtons[i].CantReward.ToString();
                                            }
                                            else
                                            {
                                                TitleScene.RewardWindow.GetComponent<D3RewardWindow>().TextReward.text = "";
                                            }
                                            if (TitleScene.RewardWindow.GetComponent<D3RewardWindow>().ImageReward != null)
                                            {
                                                TitleScene.RewardWindow.GetComponent<D3RewardWindow>().ImageReward.sprite = TitleScene.ListRewardedADButtons[i].ImgReward;
                                            }
                                            else
                                            {
                                                TitleScene.RewardWindow.GetComponent<D3RewardWindow>().ImageReward.sprite = null;
                                            }
                                            TitleScene.OpenRewardWindow();
                                        }
                                    }

                                }

                                AddLife(TitleScene.ListRewardedADButtons[i].CantReward);
                                Debug.Log("Unity Ads Rewarded: " + TitleScene.ListRewardedADButtons[i].TypeReward + " " + "Cant:  " + TitleScene.ListRewardedADButtons[i].CantReward);
                            }
                            if (TitleScene.ListRewardedADButtons[i].TypeReward == D3ADSTypeReward.D3TypeReward.HoverboardKeyUse)
                            {
                                if (TitleScene.EnableRewardedWindow)
                                {
                                    if (TitleScene.RewardWindow)
                                    {
                                        if (TitleScene.RewardWindow.GetComponent<D3RewardWindow>())
                                        {
                                            if (TitleScene.RewardWindow.GetComponent<D3RewardWindow>().TextReward != null)
                                            {
                                                TitleScene.RewardWindow.GetComponent<D3RewardWindow>().TextReward.text = "Hoverboard Key Use + " + TitleScene.ListRewardedADButtons[i].CantReward.ToString();
                                            }
                                            else
                                            {
                                                TitleScene.RewardWindow.GetComponent<D3RewardWindow>().TextReward.text = "";
                                            }
                                            if (TitleScene.RewardWindow.GetComponent<D3RewardWindow>().ImageReward != null)
                                            {
                                                TitleScene.RewardWindow.GetComponent<D3RewardWindow>().ImageReward.sprite = TitleScene.ListRewardedADButtons[i].ImgReward;
                                            }
                                            else
                                            {
                                                TitleScene.RewardWindow.GetComponent<D3RewardWindow>().ImageReward.sprite = null;
                                            }
                                            TitleScene.OpenRewardWindow();
                                        }
                                    }

                                }
                                AddHoverBoard(TitleScene.ListRewardedADButtons[i].CantReward);
                                Debug.Log("Unity Ads Rewarded: " + TitleScene.ListRewardedADButtons[i].TypeReward + " " + "Cant:  " + TitleScene.ListRewardedADButtons[i].CantReward);
                            }
                            if (TitleScene.ListRewardedADButtons[i].TypeReward == D3ADSTypeReward.D3TypeReward.Coin)
                            {
                                if (TitleScene.EnableRewardedWindow)
                                {
                                    if (TitleScene.RewardWindow)
                                    {
                                        if (TitleScene.RewardWindow.GetComponent<D3RewardWindow>())
                                        {
                                            if (TitleScene.RewardWindow.GetComponent<D3RewardWindow>().TextReward != null)
                                            {
                                                TitleScene.RewardWindow.GetComponent<D3RewardWindow>().TextReward.text = "Coin + " + TitleScene.ListRewardedADButtons[i].CantReward.ToString();
                                            }
                                            else
                                            {
                                                TitleScene.RewardWindow.GetComponent<D3RewardWindow>().TextReward.text = "";
                                            }
                                            if (TitleScene.RewardWindow.GetComponent<D3RewardWindow>().ImageReward != null)
                                            {
                                                TitleScene.RewardWindow.GetComponent<D3RewardWindow>().ImageReward.sprite = TitleScene.ListRewardedADButtons[i].ImgReward;
                                            }
                                            else
                                            {
                                                TitleScene.RewardWindow.GetComponent<D3RewardWindow>().ImageReward.sprite = null;
                                            }
                                            TitleScene.OpenRewardWindow();
                                        }
                                    }

                                }
                                ADDCoin(TitleScene.ListRewardedADButtons[i].CantReward);
                                Debug.Log("Unity Ads Rewarded: " + TitleScene.ListRewardedADButtons[i].TypeReward + " " + "Cant:  " + TitleScene.ListRewardedADButtons[i].CantReward);
                            }

                        }

                    }
                }
            }

        }

        if (GameScene != null)
        {
            if (D3PanelRewardADS.instance)
            {
                if (D3PanelRewardADS.instance.EnabledPanelRewardADS && D3PanelRewardADS.instance.RewardButton != null)
                {
                    if (D3GameController.instace.ListRandomRewardedAD.Count > 0)
                    {
                        D3PanelRewardADS.instance.RewardButton.interactable = false;

                        for (int i = 0; D3GameController.instace.ListRandomRewardedAD.Count > i; i++)
                        {
                            if (i == D3GameController.instace.RewardSelect)
                            {
                                Debug.Log("Rencompensa ID : " + i);

                                if (D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>())
                                {
                                    Debug.Log("Se encontro la ventana");

                                    if (D3GameController.instace.ListRandomRewardedAD[i].TypeReward == D3ADSRandomReward.D3TypeRandomReward.Null)
                                    {
                                        Debug.Log("Unity Ads Rewarded NONE");
                                    }
                                    else
                                    {
                                        if (D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().TextReward == null)
                                        {
                                            D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().TextReward.text = "";
                                        }

                                        if (D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().ImageReward == null)
                                        {
                                            D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().ImageReward.sprite = null;
                                        }

                                        if (D3GameController.instace.ListRandomRewardedAD[i].TypeReward == D3ADSRandomReward.D3TypeRandomReward.Life)
                                        {
                                            if (D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().TextReward != null)
                                            {
                                                D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().TextReward.text = "Life + " + D3GameController.instace.ListRandomRewardedAD[i].CantReward.ToString();
                                            }
                                            if (D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().ImageReward != null)
                                            {
                                                D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().ImageReward.sprite = D3GameController.instace.ListRandomRewardedAD[i].ImgReward;
                                            }

                                            AddLife(D3GameController.instace.ListRandomRewardedAD[i].CantReward);
                                            Debug.Log("Unity Ads Rewarded: " + D3GameController.instace.ListRandomRewardedAD[i].TypeReward + " " + "Cant:  " + D3GameController.instace.ListRandomRewardedAD[i].CantReward);
                                        }

                                        if (D3GameController.instace.ListRandomRewardedAD[i].TypeReward == D3ADSRandomReward.D3TypeRandomReward.HoverboardKeyUse)
                                        {
                                            if (D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().TextReward != null)
                                            {
                                                D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().TextReward.text = "Hoverboard Key Use + " + D3GameController.instace.ListRandomRewardedAD[i].CantReward.ToString();
                                            }
                                            if (D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().ImageReward != null)
                                            {
                                                D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().ImageReward.sprite = D3GameController.instace.ListRandomRewardedAD[i].ImgReward;
                                            }

                                            AddHoverBoard(D3GameController.instace.ListRandomRewardedAD[i].CantReward);
                                            Debug.Log("Unity Ads Rewarded: " + D3GameController.instace.ListRandomRewardedAD[i].TypeReward + " " + "Cant:  " + D3GameController.instace.ListRandomRewardedAD[i].CantReward);
                                        }

                                        if (D3GameController.instace.ListRandomRewardedAD[i].TypeReward == D3ADSRandomReward.D3TypeRandomReward.Coin)
                                        {
                                            if (D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().TextReward != null)
                                            {
                                                D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().TextReward.text = "Coin + " + D3GameController.instace.ListRandomRewardedAD[i].CantReward.ToString();
                                            }
                                            if (D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().ImageReward != null)
                                            {
                                                D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().ImageReward.sprite = D3GameController.instace.ListRandomRewardedAD[i].ImgReward;
                                            }

                                            ADDCoin(D3GameController.instace.ListRandomRewardedAD[i].CantReward);
                                            Debug.Log("Unity Ads Rewarded: " + D3GameController.instace.ListRandomRewardedAD[i].TypeReward + " " + "Cant:  " + D3GameController.instace.ListRandomRewardedAD[i].CantReward);
                                        }

                                        if (D3GameController.instace.ListRandomRewardedAD[i].TypeReward == D3ADSRandomReward.D3TypeRandomReward.ItemJump)
                                        {
                                            if (D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().TextReward != null)
                                            {
                                                D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().TextReward.text = "Item Jump Duration " + D3GameController.instace.ListRandomRewardedAD[i].duration.ToString();
                                            }
                                            if (D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().ImageReward != null)
                                            {
                                                D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().ImageReward.sprite = D3GameController.instace.ListRandomRewardedAD[i].ImgReward;
                                            }
                                            D3Controller.instace.JumpDouble(D3GameController.instace.ListRandomRewardedAD[i].duration);
                                            Debug.Log("Unity Ads Rewarded: " + D3GameController.instace.ListRandomRewardedAD[i].TypeReward + " " + "Duration:  " + D3GameController.instace.ListRandomRewardedAD[i].duration);
                                        }

                                        if (D3GameController.instace.ListRandomRewardedAD[i].TypeReward == D3ADSRandomReward.D3TypeRandomReward.ItemMagnet)
                                        {
                                            if (D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().TextReward != null)
                                            {
                                                D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().TextReward.text = "Item Magnet Duration " + D3GameController.instace.ListRandomRewardedAD[i].duration.ToString();
                                            }
                                            if (D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().ImageReward != null)
                                            {
                                                D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().ImageReward.sprite = D3GameController.instace.ListRandomRewardedAD[i].ImgReward;
                                            }

                                            D3Controller.instace.Magnet(D3GameController.instace.ListRandomRewardedAD[i].duration);
                                            Debug.Log("Unity Ads Rewarded: " + D3GameController.instace.ListRandomRewardedAD[i].TypeReward + " " + "Duration:  " + D3GameController.instace.ListRandomRewardedAD[i].duration);
                                        }

                                        if (D3GameController.instace.ListRandomRewardedAD[i].TypeReward == D3ADSRandomReward.D3TypeRandomReward.ItemMultiply)
                                        {
                                            if (D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().TextReward != null)
                                            {
                                                D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().TextReward.text = "Item Multiply Duration " + D3GameController.instace.ListRandomRewardedAD[i].duration.ToString();
                                            }
                                            if (D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().ImageReward != null)
                                            {
                                                D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().ImageReward.sprite = D3GameController.instace.ListRandomRewardedAD[i].ImgReward;
                                            }

                                            D3Controller.instace.Multiply(D3GameController.instace.ListRandomRewardedAD[i].duration);
                                            D3GameAttribute.gameAttribute.multiplyValue = 2;
                                            Debug.Log("Unity Ads Rewarded: " + D3GameController.instace.ListRandomRewardedAD[i].TypeReward + " " + "Duration:  " + D3GameController.instace.ListRandomRewardedAD[i].duration);
                                        }

                                        if (D3GameController.instace.ListRandomRewardedAD[i].TypeReward == D3ADSRandomReward.D3TypeRandomReward.ItemShield)
                                        {
                                            if (D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().TextReward != null)
                                            {
                                                D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().TextReward.text = "Item Shield Duration " + D3GameController.instace.ListRandomRewardedAD[i].duration.ToString();
                                            }
                                            if (D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().ImageReward != null)
                                            {
                                                D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().ImageReward.sprite = D3GameController.instace.ListRandomRewardedAD[i].ImgReward;
                                            }
                                            D3Controller.instace.ItemImmortality(D3GameController.instace.ListRandomRewardedAD[i].duration);
                                            Debug.Log("Unity Ads Rewarded: " + D3GameController.instace.ListRandomRewardedAD[i].TypeReward + " " + "Duration:  " + D3GameController.instace.ListRandomRewardedAD[i].duration);
                                        }
                                        D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().AutoClose = true;
                                        D3GUIManager.instance.OpenRewardWindow();
                                    }

                                }

                            }
                        }

                    }

                    D3Controller.instace.PauseOff();

                    if (D3PanelRewardADS.instance)
                    {
                        D3PanelRewardADS.instance.EnabledPanel();
                    }

                    D3GameAttribute.gameAttribute.Pause(false);
                }
            }
        }

    }



#if UNITY_ADS && ENABLE_UNITY_ADS

    public void DisableADS()
    {
        EnableUnityADS = false;
       
        if (GameScene != null)
        {
            GameScene.EnableADSOnScene = false;
            if (GameScene.UseBanner)
            {
                DestroyBanner();
            }

        }
        if (TitleScene != null)
        {
            TitleScene.EnableADSOnScene= false;
            if (TitleScene.UseBanner)
            {
                DestroyBanner();
            }
        }
    }

    public void EnabledADS()
    {
        EnableUnityADS = true;
        if (GameScene != null)
        {
            GameScene.EnableADSOnScene = true;
        }
        if (TitleScene != null)
        {
            TitleScene.EnableADSOnScene = true;
        }
    }
    
    
    public void OnUnityAdsRewarded(int Id)
    {
        IDButton = Id;
        if (TitleScene != null)
        {
            if (TitleScene.EnableRewardedADOnScene)
            {
                if (TitleScene.ListRewardedADButtons.Count > 0)
                {
                    for (int i = 0; TitleScene.ListRewardedADButtons.Count > i; i++)
                    {
                        if (TitleScene.ListRewardedADButtons[i].showAdButton != null)
                        {
                            TitleScene.ListRewardedADButtons[i].showAdButton.interactable = false;
                        }
                    }
                }
                ShowRewardedUnityVideo();
            }
        }

        if (GameScene != null)
        {
            if (GameScene.EnableRewardedADOnScene)
            {
                if (D3PanelRewardADS.instance)
                {
                    if (D3PanelRewardADS.instance.EnabledPanelRewardADS && D3PanelRewardADS.instance.RewardButton != null)
                    {
                        if (D3GameController.instace.ListRandomRewardedAD.Count > 0)
                        {
                            D3PanelRewardADS.instance.RewardButton.interactable = false;
                        }
                    }
                }
                ShowRewardedUnityVideo();
            }
        }

    }

    ///////Unity Interstitial
    #region Unity Interstitial
    public void LoadInstertialADS()
    {
        if (EnableUnityADS)
        {
#if UNITY_IOS
		_adInterstitialUnitId = _AdInterstitiaUnitName;
#elif UNITY_ANDROID
            _adInterstitialUnitId = _AdInterstitiaUnitName;
#endif
            Advertisement.Load(_adInterstitialUnitId, this);
        }
        else
        {
            Debug.Log("Unity ADS Disable");
        }
    }

    public void ShowUnityInterstitialADS()
    {
        if (ADSReady)
        {
            if (ADSInsterstitialReady)
            {
                Advertisement.Show(_adInterstitialUnitId, this);
                LoadInstertialADS();

            }
            else
            {
                LoadInstertialADS();

                if (TitleScene != null)
                {
                    if (TitleScene.EnableInterstitialADSOnScene)
                    {
                        if (D3LevelSystemManager.Instance)
                        {
                            if (D3LevelSystemManager.Instance.EnabledLevelSystem)
                            {
                                TitleScene.OpenLevelSystemWindow();
                            }
                            else
                            {
                                if (TitleScene.GameScene != null)
                                {
                                    if (SceneUtility.GetBuildIndexByScenePath(TitleScene.GameScene) == 0)
                                    {
                                        Debug.LogError("Could not find scene name '" + TitleScene.GameScene + "'");
                                        return;
                                    }
                                    else
                                    {
                                        SceneManager.LoadScene(TitleScene.GameScene);
                                    }
                                }
                                else
                                {
                                    Debug.LogError("Scene Game is not assigned in Tab Level System");
                                    return;
                                }
                            }
                        }
                        else
                        {
                            if (TitleScene.GameScene != null)
                            {
                                if (SceneUtility.GetBuildIndexByScenePath(TitleScene.GameScene) == 0)
                                {
                                    Debug.LogError("Could not find scene name '" + TitleScene.GameScene + "'");
                                    return;
                                }
                                else
                                {
                                    SceneManager.LoadScene(TitleScene.GameScene);
                                }
                            }
                            else
                            {
                                Debug.LogError("Scene Game is not assigned in Tab Level System");
                                return;
                            }
                        }
                    }
                }

            }

        }
        else
        {
            InitializeAds();
            if (TitleScene != null)
            {
                if (TitleScene.EnableInterstitialADSOnScene)
                {
                    if (D3LevelSystemManager.Instance)
                    {
                        if (D3LevelSystemManager.Instance.EnabledLevelSystem)
                        {
                            TitleScene.OpenLevelSystemWindow();
                        }
                        else
                        {
                            if (TitleScene.GameScene != null)
                            {
                                if (SceneUtility.GetBuildIndexByScenePath(TitleScene.GameScene) == 0)
                                {
                                    Debug.LogError("Could not find scene name '" + TitleScene.GameScene + "'");
                                    return;
                                }
                                else
                                {
                                    SceneManager.LoadScene(TitleScene.GameScene);
                                }
                            }
                            else
                            {
                                Debug.LogError("Scene Game is not assigned in Tab Level System");
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (TitleScene.GameScene != null)
                        {
                            if (SceneUtility.GetBuildIndexByScenePath(TitleScene.GameScene) == 0)
                            {
                                Debug.LogError("Could not find scene name '" + TitleScene.GameScene + "'");
                                return;
                            }
                            else
                            {
                                SceneManager.LoadScene(TitleScene.GameScene);
                            }
                        }
                        else
                        {
                            Debug.LogError("Scene Game is not assigned in Tab Level System");
                            return;
                        }
                    }
                }
            }
        }
    }


    #endregion

    ///////Unity Rewarded
    #region Unity Rewarded

    public void LoadRewardedUnityVideo()
    {

#if UNITY_IOS
			_adVideoUnitId = _AdVideoUnitName;
#elif UNITY_ANDROID
        _adVideoUnitId = _AdVideoUnitName;
#endif
        if (TitleScene != null)
        {
            if (TitleScene.EnableRewardedADOnScene)
            {
                if (TitleScene.ListRewardedADButtons.Count > 0)
                {
                    for (int i = 0; TitleScene.ListRewardedADButtons.Count > i; i++)
                    {
                        if (TitleScene.ListRewardedADButtons[i].showAdButton != null)
                        {
                            TitleScene.ListRewardedADButtons[i].showAdButton.interactable = false;
                        }
                    }
                }
            }
        }

        if (GameScene != null)
        {
            if (GameScene.EnableRewardedADOnScene)
            {
                if (D3PanelRewardADS.instance)
                {
                    if (D3PanelRewardADS.instance.EnabledPanelRewardADS && D3PanelRewardADS.instance.RewardButton != null)
                    {
                        if (D3GameController.instace.ListRandomRewardedAD.Count > 0)
                        {
                            D3PanelRewardADS.instance.RewardButton.interactable = false;
                        }
                    }
                }
            }
        }

        Advertisement.Load(_adVideoUnitId, this);
        
    }

    public void ShowRewardedUnityVideo()
    {
#if UNITY_ADS && ENABLE_UNITY_ADS
        Advertisement.Show(_adVideoUnitId, this);

#endif
#if !UNITY_ADS || !ENABLE_UNITY_ADS
         Debug.Log("Unity ADS Disable");
         OnUnityNoADS(IDButton);
#endif
    }

    #endregion


    /////Unity Banner
    #region Unity Banner

    public void RequestBanner()
    {
#if UNITY_IOS
				_adBannerUnitId = _AdBannerUnitName;
#elif UNITY_ANDROID
        _adBannerUnitId = _AdBannerUnitName;
#endif

        if (EnableUnityADS)
        {
#if UNITY_ADS && ENABLE_UNITY_ADS
            Advertisement.Banner.SetPosition(_bannerPosition);

            BannerLoadOptions options = new BannerLoadOptions
            {
                loadCallback = OnBannerLoaded,
                errorCallback = OnBannerError
            };

            Advertisement.Banner.Load(_adBannerUnitId, options);
            ShowUnityBanner();
#endif
        }

        if (!EnableUnityADS)
        {
            Debug.Log("Unity ADS Disable");
        }

    }

    public void DestroyBanner()
    {
        BannerDestroyOrHide.Invoke();

        Advertisement.Banner.Hide();
    }

    void ShowUnityBanner()
	{
#if UNITY_IOS
				_adBannerUnitId = _AdBannerUnitName;
#elif UNITY_ANDROID
        _adBannerUnitId = _AdBannerUnitName;
#endif

        BannerOptions options2 = new BannerOptions
		{
			clickCallback = OnBannerClicked,
			hideCallback = OnBannerHidden,
			showCallback = OnBannerShown
		};

		Advertisement.Banner.Show(_adBannerUnitId, options2);

	}
	void OnBannerLoaded()
	{
        ShowUnityBanner();
    }
	void OnBannerError(string message)
	{
		Debug.Log($"Banner Error: {message}");
		// Optionally execute additional code, such as attempting to load another ad.
	}
	void OnBannerClicked() { }
	void OnBannerShown() 
	{
		ShowCompleteUnityBanner.Invoke();
	}
	void OnBannerHidden() { }
    #endregion

    //// Unity Events
    #region Unity Events
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adVideoUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            if (TitleScene != null)
            {
                if (TitleScene.EnableRewardedADOnScene)
                {
                    if (TitleScene.ListRewardedADButtons.Count > 0)
                    {
                        for (int i = 0; TitleScene.ListRewardedADButtons.Count > i; i++)
                        {
                            if (i == IDButton)
                            {
                                if (TitleScene.ListRewardedADButtons[i].TypeReward == D3ADSTypeReward.D3TypeReward.None)
                                {
                                    Debug.Log("Unity Ads Rewarded NONE");
                                }
                                else
                                {
                                    if (TitleScene.ListRewardedADButtons[i].TypeReward == D3ADSTypeReward.D3TypeReward.Life)
                                    {
                                        if (TitleScene.EnableRewardedWindow)
                                        {
                                            if (TitleScene.RewardWindow)
                                            {
                                                if (TitleScene.RewardWindow.GetComponent<D3RewardWindow>())
                                                {
                                                    if (TitleScene.RewardWindow.GetComponent<D3RewardWindow>().TextReward != null)
                                                    {
                                                        TitleScene.RewardWindow.GetComponent<D3RewardWindow>().TextReward.text = "Life + "+TitleScene.ListRewardedADButtons[i].CantReward.ToString();
                                                    }
                                                    else
                                                    {
                                                        TitleScene.RewardWindow.GetComponent<D3RewardWindow>().TextReward.text = "";
                                                    }
                                                    if (TitleScene.RewardWindow.GetComponent<D3RewardWindow>().ImageReward != null)
                                                    {
                                                        TitleScene.RewardWindow.GetComponent<D3RewardWindow>().ImageReward.sprite = TitleScene.ListRewardedADButtons[i].ImgReward;
                                                    }
                                                    else {
                                                        TitleScene.RewardWindow.GetComponent<D3RewardWindow>().ImageReward.sprite = null;
                                                    }
                                                    TitleScene.OpenRewardWindow();
                                                }
                                            }

                                        }

                                        AddLife(TitleScene.ListRewardedADButtons[i].CantReward);
                                        Debug.Log("Unity Ads Rewarded: " + TitleScene.ListRewardedADButtons[i].TypeReward + " " + "Cant:  " + TitleScene.ListRewardedADButtons[i].CantReward);
                                    }
                                    if (TitleScene.ListRewardedADButtons[i].TypeReward == D3ADSTypeReward.D3TypeReward.HoverboardKeyUse)
                                    {
                                        if (TitleScene.EnableRewardedWindow)
                                        {
                                            if (TitleScene.RewardWindow)
                                            {
                                                if (TitleScene.RewardWindow.GetComponent<D3RewardWindow>())
                                                {
                                                    if (TitleScene.RewardWindow.GetComponent<D3RewardWindow>().TextReward != null)
                                                    {
                                                        TitleScene.RewardWindow.GetComponent<D3RewardWindow>().TextReward.text = "Hoverboard Key Use + " + TitleScene.ListRewardedADButtons[i].CantReward.ToString();
                                                    }
                                                    else {
                                                        TitleScene.RewardWindow.GetComponent<D3RewardWindow>().TextReward.text = "";
                                                    }
                                                    if (TitleScene.RewardWindow.GetComponent<D3RewardWindow>().ImageReward != null)
                                                    {
                                                        TitleScene.RewardWindow.GetComponent<D3RewardWindow>().ImageReward.sprite = TitleScene.ListRewardedADButtons[i].ImgReward;
                                                    }
                                                    else
                                                    {
                                                        TitleScene.RewardWindow.GetComponent<D3RewardWindow>().ImageReward.sprite = null;
                                                    }
                                                    TitleScene.OpenRewardWindow();
                                                }
                                            }

                                        }
                                        AddHoverBoard(TitleScene.ListRewardedADButtons[i].CantReward);
                                        Debug.Log("Unity Ads Rewarded: " + TitleScene.ListRewardedADButtons[i].TypeReward + " " + "Cant:  " + TitleScene.ListRewardedADButtons[i].CantReward);
                                    }
                                    if (TitleScene.ListRewardedADButtons[i].TypeReward == D3ADSTypeReward.D3TypeReward.Coin)
                                    {
                                        if (TitleScene.EnableRewardedWindow)
                                        {
                                            if (TitleScene.RewardWindow)
                                            {
                                                if (TitleScene.RewardWindow.GetComponent<D3RewardWindow>())
                                                {
                                                    if (TitleScene.RewardWindow.GetComponent<D3RewardWindow>().TextReward != null)
                                                    {
                                                        TitleScene.RewardWindow.GetComponent<D3RewardWindow>().TextReward.text = "Coin + " + TitleScene.ListRewardedADButtons[i].CantReward.ToString();
                                                    }
                                                    else
                                                    {
                                                        TitleScene.RewardWindow.GetComponent<D3RewardWindow>().TextReward.text = "";
                                                    }
                                                    if (TitleScene.RewardWindow.GetComponent<D3RewardWindow>().ImageReward != null)
                                                    {
                                                        TitleScene.RewardWindow.GetComponent<D3RewardWindow>().ImageReward.sprite = TitleScene.ListRewardedADButtons[i].ImgReward;
                                                    }
                                                    else
                                                    {
                                                        TitleScene.RewardWindow.GetComponent<D3RewardWindow>().ImageReward.sprite = null;
                                                    }
                                                    TitleScene.OpenRewardWindow();
                                                }
                                            }

                                        }
                                        ADDCoin(TitleScene.ListRewardedADButtons[i].CantReward);
                                        Debug.Log("Unity Ads Rewarded: " + TitleScene.ListRewardedADButtons[i].TypeReward + " " + "Cant:  " + TitleScene.ListRewardedADButtons[i].CantReward);
                                    }

                                }

                            }
                        }
                    }
                }
            }

            if (GameScene != null)
            {
                if (GameScene.EnableRewardedADOnScene && GameScene.EnableRewardedWindow)
                {
                    if (D3PanelRewardADS.instance)
                    {
                        if (D3PanelRewardADS.instance.EnabledPanelRewardADS)
                        {
                            if (D3GameController.instace.ListRandomRewardedAD.Count > 0)
                            {
                                D3PanelRewardADS.instance.RewardButton.interactable = false;

                                for (int i = 0; D3GameController.instace.ListRandomRewardedAD.Count > i; i++)
                                {
                                    if (i == D3GameController.instace.RewardSelect)
                                    {
                                        Debug.Log("Rencompensa ID : " + i);

                                        if (D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>())
                                        {
                                            Debug.Log("Se encontro la ventana");

                                            if (D3GameController.instace.ListRandomRewardedAD[i].TypeReward == D3ADSRandomReward.D3TypeRandomReward.Null)
                                            {
                                                Debug.Log("Unity Ads Rewarded NONE");
                                            }
                                            else
                                            {
                                                if (D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().TextReward == null)
                                                {
                                                    D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().TextReward.text = "";
                                                }

                                                if (D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().ImageReward == null)
                                                {
                                                    D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().ImageReward.sprite = null;
                                                }

                                                if (D3GameController.instace.ListRandomRewardedAD[i].TypeReward == D3ADSRandomReward.D3TypeRandomReward.Life)
                                                {
                                                    if (D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().TextReward != null)
                                                    {
                                                        D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().TextReward.text = "Life + " + D3GameController.instace.ListRandomRewardedAD[i].CantReward.ToString();
                                                    }
                                                    if (D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().ImageReward != null)
                                                    {
                                                        D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().ImageReward.sprite = D3GameController.instace.ListRandomRewardedAD[i].ImgReward;
                                                    }

                                                    AddLife(D3GameController.instace.ListRandomRewardedAD[i].CantReward);
                                                    Debug.Log("Unity Ads Rewarded: " + D3GameController.instace.ListRandomRewardedAD[i].TypeReward + " " + "Cant:  " + D3GameController.instace.ListRandomRewardedAD[i].CantReward);
                                                }

                                                if (D3GameController.instace.ListRandomRewardedAD[i].TypeReward == D3ADSRandomReward.D3TypeRandomReward.HoverboardKeyUse)
                                                {
                                                    if (D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().TextReward != null)
                                                    {
                                                        D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().TextReward.text = "Hoverboard Key Use + " + D3GameController.instace.ListRandomRewardedAD[i].CantReward.ToString();
                                                    }
                                                    if (D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().ImageReward != null)
                                                    {
                                                        D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().ImageReward.sprite = D3GameController.instace.ListRandomRewardedAD[i].ImgReward;
                                                    }

                                                    AddHoverBoard(D3GameController.instace.ListRandomRewardedAD[i].CantReward);
                                                    Debug.Log("Unity Ads Rewarded: " + D3GameController.instace.ListRandomRewardedAD[i].TypeReward + " " + "Cant:  " + D3GameController.instace.ListRandomRewardedAD[i].CantReward);
                                                }

                                                if (D3GameController.instace.ListRandomRewardedAD[i].TypeReward == D3ADSRandomReward.D3TypeRandomReward.Coin)
                                                {
                                                    if (D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().TextReward != null)
                                                    {
                                                        D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().TextReward.text = "Coin + " + D3GameController.instace.ListRandomRewardedAD[i].CantReward.ToString();
                                                    }
                                                    if (D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().ImageReward != null)
                                                    {
                                                        D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().ImageReward.sprite = D3GameController.instace.ListRandomRewardedAD[i].ImgReward;
                                                    }

                                                    ADDCoin(D3GameController.instace.ListRandomRewardedAD[i].CantReward);
                                                    Debug.Log("Unity Ads Rewarded: " + D3GameController.instace.ListRandomRewardedAD[i].TypeReward + " " + "Cant:  " + D3GameController.instace.ListRandomRewardedAD[i].CantReward);
                                                }

                                                if (D3GameController.instace.ListRandomRewardedAD[i].TypeReward == D3ADSRandomReward.D3TypeRandomReward.ItemJump)
                                                {
                                                    if (D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().TextReward != null)
                                                    {
                                                        D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().TextReward.text = "Item Jump Duration " + D3GameController.instace.ListRandomRewardedAD[i].duration.ToString();
                                                    }
                                                    if (D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().ImageReward != null)
                                                    {
                                                        D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().ImageReward.sprite = D3GameController.instace.ListRandomRewardedAD[i].ImgReward;
                                                    }
                                                    D3Controller.instace.JumpDouble(D3GameController.instace.ListRandomRewardedAD[i].duration);
                                                    Debug.Log("Unity Ads Rewarded: " + D3GameController.instace.ListRandomRewardedAD[i].TypeReward + " " + "Duration:  " + D3GameController.instace.ListRandomRewardedAD[i].duration);
                                                }

                                                if (D3GameController.instace.ListRandomRewardedAD[i].TypeReward == D3ADSRandomReward.D3TypeRandomReward.ItemMagnet)
                                                {
                                                    if (D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().TextReward != null)
                                                    {
                                                        D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().TextReward.text = "Item Magnet Duration " + D3GameController.instace.ListRandomRewardedAD[i].duration.ToString();
                                                    }
                                                    if (D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().ImageReward != null)
                                                    {
                                                        D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().ImageReward.sprite = D3GameController.instace.ListRandomRewardedAD[i].ImgReward;
                                                    }

                                                    D3Controller.instace.Magnet(D3GameController.instace.ListRandomRewardedAD[i].duration);
                                                    Debug.Log("Unity Ads Rewarded: " + D3GameController.instace.ListRandomRewardedAD[i].TypeReward + " " + "Duration:  " + D3GameController.instace.ListRandomRewardedAD[i].duration);
                                                }

                                                if (D3GameController.instace.ListRandomRewardedAD[i].TypeReward == D3ADSRandomReward.D3TypeRandomReward.ItemMultiply)
                                                {
                                                    if (D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().TextReward != null)
                                                    {
                                                        D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().TextReward.text = "Item Multiply Duration " + D3GameController.instace.ListRandomRewardedAD[i].duration.ToString();
                                                    }
                                                    if (D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().ImageReward != null)
                                                    {
                                                        D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().ImageReward.sprite = D3GameController.instace.ListRandomRewardedAD[i].ImgReward;
                                                    }

                                                    D3Controller.instace.Multiply(D3GameController.instace.ListRandomRewardedAD[i].duration);
                                                    D3GameAttribute.gameAttribute.multiplyValue = 2;
                                                    Debug.Log("Unity Ads Rewarded: " + D3GameController.instace.ListRandomRewardedAD[i].TypeReward + " " + "Duration:  " + D3GameController.instace.ListRandomRewardedAD[i].duration);
                                                }

                                                if (D3GameController.instace.ListRandomRewardedAD[i].TypeReward == D3ADSRandomReward.D3TypeRandomReward.ItemShield)
                                                {
                                                    if (D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().TextReward != null)
                                                    {
                                                        D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().TextReward.text = "Item Shield Duration " + D3GameController.instace.ListRandomRewardedAD[i].duration.ToString();
                                                    }
                                                    if (D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().ImageReward != null)
                                                    {
                                                        D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().ImageReward.sprite = D3GameController.instace.ListRandomRewardedAD[i].ImgReward;
                                                    }
                                                    D3Controller.instace.ItemImmortality(D3GameController.instace.ListRandomRewardedAD[i].duration);
                                                    Debug.Log("Unity Ads Rewarded: " + D3GameController.instace.ListRandomRewardedAD[i].TypeReward + " " + "Duration:  " + D3GameController.instace.ListRandomRewardedAD[i].duration);
                                                }
                                                D3GUIManager.instance.RewardWindow.GetComponent<D3RewardWindow>().AutoClose = true;
                                                D3GUIManager.instance.OpenRewardWindow();
                                            }

                                        }

                                    }
                                }

                            }

                            D3Controller.instace.PauseOff();
                            if (D3PanelRewardADS.instance)
                            {
                                D3PanelRewardADS.instance.EnabledPanel();
                            }
                            D3GameAttribute.gameAttribute.Pause(false);
                        }
                    }
                    
                }
            }


            Debug.Log("Unity Ads Rewarded Ad COMPLETED");
            // Grant a reward.
            ShowCompleteUnityRewardedVideo.Invoke();

            // Load another ad:
            LoadRewardedUnityVideo();

        }

        if (adUnitId.Equals(_adVideoUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.SKIPPED))
        {
            Debug.Log("Unity Ads Rewarded Ad SKIPPED");
            // Grant a reward.
            SkippedUnityRewardedVideo.Invoke();
            // Load another ad:
            LoadRewardedUnityVideo();

            if (GameScene != null)
            {
                D3Controller.instace.PauseOff();
                D3GameAttribute.gameAttribute.Pause(false);
            }

        }

        if (adUnitId.Equals(_adInterstitialUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Interstitial COMPLETED");
            // Grant a reward.
            ShowCompleteUnityInterstitial.Invoke();
            // Load another ad:
            LoadInstertialADS();

            if (TitleScene != null)
            {
                if (TitleScene.EnableInterstitialADSOnScene)
                {
                    if (D3LevelSystemManager.Instance)
                    {
                        if (D3LevelSystemManager.Instance.EnabledLevelSystem)
                        {
                            TitleScene.OpenLevelSystemWindow();
                        }
                        else
                        {
                            if (TitleScene.GameScene != null)
                            {
                                if (SceneUtility.GetBuildIndexByScenePath(TitleScene.GameScene) == 0)
                                {
                                    Debug.LogError("Could not find scene name '" + TitleScene.GameScene + "'");
                                    return;
                                }
                                else
                                {
                                    SceneManager.LoadScene(TitleScene.GameScene);
                                }
                            }
                            else
                            {
                                Debug.LogError("Scene Game is not assigned in Tab Level System");
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (TitleScene.GameScene != null)
                        {
                            if (SceneUtility.GetBuildIndexByScenePath(TitleScene.GameScene) == 0)
                            {
                                Debug.LogError("Could not find scene name '" + TitleScene.GameScene + "'");
                                return;
                            }
                            else
                            {
                                SceneManager.LoadScene(TitleScene.GameScene);
                            }
                        }
                        else
                        {
                            Debug.LogError("Scene Game is not assigned in Tab Level System");
                            return;
                        }
                    }
                }
            }

        }
       
        if (adUnitId.Equals(_adInterstitialUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.SKIPPED))
        {
            // Grant a reward.
            SkippedUnityinterstitial.Invoke();

            // Load another ad:
            LoadInstertialADS();

            if (TitleScene != null)
            {
                if (TitleScene.EnableInterstitialADSOnScene)
                {
                    TitleScene.OpenTitle();
                }
            }
        }
    }

    public void OnInitializationComplete()
    {

        if (TitleScene != null)
        {
            if (TitleScene.EnableADSOnScene)
            {
                if (TitleScene.UseBanner)
                {
                    RequestBanner();
                }
                if (TitleScene.EnableInterstitialADSOnScene)
                {
                    LoadInstertialADS();
                }
                if (TitleScene.EnableRewardedADOnScene)
                {
                    LoadRewardedUnityVideo();

                    if (TitleScene.ListRewardedADButtons.Count > 0)
                    {
                        for (int i = 0; TitleScene.ListRewardedADButtons.Count > i; i++)
                        {
                            if (TitleScene.ListRewardedADButtons[i].showAdButton != null)
                            {
                                GameObject Button = TitleScene.ListRewardedADButtons[i].showAdButton.gameObject;
                                Button.AddComponent<D3ADSVideoReward>();
                                TitleScene.ListRewardedADButtons[i].showAdButton.GetComponent<D3ADSVideoReward>().IDButton = i;
                                TitleScene.ListRewardedADButtons[i].showAdButton.GetComponent<D3ADSVideoReward>().TitleScene = TitleScene;
                            }
                        }
                    }

                }
            }
            
        }

        if (GameScene != null)
        {
            if (GameScene.EnableADSOnScene)
            {
                if (GameScene.UseBanner)
                {
                    RequestBanner();
                }
                if (GameScene.EnableInterstitialADSOnWim || GameScene.EnableInterstitialADSOnGameOver)
                {
                    LoadInstertialADS();
                }
                if (GameScene.EnableRewardedADOnScene)
                {
                    LoadRewardedUnityVideo();
                }
            }

        }

        ADSReady = true;

    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        if (adUnitId.Equals(_adVideoUnitId))
        {
            if (TitleScene != null)
            {
                if (TitleScene.EnableRewardedADOnScene)
                {
                    if (TitleScene.ListRewardedADButtons.Count > 0)
                    {
                        for (int i = 0; TitleScene.ListRewardedADButtons.Count > i; i++)
                        {
                            if (TitleScene.ListRewardedADButtons[i].showAdButton != null)
                            {
                                TitleScene.ListRewardedADButtons[i].showAdButton.interactable = true;

                            }
                        }
                    }
                }
            }
            if (GameScene != null)
            {
                if (GameScene.EnableRewardedADOnScene)
                {
                    if (D3PanelRewardADS.instance)
                    {
                        if (D3PanelRewardADS.instance.EnabledPanelRewardADS && D3PanelRewardADS.instance.RewardButton != null)
                        {
                            if (D3GameController.instace.ListRandomRewardedAD.Count > 0)
                            {
                                D3PanelRewardADS.instance.RewardButton.interactable = true;
                            }
                        }
                    }
                }
            }
            ADSRewardReady = true;
        }

        if (adUnitId.Equals(_adInterstitialUnitId))
        {
            ADSInsterstitialReady = true;
        }
    
    }

	public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
	{
        if (adUnitId.Equals(_adVideoUnitId))
        {
            ADSRewardReady = false;
        }
        Debug.Log($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
        
        // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
    }

	public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
	{
		Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
		// Optionally execute code if the Ad Unit fails to show, such as loading another ad.
	}

	public void OnUnityAdsShowStart(string adUnitId) { }
	public void OnUnityAdsShowClick(string adUnitId) { }

    #endregion

#endif

}
