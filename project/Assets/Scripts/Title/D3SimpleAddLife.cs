using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D3SimpleAddLife : MonoBehaviour
{
    // Start is called before the first frame update

    public int CantLifeAdd = 3;
    public int CantHoverAdd = 0;
    public int Coin = 10000;
    public void AddLife()
    {
        if (D3SoundManager.instance != null)
            D3SoundManager.instance.PlayingSound("Button");

        //Example To add Life
        int lifesave = PlayerPrefs.GetInt("Life") + CantLifeAdd;
        D3GameData.SaveLife(lifesave);

        //Update Text info 
        D3TitleCharacter.instance.UpdateText();
    }

    public void ADDCoin()
    {

        if (D3SoundManager.instance != null)
            D3SoundManager.instance.PlayingSound("Button");


        //Example To add Coin
        int Coinsave = PlayerPrefs.GetInt("Coin") + Coin;
        D3GameData.SaveCoin(Coinsave);

        //Update Text info 
        D3TitleCharacter.instance.UpdateText();

    }


    public void AddHoverBoard()
    {
        if (D3SoundManager.instance != null)
            D3SoundManager.instance.PlayingSound("Button");


        //Example To add HoverBoard
        int Hoversave = PlayerPrefs.GetInt("HoveBoard") + CantHoverAdd;
        D3GameData.SaveHoveBoard(Hoversave);

        //Update Text info 
        D3TitleCharacter.instance.UpdateText();
    }
}
