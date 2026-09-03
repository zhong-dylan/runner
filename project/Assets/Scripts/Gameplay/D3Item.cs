using UnityEngine;
using System.Collections;
using UnityEditor;

public class D3Item : MonoBehaviour {
	
	public float scoreAdd; //add money if item = coin
	public int decreaseLife; //decrease life if item = obstacle 
	[HideInInspector] public int itemID; //item id
	public float speedMove; //speed move for moving obstacle
	public float duration; // duration item
	public float itemEffectValue; // effect value(if item star = speed , if item multiply = multiply number)
	public D3ItemCoinRotate itemRotate; // rotate item
	public GameObject effectHit; // effect when hit item

	

	//Lights
    public GameObject listLights;
    public AudioClip audioHorn;
    [Range(0, 100)]
    public int percentPlay = 100;
    [HideInInspector] public  bool statusPlay = true;


    public bool itemActive = false;


    bool Mesh = true;

	[HideInInspector] public bool isEditing;
	[HideInInspector] public Object targetPref;
	[HideInInspector] public string scenePath;


	public Color colorPattern;
	
	public enum TypeItem{
		Null, Coin, Obstacle, Obstacle_Roll, ItemJump, ItemSprint, ItemMagnet, ItemMultiply, Moving_Obstacle, ItemShield, ItemSpecial,Enemy
    }
	
	public TypeItem typeItem;
	
	[HideInInspector]
	public bool useAbsorb = false;
	
	public static D3Item instance;

