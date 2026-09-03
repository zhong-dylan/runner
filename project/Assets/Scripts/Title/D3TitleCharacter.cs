using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class D3TitleCharacter : MonoBehaviour 
{
#if UNITY_EDITOR
    public SceneAsset Scene;
#endif

    public string GameScene;

    public int coin;
	public int Life;
	public int HoverBoard;
	public int BestScore;
	public Text coinText;
	public Text LifeText;
	public String GameVersionInfo = "Engine Version: ";
    public Text GameVersionText;
    public Text HoverBoardText;
	public Text BestScoreText;

	public Button BtnPlay;
	public Button BtnShop;

	public GameObject TitleGUI;
	public GameObject TextObject;
	public GameObject SettingGUI;
	public GameObject NoLifeGUI;
	public GameObject ShopGui;
    public GameObject HeroController;
    public GameObject RewardWindow;
    public D3LevelSystem WindowLevelSystem;
    public D3LevelSystemManager LevelSystemManager;

    private readonly string _appearTrigger = "Appear";
	private readonly string _disappearTrigger = "Disappear";

	public static D3TitleCharacter instance;


    public bool EnableADSOnScene = false;
	public bool UseBanner= false;

	public bool EnableRewardedADOnScene = false;
    public List<D3ADSTypeReward> ListRewardedADButtons;

	public bool EnableInterstitialADSOnScene = false;
    public bool EnableRewardedWindow = true;

    [HideInInspector]
    int FirstTime = 0;
    [HideInInspector]
    public int lifeInitial = 3;

    public bool EnabledLevelSystem = false;

    void Awake()
    {
        instance = this;
    }

    IEnumerator Start(){
        yield return D3MainUIManager.GetOrCreate().LoadTitle(this);

        FirstTime = PlayerPrefs.GetInt("FirstTime");
        if (FirstTime == 1)
        {
            Life = D3GameData.LoadLife();
        }
        if (FirstTime == 0)
        {
            Life = lifeInitial;
            PlayerPrefs.SetInt("FirstTime", 1);
            D3GameData.SaveLife(Life);

        }

        if (D3LevelSystemManager.Instance)
        {
            D3LevelSystemManager.Instance.EnabledLevelSystem = EnabledLevelSystem;
        }

        UpdateText();

		OpenTitle();

#if UNITY_ADS && ENABLE_UNITY_ADS
        if (D3ADSManager.D3AdsManager && D3ADSManager.D3AdsManager.ADSReady)
        {
            if (EnableADSOnScene)
            {
                if (UseBanner)
                {
                    D3ADSManager.D3AdsManager.RequestBanner();
                }
                if (EnableInterstitialADSOnScene)
                {
                    D3ADSManager.D3AdsManager.LoadInstertialADS();
                }
                if (EnableRewardedADOnScene)
                {
                    D3ADSManager.D3AdsManager.LoadRewardedUnityVideo();

                    if (ListRewardedADButtons.Count > 0)
                    {
                        for (int i = 0; ListRewardedADButtons.Count > i; i++)
                        {
                            if (ListRewardedADButtons[i].showAdButton != null)
                            {
                                if (!ListRewardedADButtons[i].showAdButton.GetComponent<D3ADSVideoReward>())
                                {
                                    GameObject Button = ListRewardedADButtons[i].showAdButton.gameObject;
                                    Button.AddComponent<D3ADSVideoReward>();
                                    ListRewardedADButtons[i].showAdButton.GetComponent<D3ADSVideoReward>().IDButton = i;
                                    ListRewardedADButtons[i].showAdButton.GetComponent<D3ADSVideoReward>().TitleScene = this;
                                }
                                if (ListRewardedADButtons[i].showAdButton.GetComponent<D3ADSVideoReward>())
                                {
                                    ListRewardedADButtons[i].showAdButton.GetComponent<D3ADSVideoReward>().IDButton = i;
                                    ListRewardedADButtons[i].showAdButton.GetComponent<D3ADSVideoReward>().TitleScene = this;
                                }
                            }
                        }
                    }

                }
                if (!EnableRewardedADOnScene)
                {
                    for (int i = 0; ListRewardedADButtons.Count > i; i++)
                    {
                        if (ListRewardedADButtons[i].showAdButton != null)
                        {
                            ListRewardedADButtons[i].showAdButton.gameObject.SetActive(false);
                        }
                    }

                }
            }
        }
#endif
#if !UNITY_ADS || !ENABLE_UNITY_ADS
        if (ListRewardedADButtons.Count > 0)
        {
            for (int i = 0; ListRewardedADButtons.Count > i; i++)
            {
                if (ListRewardedADButtons[i].showAdButton != null)
                {
                    ListRewardedADButtons[i].showAdButton.gameObject.SetActive(false);
                }
            }
        }
#endif

    }

	    public void UpdateText()
		{

			if (GameVersionText != null)
			{
				GameVersionText.text = GameVersionInfo + Application.version;
			}


	        coin = D3GameData.LoadCoin();

		Life = D3GameData.LoadLife();

		BestScore = D3GameData.LoadBestScore();

			HoverBoard = D3GameData.LoadHoveBoard();

			if (HoverBoardText != null)
			{
				HoverBoardText.text = HoverBoard.ToString();
			}

			if (BestScoreText != null)
			{
				BestScoreText.text = BestScore.ToString();
			}

			if (LifeText != null)
			{
				LifeText.text = Life.ToString();
			}

			if (coinText != null)
			{
				coinText.text = coin.ToString();
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

		//AudioOpen

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

		//AudioClose

		anim.SetTrigger(_disappearTrigger);
	}

	public void OpenTitle()
	{
		SetActiveIfLoaded(NoLifeGUI, false);
		SetActiveIfLoaded(SettingGUI, false);
		SetActiveIfLoaded(ShopGui, false);
		SetActiveIfLoaded(TextObject, true);
		SetActiveIfLoaded(TitleGUI, true);
		SetActiveIfLoaded(HeroController, false);
        SetActiveIfLoaded(RewardWindow, false);
        AppearWindow(TitleGUI);
        if (D3LevelSystemManager.Instance)
        {
            D3LevelSystemManager.Instance.LoadDataFronMemory();
        }
	}

	public void OpenSettings()
	{
        StartCoroutine(OpenSettingsRoutine());
    }

    private IEnumerator OpenSettingsRoutine()
    {
        yield return D3MainUIManager.GetOrCreate().LoadSettings(this);
        if (D3SoundManager.instance != null)
        {
            D3SoundManager.instance.PlayingSound("Button");
            D3SoundManager.instance.loadVolumen();
        }
        SettingGUI.SetActive(true);
		AppearWindow(SettingGUI);
	}
	public void CloseSettings()
	{
		if (D3SoundManager.instance != null)
			D3SoundManager.instance.PlayingSound("Button");
		
		DisappearWindow(SettingGUI);

        SettingGUI.SetActive(false);
    }

    public void OpenHeroWindow()
    {
        StartCoroutine(OpenHeroWindowRoutine());
    }

    private IEnumerator OpenHeroWindowRoutine()
    {
        yield return D3MainUIManager.GetOrCreate().LoadHero(this);
        if (D3SoundManager.instance != null)
        {
            D3SoundManager.instance.PlayingSound("Button");
            D3SoundManager.instance.loadVolumen();
        }
        HeroController.SetActive(true);
        var hero = HeroController.GetComponent<D3HeroController>();
        if (hero != null && hero.ShopContoller != null)
        {
            hero.UpdateText();
        }
        AppearWindow(HeroController);
    }
    public void CloseHeroWindow()
    {
        if (D3SoundManager.instance != null)
            D3SoundManager.instance.PlayingSound("Button");
        
        DisappearWindow(HeroController);

        HeroController.SetActive(false);
    }


    public void OpenNoLifeWindow()
	{
        StartCoroutine(OpenNoLifeWindowRoutine());
    }

    private IEnumerator OpenNoLifeWindowRoutine()
    {
        if (D3SoundManager.instance != null)
            D3SoundManager.instance.PlayingSound("Button");

#if !UNITY_ADS || !ENABLE_UNITY_ADS
        yield return D3MainUIManager.GetOrCreate().LoadShop(this);
        LoadShopItemsIfNeeded();
        ShopGui.SetActive(true);
        ResetShopScrollbars();

        AppearWindow(ShopGui);

#endif
#if UNITY_ADS && ENABLE_UNITY_ADS
        if (EnableRewardedADOnScene)
        {
            yield return D3MainUIManager.GetOrCreate().LoadNoLife(this);
            SetActiveIfLoaded(ShopGui, false);
            SetActiveIfLoaded(HeroController, false);
            SetActiveIfLoaded(RewardWindow, false);
            NoLifeGUI.SetActive(true);
            AppearWindow(NoLifeGUI);
        }
        if (!EnableRewardedADOnScene)
        {
            yield return D3MainUIManager.GetOrCreate().LoadShop(this);
            LoadShopItemsIfNeeded();
            ShopGui.SetActive(true);
            ResetShopScrollbars();

            AppearWindow(ShopGui);
        }
#endif
    }
    public void CloseNoLifeWindow()
	{
		if (D3SoundManager.instance != null)
			D3SoundManager.instance.PlayingSound("Button");
		
		DisappearWindow(NoLifeGUI);

        NoLifeGUI.SetActive(false);
    }

    public void OpenRewardWindow()
    {
        StartCoroutine(OpenRewardWindowRoutine());
    }

    private IEnumerator OpenRewardWindowRoutine()
    {
        yield return D3MainUIManager.GetOrCreate().LoadRewardWindow(this);
        if (D3SoundManager.instance != null)
            D3SoundManager.instance.PlayingSound("Button");
        SetActiveIfLoaded(ShopGui, false);
        SetActiveIfLoaded(HeroController, false);
        SetActiveIfLoaded(NoLifeGUI, false);
        RewardWindow.SetActive(true);
        AppearWindow(RewardWindow);
    }
    public void CloseRewardWindow()
    {
        if (D3SoundManager.instance != null)
            D3SoundManager.instance.PlayingSound("Button");
        
        DisappearWindow(RewardWindow);

        RewardWindow.SetActive(false);
    }


    public void StartGame()
	{
		if (Life > 0)
		{
			if (D3SoundManager.instance != null)
				D3SoundManager.instance.PlayingSound("Button");
            TitleGUI.SetActive(false);
            TextObject.SetActive(false);
            DisappearWindow(TitleGUI);
#if UNITY_ADS && ENABLE_UNITY_ADS
			if (EnableADSOnScene)
			{
				if (UseBanner)
				{
					if (D3ADSManager.D3AdsManager != null)
					{
						D3ADSManager.D3AdsManager.DestroyBanner();

					}
				}
				if (EnableInterstitialADSOnScene)
				{
                    
					D3ADSManager.D3AdsManager.ShowUnityInterstitialADS();

					
				}
				else
				{
                    if (D3LevelSystemManager.Instance)
                    {
                        if (D3LevelSystemManager.Instance.EnabledLevelSystem)
                        {
                            OpenLevelSystemWindow();
                        }
                        else
                        {
                            if (GameScene != null)
                            {
                                if (string.IsNullOrEmpty(GameScene))
                                {
                                    Debug.LogError("Could not find scene name '" + GameScene + "'");
                                    return;
                                }
                                else
                                {
                                    D3AddressablesManager.LoadSceneByName(GameScene);
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
                        if (GameScene != null)
                        {
                            if (string.IsNullOrEmpty(GameScene))
                            {
                                Debug.LogError("Could not find scene name '" + GameScene + "'");
                                return;
                            }
                            else
                            {
                                D3AddressablesManager.LoadSceneByName(GameScene);
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
			else {
                if (D3LevelSystemManager.Instance)
                {
                    if (D3LevelSystemManager.Instance.EnabledLevelSystem)
                    {
                        OpenLevelSystemWindow();
                    }
                    else
                    {
                        if (GameScene != null)
                        {
                            if (string.IsNullOrEmpty(GameScene))
                            {
                                Debug.LogError("Could not find scene name '" + GameScene + "'");
                                return;
                            }
                            else
                            {
                                D3AddressablesManager.LoadSceneByName(GameScene);
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
                    if (GameScene != null)
                    {
                        if (string.IsNullOrEmpty(GameScene))
                        {
                            Debug.LogError("Could not find scene name '" + GameScene + "'");
                            return;
                        }
                        else
                        {
                            D3AddressablesManager.LoadSceneByName(GameScene);
                        }
                    }
                    else
                    {
                        Debug.LogError("Scene Game is not assigned in Tab Level System");
                        return;
                    }
                }
            }

#endif
#if !UNITY_ADS || !ENABLE_UNITY_ADS
            if (D3LevelSystemManager.Instance)
            {
                if (D3LevelSystemManager.Instance.EnabledLevelSystem)
                {
                    OpenLevelSystemWindow();
                }
                else
                {
                    if (GameScene != null)
                    {
                        if (string.IsNullOrEmpty(GameScene))
                        {
                            Debug.LogError("Could not find scene name '" + GameScene + "'");
                            return;
                        }
                        else
                        {
                            D3AddressablesManager.LoadSceneByName(GameScene);
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
                if (GameScene != null)
                {
                    if (string.IsNullOrEmpty(GameScene))
                    {
                        Debug.LogError("Could not find scene name '" + GameScene + "'");
                        return;
                    }
                    else
                    {
                        D3AddressablesManager.LoadSceneByName(GameScene);
                    }
                }
                else
                {
                    Debug.LogError("Scene Game is not assigned in Tab Level System");
                    return;
                }
            }
#endif
        }

        if (Life <= 0)
		{
			OpenNoLifeWindow();
		}
	}

	public void ShopSceneOpen()
	{
        StartCoroutine(ShopSceneOpenRoutine());
    }

    private IEnumerator ShopSceneOpenRoutine()
    {
        yield return D3MainUIManager.GetOrCreate().LoadShop(this);
        if (D3SoundManager.instance != null)
            D3SoundManager.instance.PlayingSound("Button");
        LoadShopItemsIfNeeded();
		ShopGui.SetActive(true);
        ResetShopScrollbars();

        AppearWindow(ShopGui);
			
	}

	public void ShopSceneClose()
	{
		if (D3SoundManager.instance != null)
			D3SoundManager.instance.PlayingSound("Button");
        ResetShopScrollbars();
		DisappearWindow(ShopGui);
        ShopGui.SetActive(false);
    }

    private void LoadShopItemsIfNeeded()
    {
        if (D3ShopCharacter.instace != null)
        {
            D3ShopCharacter.instace.LoadItemsIfNeeded();
        }
    }

    private void ResetShopScrollbars()
    {
        if (D3ShopCharacter.instace == null)
        {
            return;
        }

        if (D3ShopCharacter.instace.ScrollbarContentHero != null)
        {
            D3ShopCharacter.instace.ScrollbarContentHero.value = 0f;
        }
        if (D3ShopCharacter.instace.ScrollbarContentHoverboard != null)
        {
            D3ShopCharacter.instace.ScrollbarContentHoverboard.value = 0f;
        }
        if (D3ShopCharacter.instace.ScrollbarContentItems != null)
        {
            D3ShopCharacter.instace.ScrollbarContentItems.value = 0f;
        }
    }

    private void SetActiveIfLoaded(GameObject target, bool active)
    {
        if (target != null)
        {
            target.SetActive(active);
        }
    }

    public void OpenLevelSystemWindow()
    {
        if (D3SoundManager.instance != null)
            D3SoundManager.instance.PlayingSound("Button");
        ShopGui.SetActive(false);
        WindowLevelSystem.gameObject.SetActive(true);
        AppearWindow(WindowLevelSystem.gameObject);
    }

    public void CloseLevelSystemWindow()
    {
        if (D3SoundManager.instance != null)
            D3SoundManager.instance.PlayingSound("Button");
        DisappearWindow(WindowLevelSystem.gameObject);
        WindowLevelSystem.gameObject.SetActive(false);
        OpenTitle();
    }
}
