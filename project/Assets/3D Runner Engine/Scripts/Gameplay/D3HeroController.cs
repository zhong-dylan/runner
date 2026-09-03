using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class D3HeroController : MonoBehaviour
{
    
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

    public D3ShopCharacter ShopContoller;
    // Start is called before the first frame update
    void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        for (int i = 0; ShopContoller.ListItemShop.Count > i; i++)
        {
            if (ShopContoller.ListItemShop[i].TypeItem == D3ItemShop.TypeItemShop.Character)
            {
                if (ShopContoller.ListItemShop[i].ID == ShopContoller.selectPlayer)
                {
                    if (ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab != null)
                    {
                        ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.LoadData();

                        NamePlayerText.text = ShopContoller.ListItemShop[i].Name;

                        #region Sprint Time
                        if (ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentAddSprintTime < ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddSprintTimeMax)
                        {
                            ScrollbarAddTimeSprintTime.maxValue = ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddSprintTimeMax;

                            if (!PlayerPrefs.HasKey(ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "SprintTime"))
                            {
                                ScrollbarAddTimeSprintTime.value = ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentAddSprintTime;
                                TextAddTimeSprintTime.text = ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentAddSprintTime + "/" + ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddSprintTimeMax;
                            }
                            else
                            {
                                ScrollbarAddTimeSprintTime.value = PlayerPrefs.GetFloat(ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "SprintTime");
                                TextAddTimeSprintTime.text = PlayerPrefs.GetFloat(ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "SprintTime") + "/" + ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddSprintTimeMax;
                            }

                            if (D3TitleCharacter.instance.coin >= ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentPriceSprintTime)
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

                            PriceTextSprintTime.text = ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentPriceSprintTime.ToString();
                        }
                        else {
                            ButtonSprintTime.interactable = false;
                            ButtonTextSprintTime.text = FinishUpgradeText;
                            ScrollbarAddTimeSprintTime.maxValue = ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddSprintTimeMax;
                            ScrollbarAddTimeSprintTime.value = ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentAddSprintTime;
                            ButtonSprintTime.image.sprite = ImageButtonNoCoin;
                            TextAddTimeSprintTime.text = PlayerPrefs.GetFloat(ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "SprintTime") + "/" + ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddSprintTimeMax;
                            PriceTextSprintTime.text = FinishUpgradeText.ToString();
                        }

                        #endregion

                        #region SpecialTime
                        if (ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentAddSpecialTime < ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddSpecialTimeMax)
                        {
                            ScrollbarAddTimeSpecialTime.maxValue = ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddSpecialTimeMax;

                            if (!PlayerPrefs.HasKey(ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "SpecialTime"))
                            {
                                ScrollbarAddTimeSpecialTime.value = ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentAddSpecialTime;
                                TextAddTimeSpecialTime.text = ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentAddSpecialTime + "/" + ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddSpecialTimeMax;
                            }
                            else
                            {
                                ScrollbarAddTimeSpecialTime.value = PlayerPrefs.GetFloat(ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "SpecialTime");
                                TextAddTimeSpecialTime.text = PlayerPrefs.GetFloat(ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "SpecialTime") + "/" + ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddSpecialTimeMax;
                            }

                            if (D3TitleCharacter.instance.coin >= ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentPriceSpecialTime)
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
                            PriceTextSpecialTime.text = ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentPriceSpecialTime.ToString();

                        }
                        else
                        {
                            ButtonSpecialTime.interactable = false;
                            ButtonTextSpecialTime.text = FinishUpgradeText;
                            ScrollbarAddTimeSpecialTime.maxValue = ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddSpecialTimeMax;
                            ScrollbarAddTimeSpecialTime.value = ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentAddSpecialTime;
                            ButtonSpecialTime.image.sprite = ImageButtonNoCoin;
                            TextAddTimeSpecialTime.text = PlayerPrefs.GetFloat(ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "SpecialTime") + "/" + ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddSpecialTimeMax;
                            PriceTextSpecialTime.text = FinishUpgradeText.ToString();


                        }


                        #endregion

                        #region MultiplyTime
                        if (ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentAddMultiplyTime < ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddMultiplyTimeMax)
                        {
                            ScrollbarAddTimeMultiplyTime.maxValue = ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddMultiplyTimeMax;

                            if (!PlayerPrefs.HasKey(ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "MultiplyTime"))
                            {
                                ScrollbarAddTimeMultiplyTime.value = ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentAddMultiplyTime;
                                TextAddTimeMultiplyTime.text = ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentAddMultiplyTime + "/" + ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddMultiplyTimeMax;
                            }
                            else
                            {
                                ScrollbarAddTimeMultiplyTime.value = PlayerPrefs.GetFloat(ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "MultiplyTime");
                                TextAddTimeMultiplyTime.text = PlayerPrefs.GetFloat(ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "MultiplyTime") + "/" + ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddMultiplyTimeMax;
                            }

                            if (D3TitleCharacter.instance.coin >= ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentPriceMultiplyTime)
                            {
                                if (ButtonMultiplyTime.image.sprite != ImageButtonBuy)
                                {
                                    ButtonMultiplyTime.image.sprite = ImageButtonBuy;
                                }
                                ButtonMultiplyTime.interactable = true;
                                ButtonTextMultiplyTime.text = BuyText;
                            }else
                            {
                                if (ButtonMultiplyTime.image.sprite != ImageButtonNoCoin)
                                {
                                    ButtonMultiplyTime.image.sprite = ImageButtonNoCoin;
                                }
                                ButtonMultiplyTime.interactable = false;
                                ButtonTextMultiplyTime.text = NOCoinText;
                            }

                            PriceTextMultiplyTime.text = ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentPriceMultiplyTime.ToString();
                        }
                        else
                        {
                            ButtonMultiplyTime.interactable = false;
                            ButtonTextMultiplyTime.text = FinishUpgradeText;
                            ScrollbarAddTimeMultiplyTime.maxValue = ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddMultiplyTimeMax;
                            ScrollbarAddTimeMultiplyTime.value = ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentAddMultiplyTime;
                            ButtonMultiplyTime.image.sprite = ImageButtonNoCoin;
                            TextAddTimeMultiplyTime.text = PlayerPrefs.GetFloat(ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "MultiplyTime") + "/" + ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddMultiplyTimeMax;
                            PriceTextMultiplyTime.text = FinishUpgradeText.ToString();


                        }

                        #endregion

                        #region MagnetTime
                        if (ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentAddMagnetTime < ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddMagnetTimeMax)
                        {
                            ScrollbarAddTimeMagnetTime.maxValue = ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddMagnetTimeMax;

                            if (!PlayerPrefs.HasKey(ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "MagnetTime"))
                            {
                                ScrollbarAddTimeMagnetTime.value = ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentAddMagnetTime;
                                TextAddTimeMagnetTime.text = ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentAddMagnetTime + "/" + ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddMagnetTimeMax;
                            }
                            else
                            {
                                ScrollbarAddTimeMagnetTime.value = PlayerPrefs.GetFloat(ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "MagnetTime");
                                TextAddTimeMagnetTime.text = PlayerPrefs.GetFloat(ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "MagnetTime") + "/" + ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddMagnetTimeMax;
                            }

                            if (D3TitleCharacter.instance.coin >= ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentPriceMagnetTime)
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

                            PriceTextMagnetTime.text = ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentPriceMagnetTime.ToString();
                        }
                        else
                        {
                            ButtonMagnetTime.interactable = false;
                            ButtonTextMagnetTime.text = FinishUpgradeText;
                            ScrollbarAddTimeMagnetTime.maxValue = ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddMagnetTimeMax;
                            ScrollbarAddTimeMagnetTime.value = ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentAddMagnetTime;
                            ButtonMagnetTime.image.sprite = ImageButtonNoCoin;
                            TextAddTimeMagnetTime.text = PlayerPrefs.GetFloat(ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "MagnetTime") + "/" + ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddMagnetTimeMax;
                            PriceTextMagnetTime.text = FinishUpgradeText.ToString();


                        }

                        #endregion

                        #region ShieldTime
                        if (ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentAddShieldTime < ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddShieldTimeMax)
                        {
                            ScrollbarAddTimeShieldTime.maxValue = ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddShieldTimeMax;

                            if (!PlayerPrefs.HasKey(ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "ShieldTime"))
                            {
                                ScrollbarAddTimeShieldTime.value = ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentAddShieldTime;
                                TextAddTimeShieldTime.text = ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddShieldTime + "/" + ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddShieldTimeMax;
                            }
                            else
                            {
                                ScrollbarAddTimeShieldTime.value = PlayerPrefs.GetFloat(ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "ShieldTime");
                                TextAddTimeShieldTime.text = PlayerPrefs.GetFloat(ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "ShieldTime") + "/" + ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddShieldTimeMax;
                            }

                            if (D3TitleCharacter.instance.coin >= ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentPriceShieldTime)
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

                            PriceTextShieldTime.text = ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentPriceShieldTime.ToString();
                        }
                        else
                        {
                            ButtonShieldTime.interactable = false;
                            ButtonTextShieldTime.text = FinishUpgradeText;
                            ScrollbarAddTimeShieldTime.maxValue = ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddShieldTimeMax;
                            ScrollbarAddTimeShieldTime.value = ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentAddShieldTime;
                            ButtonShieldTime.image.sprite = ImageButtonNoCoin;
                            TextAddTimeShieldTime.text = PlayerPrefs.GetFloat(ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "ShieldTime") + "/" + ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddShieldTimeMax;
                            PriceTextShieldTime.text = FinishUpgradeText.ToString();


                        }

                        #endregion

                    }
                }
            }
        }


    }

    public void BuySprintTime()
    {
        if (D3SoundManager.instance != null)
            D3SoundManager.instance.PlayingSound("Button");

        for (int i = 0; ShopContoller.ListItemShop.Count > i; i++)
        {
            if (ShopContoller.ListItemShop[i].TypeItem == D3ItemShop.TypeItemShop.Character)
            {
                if (ShopContoller.ListItemShop[i].ID == ShopContoller.selectPlayer)
                {
                    if (ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab != null)
                    {
                        if (D3TitleCharacter.instance.coin >= ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentPriceSprintTime)
                        {
                            int Coinsave = PlayerPrefs.GetInt("Coin") - ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentPriceSprintTime;
                            D3GameData.SaveCoin(Coinsave);

                            ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddSprintTimeVoid(MultiplyPriceWhenPurchasing);

                            UpdateText();

                            D3TitleCharacter.instance.UpdateText();
                        }
                        else {
                            UpdateText();
                        }
                    }
                }
            }
        }

    }


    public void BuySpecialTime()
    {
        if (D3SoundManager.instance != null)
            D3SoundManager.instance.PlayingSound("Button");

        for (int i = 0; ShopContoller.ListItemShop.Count > i; i++)
        {
            if (ShopContoller.ListItemShop[i].TypeItem == D3ItemShop.TypeItemShop.Character)
            {
                if (ShopContoller.ListItemShop[i].ID == ShopContoller.selectPlayer)
                {
                    if (ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab != null)
                    {
                        if (D3TitleCharacter.instance.coin >= ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentPriceSpecialTime)
                        {
                            int Coinsave = PlayerPrefs.GetInt("Coin") - ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentPriceSpecialTime;
                            D3GameData.SaveCoin(Coinsave);

                            ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddSpecialTimeVoid(MultiplyPriceWhenPurchasing);

                            UpdateText();

                            D3TitleCharacter.instance.UpdateText();
                        }
                        else
                        {
                            UpdateText();
                        }
                    }
                }
            }
        }

    }


    public void BuyMultiplyTime()
    {
        if (D3SoundManager.instance != null)
            D3SoundManager.instance.PlayingSound("Button");

        for (int i = 0; ShopContoller.ListItemShop.Count > i; i++)
        {
            if (ShopContoller.ListItemShop[i].TypeItem == D3ItemShop.TypeItemShop.Character)
            {
                if (ShopContoller.ListItemShop[i].ID == ShopContoller.selectPlayer)
                {
                    if (ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab != null)
                    {
                        if (D3TitleCharacter.instance.coin >= ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentPriceMultiplyTime)
                        {
                            int Coinsave = PlayerPrefs.GetInt("Coin") - ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentPriceMultiplyTime;
                            D3GameData.SaveCoin(Coinsave);

                            ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddMultiplyTimeVoid(MultiplyPriceWhenPurchasing);

                            UpdateText();

                            D3TitleCharacter.instance.UpdateText();
                        } else
                        {
                            UpdateText();
                        }
                    }
                }
            }
        }

    }

    public void BuyMagnetTime()
    {
        if (D3SoundManager.instance != null)
            D3SoundManager.instance.PlayingSound("Button");

        for (int i = 0; ShopContoller.ListItemShop.Count > i; i++)
        {
            if (ShopContoller.ListItemShop[i].TypeItem == D3ItemShop.TypeItemShop.Character)
            {
                if (ShopContoller.ListItemShop[i].ID == ShopContoller.selectPlayer)
                {
                    if (ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab != null)
                    {
                        if (D3TitleCharacter.instance.coin >= ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentPriceMagnetTime)
                        {
                            int Coinsave = PlayerPrefs.GetInt("Coin") - ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentPriceMagnetTime;
                            D3GameData.SaveCoin(Coinsave);

                            ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddMagnetTimeVoid(MultiplyPriceWhenPurchasing);


                            UpdateText();

                            D3TitleCharacter.instance.UpdateText();
                        }else
                        {
                            UpdateText();
                        }
                    }
                }
            }
        }

    }

    public void BuyShieldTime()
    {
        if (D3SoundManager.instance != null)
            D3SoundManager.instance.PlayingSound("Button");

        for (int i = 0; ShopContoller.ListItemShop.Count > i; i++)
        {
            if (ShopContoller.ListItemShop[i].TypeItem == D3ItemShop.TypeItemShop.Character)
            {
                if (ShopContoller.ListItemShop[i].ID == ShopContoller.selectPlayer)
                {
                    if (ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab != null)
                    {
                        if (D3TitleCharacter.instance.coin >= ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentPriceShieldTime)
                        {
                            int Coinsave = PlayerPrefs.GetInt("Coin") - ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentPriceShieldTime;
                            D3GameData.SaveCoin(Coinsave);

                            ShopContoller.ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddShieldTimeVoid(MultiplyPriceWhenPurchasing);


                            UpdateText();

                            D3TitleCharacter.instance.UpdateText();
                        }else
                        {
                            UpdateText();
                        }
                    }
                }
            }
        }

    }


}

