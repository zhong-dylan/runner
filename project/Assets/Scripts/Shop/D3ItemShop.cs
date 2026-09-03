using UnityEngine;

[System.Serializable]
public class D3ItemShop
{
    public enum D3ProductType
    {
        VirtualMoney = 0,
        RealMoneyConsumable = 1,
        RealMoneyNoADS = 2
    }

    public enum TypeItemShop
    {
        None, Character, HoverBoard, Life, HoverboardKeyUse
    }

    public TypeItemShop TypeItem;
    public string Name;
    public string ProductCode;
    public D3ProductType ProductType;
    public int ID;
    public int Cant;
    public float Price;
    public Sprite Image;
    public int Unlocked;
    public D3Controller OnlyForCharacterTypeAddPrefab;

    public D3ItemShop(TypeItemShop TypeItem, string Name, string ProductCode, D3ProductType ProductType, int ID, int Cant, float Price, Sprite Image, int Unlocked, D3Controller OnlyForCharacterTypeAddPrefab )
    {
        this.TypeItem = TypeItem;
        this.Name = Name;
        this.ProductCode = ProductCode;
        this.ProductType = ProductType;
        this.ID = ID;
        this.Cant = Cant;
        this.Price = Price;
        this.Image = Image;
        this.Unlocked = Unlocked;
        this.OnlyForCharacterTypeAddPrefab = OnlyForCharacterTypeAddPrefab;     
    }

    public D3ItemShop()
    {
        TypeItem = TypeItemShop.None;
        Name = "";
        ProductCode = "";
        ProductType = D3ProductType.VirtualMoney;
        ID = 0;
        Cant = 0;
        Price = 0f;
        Image = null;
        Unlocked = 0;
        OnlyForCharacterTypeAddPrefab = null;
    }

}

