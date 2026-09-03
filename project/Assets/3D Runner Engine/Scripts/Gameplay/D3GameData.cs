using UnityEngine;

public class D3GameData : MonoBehaviour {

	public static void SaveCoin(int coin){

		PlayerPrefs.SetInt ("Coin", coin);
	}


	public static int LoadCoin(){

		return PlayerPrefs.GetInt ("Coin");
	}

	public static void SaveLife(int Life)
	{
		PlayerPrefs.SetInt("Life", Life);
	}

	public static int LoadLife()
	{
		return PlayerPrefs.GetInt("Life");
	}


	public static void SaveHoveBoard(int HoveBoard)
	{
		PlayerPrefs.SetInt("HoveBoard", HoveBoard);
	}

	public static int LoadHoveBoard()
	{
		return PlayerPrefs.GetInt("HoveBoard");
	}
	public static void SaveBestScore(int score)
	{
		PlayerPrefs.SetInt("BestScore", score);
	}

	public static int LoadBestScore()
	{
		return PlayerPrefs.GetInt("BestScore");
	}

}
