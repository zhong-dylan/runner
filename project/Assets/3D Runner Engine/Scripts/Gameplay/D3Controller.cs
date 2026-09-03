using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(D3AnimationManager))]
public class D3Controller : MonoBehaviour {


    private Vector3 oldPointBefore = Vector3.zero;
    public Vector3 GetOldPoint()
    {
        return oldPointBefore;
    }

    public enum DirectionInput {
		Null, Left, Right, Up, Down
	}

	public enum Position {
		Middle, Left, Right
	}

	[HideInInspector] public int TimeToShowTheWindowDeath = 0;

	public string Name = "Player";
	public D3CoinRotation coinRotate;
	public GameObject magnetCollider;
	public float speedMove = 5;
	public float gravity = 20;
	public float jumpValue = 8;
	public bool useImmortalityOnInit = true;
    public float ImmortalityTimeWhenRevived = 2;
	public GameObject ImmortalityEffect;
	public GameObject HoverBike;
    public GameObject SpecialItem;
    public GameObject HoverBoardPositionList;
	public List<GameObject> HoverBoardList;

	//stats
	public float AddSprintTime =  0;
    public float CurrentAddSprintTime = 0;
    public float AddSprintTimeMax = 10;
    public int InitialPriceSprintTime;
    public int CurrentPriceSprintTime;

    public float AddSpecialTime = 0;
    public float CurrentAddSpecialTime = 0;
    public float AddSpecialTimeMax = 10;
    public int InitialPriceSpecialTime;
    public int CurrentPriceSpecialTime;

    public float AddMultiplyTime = 0;
    public float CurrentAddMultiplyTime = 0;
    public float AddMultiplyTimeMax = 10;
    public int InitialPriceMultiplyTime;
    public int CurrentPriceMultiplyTime;

    public float AddMagnetTime = 0;
    public float CurrentAddMagnetTime = 0;
    public float AddMagnetTimeMax = 10;
    public int InitialPriceMagnetTime;
    public int CurrentPriceMagnetTime;

    public float AddShieldTime = 0;
    public float CurrentAddShieldTime = 0;
    public float AddShieldTimeMax = 10;
    public int InitialPriceShieldTime;
    public int CurrentPriceShieldTime;


    [HideInInspector]
	public int selectHoverBoardr;
	[HideInInspector]
	public bool IsHoverboard;

	public ParticleSystem EffectHover;


	[HideInInspector]
	public bool isRoll;
	[HideInInspector]
	public bool isDoubleJump;
	[HideInInspector]
	public bool isMultiply;
	[HideInInspector]
	public bool isSprint;
	[HideInInspector]
	public bool IsSpecial = false;
	[HideInInspector]
	public CharacterController characterController;

	[HideInInspector]
	public float timeSprint;
	[HideInInspector]
	public float timeMagnet;
	[HideInInspector]
	public float _ImmortalityTime;
	[HideInInspector]
	public bool IsImmortality = false;
	[HideInInspector]
	public bool IsImmortalityAnimation = false;
	[HideInInspector]
	public float timeMultiply;
	[HideInInspector]
	public float timeJump;
	[HideInInspector]
	public bool EnableControl = true;
	[HideInInspector]
	public float timeSpecial;

	public bool activeInput;
	private bool jumpSecond;

	private Vector3 moveDir;
	private Vector2 currentPos;

	private Position positionStand;
	public DirectionInput directInput;
	private D3AnimationManager animationManager;

	private Vector3 direction;
	private Vector3 currectPosCharacter;

	public static D3Controller instace;

	bool AniHoverBike = false;

	float gravitysave;


	bool ControlPosY = false;
	bool ForRocket = false;
	bool AnimationForRocket = false;

    bool ForSpecial = false;
    bool AnimationForSpecial = false;
    bool AnimationSpecial = false;
	[HideInInspector]
	public bool ControlSpecial = false;
    [HideInInspector]
    public bool ColActivate = false;
    [HideInInspector]
    public bool IsArrest = true;
    int FirstLaunchData = 0;

	bool StopPlayer = false;

    //Check item collider

