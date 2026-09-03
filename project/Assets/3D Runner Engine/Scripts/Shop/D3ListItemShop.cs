using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class D3ListItemShop : MonoBehaviour
{
    public enum D3TypeTemplate
    { 
        VirtualOrRealMoney, NoADS

    }
    [HideInInspector]
    public D3TypeTemplate TypeTemplate;
    public Image Image;
    public Text NameText;
    public Text PriceText;
    public Button Button;
    public Sprite ImageButtonBuy;
    public Sprite ImageButtonSelect;
    public Text ButtonText;
    public Image ImageIconCoin;
    public Sprite ImageIconVirtualCoin;
    public Sprite ImageIconRealCoin;

    public GameObject ContentStatistics;
    public Slider ScrollbarAddSprintTime;
    public TextMeshProUGUI TextAddSprintTime;
    public Slider ScrollbarAddSpecialTime;
    public TextMeshProUGUI TextAddSpecialTime;
    public Slider ScrollbarAddMultiplyTime;
    public TextMeshProUGUI TextAddMultiplyTime;
    public Slider ScrollbarAddMagnetTime;
    public TextMeshProUGUI TextAddMagnetTime;
    public Slider ScrollbarAddShieldTime;
    public TextMeshProUGUI TextAddShieldTime;

    [HideInInspector]
    public D3ItemShop Item;

    public string BuyText = "BUY";
    public string NOCoinText = "NO COIN";
    public string SelectText = "SELECT";
    public string InUseText = "IN USE";


    // Start is called before the first frame update
    void Start()
    {
        Image.sprite = Item.Image;
        if (Item.TypeItem == D3ItemShop.TypeItemShop.Life || Item.TypeItem == D3ItemShop.TypeItemShop.HoverboardKeyUse)
        {
            NameText.text = Item.Name + " X" + Item.Cant.ToString();
        }
        else {

            NameText.text = Item.Name;
        }

        PriceText.text = Item.Price.ToString();

        Item.Unlocked = PlayerPrefs.GetInt(Item.Name + Item.ID);

#if UNITY_In_APP && ENABLE_UNITY_IAP
        if (Item.ProductType == D3ItemShop.D3ProductType.RealMoneyConsumable || Item.ProductType == D3ItemShop.D3ProductType.RealMoneyNoADS)
        {
            Button.onClick.AddListener(BuyConsumableOrNoConsumableItem);
            ImageIconCoin.sprite = ImageIconRealCoin;
            PriceText.text = "$ " + Item.Price.ToString();
        }
        if (Item.ProductType == D3ItemShop.D3ProductType.VirtualMoney)
        {
            Button.onClick.AddListener(BuyOrSelectItem);
            ImageIconCoin.sprite = ImageIconVirtualCoin;
        }

#endif



#if UNITY_ADS && ENABLE_UNITY_ADS
        if (Item.ProductType == D3ItemShop.D3ProductType.RealMoneyNoADS)
        {
            Item.Unlocked = PlayerPrefs.GetInt("EnableUnityADS");
        }
#endif
#if !UNITY_In_APP || !ENABLE_UNITY_IAP
        Button.onClick.AddListener(BuyOrSelectItem);
        ImageIconCoin.sprite = ImageIconVirtualCoin;
#endif
    }

    // Update is called once per frame
    void Update()
    {
        if (Item.TypeItem == D3ItemShop.TypeItemShop.Life || Item.TypeItem == D3ItemShop.TypeItemShop.HoverboardKeyUse )
        {
            if (Item.ProductType == D3ItemShop.D3ProductType.VirtualMoney)
            {
                if (D3TitleCharacter.instance.coin >= Item.Price)
                {
                    if (Button.image.sprite != ImageButtonBuy)
                    {
                        Button.image.sprite = ImageButtonBuy;
                    }
                    Button.interactable = true;
                    ButtonText.text = BuyText;
                }
                if (D3TitleCharacter.instance.coin < Item.Price)
                {
                    if (Button.image.sprite != ImageButtonBuy)
                    {
                        Button.image.sprite = ImageButtonBuy;
                    }
                    Button.interactable = false;
                    ButtonText.text = NOCoinText;
                }
            }
            else {
                if (Button.image.sprite != ImageButtonBuy)
                {
                    Button.image.sprite = ImageButtonBuy;
                }
                Button.interactable = true;
                ButtonText.text = BuyText;
            }
            
        }

        if (Item.TypeItem == D3ItemShop.TypeItemShop.Character || Item.TypeItem == D3ItemShop.TypeItemShop.None)
        {
            if (Item.OnlyForCharacterTypeAddPrefab != null)
            {
                ScrollbarAddSprintTime.maxValue = Item.OnlyForCharacterTypeAddPrefab.AddSprintTimeMax;

                if (!PlayerPrefs.HasKey(Item.OnlyForCharacterTypeAddPrefab.name + "SprintTime"))
                {
                    ScrollbarAddSprintTime.value = Item.OnlyForCharacterTypeAddPrefab.CurrentAddSprintTime;
                    TextAddSprintTime.text = Item.OnlyForCharacterTypeAddPrefab.CurrentAddSprintTime + "/" + Item.OnlyForCharacterTypeAddPrefab.AddSprintTimeMax;
                }else
                {
                    ScrollbarAddSprintTime.value = PlayerPrefs.GetFloat(Item.OnlyForCharacterTypeAddPrefab.name + "SprintTime");
                    TextAddSprintTime.text = PlayerPrefs.GetFloat(Item.OnlyForCharacterTypeAddPrefab.name + "SprintTime") + "/" + Item.OnlyForCharacterTypeAddPrefab.AddSprintTimeMax;
                }

                ScrollbarAddSpecialTime.maxValue = Item.OnlyForCharacterTypeAddPrefab.AddSpecialTimeMax;

                if (!PlayerPrefs.HasKey(Item.OnlyForCharacterTypeAddPrefab.name + "SpecialTime"))
                {
                    ScrollbarAddSpecialTime.value = Item.OnlyForCharacterTypeAddPrefab.CurrentAddSpecialTime;
                    TextAddSpecialTime.text = Item.OnlyForCharacterTypeAddPrefab.CurrentAddSpecialTime + "/" + Item.OnlyForCharacterTypeAddPrefab.AddSpecialTimeMax;
                }
                else
                {
                    ScrollbarAddSpecialTime.value = PlayerPrefs.GetFloat(Item.OnlyForCharacterTypeAddPrefab.name + "SpecialTime");
                    TextAddSpecialTime.text = PlayerPrefs.GetFloat(Item.OnlyForCharacterTypeAddPrefab.name + "SpecialTime") + "/" + Item.OnlyForCharacterTypeAddPrefab.AddSpecialTimeMax;
                }

                ScrollbarAddMultiplyTime.maxValue = Item.OnlyForCharacterTypeAddPrefab.AddMultiplyTimeMax;

                if (!PlayerPrefs.HasKey(Item.OnlyForCharacterTypeAddPrefab.name + "MultiplyTime"))
                {
                    ScrollbarAddMultiplyTime.value = Item.OnlyForCharacterTypeAddPrefab.CurrentAddMultiplyTime;
                    TextAddMultiplyTime.text = Item.OnlyForCharacterTypeAddPrefab.CurrentAddMultiplyTime + "/" + Item.OnlyForCharacterTypeAddPrefab.AddMultiplyTimeMax;
                }
                else
                {
                    ScrollbarAddMultiplyTime.value = PlayerPrefs.GetFloat(Item.OnlyForCharacterTypeAddPrefab.name + "MultiplyTime");
                    TextAddMultiplyTime.text = PlayerPrefs.GetFloat(Item.OnlyForCharacterTypeAddPrefab.name + "MultiplyTime") + "/" + Item.OnlyForCharacterTypeAddPrefab.AddMultiplyTimeMax;
                }

                ScrollbarAddMagnetTime.maxValue = Item.OnlyForCharacterTypeAddPrefab.AddMagnetTimeMax;

                if (!PlayerPrefs.HasKey(Item.OnlyForCharacterTypeAddPrefab.name + "MagnetTime"))
                {
                    ScrollbarAddMagnetTime.value = Item.OnlyForCharacterTypeAddPrefab.CurrentAddMagnetTime;
                    TextAddMagnetTime.text = Item.OnlyForCharacterTypeAddPrefab.CurrentAddMagnetTime + "/" + Item.OnlyForCharacterTypeAddPrefab.AddMagnetTimeMax;
                }
                else
                {
                    ScrollbarAddMagnetTime.value = PlayerPrefs.GetFloat(Item.OnlyForCharacterTypeAddPrefab.name + "MagnetTime");
                    TextAddMagnetTime.text = PlayerPrefs.GetFloat(Item.OnlyForCharacterTypeAddPrefab.name + "MagnetTime") + "/" + Item.OnlyForCharacterTypeAddPrefab.AddMagnetTimeMax;
                }

                ScrollbarAddShieldTime.maxValue = Item.OnlyForCharacterTypeAddPrefab.AddShieldTimeMax;

                if (!PlayerPrefs.HasKey(Item.OnlyForCharacterTypeAddPrefab.name + "ShieldTime"))
                {
                    ScrollbarAddShieldTime.value = Item.OnlyForCharacterTypeAddPrefab.CurrentAddShieldTime;
                    TextAddShieldTime.text = Item.OnlyForCharacterTypeAddPrefab.CurrentAddShieldTime + "/" + Item.OnlyForCharacterTypeAddPrefab.AddShieldTimeMax;
                }
                else
                {
                    ScrollbarAddShieldTime.value = PlayerPrefs.GetFloat(Item.OnlyForCharacterTypeAddPrefab.name + "ShieldTime");
                    TextAddShieldTime.text = PlayerPrefs.GetFloat(Item.OnlyForCharacterTypeAddPrefab.name + "ShieldTime") + "/" + Item.OnlyForCharacterTypeAddPrefab.AddShieldTimeMax;
                }
            }

            if (Item.Unlocked == 0)
            {
                if (Item.ProductType == D3ItemShop.D3ProductType.VirtualMoney)
                {
                    if (D3TitleCharacter.instance.coin >= Item.Price)
                    {
                        if (Button.image.sprite != ImageButtonBuy)
                        {
                            Button.image.sprite = ImageButtonBuy;
                        }
                        Button.interactable = true;
                        ButtonText.text = BuyText;
                    }
                    if (D3TitleCharacter.instance.coin < Item.Price)
                    {
                        if (Button.image.sprite != ImageButtonBuy)
                        {
                            Button.image.sprite = ImageButtonBuy;
                        }
                        Button.interactable = false;
                        ButtonText.text = NOCoinText;
                    }
                }

                if (Item.ProductType == D3ItemShop.D3ProductType.RealMoneyNoADS)
                {
#if UNITY_ADS && ENABLE_UNITY_ADS
                    if (Item.Unlocked != PlayerPrefs.GetInt("EnableUnityADS"))
                    {
                        Item.Unlocked = PlayerPrefs.GetInt("EnableUnityADS");
                    }
#endif
                    if (Button.image.sprite != ImageButtonBuy)
                    {
                        Button.image.sprite = ImageButtonBuy;
                    }
                    Button.interactable = true;
                    ButtonText.text = BuyText;
                }

                if (Item.ProductType == D3ItemShop.D3ProductType.RealMoneyConsumable)
                {
                    if (Button.image.sprite != ImageButtonBuy)
                    {
                        Button.image.sprite = ImageButtonBuy;
                    }
                    Button.interactable = true;
                    ButtonText.text = BuyText;
                }

            }

            if (Item.Unlocked == 1)
            {
                if (Item.ProductType == D3ItemShop.D3ProductType.VirtualMoney || Item.ProductType == D3ItemShop.D3ProductType.RealMoneyConsumable)
                {
                    if (D3ShopCharacter.instace.selectPlayer == Item.ID)
                    {
                        if (Button.image.sprite != ImageButtonSelect)
                        {
                            Button.image.sprite = ImageButtonSelect;
                        }
                        Button.interactable = false;
                        ButtonText.text = InUseText;
                    }

                    if (D3ShopCharacter.instace.selectPlayer != Item.ID)
                    {
                        if (Button.image.sprite != ImageButtonSelect)
                        {
                            Button.image.sprite = ImageButtonSelect;
                        }
                        Button.interactable = true;
                        ButtonText.text = SelectText;
                    }
                }
              
                if (Item.ProductType == D3ItemShop.D3ProductType.RealMoneyNoADS)
                {
#if UNITY_ADS && ENABLE_UNITY_ADS
                    if (Item.Unlocked != PlayerPrefs.GetInt("EnableUnityADS"))
                    {
                        Item.Unlocked = PlayerPrefs.GetInt("EnableUnityADS");
                    }
#endif
                    if (Button.image.sprite != ImageButtonSelect)
                    {
                        Button.image.sprite = ImageButtonSelect;
                    }
                    Button.interactable = false;
                    ButtonText.text = InUseText;
                }
            }
        }

        if (Item.TypeItem == D3ItemShop.TypeItemShop.HoverBoard)
        {
            if (Item.Unlocked == 0)
            {
                if (Item.ProductType == D3ItemShop.D3ProductType.VirtualMoney)
                {
                    if (D3TitleCharacter.instance.coin >= Item.Price)
                    {
                        if (Button.image.sprite != ImageButtonBuy)
                        {
                            Button.image.sprite = ImageButtonBuy;
                        }
                        Button.interactable = true;
                        ButtonText.text = BuyText;
                    }
                    if (D3TitleCharacter.instance.coin < Item.Price)
                    {
                        if (Button.image.sprite != ImageButtonBuy)
                        {
                            Button.image.sprite = ImageButtonBuy;
                        }
                        Button.interactable = false;
                        ButtonText.text = NOCoinText;
                    }
                }
                else {
                    if (Button.image.sprite != ImageButtonBuy)
                    {
                        Button.image.sprite = ImageButtonBuy;
                    }
                    Button.interactable = true;
                    ButtonText.text = BuyText;
                }
                
            }

            if (Item.Unlocked == 1)
            {
                if (D3ShopCharacter.instace.selectHoverBoardr == Item.ID)
                {
                    if (Button.image.sprite != ImageButtonSelect)
                    {
                        Button.image.sprite = ImageButtonSelect;
                    }
                    Button.interactable = false;
                    ButtonText.text = InUseText;
                }

                if (D3ShopCharacter.instace.selectHoverBoardr != Item.ID)
                {
                    if (Button.image.sprite != ImageButtonSelect)
                    {
                        Button.image.sprite = ImageButtonSelect;
                    }
                    Button.interactable = true;
                    ButtonText.text = SelectText;
                }

            }


        }

        }

    public void BuyOrSelectItem()
    {

        if (D3SoundManager.instance != null)
            D3SoundManager.instance.PlayingSound("Button");

        if (Item.TypeItem == D3ItemShop.TypeItemShop.Life)
        {
            if (D3TitleCharacter.instance.coin >= Item.Price)
            {
                int Coinsave = PlayerPrefs.GetInt("Coin") - (int)Item.Price;
                D3GameData.SaveCoin(Coinsave);

                int lifesave = PlayerPrefs.GetInt("Life") + Item.Cant;
                D3GameData.SaveLife(lifesave);

                D3TitleCharacter.instance.UpdateText();
            }
            
        }

        if (Item.TypeItem == D3ItemShop.TypeItemShop.HoverboardKeyUse)
        {
            if (D3TitleCharacter.instance.coin >= Item.Price)
            {
                int Coinsave = PlayerPrefs.GetInt("Coin") - (int)Item.Price;
                D3GameData.SaveCoin(Coinsave);

                int Hoversave = PlayerPrefs.GetInt("HoveBoard") + Item.Cant;
                D3GameData.SaveHoveBoard(Hoversave);

                D3TitleCharacter.instance.UpdateText();
            }
          
        }

        if (Item.TypeItem == D3ItemShop.TypeItemShop.Character)
        {
            if (Item.Unlocked == 0)
            {
                if (D3TitleCharacter.instance.coin >= Item.Price)
                {
                    Item.Unlocked = 1;
                    PlayerPrefs.SetInt(Item.Name + Item.ID,1);
                    Item.Unlocked = PlayerPrefs.GetInt(Item.Name + Item.ID);
                    int Coinsave = PlayerPrefs.GetInt("Coin") - (int)Item.Price;
                    D3GameData.SaveCoin(Coinsave);
                    D3TitleCharacter.instance.UpdateText();
                }
            }
            if (Item.Unlocked == 1)
            {
                if (D3ShopCharacter.instace.selectPlayer != Item.ID)
                {
                    PlayerPrefs.SetInt("SelectPlayer", Item.ID);
                    D3ShopCharacter.instace.UpdateSelectCharacter();
                }

            }

        }

        if (Item.TypeItem == D3ItemShop.TypeItemShop.HoverBoard)
        {
            if (Item.Unlocked == 0)
            {
                if (D3TitleCharacter.instance.coin >= Item.Price)
                {
                    Item.Unlocked = 1;
                    PlayerPrefs.SetInt(Item.Name + Item.ID, 1);
                    Item.Unlocked = PlayerPrefs.GetInt(Item.Name + Item.ID);
                    int Coinsave = PlayerPrefs.GetInt("Coin") - (int)Item.Price;
                    D3GameData.SaveCoin(Coinsave);
                    D3TitleCharacter.instance.UpdateText();
                }
            }
            if (Item.Unlocked == 1)
            {
                if (D3ShopCharacter.instace.selectHoverBoardr != Item.ID)
                {
                    PlayerPrefs.SetInt("selectHoverBoardr", Item.ID);
                    D3ShopCharacter.instace.UpdateSelectCharacter();
                }

            }
        }
    }

#if UNITY_In_APP && ENABLE_UNITY_IAP
    public void BuyConsumableOrNoConsumableItem()
    {
        if (D3SoundManager.instance != null)
            D3SoundManager.instance.PlayingSound("Button");

        if (D3ShopCharacter.instace != null)
        {
            if (Item.TypeItem == D3ItemShop.TypeItemShop.Life || Item.TypeItem == D3ItemShop.TypeItemShop.HoverboardKeyUse || Item.TypeItem == D3ItemShop.TypeItemShop.None)
            {
                D3ShopCharacter.instace.OnInitiatePurchaseItem(Item);
            }
            if (Item.TypeItem == D3ItemShop.TypeItemShop.Character || Item.TypeItem == D3ItemShop.TypeItemShop.HoverBoard) 
            {
                if (Item.Unlocked == 0)
                {
                    D3ShopCharacter.instace.OnInitiatePurchaseItem(Item);
                }
                if (Item.Unlocked == 1)
                {
                    if (Item.TypeItem == D3ItemShop.TypeItemShop.Character)
                    {
                        if (D3ShopCharacter.instace.selectPlayer != Item.ID)
                        {
                            PlayerPrefs.SetInt("SelectPlayer", Item.ID);
                            D3ShopCharacter.instace.UpdateSelectCharacter();
                        }
                    }
                    if (Item.TypeItem == D3ItemShop.TypeItemShop.HoverBoard)
                    {
                        if (D3ShopCharacter.instace.selectHoverBoardr != Item.ID)
                        {
                            PlayerPrefs.SetInt("selectHoverBoardr", Item.ID);
                            D3ShopCharacter.instace.UpdateSelectCharacter();
                        }
                    }
                }
               
            }

        }

    }
#endif

}
