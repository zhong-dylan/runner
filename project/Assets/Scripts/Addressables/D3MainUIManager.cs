using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class D3MainUIManager : MonoBehaviour
{
    public static D3MainUIManager Instance { get; private set; }

    public Transform uiRoot;
    public string TitleGUIAddress = "Prefabs/MainUI/TitleGUI";
    public string SettingsAddress = "Prefabs/MainUI/Settings";
    public string ShopAddress = "Prefabs/MainUI/Shop";
    public string HeroAddress = "Prefabs/MainUI/Hero";
    public string WindowNoLifeAddress = "Prefabs/MainUI/WindowNoLife";
    public string RewardWindowAddress = "Prefabs/MainUI/RewardWindow";
    public string TextObjectAddress = "Prefabs/MainUI/TextObject";

    public GameObject TitleGUI { get; private set; }
    public GameObject Settings { get; private set; }
    public GameObject Shop { get; private set; }
    public GameObject Hero { get; private set; }
    public GameObject WindowNoLife { get; private set; }
    public GameObject RewardWindow { get; private set; }
    public GameObject TextObject { get; private set; }
    private Transform hiddenLoadRoot;

    private void Awake()
    {
        Instance = this;
    }

    public static D3MainUIManager GetOrCreate()
    {
        if (Instance != null)
        {
            return Instance;
        }

        var gameObject = new GameObject(nameof(D3MainUIManager));
        Instance = gameObject.AddComponent<D3MainUIManager>();
        return Instance;
    }

    public IEnumerator LoadTitle(D3TitleCharacter title)
    {
        yield return LoadPrefab(TitleGUIAddress, value => TitleGUI = value, () => TitleGUI, false);
        yield return LoadPrefab(TextObjectAddress, value => TextObject = value, () => TextObject, true);
        BindTitleCharacter(title);
    }

    public IEnumerator LoadSettings(D3TitleCharacter title)
    {
        yield return LoadPrefab(SettingsAddress, value => Settings = value, () => Settings, false);
        BindTitleCharacter(title);
    }

    public IEnumerator LoadShop(D3TitleCharacter title)
    {
        yield return LoadPrefab(ShopAddress, value => Shop = value, () => Shop, false);
        BindTitleCharacter(title);
    }

    public IEnumerator LoadHero(D3TitleCharacter title)
    {
        yield return LoadPrefab(HeroAddress, value => Hero = value, () => Hero, false);
        BindTitleCharacter(title);
    }

    public IEnumerator LoadNoLife(D3TitleCharacter title)
    {
        yield return LoadPrefab(WindowNoLifeAddress, value => WindowNoLife = value, () => WindowNoLife, false);
        BindTitleCharacter(title);
    }

    public IEnumerator LoadRewardWindow(D3TitleCharacter title)
    {
        yield return LoadPrefab(RewardWindowAddress, value => RewardWindow = value, () => RewardWindow, false);
        BindTitleCharacter(title);
    }

    private IEnumerator LoadPrefab(string address, System.Action<GameObject> assign, System.Func<GameObject> current, bool activeOnLoad)
    {
        var currentObject = current();
        if (currentObject != null)
        {
            currentObject.SetActive(activeOnLoad);
            yield break;
        }

        if (string.IsNullOrEmpty(address))
        {
            assign(null);
            yield break;
        }

        AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(address, GetHiddenLoadRoot(), false);
        yield return handle;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            handle.Result.name = System.IO.Path.GetFileName(address);
            handle.Result.SetActive(activeOnLoad);
            handle.Result.transform.SetParent(GetParent(), false);
            assign(handle.Result);
        }
        else
        {
            Debug.LogError("Failed to load UI address: " + address);
            assign(null);
        }
    }

    private Transform GetParent()
    {
        return uiRoot != null ? uiRoot : transform.parent;
    }

    private Transform GetHiddenLoadRoot()
    {
        if (hiddenLoadRoot != null)
        {
            return hiddenLoadRoot;
        }

        var hiddenRoot = new GameObject("HiddenLoadRoot");
        hiddenRoot.transform.SetParent(transform, false);
        hiddenRoot.SetActive(false);
        hiddenLoadRoot = hiddenRoot.transform;
        return hiddenLoadRoot;
    }

    private void BindTitleCharacter(D3TitleCharacter title)
    {
        if (title == null)
        {
            title = FindObjectOfType<D3TitleCharacter>();
        }

        if (title == null)
        {
            return;
        }

        title.TitleGUI = TitleGUI;
        title.SettingGUI = Settings;
        title.ShopGui = Shop;
        title.HeroController = Hero;
        title.NoLifeGUI = WindowNoLife;
        title.RewardWindow = RewardWindow;
        title.TextObject = TextObject;

        BindShopReferences();
        BindTextObjectButtons(title);

        title.BtnPlay = FindButton(TitleGUI, "ButtonPlay");
        title.BtnShop = FindButton(TitleGUI, "ButtonShop");
        BindTitleTexts(title);

        BindButton(TitleGUI, "ButtonPlay", title.StartGame);
        BindButton(TitleGUI, "ButtonShop", title.ShopSceneOpen);
        BindButton(TitleGUI, "ButtonSettings", title.OpenSettings);
        BindButton(TitleGUI, "ButtonHero", title.OpenHeroWindow);

        BindButton(Settings, "ButtonClose", title.CloseSettings);
        BindButton(Settings, "ButtonOK", title.CloseSettings);

        BindButton(Shop, "ButtonClose", title.ShopSceneClose);
        BindButton(Shop, "ButtonShop", title.ShopSceneOpen);

        BindButton(Hero, "ButtonExit", title.CloseHeroWindow);
        BindButton(Hero, "ButtonClose", title.CloseHeroWindow);

        BindButton(WindowNoLife, "ButtonClose", title.CloseNoLifeWindow);
        BindButton(WindowNoLife, "ButtonOK", title.CloseNoLifeWindow);

        BindButton(RewardWindow, "ButtonClose", title.CloseRewardWindow);
    }

    private void BindTextObjectButtons(D3TitleCharacter title)
    {
        if (TextObject == null || title == null)
        {
            return;
        }

        var binder = TextObject.GetComponent<D3TextObjectButtonBinder>();
        if (binder != null)
        {
            binder.Bind(title);
        }
    }

    private void BindTitleTexts(D3TitleCharacter title)
    {
        if (title.GameVersionText == null)
        {
            title.GameVersionText = FindText("InfoVer. (1)", "InfoVer", "GameVersionText", "GameVersion");
        }

        if (title.coinText == null)
        {
            title.coinText = FindText("TotalCOIN", "TotalCoin", "CoinText", "coinText");
        }

        if (title.LifeText == null)
        {
            title.LifeText = FindText("TotalLIFE", "TotalLife", "LifeText");
        }

        if (title.HoverBoardText == null)
        {
            title.HoverBoardText = FindText("TotalHoverBoard", "HoverBoardText");
        }

        if (title.BestScoreText == null)
        {
            title.BestScoreText = FindText("ScoreBest", "BestScoreText");
        }
    }

    private void BindShopReferences()
    {
        var shopCharacter = Shop != null ? Shop.GetComponentInChildren<D3ShopCharacter>(true) : null;
        if (shopCharacter == null)
        {
            return;
        }

        shopCharacter.BindRuntimeReferences(Shop);

        if (shopCharacter.PlayerView == null)
        {
            var playerView = FindChild(Hero, "PlayerView");
            if (playerView != null)
            {
                shopCharacter.PlayerView = playerView;
            }
        }

        if (shopCharacter.m_ContentHero == null)
        {
            var contentHero = FindChild(Shop, "ContentHero");
            if (contentHero != null)
            {
                shopCharacter.m_ContentHero = contentHero.transform;
            }
        }

        if (shopCharacter.m_ContentHoverboard == null)
        {
            var contentHoverboard = FindChild(Shop, "ContentHoverboard") ?? FindChild(Shop, "ContentHoverBoard");
            if (contentHoverboard != null)
            {
                shopCharacter.m_ContentHoverboard = contentHoverboard.transform;
            }
        }

        if (shopCharacter.m_ContentItems == null)
        {
            var contentItems = FindChild(Shop, "ContentItems");
            if (contentItems != null)
            {
                shopCharacter.m_ContentItems = contentItems.transform;
            }
        }

        var heroController = Hero != null ? Hero.GetComponentInChildren<D3HeroController>(true) : null;
        if (heroController != null)
        {
            heroController.ShopContoller = shopCharacter;
        }
    }

    private static void BindButton(GameObject root, string buttonName, UnityAction action)
    {
        var button = FindButton(root, buttonName);
        if (button == null)
        {
            return;
        }

        button.onClick.RemoveListener(action);
        button.onClick.AddListener(action);
    }

    private static Button FindButton(GameObject root, string buttonName)
    {
        var child = FindChild(root, buttonName);
        return child != null ? child.GetComponent<Button>() ?? child.GetComponentInChildren<Button>(true) : null;
    }

    private Text FindText(params string[] textNames)
    {
        var text = FindTextInRoot(TextObject, textNames);
        if (text != null)
        {
            return text;
        }

        return FindTextInRoot(TitleGUI, textNames);
    }

    private static Text FindTextInRoot(GameObject root, params string[] textNames)
    {
        foreach (var textName in textNames)
        {
            var child = FindChild(root, textName);
            if (child == null)
            {
                continue;
            }

            var text = child.GetComponent<Text>();
            if (text != null)
            {
                return text;
            }

            text = child.GetComponentInChildren<Text>(true);
            if (text != null)
            {
                return text;
            }
        }

        return null;
    }

    private static GameObject FindChild(GameObject root, string childName)
    {
        if (root == null)
        {
            return null;
        }

        var transforms = root.GetComponentsInChildren<Transform>(true);
        foreach (var transform in transforms)
        {
            if (transform.name == childName)
            {
                return transform.gameObject;
            }
        }

        return null;
    }
}