    public void LoadData()
	{
        CurrentAddSprintTime = 0;
        CurrentAddSpecialTime = 0;
        CurrentAddMultiplyTime = 0;
        CurrentAddMagnetTime = 0;
        CurrentAddShieldTime = 0;

        CurrentPriceSprintTime = 0;
        CurrentPriceSpecialTime = 0;
        CurrentPriceMultiplyTime = 0;
        CurrentPriceMagnetTime = 0;
        CurrentPriceShieldTime = 0;
        FirstLaunchData = PlayerPrefs.GetInt(gameObject.name + "FirstLaunchData");
        if (FirstLaunchData == 0)
        {
			CurrentAddSprintTime = AddSprintTime;
            CurrentAddSpecialTime = AddSpecialTime;
            CurrentAddMultiplyTime = AddMultiplyTime;
            CurrentAddMagnetTime = AddMagnetTime;
            CurrentAddShieldTime = AddShieldTime;

			CurrentPriceSprintTime = InitialPriceSprintTime;
            CurrentPriceSpecialTime = InitialPriceSpecialTime;
            CurrentPriceMultiplyTime = InitialPriceMultiplyTime;
            CurrentPriceMagnetTime = InitialPriceMagnetTime;
            CurrentPriceShieldTime = InitialPriceShieldTime;

            PlayerPrefs.SetFloat(gameObject.name + "SprintTime", CurrentAddSprintTime);
            PlayerPrefs.SetFloat(gameObject.name + "SpecialTime", CurrentAddSpecialTime);
            PlayerPrefs.SetFloat(gameObject.name + "MultiplyTime", CurrentAddMultiplyTime);
            PlayerPrefs.SetFloat(gameObject.name + "MagnetTime", CurrentAddMagnetTime);
            PlayerPrefs.SetFloat(gameObject.name + "ShieldTime", CurrentAddShieldTime);

            PlayerPrefs.SetInt(gameObject.name + "CurrentPriceSprintTime", CurrentPriceSprintTime);
            PlayerPrefs.SetInt(gameObject.name + "CurrentPriceSpecialTime", CurrentPriceSpecialTime);
            PlayerPrefs.SetInt(gameObject.name + "CurrentPriceMultiplyTime", CurrentPriceMultiplyTime);
            PlayerPrefs.SetInt(gameObject.name + "CurrentPriceMagnetTime", CurrentPriceMagnetTime);
            PlayerPrefs.SetInt(gameObject.name + "CurrentPriceShieldTime", CurrentPriceShieldTime);
#if UNITY_EDITOR
			PrefabUtility.RecordPrefabInstancePropertyModifications(gameObject);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif

			PlayerPrefs.SetInt(gameObject.name + "FirstLaunchData", 1);
        }
        if (FirstLaunchData == 1)
        {
            CurrentAddSprintTime = PlayerPrefs.GetFloat(gameObject.name + "SprintTime");
            CurrentAddSpecialTime = PlayerPrefs.GetFloat(gameObject.name + "SpecialTime"); 
            CurrentAddMultiplyTime = PlayerPrefs.GetFloat(gameObject.name + "MultiplyTime"); 
            CurrentAddMagnetTime = PlayerPrefs.GetFloat(gameObject.name + "MagnetTime"); 
            CurrentAddShieldTime = PlayerPrefs.GetFloat(gameObject.name + "ShieldTime"); 

            CurrentPriceSprintTime = PlayerPrefs.GetInt(gameObject.name + "CurrentPriceSprintTime"); 
            CurrentPriceSpecialTime = PlayerPrefs.GetInt(gameObject.name + "CurrentPriceSpecialTime");
            CurrentPriceMultiplyTime = PlayerPrefs.GetInt(gameObject.name + "CurrentPriceMultiplyTime");
            CurrentPriceMagnetTime = PlayerPrefs.GetInt(gameObject.name + "CurrentPriceMagnetTime");
            CurrentPriceShieldTime = PlayerPrefs.GetInt(gameObject.name + "CurrentPriceShieldTime");

        }

	}

    public void SaveData()
    {
        PlayerPrefs.SetInt(gameObject.name + "CurrentPriceSprintTime", CurrentPriceSprintTime);
        PlayerPrefs.SetInt(gameObject.name + "CurrentPriceSpecialTime", CurrentPriceSpecialTime);
        PlayerPrefs.SetInt(gameObject.name + "CurrentPriceMultiplyTime", CurrentPriceMultiplyTime);
        PlayerPrefs.SetInt(gameObject.name + "CurrentPriceMagnetTime", CurrentPriceMagnetTime);
        PlayerPrefs.SetInt(gameObject.name + "CurrentPriceShieldTime", CurrentPriceShieldTime);

        PlayerPrefs.SetFloat(gameObject.name + "SprintTime", CurrentAddSprintTime);
        PlayerPrefs.SetFloat(gameObject.name + "SpecialTime", CurrentAddSpecialTime);
        PlayerPrefs.SetFloat(gameObject.name + "MultiplyTime", CurrentAddMultiplyTime);
        PlayerPrefs.SetFloat(gameObject.name + "MagnetTime", CurrentAddMagnetTime);
        PlayerPrefs.SetFloat(gameObject.name + "ShieldTime", CurrentAddShieldTime);

    }

    public void AddSprintTimeVoid(int multiply)
	{
		LoadData();

        float sum = CurrentAddSprintTime += 2;
		CurrentPriceSprintTime = CurrentPriceSprintTime * multiply;

        if (sum < AddSprintTimeMax)
		{
            CurrentAddSprintTime += 2;
		}
		else {
            CurrentAddSprintTime = AddSprintTimeMax;
        }
        SaveData();

    }
    public void AddSpecialTimeVoid(int multiply)
    {
        LoadData();
        float sum = CurrentAddSpecialTime += 2;
        CurrentPriceSpecialTime = CurrentPriceSpecialTime * multiply;
        if (sum < AddSpecialTimeMax)
        {
            CurrentAddSpecialTime += 2;
        }
        else
        {
            CurrentAddSpecialTime = AddSpecialTimeMax;
        }
        SaveData();
    }
    public void AddMultiplyTimeVoid(int multiply)
    {
        LoadData();
        float sum = CurrentAddMultiplyTime += 2;
        CurrentPriceMultiplyTime = CurrentPriceMultiplyTime * multiply;
        if (sum < AddMultiplyTimeMax)
        {
            CurrentAddMultiplyTime += 2;
        }
        else
        {
            CurrentAddMultiplyTime = AddMultiplyTimeMax;
        }
        SaveData();
    }
    public void AddMagnetTimeVoid(int multiply)
    {
        LoadData();
        float sum = CurrentAddMagnetTime += 2;
        CurrentPriceMagnetTime = CurrentPriceMagnetTime * multiply;
		Debug.Log(CurrentPriceMagnetTime);
        if (sum < AddMagnetTimeMax)
        {
            CurrentAddMagnetTime += 2;
        }
        else
        {
            CurrentAddMagnetTime = AddMagnetTimeMax;
        }
        SaveData();
    }
    public void AddShieldTimeVoid(int multiply)
    {
        LoadData();
        float sum = CurrentAddShieldTime += 2;
        CurrentPriceShieldTime = CurrentPriceShieldTime * multiply;
        if (sum < AddShieldTimeMax)
        {
            CurrentAddShieldTime += 2;
        }
        else
        {
            CurrentAddShieldTime = AddShieldTimeMax;
        }
        SaveData();
    }

