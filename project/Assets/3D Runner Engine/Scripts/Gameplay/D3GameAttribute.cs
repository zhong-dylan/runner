
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class D3GameAttribute : MonoBehaviour {

	[HideInInspector]
	public int TimeToShowTheWindowDeath = 0;
	[HideInInspector]
	public float starterSpeed = 5; //Speed Character
	[HideInInspector]
	public float distance;
	[HideInInspector]
	public float coin;
	[HideInInspector]
	public float Totalcoin;
	[HideInInspector]
	public int ScoreToWin;
	[HideInInspector]
	public int BestScore;
	[HideInInspector]
	public int level = 0;
	[HideInInspector]
	public bool isPlaying;
	[HideInInspector]
	public bool pause = false;
	[HideInInspector]
	public bool ageless = false;
	[HideInInspector]
	public bool deleyDetect = false;
	[HideInInspector]
	public float multiplyValue;
	[HideInInspector]
	public int lifeSave;
	[HideInInspector]
	public float speed = 5;
	[HideInInspector]
	public float life;
    [HideInInspector]
    public int HoveBoard;
	public static D3GameAttribute gameAttribute;
    [HideInInspector]
    public string SceneTitleToload = "TitleScene";
    [HideInInspector]
    public bool UseRandomScore = true;
    [HideInInspector]
    public bool UseSpecificScore = false;
    [HideInInspector]
    public int MinScoreToWin = 0;
    [HideInInspector]
    public int MaxScoreToWin = 0;
    [HideInInspector] 
	public bool showSumGUI;
    [HideInInspector]
    public bool UseStartSystem = true;

	

    void Start(){
		//Setup all attribute
		gameAttribute = this;
        LoadScoreTowin();
        speed = starterSpeed;
		distance = 0;
		coin = 0;
		life = 1;
        lifeSave = D3GameData.LoadLife();
        Totalcoin = D3GameData.LoadCoin();
		BestScore = D3GameData.LoadBestScore();
		HoveBoard = D3GameData.LoadHoveBoard();
		level = 0;
		pause = false;
		deleyDetect = false;
		ageless = false;
		isPlaying = true;
	}

	public void LoadScoreTowin()
	{
        if (UseRandomScore)
        {
            ScoreToWin = Random.Range(MinScoreToWin, MaxScoreToWin);
        }
        if (!UseRandomScore)
        {
            if (UseSpecificScore)
            {
                ScoreToWin = MinScoreToWin;
            }
            if (!UseSpecificScore)
            {
				ScoreToWin = 0 ;
            }
        }

    }
	
	public void CountDistance(float amountCount){
		distance += amountCount * Time.smoothDeltaTime;	
	}
	
	public void ActiveShakeCamera(){
		D3CameraFollow.instace.ActiveShake();	
	}

	public void Pause(bool isPause)
	{
		//pause varible
		pause = isPause;
	}

	public void Resume()
	{
		//resume
		pause = false;
	}

	public void Reset(){
		//Reset all attribute when character die
		if (!D3Controller.instace.IsSpecial)
		{
			speed = starterSpeed;
		}
		else {
			speed = D3Controller.instace.speedMove;

        }
		distance = 0;
		coin = 0;
		life = 1;
		level = 0;
		pause = false;
		deleyDetect = false;
		ageless = false;
		isPlaying = true;
		D3GUIManager.instance.Gameover();
		D3Building.instance.Reset();
		D3PatternSystem.instance.Reseted();
		D3Controller.instace.Reset();
		D3Controller.instace.timeJump = 0;
		D3Controller.instace.timeMagnet = 0;
		D3Controller.instace.timeMultiply = 0;
		D3Controller.instace.timeSprint = 0;
		D3GUIManager.instance.Reset();
        D3Item.instance.Reset();

    }
	public void Revival()
	{
        if (!D3Controller.instace.IsSpecial)
        {
            speed = starterSpeed;
            speed += D3GameController.instace.countAddSpeed * D3GameController.instace.speedAdd;
            if (speed > D3GameController.instace.speedMax)
            {
                speed = D3GameController.instace.speedAddEveryDistance;
            }
        }
        else
        {
            speed = D3Controller.instace.speedMove;

        }
        life = 1;
        pause = false;
		deleyDetect = false;
		ageless = false;
		isPlaying = true;
		if (D3Building.instance !=null)
		{
            D3Building.instance.Reset();
        }
		if (D3Item.instance !=null)
		{
            D3Item.instance.Reset();
        }
		
		D3PatternSystem.instance.Reseted();
		D3Controller.instace.Reset();
		D3Controller.instace.timeJump = 0;
		D3Controller.instace.timeMagnet = 0;
		D3Controller.instace.timeMultiply = 0;
		D3Controller.instace.timeSprint = 0;
		D3GUIManager.instance.Reset();
	}

}
