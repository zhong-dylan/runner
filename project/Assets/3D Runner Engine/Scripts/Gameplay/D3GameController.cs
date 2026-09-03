using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class D3GameController : MonoBehaviour {
	
	public D3PatternSystem patSysm; //pattern system
	public D3CameraFollow cameraFol;	//camera
	public float speedAddEveryDistance = 300;
	public float speedAdd = 0.5f;
	public float speedMax = 20;
	public int selectPlayer;
	public List<D3Controller> playerPref = new List<D3Controller>();
	public Vector3 posStart;
	public bool useShowPercent;
	private float percentCount;
	private float distanceCheck;
	[HideInInspector]
	public int countAddSpeed;
	private D3CalOnGUI calOnGUI;
	public bool UseEnemyFollower;
	public GameObject Player;
	public D3EnemyController Enemy;
    public GameObject PlayerView;
    public GameObject PlayerCam;
    public static D3GameController instace;


    public bool EnableADSOnScene = false;
    public bool UseBanner = false;

    public bool EnableRewardedADOnScene = false;

    public bool EnableInterstitialADSOnGameOver= false;
    public bool EnableInterstitialADSOnWim = false;

    public bool EnableRewardedWindow = true;

    public int RewardSelect = 0;
    
    public List<D3ADSRandomReward> ListRandomRewardedAD;


    public bool SpawnPlayerOnDead;
    void Start(){
		if(Application.isPlaying == true){
			selectPlayer = PlayerPrefs.GetInt("SelectPlayer");
			instace = this;
			calOnGUI = GetComponent<D3CalOnGUI>();
			StartCoroutine(WaitLoading());
		}
	}
	
	//Loading method
	IEnumerator WaitLoading(){
		while(patSysm.loadingComplete == false){
			if(D3GUIManager.instance !=null)
            {
				percentCount = Mathf.Lerp(percentCount, patSysm.loadingPercent, 5 * Time.deltaTime);
				D3GUIManager.instance.BGLoading.SetActive(true);
				D3GUIManager.instance.LoadingBar.value = percentCount;
				if (useShowPercent)
				{
					D3GUIManager.instance.LoadingBarText.text = percentCount.ToString("0") + "%";
				}
				else {
					D3GUIManager.instance.LoadingBarText.text = "";
				}
				
			}
			yield return 0;	
		}
		StartCoroutine(InitPlayer());
		if (D3GUIManager.instance != null)
			D3GUIManager.instance.BGLoading.SetActive(false);
	}
	
	//Spawn player method
	IEnumerator InitPlayer(){
        
        Player = Instantiate(playerPref[selectPlayer].gameObject, posStart, Quaternion.identity);
		Player.name = playerPref[selectPlayer].gameObject.name;
        cameraFol.target = Player.transform;
        InstantiatePlayerCam();
        if (UseEnemyFollower)
		{
            if (D3GUIManager.instance)
            {
                if (D3GUIManager.instance.AutoRun)
                {
                    InstantiateEnemy();
                }
            }
            
        }
		yield return 0;
		StartCoroutine(UpdatePerDistance());
	}

    public void InstantiateEnemy()
    {
        GameObject EnemySpawn = Instantiate(Enemy.gameObject, posStart + new Vector3(0, 0, 10), Quaternion.identity);
    }

    void InstantiatePlayerCam()
    {
        if (SpawnPlayerOnDead)
        {
            PlayerCam = Instantiate(playerPref[selectPlayer].gameObject, PlayerView.transform.position, Quaternion.identity);
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
                DestroyImmediate(PlayerCam.GetComponent<D3Controller>());
            }
            if (instace.PlayerCam.GetComponent<D3AnimationManager>())
            {
                DestroyImmediate(PlayerCam.GetComponent<D3AnimationManager>());
            }
            if (PlayerCam.GetComponent<CharacterController>())
            {
                DestroyImmediate(PlayerCam.GetComponent<CharacterController>());
            }
            PlayerCam.name = "Player";
            PlayerCam.transform.parent = instace.PlayerView.transform;
        }
    }

    //update distance score
    IEnumerator UpdatePerDistance(){
		while(true){
			if(D3PatternSystem.instance.loadingComplete){
				if(D3GameAttribute.gameAttribute.pause == false && D3GameAttribute.gameAttribute.isPlaying == true && D3GameAttribute.gameAttribute.life > 0){
					if(D3Controller.instace.transform.position.z > 0){
						D3GameAttribute.gameAttribute.distance += D3GameAttribute.gameAttribute.speed * Time.deltaTime;
						distanceCheck += D3GameAttribute.gameAttribute.speed * Time.deltaTime;
						if(distanceCheck >= speedAddEveryDistance){
							D3GameAttribute.gameAttribute.speed += speedAdd;
							if(D3GameAttribute.gameAttribute.speed >= speedMax){
								D3GameAttribute.gameAttribute.speed = speedMax;	
							}
							countAddSpeed++;
							distanceCheck = 0;
						}
					}
				}
			}
			yield return 0;
		}
	}
	
	//reset game
	public IEnumerator ResetGame(){
		if (D3GameAttribute.gameAttribute != null)
		{
            D3GameAttribute.gameAttribute.isPlaying = false;
        }

        distanceCheck = 0;
        countAddSpeed = 0;
        if (D3GUIManager.instance != null)
		{
            D3GameAttribute.gameAttribute.showSumGUI = true;

            D3GUIManager.instance.Gameover();
        }
		yield return 0;
	}
	
}