    void OnTriggerEnter(Collider col) {
		if (ColActivate)
		{
			if (col.tag == "Item")
			{
                if (col.GetComponent<D3Item>())
                {

                    if (col.GetComponent<D3Item>().useAbsorb)
                    {
                        col.GetComponent<D3Item>().useAbsorb = false;
                        col.GetComponent<D3Item>().StopAllCoroutines();
                    }


                    col.GetComponent<D3Item>().ItemGet(CurrentAddSprintTime, CurrentAddSpecialTime, CurrentAddMultiplyTime, CurrentAddMagnetTime, CurrentAddShieldTime);

                    if (col.GetComponent<D3Item>().typeItem == D3Item.TypeItem.Obstacle || col.GetComponent<D3Item>().typeItem == D3Item.TypeItem.Moving_Obstacle)
                    {
                        if (D3EnemyController.instance)
                        {

                            D3EnemyController.instance.ResetCount();

                        }
                    }
                }
				if (col.GetComponent<D3EnemyController>())
				{
					if (col.GetComponent<D3EnemyController>().mesh)
					{
                        ColActivate = false;
                        InitArrest();
                    }
				}
            }
        }
	}

    void Start() {
        //Set state character
        instace = this;

        LoadData();

        ColActivate = true;
        if (HoverBike != null)
        {
            HoverBike.SetActive(false);
        }
        if (SpecialItem != null)
        {
            SpecialItem.SetActive(false);
        }

        for (int i = 0; HoverBoardList.Count > i; i++)
        {
            if (HoverBoardList[i] != null)
            {
                HoverBoardList[i].SetActive(false);
            }

        }
        if (magnetCollider.GetComponent<D3Magnet>() != null)
        {
            if (magnetCollider.GetComponent<D3Magnet>().ObjectMagnet != null)
            {
                magnetCollider.GetComponent<D3Magnet>().ObjectMagnet.SetActive(false);
            }

        }

        if (ImmortalityEffect != null)
        {
            ImmortalityEffect.SetActive(false);
        }

        selectHoverBoardr = PlayerPrefs.GetInt("selectHoverBoardr");
        characterController = this.GetComponent<CharacterController>();
        animationManager = this.GetComponent<D3AnimationManager>();
        speedMove = D3GameAttribute.gameAttribute.speed;
        TimeToShowTheWindowDeath = D3GameAttribute.gameAttribute.TimeToShowTheWindowDeath;
        jumpSecond = false;
        if (magnetCollider.GetComponent<D3Magnet>() != null)
        {
            magnetCollider.GetComponent<D3Magnet>().ObjectMagnet.gameObject.SetActive(false);

            magnetCollider.SetActive(false);
        }
        Invoke("WaitStart", 0.2f);
    }

	//Reset state,variable when character die
	public void Reset() {
        ColActivate = false;
		if (D3PanelBestScore.instance)
		{
			D3PanelBestScore.instance.ResetPanel();

        }
        animationManager.m_Animator.SetFloat("IsMagnet", 0);
        animationManager.m_Animator.SetBool("isSprint", false);
        animationManager.m_Animator.SetBool("ForRocket", false);
        animationManager.m_Animator.SetBool("IsCatch", false);
        if (magnetCollider.GetComponent<D3Magnet>() != null)
        {
            if (magnetCollider.GetComponent<D3Magnet>().ObjectMagnet != null)
            {
                magnetCollider.GetComponent<D3Magnet>().ObjectMagnet.SetActive(false);
            }
        }
        if (gravity <= 0 && gravitysave > 0)
        {
            gravity = gravitysave;
        }
        positionStand = Position.Middle;
        EnableControl = true;
        timeJump = 0;
        IsArrest = false;
        jumpSecond = false;
        isRoll = false;
        isDoubleJump = false;
        isMultiply = false;
        if (isSprint)
        {
            timeSprint = 0;
            AniHoverBike = false;
            if (HoverBike != null)
            {
                if (EffectHover != null)
                {
                    EffectHover.Play();
                }
                HoverBike.SetActive(false);

            }
            isSprint = false;
            ForRocket = false;
        }
        if (ControlSpecial)
        {
            timeSpecial = 0;
            ControlSpecial = false;
        }

        magnetCollider.SetActive(false);

        if (IsHoverboard)
        {
            animationManager.m_Animator.SetBool("IsHoverBoard", true);
        }
        else
        {
            animationManager.isRum();
        }

        if (D3GameController.instace != null)
        {
            transform.position = D3GameController.instace.posStart;
        }
		LoadData();
        StopAllCoroutines();
        StartCoroutine(UpdateAction());
    }

	void WaitStart() {
		StartCoroutine(UpdateAction());
	}

