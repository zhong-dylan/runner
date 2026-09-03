using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class D3ADSTypeReward
{
    public enum D3TypeReward
    {
        None, Life, HoverboardKeyUse, Coin

    }
    [HideInInspector]
    public int IdButton;
    public Button showAdButton;
    public D3TypeReward TypeReward;
    public int CantReward;
    public Sprite ImgReward;
}


[System.Serializable]
public class D3ADSRandomReward
{
    public enum D3TypeRandomReward
    {
        Null, Life, HoverboardKeyUse, Coin, ItemJump, ItemMagnet, ItemMultiply, ItemShield

    }
    [HideInInspector]
    public int IdButton;
    public D3TypeRandomReward TypeReward;
    public int CantReward;
    public float duration; // duration item
    public Sprite ImgReward;
}