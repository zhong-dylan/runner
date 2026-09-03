using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;


#if UNITY_In_APP && ENABLE_UNITY_IAP
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;
using UnityEngine.Purchasing.Extension;
#endif


[System.Serializable]
public class D3ShopCharacter : MonoBehaviour
#if UNITY_In_APP && ENABLE_UNITY_IAP
    , IDetailedStoreListener
#endif
{
    public int selectPlayer;

    public int selectHoverBoardr;

    public static D3ShopCharacter instace;

    public D3ListItemShop TempleteListItem;
    public D3ListItemShop TempleteListItemNonConsumable;

    public Transform m_ContentHero;
    public Transform m_ContentHoverboard;
    public Transform m_ContentItems;
    public Transform m_ContentNonConsumable;

    public Scrollbar ScrollbarContentHero;
    public Scrollbar ScrollbarContentHoverboard;
    public Scrollbar ScrollbarContentItems;

    public GameObject PlayerView;
    public GameObject PlayerCam;

    public bool EnableInAppPurchasing = false;
    public bool useForGooglePlay;
    public bool useForAmazon;
    public bool useForWindows;
    public bool debug;
    public bool useReceiptValidation;

    public List<D3ItemShop> ListItemShop;
    private bool itemsGenerated;


#if UNITY_In_APP && ENABLE_UNITY_IAP
    public IStoreController controller;
    public IExtensionProvider extensions;
    public D3ItemShop Item;
    public string environment = "production";



    void Start()
    {
        Init();
    }

#endif

#if !UNITY_In_APP || !ENABLE_UNITY_IAP
    void Start()
    {
    Init();
    }
#endif
    void Init()
    {
        instace = this;

    }
    void InstantiatePlayerCam()
    {
        if (PlayerView == null)
        {
            return;
        }

        if (PlayerCam != null)
        {
            DestroyImmediate(PlayerCam);
        }

        for (int i = 0; ListItemShop.Count > i; i++)
        {
            if (ListItemShop[i].TypeItem == D3ItemShop.TypeItemShop.Character)
            {
                if (ListItemShop[i].ID == selectPlayer)
                {
                    PlayerCam = Instantiate(ListItemShop[i].OnlyForCharacterTypeAddPrefab.gameObject, PlayerView.transform.position, Quaternion.identity);

                    if (PlayerCam.GetComponent<D3Controller>())
                    {
                        if (PlayerCam.GetComponent<D3Controller>().magnetCollider.gameObject.GetComponent<D3Magnet>() != null)
                        {
                            if (PlayerCam.GetComponent<D3Controller>().magnetCollider.GetComponent<D3Magnet>().ObjectMagnet != null)
                            {
                                PlayerCam.GetComponent<D3Controller>().magnetCollider.GetComponent<D3Magnet>().ObjectMagnet.SetActive(false);
                            }
                        }
                        if (PlayerCam.GetComponent<D3Controller>().magnetCollider != null)
                        {
                            PlayerCam.GetComponent<D3Controller>().magnetCollider.SetActive(false);
                        }
                        if (PlayerCam.GetComponent<D3Controller>().coinRotate != null)
                        {
                            PlayerCam.GetComponent<D3Controller>().coinRotate.gameObject.SetActive(false);
                        }
                        if (PlayerCam.GetComponent<D3Controller>().ImmortalityEffect != null)
                        {
                            PlayerCam.GetComponent<D3Controller>().ImmortalityEffect.SetActive(false);
                        }
                        if (PlayerCam.GetComponent<D3Controller>().ImmortalityEffect != null)
                        {
                            PlayerCam.GetComponent<D3Controller>().ImmortalityEffect.SetActive(false);
                        }
                        if (PlayerCam.GetComponent<D3Controller>().HoverBike != null)
                        {
                            PlayerCam.GetComponent<D3Controller>().HoverBike.SetActive(false);
                        }
                        if (PlayerCam.GetComponent<D3Controller>().SpecialItem != null)
                        {
                            PlayerCam.GetComponent<D3Controller>().SpecialItem.SetActive(false);
                        }
                        if (PlayerCam.GetComponent<D3Controller>().HoverBoardPositionList != null)
                        {
                            PlayerCam.GetComponent<D3Controller>().HoverBoardPositionList.SetActive(false);
                        }
                        Destroy(PlayerCam.GetComponent<D3Controller>());
                    }
                    if (instace.PlayerCam.GetComponent<D3AnimationManager>())
                    {
                        Destroy(PlayerCam.GetComponent<D3AnimationManager>());
                    }
                    if (PlayerCam.GetComponent<CharacterController>())
                    {
                        Destroy(PlayerCam.GetComponent<CharacterController>());
                    }
                    Destroy(PlayerCam.GetComponent<D3Controller>());
                    PlayerCam.name = "Player";
                    PlayerCam.transform.parent = instace.PlayerView.transform;
                    PlayerCam.GetComponent<Animator>().PlayInFixedTime("PauseUI");
                    PlayerCam.GetComponent<Animator>().SetBool("IsUIManger", true);
                }

            }
        }

        
    }


    public void UpdateSelectCharacter()
	{
		selectPlayer = PlayerPrefs.GetInt("SelectPlayer");

        selectHoverBoardr = PlayerPrefs.GetInt("selectHoverBoardr");

        InstantiatePlayerCam();
    }



    public void LoadItemsIfNeeded()
    {
        ResolveContentReferences();
        UpdateSelectCharacter();

        if (itemsGenerated)
        {
            return;
        }

        ResolveTemplateReferences();
        if (TempleteListItem == null)
        {
            Debug.LogError("Shop item template is not assigned. Found templates: " + GetTemplateDebugNames());
            return;
        }

        if (ListItemShop == null)
        {
            Debug.LogError("Shop item list is not assigned.");
            return;
        }

        if (m_ContentHero == null || m_ContentHoverboard == null || m_ContentItems == null)
        {
            Debug.LogError("Shop content root is not assigned.");
            return;
        }

        GenerateItems();
        itemsGenerated = true;
    }

    private void ResolveTemplateReferences()
    {
        if (TempleteListItem != null && TempleteListItemNonConsumable != null)
        {
            return;
        }

        if (TempleteListItem == null)
        {
            var template = FindChild("TemplateListShopItemHero");
            if (template != null)
            {
                TempleteListItem = template.GetComponentInChildren<D3ListItemShop>(true);
            }
        }

        if (TempleteListItemNonConsumable == null)
        {
            var template = FindChild("TemplateListNonConsumableItems");
            if (template != null)
            {
                TempleteListItemNonConsumable = template.GetComponentInChildren<D3ListItemShop>(true);
            }
        }

        var templates = GetComponentsInChildren<D3ListItemShop>(true);
        foreach (var template in templates)
        {
            if (template == null)
            {
                continue;
            }

            if (TempleteListItem == null && template.TypeTemplate == D3ListItemShop.D3TypeTemplate.VirtualOrRealMoney)
            {
                TempleteListItem = template;
            }

            if (TempleteListItemNonConsumable == null && template.TypeTemplate == D3ListItemShop.D3TypeTemplate.NoADS)
            {
                TempleteListItemNonConsumable = template;
            }
        }
    }

    public void BindRuntimeReferences(GameObject shopRoot)
    {
        if (shopRoot == null)
        {
            return;
        }

        if (TempleteListItem == null)
        {
            var template = FindChild(shopRoot, "TemplateListShopItemHero");
            if (template != null)
            {
                TempleteListItem = template.GetComponentInChildren<D3ListItemShop>(true);
            }
        }

        if (TempleteListItemNonConsumable == null)
        {
            var template = FindChild(shopRoot, "TemplateListNonConsumableItems");
            if (template != null)
            {
                TempleteListItemNonConsumable = template.GetComponentInChildren<D3ListItemShop>(true);
            }
        }

        foreach (var template in shopRoot.GetComponentsInChildren<D3ListItemShop>(true))
        {
            if (template == null)
            {
                continue;
            }

            if (TempleteListItem == null && IsNamedTemplate(template.transform, "TemplateListShopItemHero"))
            {
                TempleteListItem = template;
            }

            if (TempleteListItemNonConsumable == null && IsNamedTemplate(template.transform, "TemplateListNonConsumableItems"))
            {
                TempleteListItemNonConsumable = template;
            }
        }

        if (m_ContentHero == null)
        {
            var content = FindChild(shopRoot, "ContentHero");
            if (content != null)
            {
                m_ContentHero = content.transform;
            }
        }

        if (m_ContentHoverboard == null)
        {
            var content = FindChild(shopRoot, "ContentHoverboard") ?? FindChild(shopRoot, "ContentHoverBoard");
            if (content != null)
            {
                m_ContentHoverboard = content.transform;
            }
        }

        if (m_ContentItems == null)
        {
            var content = FindChild(shopRoot, "ContentItems");
            if (content != null)
            {
                m_ContentItems = content.transform;
            }
        }

        if (m_ContentNonConsumable == null)
        {
            var content = FindChild(shopRoot, "ContentNonConsumable");
            if (content != null)
            {
                m_ContentNonConsumable = content.transform;
            }
        }
    }

    private void ResolveContentReferences()
    {
        if (m_ContentHero == null)
        {
            var contentHero = FindChild("ContentHero");
            if (contentHero != null)
            {
                m_ContentHero = contentHero.transform;
            }
        }

        if (m_ContentHoverboard == null)
        {
            var contentHoverboard = FindChild("ContentHoverboard") ?? FindChild("ContentHoverBoard");
            if (contentHoverboard != null)
            {
                m_ContentHoverboard = contentHoverboard.transform;
            }
        }

        if (m_ContentItems == null)
        {
            var contentItems = FindChild("ContentItems");
            if (contentItems != null)
            {
                m_ContentItems = contentItems.transform;
            }
        }

        if (m_ContentNonConsumable == null)
        {
            var contentNonConsumable = FindChild("ContentNonConsumable");
            if (contentNonConsumable != null)
            {
                m_ContentNonConsumable = contentNonConsumable.transform;
            }
        }
    }

    private GameObject FindChild(string childName)
    {
        var child = FindChild(gameObject, childName);
        if (child != null)
        {
            return child;
        }

        return transform.root != transform ? FindChild(transform.root.gameObject, childName) : null;
    }

    private static GameObject FindChild(GameObject root, string childName)
    {
        var transforms = root.GetComponentsInChildren<Transform>(true);
        foreach (var child in transforms)
        {
            if (child.name == childName)
            {
                return child.gameObject;
            }
        }

        return null;
    }

    private static bool IsNamedTemplate(Transform template, string templateName)
    {
        while (template != null)
        {
            if (template.name == templateName)
            {
                return true;
            }

            template = template.parent;
        }

        return false;
    }

    private string GetTemplateDebugNames()
    {
        var templates = GetComponentsInChildren<D3ListItemShop>(true);
        var names = new List<string>();
        foreach (var template in templates)
        {
            if (template != null)
            {
                names.Add(template.name);
            }
        }

        if (transform.root != transform)
        {
            foreach (var template in transform.root.GetComponentsInChildren<D3ListItemShop>(true))
            {
                if (template != null && !names.Contains(template.name))
                {
                    names.Add(template.name);
                }
            }
        }

        return names.Count > 0 ? string.Join(", ", names) : "none";
    }

    void GenerateItems()
	{
#if UNITY_In_APP && ENABLE_UNITY_IAP
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            bool InitShop = false;
#endif
        for (int i = 0; ListItemShop.Count > i; i++)
        {
            var item_go = Instantiate(TempleteListItem);
            var item_NonConsumable = TempleteListItemNonConsumable != null ? Instantiate(TempleteListItemNonConsumable) : null;
            item_go.gameObject.SetActive(false);
            if (item_NonConsumable != null)
            {
                item_NonConsumable.gameObject.SetActive(false);
            }

            if (EnableInAppPurchasing)
            {
#if UNITY_In_APP && ENABLE_UNITY_IAP
                if (ListItemShop[i].ProductType == D3ItemShop.D3ProductType.RealMoneyConsumable)
                {
                    if (item_go.Item == null)
                    {
                        item_go.Item = new D3ItemShop();
                    }

                    item_go.Item.TypeItem = ListItemShop[i].TypeItem;
                    item_go.Item.Name = ListItemShop[i].Name;
                    item_go.Item.ProductCode = ListItemShop[i].ProductCode;
                    item_go.Item.ProductType = ListItemShop[i].ProductType;
                    item_go.Item.ID = ListItemShop[i].ID;
                    item_go.Item.Cant = ListItemShop[i].Cant;
                    item_go.Item.Price = ListItemShop[i].Price;
                    item_go.Item.Image = ListItemShop[i].Image;
                    item_go.Item.Unlocked = ListItemShop[i].Unlocked;
                    if (!item_go.gameObject.activeSelf)
                        item_go.gameObject.SetActive(true);

                    if (ListItemShop[i].TypeItem == D3ItemShop.TypeItemShop.Character)
                    {
                        if (ListItemShop[i].OnlyForCharacterTypeAddPrefab != null)
                        {
                            item_go.Item.OnlyForCharacterTypeAddPrefab = ListItemShop[i].OnlyForCharacterTypeAddPrefab;

                            item_go.ContentStatistics.SetActive(true);

                            item_go.ScrollbarAddSprintTime.maxValue = ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddSprintTimeMax;

                            if (!PlayerPrefs.HasKey(ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "SprintTime"))
                            {

                                item_go.ScrollbarAddSprintTime.value = ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentAddSprintTime;
                                item_go.TextAddSprintTime.text = ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentAddSprintTime + "/" + ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddSprintTimeMax;
                            }
                            else
                            {
                                item_go.ScrollbarAddSprintTime.value = PlayerPrefs.GetFloat(ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "SprintTime");
                                item_go.TextAddSprintTime.text = PlayerPrefs.GetFloat(ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "SprintTime") + "/" + ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddSprintTimeMax;
                            }

                            item_go.ScrollbarAddSpecialTime.maxValue = ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddSpecialTimeMax;

                            if (!PlayerPrefs.HasKey(ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "SpecialTime"))
                            {
                                item_go.ScrollbarAddSpecialTime.value = ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentAddSpecialTime;
                                item_go.TextAddSpecialTime.text = ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddSpecialTime + "/" + ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddSpecialTimeMax;
                            }
                            else
                            {
                                item_go.ScrollbarAddSpecialTime.value = PlayerPrefs.GetFloat(ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "SpecialTime");
                                item_go.TextAddSpecialTime.text = PlayerPrefs.GetFloat(ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "SpecialTime") + "/" + ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddSpecialTimeMax;
                            }

                            item_go.ScrollbarAddMultiplyTime.maxValue = ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddMultiplyTimeMax;

                            if (!PlayerPrefs.HasKey(ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "MultiplyTime"))
                            {
                                item_go.ScrollbarAddMultiplyTime.value = ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentAddMultiplyTime;
                                item_go.TextAddMultiplyTime.text = ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddMultiplyTime + "/" + ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddMultiplyTimeMax;
                            }
                            else
                            {
                                item_go.ScrollbarAddMultiplyTime.value = PlayerPrefs.GetFloat(ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "MultiplyTime");
                                item_go.TextAddMultiplyTime.text = PlayerPrefs.GetFloat(ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "MultiplyTime") + "/" + ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddMultiplyTimeMax;
                            }

                            item_go.ScrollbarAddMagnetTime.maxValue = ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddMagnetTimeMax;

                            if (!PlayerPrefs.HasKey(ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "MagnetTime"))
                            {
                                item_go.ScrollbarAddMagnetTime.value = ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentAddMagnetTime;
                                item_go.TextAddMagnetTime.text = ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddMagnetTime + "/" + ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddMagnetTimeMax;
                            }
                            else
                            {
                                item_go.ScrollbarAddMagnetTime.value = PlayerPrefs.GetFloat(ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "MagnetTime");
                                item_go.TextAddMagnetTime.text = PlayerPrefs.GetFloat(ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "MagnetTime") + "/" + ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddMagnetTimeMax;
                            }

                            item_go.ScrollbarAddShieldTime.maxValue = ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddShieldTimeMax;

                            if (!PlayerPrefs.HasKey(ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "ShieldTime"))
                            {
                                item_go.ScrollbarAddShieldTime.value = ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentAddShieldTime;
                                item_go.TextAddShieldTime.text = ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddShieldTime + "/" + ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddShieldTimeMax;
                            }
                            else
                            {
                                item_go.ScrollbarAddShieldTime.value = PlayerPrefs.GetFloat(ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "ShieldTime");
                                item_go.TextAddShieldTime.text = PlayerPrefs.GetFloat(ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "ShieldTime") + "/" + ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddShieldTimeMax;
                            }

                        }
                        else
                        {
                            item_go.ContentStatistics.SetActive(false);

                        }
                        item_go.gameObject.transform.SetParent(m_ContentHero);
                        item_go.transform.localScale = Vector3.one;

                    }

                    if (ListItemShop[i].TypeItem == D3ItemShop.TypeItemShop.HoverBoard)
                    {
                        item_go.ContentStatistics.SetActive(false);
                        item_go.gameObject.transform.SetParent(m_ContentHoverboard);
                        item_go.transform.localScale = Vector3.one;
                    }

                    if (ListItemShop[i].TypeItem == D3ItemShop.TypeItemShop.Life || ListItemShop[i].TypeItem == D3ItemShop.TypeItemShop.HoverboardKeyUse)
                    {
                        item_go.ContentStatistics.SetActive(false);
                        item_go.gameObject.transform.SetParent(m_ContentItems);
                        item_go.transform.localScale = Vector3.one;
                    }

                    builder.AddProduct(ListItemShop[i].ProductCode, ProductType.Consumable);
                    InitShop = true;

                }

                if (ListItemShop[i].ProductType == D3ItemShop.D3ProductType.RealMoneyNoADS)
                {
                    if (item_NonConsumable == null)
                    {
                        continue;
                    }

                    if (item_NonConsumable.Item == null)
                    {
                        item_NonConsumable.Item = new D3ItemShop();
                    }

                    item_NonConsumable.Item.TypeItem = ListItemShop[i].TypeItem;
                    item_NonConsumable.Item.Name = ListItemShop[i].Name;
                    item_NonConsumable.Item.ProductCode = ListItemShop[i].ProductCode;
                    item_NonConsumable.Item.ProductType = ListItemShop[i].ProductType;
                    item_NonConsumable.Item.ID = ListItemShop[i].ID;
                    item_NonConsumable.Item.Cant = ListItemShop[i].Cant;
                    item_NonConsumable.Item.Price = ListItemShop[i].Price;
                    item_NonConsumable.Item.Image = ListItemShop[i].Image;
                    item_NonConsumable.Item.Unlocked = ListItemShop[i].Unlocked;
                    if (!item_NonConsumable.gameObject.activeSelf)
                    {
                        item_NonConsumable.gameObject.SetActive(true);
                    }
                    item_NonConsumable.gameObject.transform.SetParent(m_ContentNonConsumable);
                    item_NonConsumable.transform.localScale = Vector3.one;
                    item_NonConsumable.transform.transform.localPosition = new Vector3(0f, 0f, 0f);

                    builder.AddProduct(ListItemShop[i].ProductCode, ProductType.NonConsumable);
                    InitShop = true;


                }


#endif
            }

            if (ListItemShop[i].ProductType == D3ItemShop.D3ProductType.VirtualMoney)
            {
                if (item_go.Item == null)
                {
                    item_go.Item = new D3ItemShop();
                }

                item_go.Item.TypeItem = ListItemShop[i].TypeItem;
                item_go.Item.Name = ListItemShop[i].Name;
                item_go.Item.ProductCode = ListItemShop[i].ProductCode;
                item_go.Item.ProductType = ListItemShop[i].ProductType;
                item_go.Item.ID = ListItemShop[i].ID;
                item_go.Item.Cant = ListItemShop[i].Cant;
                item_go.Item.Price = ListItemShop[i].Price;
                item_go.Item.Image = ListItemShop[i].Image;
                item_go.Item.Unlocked = ListItemShop[i].Unlocked;
                if (!item_go.gameObject.activeSelf)
                    item_go.gameObject.SetActive(true);

                if (ListItemShop[i].TypeItem == D3ItemShop.TypeItemShop.Character)
                {
                    if (ListItemShop[i].OnlyForCharacterTypeAddPrefab != null)
                    {
                        item_go.Item.OnlyForCharacterTypeAddPrefab = ListItemShop[i].OnlyForCharacterTypeAddPrefab;

                        item_go.ContentStatistics.SetActive(true);

                        item_go.ScrollbarAddSprintTime.maxValue = ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddSprintTimeMax;

                        if (!PlayerPrefs.HasKey(ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "SprintTime"))
                        {

                            item_go.ScrollbarAddSprintTime.value = ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentAddSprintTime;
                            item_go.TextAddSprintTime.text = ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentAddSprintTime + "/" + ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddSprintTimeMax;
                        }
                        else
                        {
                            item_go.ScrollbarAddSprintTime.value = PlayerPrefs.GetFloat(ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "SprintTime");
                            item_go.TextAddSprintTime.text = PlayerPrefs.GetFloat(ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "SprintTime") + "/" + ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddSprintTimeMax;
                        }

                        item_go.ScrollbarAddSpecialTime.maxValue = ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddSpecialTimeMax;

                        if (!PlayerPrefs.HasKey(ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "SpecialTime"))
                        {
                            item_go.ScrollbarAddSpecialTime.value = ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentAddSpecialTime;
                            item_go.TextAddSpecialTime.text = ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddSpecialTime + "/" + ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddSpecialTimeMax;
                        }
                        else
                        {
                            item_go.ScrollbarAddSpecialTime.value = PlayerPrefs.GetFloat(ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "SpecialTime");
                            item_go.TextAddSpecialTime.text = PlayerPrefs.GetFloat(ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "SpecialTime") + "/" + ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddSpecialTimeMax;
                        }

                        item_go.ScrollbarAddMultiplyTime.maxValue = ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddMultiplyTimeMax;

                        if (!PlayerPrefs.HasKey(ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "MultiplyTime"))
                        {
                            item_go.ScrollbarAddMultiplyTime.value = ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentAddMultiplyTime;
                            item_go.TextAddMultiplyTime.text = ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddMultiplyTime + "/" + ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddMultiplyTimeMax;
                        }
                        else
                        {
                            item_go.ScrollbarAddMultiplyTime.value = PlayerPrefs.GetFloat(ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "MultiplyTime");
                            item_go.TextAddMultiplyTime.text = PlayerPrefs.GetFloat(ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "MultiplyTime") + "/" + ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddMultiplyTimeMax;
                        }

                        item_go.ScrollbarAddMagnetTime.maxValue = ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddMagnetTimeMax;

                        if (!PlayerPrefs.HasKey(ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "MagnetTime"))
                        {
                            item_go.ScrollbarAddMagnetTime.value = ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentAddMagnetTime;
                            item_go.TextAddMagnetTime.text = ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddMagnetTime + "/" + ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddMagnetTimeMax;
                        }
                        else
                        {
                            item_go.ScrollbarAddMagnetTime.value = PlayerPrefs.GetFloat(ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "MagnetTime");
                            item_go.TextAddMagnetTime.text = PlayerPrefs.GetFloat(ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "MagnetTime") + "/" + ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddMagnetTimeMax;
                        }

                        item_go.ScrollbarAddShieldTime.maxValue = ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddShieldTimeMax;

                        if (!PlayerPrefs.HasKey(ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "ShieldTime"))
                        {
                            item_go.ScrollbarAddShieldTime.value = ListItemShop[i].OnlyForCharacterTypeAddPrefab.CurrentAddShieldTime;
                            item_go.TextAddShieldTime.text = ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddShieldTime + "/" + ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddShieldTimeMax;
                        }
                        else
                        {
                            item_go.ScrollbarAddShieldTime.value = PlayerPrefs.GetFloat(ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "ShieldTime");
                            item_go.TextAddShieldTime.text = PlayerPrefs.GetFloat(ListItemShop[i].OnlyForCharacterTypeAddPrefab.name + "ShieldTime") + "/" + ListItemShop[i].OnlyForCharacterTypeAddPrefab.AddShieldTimeMax;
                        }

                    }
                    else
                    {
                        item_go.ContentStatistics.SetActive(false);

                    }
                    item_go.gameObject.transform.SetParent(m_ContentHero);
                    item_go.transform.localScale = Vector3.one;

                }

                if (ListItemShop[i].TypeItem == D3ItemShop.TypeItemShop.HoverBoard)
                {
                    item_go.ContentStatistics.SetActive(false);
                    item_go.gameObject.transform.SetParent(m_ContentHoverboard);
                    item_go.transform.localScale = Vector3.one;
                }

                if (ListItemShop[i].TypeItem == D3ItemShop.TypeItemShop.Life || ListItemShop[i].TypeItem == D3ItemShop.TypeItemShop.HoverboardKeyUse)
                {
                    item_go.ContentStatistics.SetActive(false);
                    item_go.gameObject.transform.SetParent(m_ContentItems);
                    item_go.transform.localScale = Vector3.one;
                }

            }

            item_go.transform.transform.localPosition = new Vector3(0f, 0f, 0f);
            if (item_NonConsumable != null)
            {
                item_NonConsumable.transform.transform.localPosition = new Vector3(0f, 0f, 0f);
            }
        }
#if UNITY_In_APP && ENABLE_UNITY_IAP
        if (EnableInAppPurchasing && InitShop)
        {

            UnityPurchasing.Initialize(this, builder);
            InitShop = false;

    }
#endif
        if (ScrollbarContentHero != null)
        {
            ScrollbarContentHero.value = 0f;
        }
        if (ScrollbarContentHoverboard != null)
        {
            ScrollbarContentHoverboard.value = 0f;
        }
        if (ScrollbarContentItems != null)
        {
            ScrollbarContentItems.value = 0f;
        }
    }

#if UNITY_In_APP && ENABLE_UNITY_IAP

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        this.controller = controller;
        this.extensions = extensions;
        CheckItemPurchase();
    }

    public void OnInitiatePurchaseItem(D3ItemShop item)
    {
        Item = item;
        controller.InitiatePurchase(item.ProductCode);
    }
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {

        var product = purchaseEvent.purchasedProduct;


        if (Item.ProductType == D3ItemShop.D3ProductType.RealMoneyConsumable && product.definition.id == Item.ProductCode)
        {
            if (Item.TypeItem == D3ItemShop.TypeItemShop.Life)
            {
                int lifesave = PlayerPrefs.GetInt("Life") + Item.Cant;
                D3GameData.SaveLife(lifesave);

                D3TitleCharacter.instance.UpdateText();

            }

            if (Item.TypeItem == D3ItemShop.TypeItemShop.HoverboardKeyUse)
            {
                int Hoversave = PlayerPrefs.GetInt("HoveBoard") + Item.Cant;
                D3GameData.SaveHoveBoard(Hoversave);

                D3TitleCharacter.instance.UpdateText();

            }

            if (Item.TypeItem == D3ItemShop.TypeItemShop.Character)
            {
                if (Item.Unlocked == 0)
                {
                    Item.Unlocked = 1;
                    PlayerPrefs.SetInt(Item.Name + Item.ID, 1);
                    Item.Unlocked = PlayerPrefs.GetInt(Item.Name + Item.ID);
                    D3TitleCharacter.instance.UpdateText();
                }
                if (Item.Unlocked == 1)
                {
                    if (selectPlayer != Item.ID)
                    {
                        PlayerPrefs.SetInt("SelectPlayer", Item.ID);
                        UpdateSelectCharacter();
                    }
                }
            }

            if (Item.TypeItem == D3ItemShop.TypeItemShop.HoverBoard)
            {
                if (Item.Unlocked == 0)
                {
                    Item.Unlocked = 1;
                    PlayerPrefs.SetInt(Item.Name + Item.ID, 1);
                    Item.Unlocked = PlayerPrefs.GetInt(Item.Name + Item.ID);
                    D3TitleCharacter.instance.UpdateText();
                }
                if (Item.Unlocked == 1)
                {
                    if (selectHoverBoardr != Item.ID)
                    {
                        PlayerPrefs.SetInt("selectHoverBoardr", Item.ID);
                        UpdateSelectCharacter();
                    }
                }
            }
        }

        if (Item.ProductType == D3ItemShop.D3ProductType.RealMoneyNoADS && product.definition.id == Item.ProductCode)
        {
            PlayerPrefs.SetInt("EnableUnityADS", 1);
            Debug.Log("Purchase: " + product.definition.id);
#if UNITY_ADS && ENABLE_UNITY_ADS
            if (D3ADSManager.D3AdsManager != null)
            {
                if (D3ADSManager.D3AdsManager.EnableUnityADS)
                {
                    D3ADSManager.D3AdsManager.DisableADS();
                }
            }
#endif
        }

        return PurchaseProcessingResult.Complete;
    }

    public void CheckItemPurchase()
    {
        if (controller != null)
        {
            for (int i = 0; ListItemShop.Count > i; i++)
            {
                var product = controller.products.WithID(ListItemShop[i].ProductCode);
                
                if (product != null)
                {
                    if (product.definition.id == ListItemShop[i].ProductCode)
                    {
                        if (ListItemShop[i].ProductType == D3ItemShop.D3ProductType.RealMoneyNoADS)
                        {
                            if (product.hasReceipt)
                            {
                                ListItemShop[i].Unlocked = 1;

                                PlayerPrefs.SetInt("EnableUnityADS", 1);
#if UNITY_ADS && ENABLE_UNITY_ADS
                                if (D3ADSManager.D3AdsManager != null)
                                {
                                    if (D3ADSManager.D3AdsManager.EnableUnityADS)
                                    {
                                        D3ADSManager.D3AdsManager.DisableADS();
                                    }
                                }
#endif
                            }
                            else
                            {
                                ListItemShop[i].Unlocked = 0;
                                PlayerPrefs.SetInt("EnableUnityADS",0);
#if UNITY_ADS && ENABLE_UNITY_ADS
                                if (D3ADSManager.D3AdsManager != null)
                                {
                                    if (!D3ADSManager.D3AdsManager.EnableUnityADS)
                                    {
                                        D3ADSManager.D3AdsManager.EnabledADS();
                                    }
                                }
#endif
                            }

                        }
                        if (ListItemShop[i].ProductType == D3ItemShop.D3ProductType.RealMoneyConsumable)
                        {
                            if (ListItemShop[i].TypeItem == D3ItemShop.TypeItemShop.Character || Item.TypeItem == D3ItemShop.TypeItemShop.HoverBoard)
                            {
                                
                                if (product.hasReceipt)
                                {
                                    ListItemShop[i].Unlocked = 1;
                                    PlayerPrefs.SetInt(Item.Name + Item.ID, 1);
                                }
                                else
                                {
                                    ListItemShop[i].Unlocked = 0;
                                    PlayerPrefs.SetInt(Item.Name + Item.ID, 0);
                                }
                            }
                        }
                    }
                }


            }
        }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.Log("Unity In-App Purchase Failed");
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("Unity In-App Initialized Failed" + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log("Unity In-App Initialized Failed" + error + message);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log("Unity In-App Purchase Failed");
    }

#endif
}