	bool IsMoving = false;
	void Start(){
		instance = this;
        Mesh = true;

        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
				if (transform.GetChild(i).GetComponent<D3Item>())
				{
                    transform.GetChild(i).GetComponent<D3Item>().itemActive = true;
                }
            }
        }
    }

	float count = 8;

    void OnTriggerEnter(Collider col)
    {
		if(GetComponent<Collider>() && typeItem == TypeItem.Moving_Obstacle && itemActive)
		{
            if (col.tag == "Item")
            {
                if (col.GetComponent<D3Item>())
                {
                    col.GetComponent<D3Item>().HitObstacle();
                }
            }
        }
        
    }


    //Set item effect
    public void ItemGet(float AddSprintTime, float AddSpecialTime, float AddMultiplyTime, float AddMagnetTime, float AddShieldTime)
    {
		float SprintTime = AddSprintTime;
		float SpecialTime = AddSpecialTime;
		float MultiplierTime = AddMultiplyTime;
		float MagnetTime = AddMagnetTime;
		float PowerTime = AddShieldTime;

		if(D3GameAttribute.gameAttribute.deleyDetect == false && itemActive)
        {
			if(typeItem == TypeItem.Coin){
				HitCoin();
				//Play sfx when get coin
				D3SoundManager.instance.PlayingSound("GetCoin");
			}else if(typeItem == TypeItem.Obstacle){
				HitObstacle();
				//Play sfx when get hit
				D3SoundManager.instance.PlayingSound("HitOBJ");
			}else if(typeItem == TypeItem.Obstacle_Roll){
				if(D3Controller.instace.isRoll == false){
					HitObstacle();
					//Play sfx when get hit
					D3SoundManager.instance.PlayingSound("HitOBJ");
				}
			}else if(typeItem == TypeItem.ItemSprint){
				float sum = duration + SprintTime;
                D3Controller.instace.Sprint(itemEffectValue,sum);
				//Play sfx when get item
				D3SoundManager.instance.PlayingSound("GetItem");
				HideObj();
				initEffect(effectHit);
			}else if(typeItem == TypeItem.ItemMagnet){
				float sum = duration + MagnetTime;
                D3Controller.instace.Magnet(sum);
				//Play sfx when get item
				D3SoundManager.instance.PlayingSound("GetItem");
				HideObj();
				initEffect(effectHit);
			}else if(typeItem == TypeItem.ItemJump){
				D3Controller.instace.JumpDouble(duration);
				//Play sfx when get item
				D3SoundManager.instance.PlayingSound("GetItem");
				HideObj();
				initEffect(effectHit);
			}else if(typeItem == TypeItem.ItemMultiply){
				D3Controller.instace.Multiply(duration);
				float sum = itemEffectValue + MultiplierTime;
                D3GameAttribute.gameAttribute.multiplyValue = sum;
				//Play sfx when get item
				D3SoundManager.instance.PlayingSound("GetItem");
				HideObj();
				initEffect(effectHit);
			}else if(typeItem == TypeItem.Moving_Obstacle){
				HitObstacle();
				//Play sfx when get hit
				D3SoundManager.instance.PlayingSound("HitOBJ");
			} else if (typeItem == TypeItem.ItemShield)
            {
				float sum = duration + PowerTime;
                D3Controller.instace.ItemImmortality(sum);
                D3SoundManager.instance.PlayingSound("GetItem");
                HideObj();
                initEffect(effectHit);
            }
            else if (typeItem == TypeItem.ItemSpecial)
            {
				float sum = duration + SpecialTime;
                D3Controller.instace.ItemSpecial(itemEffectValue, sum);
                D3SoundManager.instance.PlayingSound("GetItem");
                HideObj();
                initEffect(effectHit);
            }
        }
	}

	public void UseMovingItem(){

        if (listLights != null && statusPlay && itemActive)
        {
            listLights.SetActive(false);
            statusPlay = false;
        }
        StartCoroutine (MovingItem ());
	}

    public void ShowMesh()
    {
        if (!Mesh)
        {
            Mesh = true;
            foreach (Transform tran in transform) tran.gameObject.SetActive(true);
        }
    }

    public void HideMesh()
    {
        if (Mesh)
        {
            Mesh = false;
            foreach (Transform tran in transform) tran.gameObject.SetActive(false);
        }
    }

    IEnumerator MovingItem(){
		while (itemActive && !D3GameAttribute.gameAttribute.pause) {
			transform.Translate(Vector3.back * speedMove * Time.deltaTime);
			yield return 0;
		}
	}

    private void FixedUpdate()
    {
        if (count <= 0 && itemActive && D3GameController.instace.Player && !statusPlay && !D3Controller.instace.isSprint && !D3Controller.instace.IsSpecial && typeItem == TypeItem.Moving_Obstacle)
        {
            float PlayerDist = Vector3.Distance(D3GameController.instace.Player.transform.position, transform.position);

            if (PlayerDist < 12)
            {
                int ran = Random.Range(0, 100);
                if (ran < percentPlay)
                {
                    if (audioHorn != null) D3SoundManager.instance.SFXSound.PlayOneShot(audioHorn);
                    if (listLights != null) listLights.SetActive(true);
                    statusPlay = true;
                }
            }
        }
    }
	private void Update()
	{
		if (count > 0 && typeItem == TypeItem.Moving_Obstacle && itemActive)
		{
			count -= Time.deltaTime;
		}

		if (itemActive)
		{
            ShowMesh();
        }
        if (!itemActive)
        {
            HideMesh();
        }


        if (D3GameAttribute.gameAttribute && itemActive)
		{
			if (D3GameAttribute.gameAttribute.pause)
			{
				if (!IsMoving)
				{
                    StopAllCoroutines();
					IsMoving = true;
                }
			}
			else {
				if (IsMoving)
				{
                    StartCoroutine(MovingItem());
					IsMoving= false;
                }
                
            }
		}
    }


    //Coin method
    private void HitCoin(){
		if(D3Controller.instace.isMultiply == false){
			D3GameAttribute.gameAttribute.coin += scoreAdd;
		}else{
			D3GameAttribute.gameAttribute.coin += (scoreAdd)* D3GameAttribute.gameAttribute.multiplyValue;
		}
        initEffectCoin(effectHit);
		HideObj();
	}
	
	//Obstacle method
	private void HitObstacle(){
        statusPlay = false;
        if (listLights != null) listLights.SetActive(false);
        count = 50;
        if (D3GameAttribute.gameAttribute.ageless == false){
			
			if(D3Controller.instace.timeSprint <= 0 && !D3Controller.instace.IsHoverboard && !D3Controller.instace.IsImmortality)
			{
				D3GameAttribute.gameAttribute.life -= decreaseLife;
				D3GameAttribute.gameAttribute.ActiveShakeCamera();
			}

			if (D3Controller.instace.IsHoverboard && !D3Controller.instace.IsImmortality)
			{
				HideObj();
				D3Controller.instace.DisebleHoverboard();
				D3GameAttribute.gameAttribute.ActiveShakeCamera();
			}
			if (D3Controller.instace.timeSprint > 0)
			{
				HideObj();
				D3GameAttribute.gameAttribute.ActiveShakeCamera();
			}
            if (D3Controller.instace.IsImmortality)
            {
                HideObj();
                D3GameAttribute.gameAttribute.ActiveShakeCamera();
            }

        }
	}
	
	//Spawn effect method
	private void initEffect(GameObject prefab){
		GameObject go = (GameObject) Instantiate(prefab, D3Controller.instace.transform.position, Quaternion.identity);
		go.transform.parent = D3Controller.instace.transform;
		go.transform.localPosition = new Vector3(go.transform.localPosition.x, go.transform.localPosition.y+0.5f, go.transform.localPosition.z);	
	}

    //Spawn effect method
    private void initEffectCoin(GameObject prefab)
    {
		if (D3Controller.instace.magnetCollider.GetComponent<D3Magnet>())
		{
			if (D3Controller.instace.magnetCollider.GetComponent<D3Magnet>())
			{
				if (D3Controller.instace.magnetCollider.GetComponent<D3Magnet>())
				{
					if (D3Controller.instace.magnetCollider.GetComponent<D3Magnet>().ObjectMagnet && D3Controller.instace.magnetCollider.activeInHierarchy)
					{
                        GameObject go = (GameObject)Instantiate(prefab, D3Controller.instace.magnetCollider.GetComponent<D3Magnet>().ObjectMagnet.transform.position, Quaternion.identity);
                        go.transform.parent = D3Controller.instace.transform;

                    }
                    else
                    {
                        initEffect(prefab);
                    }

                }
                else
                {
                    initEffect(prefab);
                }
            }
            else
            {
                initEffect(prefab);
            }
        }
		else {
            initEffect(prefab);
        }
    }

    //Magnet method
    public IEnumerator UseAbsorb(GameObject targetObj){
		bool isLoop = true;
		useAbsorb = true;
		while(isLoop){
			this.transform.position = Vector3.Lerp(this.transform.position, targetObj.transform.position, D3GameAttribute.gameAttribute.speed*2f * Time.smoothDeltaTime);
			if(Vector3.Distance(this.transform.position, targetObj.transform.position) < 0.6f){
				isLoop = false;
				D3SoundManager.instance.PlayingSound("GetCoin");
				HitCoin();
			}
			yield return 0;
		}
		Reset();
		StopCoroutine("UseAbsorb");
		yield return 0;
	}
	
	public void HideObj(){
		if(useAbsorb == false){
			this.transform.localPosition = new Vector3(-100,-100,-100);
            
        }
	}
	
	public void Reset(){
        if (gameObject != null)
            this.transform.position = new Vector3(-100, -100, -100);
        if (gameObject != null)
            itemActive = false;
        if (gameObject != null)
            useAbsorb = false;
        if (gameObject != null)
            statusPlay = false;
        if (gameObject != null)
            if (listLights != null) listLights.SetActive(false);
    }


	public float distanceZ = 1;
	public bool ShowGizmo = true;

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
		if (ShowGizmo)
		{
			Gizmos.color =  colorPattern;
			if (distanceZ > 2)
			{
				Gizmos.DrawCube(transform.position + new Vector3(0, 0.5f, distanceZ / 2), new Vector3(2f, 1, distanceZ));
			}
			else
			{
				Gizmos.DrawCube(transform.position + new Vector3(0, 0.5f, 0), new Vector3(2f, 1, distanceZ));
			}
		}
        
    }
#endif
}