    private void FixedUpdate()
    {
        if (transform.position.y < 0f)
        {
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        }
    }
    private void Update()
	{
        if (StopPlayer)
        {
            if (EnableControl)
            {
                if (!D3GUIManager.instance.AutoRun)
                {
                    if (D3GUIManager.instance.KeyInputContinueRun)
                    {
                        if (Input.GetKey(D3GUIManager.instance.KeyCodeContinueRun))
                        {
							InitRunFirstTime();
                        }
                    }
                    if (D3GUIManager.instance.TouchInputContinueRun)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
							InitRunFirstTime();
                        }
                    }
                }
            }
        }
        if (isSprint)
        {
            if (ControlPosY)
            {
                if (transform.position.y != D3PatternSystem.instance.PositionYItemRocket)
                {
                    transform.position = new Vector3(transform.position.x, D3PatternSystem.instance.PositionYItemRocket, transform.position.z);
                }
                ControlPosY = false;
            }

            if (!ControlPosY && transform.position.y != D3PatternSystem.instance.PositionYItemRocket)
            {
                transform.position = new Vector3(transform.position.x, D3PatternSystem.instance.PositionYItemRocket, transform.position.z);
            }

            if (timeSprint > 0)
            {
                if (!AniHoverBike)
                {
                    AniHoverBike = true;
                    if (!IsSpecial)
                    {
                        animationManager.m_Animator.SetBool("isSprint", true);
                        if (HoverBike != null)
                        {
                            if (EffectHover != null)
                            {
                                EffectHover.Play();
                            }
                            HoverBike.SetActive(true);
                        }
                        if (D3EnemyController.instance)
                        {
                            D3EnemyController.instance.statusPlay = false;

                        }
                    }

                }

            }
            if (timeSprint <= 0)
            {
                AniHoverBike = false;
                if (!IsSpecial)
                {
                    animationManager.m_Animator.SetBool("isSprint", false);
                    if (HoverBike != null)
                    {
                        if (EffectHover != null)
                        {
                            EffectHover.Play();
                        }
                        HoverBike.SetActive(false);

                    }
                }
                if (D3EnemyController.instance)
                {
                    D3EnemyController.instance.statusPlay = true;

                }
                isSprint = false;
            }
        }

        if (ForRocket)
        {
            if (!AnimationForRocket)
            {
                if (IsSpecial)
                {
                    animationManager.m_Animator.SetBool("GetItemSpecial", true);
                    animationManager.m_Animator.PlayInFixedTime("GetItemSpecial");
                }
                else
                {
                    animationManager.m_Animator.SetBool("ForRocket", true);
                    animationManager.m_Animator.PlayInFixedTime("TransitionForRocket");

                }
                AnimationForRocket = true;

            }
        }
        else
        {
            if (IsSpecial)
            {
                animationManager.m_Animator.SetBool("GetItemSpecial", false);
            }
            else
            {
                animationManager.m_Animator.SetBool("ForRocket", false);
            }
            AnimationForRocket = false;
        }


        if (IsSpecial)
        {
            if (timeSpecial > 0)
            {
                if (!AnimationSpecial)
                {
                    AnimationSpecial = true;
                    animationManager.m_Animator.SetBool("IsSpecial", true);

                    if (SpecialItem != null)
                    {
                        SpecialItem.SetActive(true);
                    }
                }
                timeSpecial -= Time.deltaTime;
                if (D3EnemyController.instance)
                {
                    D3EnemyController.instance.statusPlay = false;

                }

            }
            if (timeSpecial <= 0)
            {
                IsSpecial = false;
                D3GUIManager.instance.Revival();
                AnimationSpecial = false;
                animationManager.m_Animator.SetBool("IsSpecial", false);
                if (SpecialItem != null)
                {
                    SpecialItem.SetActive(false);

                }
                if (D3EnemyController.instance)
                {
                    if (!D3EnemyController.instance.statusPlay)
                    {
                        D3EnemyController.instance.UpdateResetTime();
                    }
                }

            }
        }

        if (ForSpecial)
        {
            if (!AnimationForSpecial)
            {
                animationManager.m_Animator.SetBool("GetItemSpecial", true);
                animationManager.m_Animator.PlayInFixedTime("GetItemSpecial");
                AnimationForSpecial = true;
            }
        }
        else
        {
            animationManager.m_Animator.SetBool("GetItemSpecial", false);
            AnimationForSpecial = false;
        }

        if (IsImmortality)
        {
            if (_ImmortalityTime > 0)
            {
                _ImmortalityTime -= Time.deltaTime;

            }

            if (_ImmortalityTime <= 0)
            {
                IsImmortality = false;
                if (IsImmortalityAnimation)
                {
                    if (ImmortalityEffect != null)
                    {
                        ImmortalityEffect.SetActive(false);
                    }
                    IsImmortalityAnimation = false;
                }
            }
        }
    }

	void InitRunFirstTime()
	{
        D3GUIManager.instance.AutoRun = true;
        moveDir += this.transform.TransformDirection(Vector3.forward * speedMove);
        moveDir.y -= gravity * Time.deltaTime;
        CheckSideCollision();
        characterController.Move((moveDir + direction) * Time.deltaTime);
        if (D3GameAttribute.gameAttribute)
        {
            D3GameAttribute.gameAttribute.Pause(false);
        }
        animationManager.PauseOff();
		if (D3GameController.instace)
		{
            if (D3GameController.instace.UseEnemyFollower)
            {
				D3GameController.instace.InstantiateEnemy();
            }
        }
        StopPlayer = false;
        if (D3GUIManager.instance.ImgInfoToRun != null)
        {
            D3GUIManager.instance.ImgInfoToRun.SetActive(false);
        }

    }

	//Update Loop
	IEnumerator UpdateAction() {

		while (D3GameAttribute.gameAttribute.life > 0) {

			if (D3GameAttribute.gameAttribute.pause == false && D3GameAttribute.gameAttribute.isPlaying && D3PatternSystem.instance.loadingComplete && D3GUIManager.instance) {

				if (D3GUIManager.instance.KeyInput)
				{
					KeyInput();
				}

				if (D3GUIManager.instance.TouchInput) {
					DirectionAngleInput();
				}
				CheckLane();
				MoveForward();
			}
			yield return 0;
		}

		StartCoroutine(MoveBack());

		Dead();


		DisebleHoverboard();
		yield return new WaitForSeconds(TimeToShowTheWindowDeath);

		D3GameController.instace.StartCoroutine(D3GameController.instace.ResetGame());

	}

	void Dead() {
        animationManager.Dead();
	}

	IEnumerator MoveBack() {
		float z = transform.position.z - 0.5f;
		bool complete = false;
		while (complete == false) {
			transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, z), 2 * Time.deltaTime);
			if ((transform.position.z - z) < 0.05f) {
				complete = true;
			}
			yield return 0;
		}

		yield return 0;
	}

	public void PauseOn()
	{
        animationManager.PauseOn();
	}

	public void PauseOff()
	{
        animationManager.PauseOff();
	}

	private void MoveForward() {
		speedMove = D3GameAttribute.gameAttribute.speed;

		if (characterController.isGrounded) {
			moveDir = Vector3.zero;
			if (directInput == DirectionInput.Up) {
				Jump();
				if (isDoubleJump) {
					jumpSecond = true;
				}
			}
		} else {
			if (directInput == DirectionInput.Down) {
				QuickGround();
			}
			if (directInput == DirectionInput.Up) {
				if (jumpSecond) {
					JumpSeccond();
					jumpSecond = false;
				}
			}

		}
		moveDir.z = 0;
		if (D3GUIManager.instance)
		{
			if (!D3GUIManager.instance.AutoRun)
			{
                if (D3GameAttribute.gameAttribute)
                {
                    D3GameAttribute.gameAttribute.Pause(true);
                }
                animationManager.PauseOn();
                StopPlayer = true;
				if (D3GUIManager.instance.ImgInfoToRun != null)
				{
                    D3GUIManager.instance.ImgInfoToRun.SetActive(true);
                }
            }
			else {
                moveDir += this.transform.TransformDirection(Vector3.forward * speedMove);
            }
		}
        moveDir.y -= gravity * Time.deltaTime;
        CheckSideCollision();
		characterController.Move((moveDir + direction) * Time.deltaTime);
	}

	private bool checkSideCollision;
	private float countDeleyInput;

	private void CheckSideCollision() {
		if (positionStand == Position.Right) {
			if ((int)characterController.collisionFlags == 5 || characterController.collisionFlags == CollisionFlags.Sides) {
				if (transform.position.x < 1.75f && checkSideCollision == false) {
					D3CameraFollow.instace.ActiveShake();
					positionStand = Position.Middle;
					checkSideCollision = true;
				}
			}
		}

		if (positionStand == Position.Left) {
			if ((int)characterController.collisionFlags == 5 || characterController.collisionFlags == CollisionFlags.Sides) {
				if (transform.position.x > -1.75f && checkSideCollision == false) {
					D3CameraFollow.instace.ActiveShake();
					positionStand = Position.Middle;
					checkSideCollision = true;
				}
			}
		}

		if (positionStand == Position.Middle) {
			if ((int)characterController.collisionFlags == 5 || characterController.collisionFlags == CollisionFlags.Sides) {
				if (transform.position.x < -0.05f && checkSideCollision == false) {
					D3CameraFollow.instace.ActiveShake();
					positionStand = Position.Left;

					checkSideCollision = true;
				} else if (transform.position.x > 0.05f && checkSideCollision == false) {
					D3CameraFollow.instace.ActiveShake();
					positionStand = Position.Right;
					checkSideCollision = true;
				}
			}
		}

		if (checkSideCollision == true) {
			countDeleyInput += Time.deltaTime;
			if (countDeleyInput >= 1f) {
				checkSideCollision = false;
				countDeleyInput = 0;
			}
		}
	}

	private void QuickGround() {
		if (!isSprint)
		{
			moveDir.y -= jumpValue * 3;
		}
	}

	public void Revive()
	{
        Reset();
        ActivateImmortality();
	}
	//Jump State
	private void Jump() {
		if (!isSprint)
		{

			D3SoundManager.instance.PlayingSound("Jump");
			if (IsHoverboard && !IsSpecial)
			{
				animationManager.HoverBoarJump();
			}
            if (IsSpecial)
            {
                animationManager.IsSpecialJump();
            }
            if (!IsHoverboard && !IsSpecial)
            {
				animationManager.Jump();
			}
			moveDir.y += jumpValue;
        }
	}

	private void JumpSeccond() {
		if (!isSprint)
		{
			if (IsHoverboard)
			{
				animationManager.HoverBoarJump();
			}
            if (IsSpecial)
            {
                animationManager.IsSpecialJump();
            }
            if (!IsHoverboard && !IsSpecial)
            {
				animationManager.JumpSecond();
			}
			moveDir.y += jumpValue * 1.6f;
		}
	}

	public void ActivateHoverboard()
	{
		if (!IsHoverboard)
		{
			for (int i = 0; HoverBoardList.Count > i; i++)
			{
				if (HoverBoardList[i] != null && i == selectHoverBoardr)
				{
					if (EffectHover != null)
					{
						EffectHover.Play();
					}
					HoverBoardList[i].SetActive(true);
					IsHoverboard = true;
					animationManager.m_Animator.SetBool("IsHoverBoard", true);
				}

			}
		}

	}
	public void DisebleHoverboard()
	{
		IsHoverboard = false;
		for (int i = 0; HoverBoardList.Count > i; i++)
		{
			if (HoverBoardList[i] != null && i == selectHoverBoardr)
			{
				if (EffectHover != null)
				{
					EffectHover.Play();
				}
				HoverBoardList[i].SetActive(false);
			}

		}
		animationManager.m_Animator.SetBool("IsHoverBoard", false);
		animationManager.Run();
	}

	private void CheckLane() {
		if (positionStand == Position.Middle) {
			if (directInput == DirectionInput.Right) {
                if (characterController.isGrounded && timeSprint <= 0 && timeSpecial <= 0) {

					if (IsHoverboard)
					{
						animationManager.HoverBoarRight();
					}
					else
					{
						animationManager.TurnRight();
					}
				}
				if (timeSprint > 0 && !IsSpecial)
				{
					animationManager.HoverRight();
				}
                if (timeSpecial > 0)
                {
                    animationManager.IsSpecialRight();
                }
                positionStand = Position.Right;
				//Play sfx when step
				D3SoundManager.instance.PlayingSound("Step");
			} else if (directInput == DirectionInput.Left) {
                if (characterController.isGrounded && timeSprint <= 0 && timeSpecial <= 0)
				{
					if (IsHoverboard)
					{
						animationManager.HoverBoarLeft();
					}
					else
					{
						animationManager.TurnLeft();
					}

				}
				if (timeSprint > 0 && !IsSpecial)
				{
					animationManager.HoverLeft();
				}
                if (timeSpecial > 0)
                {
                    animationManager.IsSpecialLeft();
                }
                positionStand = Position.Left;
				//Play sfx when step
				D3SoundManager.instance.PlayingSound("Step");
			}

			//transform.position = Vector3.Lerp(transform.position, new Vector3(0,transform.position.y,transform.position.z), 6 * Time.deltaTime);
			if (transform.position.x > 0.1f) {
				direction = Vector3.Lerp(direction, Vector3.left * 6, Time.deltaTime * 500);
			} else if (transform.position.x < -0.1f) {
				direction = Vector3.Lerp(direction, Vector3.right * 6, Time.deltaTime * 500);
			} else {
				direction = Vector3.zero;
				checkSideCollision = false;
				transform.position = Vector3.Lerp(transform.position, new Vector3(0, transform.position.y, transform.position.z), 6 * Time.deltaTime);
			}
		} else if (positionStand == Position.Left) {
			if (directInput == DirectionInput.Right) {
                if (characterController.isGrounded && timeSprint <= 0 && timeSpecial <= 0)
				{
					if (IsHoverboard)
					{
						animationManager.HoverBoarRight();
					}
					else
					{
						animationManager.TurnRight();
					}

				}
				if (timeSprint > 0 && !IsSpecial)
				{

					animationManager.HoverRight();
				}
                if (timeSpecial > 0)
                {

                    animationManager.IsSpecialRight();
                }
                positionStand = Position.Middle;
				//Play sfx when step
				D3SoundManager.instance.PlayingSound("Step");
			}
			//transform.position = Vector3.Lerp(transform.position, new Vector3(-1.8f,transform.position.y,transform.position.z), 6 * Time.deltaTime);
			if (transform.position.x > -1.8f) {
				direction = Vector3.Lerp(direction, Vector3.left * 6, Time.deltaTime * 500);
			} else {
				direction = Vector3.zero;
				checkSideCollision = false;
				transform.position = Vector3.Lerp(transform.position, new Vector3(-1.8f, transform.position.y, transform.position.z), 6 * Time.deltaTime);
			}
		} else if (positionStand == Position.Right) {
			if (directInput == DirectionInput.Left) {
                if (characterController.isGrounded && timeSprint <= 0 && timeSpecial <= 0)
				{
					if (IsHoverboard)
					{
						animationManager.HoverBoarLeft();
					}
					else
					{
						animationManager.TurnLeft();
					}

				}
				if (timeSprint > 0 && !IsSpecial)
				{
					animationManager.HoverLeft();
				}
                if (timeSpecial > 0)
                {
                    animationManager.IsSpecialLeft();
                }
                positionStand = Position.Middle;
				//Play sfx when step
				D3SoundManager.instance.PlayingSound("Step");
			}
			//transform.position = Vector3.Lerp(transform.position, new Vector3(1.8f,transform.position.y,transform.position.z), 6 * Time.deltaTime);
			if (transform.position.x < 1.8f) {
				direction = Vector3.Lerp(direction, Vector3.right * 6, Time.deltaTime * 500);
			} else {
				direction = Vector3.zero;
				checkSideCollision = false;
				transform.position = Vector3.Lerp(transform.position, new Vector3(1.8f, transform.position.y, transform.position.z), 6 * Time.deltaTime);
			}
		}
        else
        {
            direction = Vector3.zero;
            checkSideCollision = false;
            transform.position = Vector3.Lerp(transform.position, new Vector3(1.8f, transform.position.y, transform.position.z), 6 * Time.deltaTime);
        }

        if (directInput == DirectionInput.Down && timeSprint <= 0 && timeSpecial <=0)
		{
            if (IsHoverboard)
			{
				animationManager.HoverBoarRoll();
			}
			else
			{
				animationManager.Roll();
			}
			//Play sfx when roll
			D3SoundManager.instance.PlayingSound("Roll");
		}
        if (directInput == DirectionInput.Down && timeSprint <= 0 && timeSpecial > 0)
        {
            animationManager.IsSpecialRoll();
            
            //Play sfx when roll
            D3SoundManager.instance.PlayingSound("Roll");
        }

    }

	//Key input method

	private void KeyInput()
	{
		if (EnableControl)
		{
			if (Input.anyKeyDown)
			{
				activeInput = true;
			}

			if (activeInput && checkSideCollision == false)
			{
				if (Input.GetKey(D3GUIManager.instance.KeyCodeLeft))
				{
					directInput = DirectionInput.Left;
					activeInput = false;
				}
				else

				if (Input.GetKey(D3GUIManager.instance.KeyCodeRight))
				{
					directInput = DirectionInput.Right;
					activeInput = false;
				}
				else

				if (Input.GetKey(D3GUIManager.instance.KeyCodeUP))
				{
					directInput = DirectionInput.Up;
					activeInput = false;
				}
				else

				if (Input.GetKeyDown(D3GUIManager.instance.KeyCodeDown))
				{
					directInput = DirectionInput.Down;
					activeInput = false;
				}
			}
			else
			{
				directInput = DirectionInput.Null;
			}
		}

	}

	private void DirectionAngleInput() {
		if (EnableControl)
		{
			if (Input.GetMouseButtonDown(0))
			{
				currentPos = Input.mousePosition;
				activeInput = true;
			}

			if (Input.GetMouseButton(0) && checkSideCollision == false)
			{
				if (activeInput)
				{
					float ang = D3CalculateAngle.GetAngle(currentPos, Input.mousePosition);
					if ((Input.mousePosition.x - currentPos.x) > 20)
					{
						if (ang < 45 && ang > -45)
						{
							directInput = DirectionInput.Right;
							activeInput = false;
						}
						else if (ang >= 45)
						{
							directInput = DirectionInput.Up;
							activeInput = false;
						}
						else if (ang <= -45)
						{
							directInput = DirectionInput.Down;
							activeInput = false;
						}
					}
					else if ((Input.mousePosition.x - currentPos.x) < -20)
					{
						if (ang < 45 && ang > -45)
						{
							directInput = DirectionInput.Left;
							activeInput = false;
						}
						else if (ang >= 45)
						{
							directInput = DirectionInput.Down;
							activeInput = false;
						}
						else if (ang <= -45)
						{
							directInput = DirectionInput.Up;
							activeInput = false;
						}
					}
					else if ((Input.mousePosition.y - currentPos.y) > 20)
					{
						if ((Input.mousePosition.x - currentPos.x) > 0)
						{
							if (ang > 45 && ang <= 90)
							{
								directInput = DirectionInput.Up;
								activeInput = false;
							}
							else if (ang <= 45 && ang >= -45)
							{
								directInput = DirectionInput.Right;
								activeInput = false;
							}
						}
						else if ((Input.mousePosition.x - currentPos.x) < 0)
						{
							if (ang < -45 && ang >= -89)
							{
								directInput = DirectionInput.Up;
								activeInput = false;
							}
							else if (ang >= -45)
							{
								directInput = DirectionInput.Left;
								activeInput = false;
							}
						}
					}
					else if ((Input.mousePosition.y - currentPos.y) < -20)
					{
						if ((Input.mousePosition.x - currentPos.x) > 0)
						{
							if (ang > -89 && ang < -45)
							{
								directInput = DirectionInput.Down;
								activeInput = false;
							}
							else if (ang >= -45)
							{
								directInput = DirectionInput.Right;
								activeInput = false;
							}
						}
						else if ((Input.mousePosition.x - currentPos.x) < 0)
						{
							if (ang > 45 && ang < 89)
							{
								directInput = DirectionInput.Down;
								activeInput = false;
							}
							else if (ang <= 45)
							{
								directInput = DirectionInput.Left;
								activeInput = false;
							}
						}

					}
				}
			}

			if (Input.GetMouseButtonUp(0))
			{
				directInput = DirectionInput.Null;
				activeInput = false;
			}
		}

	}

	//Sprint Item
	public void Sprint(float speed, float time) {
		if (!isSprint)
		{
            if (D3EnemyController.instance)
            {

                D3EnemyController.instance.HideMesh();
                D3EnemyController.instance.statusPlay = false;

            }
            StopCoroutine("CancelSprint");
            D3GameAttribute.gameAttribute.speed = speed;
            timeSprint = time;
			D3GUIManager.instance.pauseInSpecial();
            StartCoroutine(CancelSprint());
        }
	}

	IEnumerator CancelSprint() {
		if (D3PatternSystem.instance)
		{
			EnableControl = false;
			Vector3 start = transform.position;
			Vector3 end = new Vector3(transform.position.x, D3PatternSystem.instance.PositionYItemRocket, transform.position.z);
			DisebleHoverboard();
			gravitysave = gravity;
			gravity = 0;
			float t = 0;

			ForRocket = true;

			while (t < 1)
			{
				yield return null;
				t += Time.deltaTime / D3PatternSystem.instance.TimeForTheTransitionToRocket;
				transform.position = Vector3.Lerp(start, end, t);
			}

			ForRocket = false;
			isSprint = true;
			EnableControl = true;
            D3GUIManager.instance.resumeInSpecial();
            while (timeSprint > 0)
			{
				timeSprint -= 1 * Time.deltaTime;
				yield return 0;
			}
			int i = 0;
			D3GameAttribute.gameAttribute.speed = D3GameAttribute.gameAttribute.starterSpeed;
			ControlPosY = true;
			while (i < D3GameController.instace.countAddSpeed + 1)
			{
				D3GameAttribute.gameAttribute.speed += D3GameController.instace.speedAdd;
				i++;
			}
			if (D3PatternSystem.instance.CleanwhenFinishingRocketItem)
			{
                D3PatternSystem.instance.HidenObjectInPatterSystem();
            }
			gravity = gravitysave;
			animationManager.Floating();

        }

	}

	//Magnet Item
	public void Magnet(float time) {
		StopCoroutine("CancleMagnet");
		animationManager.m_Animator.SetFloat("IsMagnet", 1f);
        magnetCollider.SetActive(true);
		if (magnetCollider.GetComponent<D3Magnet>() != null)
		{
			if (magnetCollider.GetComponent<D3Magnet>().ObjectMagnet != null)
			{
                magnetCollider.GetComponent<D3Magnet>().ObjectMagnet.SetActive(true);
			}
		}
		timeMagnet = time;
		StartCoroutine(CancleMagnet());
	}

	public void ActivateImmortality()
	{
		_ImmortalityTime = ImmortalityTimeWhenRevived;
		IsImmortality = true;
		if (!IsImmortalityAnimation)
		{
			IsImmortalityAnimation = true;
			if (ImmortalityEffect != null)
			{
				ImmortalityEffect.SetActive(true);
			}

		}
	}

	public void ItemImmortality(float time)
	{
		_ImmortalityTime = time;
		IsImmortality = true;
		if (!IsImmortalityAnimation)
		{
			IsImmortalityAnimation = true;
			if (ImmortalityEffect != null)
			{
				ImmortalityEffect.SetActive(true);
			}

		}
	}

	IEnumerator CancleMagnet() {
		while (timeMagnet > 0) {
			timeMagnet -= 1 * Time.deltaTime;
			yield return 0;
		}
		animationManager.m_Animator.SetFloat("IsMagnet", 0);
        magnetCollider.SetActive(false);
		if (magnetCollider.GetComponent<D3Magnet>() != null)
		{
			if (magnetCollider.GetComponent<D3Magnet>().ObjectMagnet != null)
			{
                magnetCollider.GetComponent<D3Magnet>().ObjectMagnet.SetActive(false);
			}
		}

	}

	//Double jump Item
	public void JumpDouble(float time) {
		StopCoroutine("CancleJumpDouble");
		isDoubleJump = true;
		timeJump = time;
		StartCoroutine(CancleJumpDouble());
	}

	IEnumerator CancleJumpDouble() {
		while (timeJump > 0) {
			timeJump -= 1 * Time.deltaTime;
			yield return 0;
		}
		isDoubleJump = false;
	}

	//Multiply Item
	public void Multiply(float time) {
		StopCoroutine("CancleMultiply");
		isMultiply = true;
		timeMultiply = time;
		StartCoroutine(CancleMultiply());
	}

	IEnumerator CancleMultiply() {
		while (timeMultiply > 0) {
			timeMultiply -= 1 * Time.deltaTime;
			yield return 0;
		}
		isMultiply = false;
	}


    public void InitArrest()
    {
        StopCoroutine("Arrest");
        D3GUIManager.instance.pauseInSpecial();
        StartCoroutine(Arrest());
    }

    IEnumerator Arrest()
    {
        if (D3PatternSystem.instance)
        {
			IsArrest = true;
            EnableControl = false;
            DisebleHoverboard();
            animationManager.IsSArrest();
			if (D3EnemyController.instance)
			{
				D3EnemyController.instance.Arrest();
			}
            yield return new WaitForSeconds(0.5f);
        }

    }


    public void ItemSpecial(float speed, float time)
	{
		if (!IsSpecial)
		{
            if (D3EnemyController.instance)
            {

                D3EnemyController.instance.HideMesh();
                D3EnemyController.instance.statusPlay = false;

            }
            StopCoroutine("InitSpecial");
            D3GUIManager.instance.pauseInSpecial();
            D3GameAttribute.gameAttribute.speed = speed;
            if (SpecialItem != null)
            {
                SpecialItem.SetActive(true);
            }
            StartCoroutine(InitSpecial());
            timeSpecial = time;
        }
    }

    IEnumerator InitSpecial()
    {
        if (D3PatternSystem.instance)
        {
            EnableControl = false;
            DisebleHoverboard();
            gravitysave = gravity;
            gravity = 0;
            ForSpecial = true;
            Vector3 start = transform.position;
            Vector3 end = new Vector3(transform.position.x, D3PatternSystem.instance.PositionAnimationYItemSpecial, transform.position.z);
            float t = 0;
            while (t < 1)
            {
                yield return null;
                t += Time.deltaTime / D3PatternSystem.instance.TimeForTheTransitionToSpecial;
                transform.position = Vector3.Lerp(start, end, t);
                
            }
            ForSpecial = false;
            D3GUIManager.instance.Revival();
            IsSpecial = true;
            EnableControl = true;
            gravity = gravitysave;

        }

    }


}

