using System;
using System.Collections.Generic;
using UnityEditor;
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
    void Start(){

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

		instance = this;

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

		GameVersionText.text = GameVersionInfo + Application.version;


        coin = D3GameData.LoadCoin();

		Life = D3GameData.LoadLife();

		BestScore = D3GameData.LoadBestScore();

		HoverBoard = D3GameData.LoadHoveBoard();

		HoverBoardText.text = HoverBoard.ToString();

		BestScoreText.text = BestScore.ToString();

		LifeText.text = Life.ToString();

		coinText.text = coin.ToString();
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
		NoLifeGUI.SetActive(false);
		SettingGUI.SetActive(false);
		ShopGui.SetActive(false);
		TextObject.SetActive(true);
		TitleGUI.SetActive(true);
		HeroController.SetActive(false);
        RewardWindow.SetActive(false);
        AppearWindow(TitleGUI);
        if (D3LevelSystemManager.Instance)
        {
            D3LevelSystemManager.Instance.LoadDataFronMemory();
        }
	}

	public void OpenSettings()
	{
		if (D3SoundManager.instance != null)
			D3SoundManager.instance.PlayingSound("Button");
		D3SoundManager.instance.loadVolumen();
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
        if (D3SoundManager.instance != null)
            D3SoundManager.instance.PlayingSound("Button");
        D3SoundManager.instance.loadVolumen();
        HeroController.SetActive(true);
        HeroController.GetComponent<D3HeroController>().UpdateText();
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
		if (D3SoundManager.instance != null)
			D3SoundManager.instance.PlayingSound("Button");

#if !UNITY_ADS || !ENABLE_UNITY_ADS
        ShopGui.SetActive(true);
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

        AppearWindow(ShopGui);

#endif
#if UNITY_ADS && ENABLE_UNITY_ADS
        if (EnableRewardedADOnScene)
        {
            ShopGui.SetActive(false);
            HeroController.SetActive(false);
            RewardWindow.SetActive(false);
            NoLifeGUI.SetActive(true);
            AppearWindow(NoLifeGUI);
        }
        if (!EnableRewardedADOnScene)
        {
            ShopGui.SetActive(true);
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
        if (D3SoundManager.instance != null)
            D3SoundManager.instance.PlayingSound("Button");
        ShopGui.SetActive(false);
        HeroController.SetActive(false);
        NoLifeGUI.SetActive(false);
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
                                if (SceneUtility.GetBuildIndexByScenePath(GameScene) == 0)
                                {
                                    Debug.LogError("Could not find scene name '" + GameScene + "'");
                                    return;
                                }
                                else
                                {
                                    SceneManager.LoadScene(GameScene);
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
                            if (SceneUtility.GetBuildIndexByScenePath(GameScene) == 0)
                            {
                                Debug.LogError("Could not find scene name '" + GameScene + "'");
                                return;
                            }
                            else
                            {
                                SceneManager.LoadScene(GameScene);
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
                            if (SceneUtility.GetBuildIndexByScenePath(GameScene) == 0)
                            {
                                Debug.LogError("Could not find scene name '" + GameScene + "'");
                                return;
                            }
                            else
                            {
                                SceneManager.LoadScene(GameScene);
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
                        if (SceneUtility.GetBuildIndexByScenePath(GameScene) == 0)
                        {
                            Debug.LogError("Could not find scene name '" + GameScene + "'");
                            return;
                        }
                        else
                        {
                            SceneManager.LoadScene(GameScene);
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
                        if (SceneUtility.GetBuildIndexByScenePath(GameScene) == 0)
                        {
                            Debug.LogError("Could not find scene name '" + GameScene + "'");
                            return;
                        }
                        else
                        {
                            SceneManager.LoadScene(GameScene);
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
                    if (SceneUtility.GetBuildIndexByScenePath(GameScene) == 0)
                    {
                        Debug.LogError("Could not find scene name '" + GameScene + "'");
                        return;
                    }
                    else
                    {
                        SceneManager.LoadScene(GameScene);
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
		if (D3SoundManager.instance != null)
			D3SoundManager.instance.PlayingSound("Button");
		ShopGui.SetActive(true);
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

        AppearWindow(ShopGui);
		
	}

	public void ShopSceneClose()
	{
		if (D3SoundManager.instance != null)
			D3SoundManager.instance.PlayingSound("Button");
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
		DisappearWindow(ShopGui);
        ShopGui.SetActive(false);
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
