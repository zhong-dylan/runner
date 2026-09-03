using UnityEngine;

public class D3AnimationManager : MonoBehaviour {

	public Animator m_Animator;
	private D3Controller controller;
	private float speed_Run;
	private float default_Speed_Run;
	[HideInInspector] public float timesliding = 1f;
	[HideInInspector] public bool IsSliding = false;

	void Start() {
		if (GetComponent<D3Controller>())
		{
            controller = this.GetComponent<D3Controller>();
            default_Speed_Run = D3GameAttribute.gameAttribute.speed;
        }
	}

	private void Update()
	{
		if (IsSliding)
		{
			if (timesliding > 0)
			{
				timesliding -= Time.deltaTime;
				if (timesliding <= 0)
				{
					IsSliding = false;
					controller.isRoll = false;
					timesliding = 1f;
				}
			}


		}
	}


	//Run State
	public void Run() {
        m_Animator.SetBool("IsPause", false);
        m_Animator.PlayInFixedTime("Rum");
		speed_Run = (D3GameAttribute.gameAttribute.speed / default_Speed_Run);

	}

	//Jump State
	public void Jump() {
		m_Animator.PlayInFixedTime("Jump");
		if (D3EnemyController.instance)
		{
			D3EnemyController.instance.Jump();
		}

	}

	//Double Jump State
	public void JumpSecond() {
		m_Animator.PlayInFixedTime("JumpSecond");
	}

	public void HoverLeft()
	{
		m_Animator.PlayInFixedTime("HoverLeft");
	}

	//Turn Right State
	public void HoverRight()
	{
		m_Animator.PlayInFixedTime("HoverRight");
	}


	public void HoverBoarLeft()
	{
		m_Animator.PlayInFixedTime("HoverBoardLeft");
	}

	//Turn Right State
	public void HoverBoarRight()
	{
		m_Animator.PlayInFixedTime("HoverBoardRight");
	}

	public void HoverBoarJump()
	{
		m_Animator.PlayInFixedTime("HoverBoardJump");

	}
	public void HoverBoarRoll()
	{
		m_Animator.PlayInFixedTime("HoverBoardScroll");
		IsSliding = true;
		controller.isRoll = true;
	}

	//Turn Left State
	public void TurnLeft() {
		m_Animator.PlayInFixedTime("Left");
		if (D3EnemyController.instance)
		{
			D3EnemyController.instance.TurnLeft();
		}
	}

	//Turn Right State
	public void TurnRight() {
		m_Animator.PlayInFixedTime("Right");
		if (D3EnemyController.instance)
		{
			D3EnemyController.instance.TurnRight();
		}
	}

	//Roll State
	public void Roll() {
		m_Animator.PlayInFixedTime("Sliding");
		if (D3EnemyController.instance)
		{
			D3EnemyController.instance.Roll();
		}
		IsSliding = true;
		controller.isRoll = true;
	}

	//Dead State
	public void Dead()
	{
		if (D3Controller.instace.IsArrest)
		{
			IsSArrest();
		}
		else {

            m_Animator.PlayInFixedTime("Dead");

            m_Animator.SetBool("IsDead", true);
        }
		if (D3EnemyController.instance)
		{ 
        D3EnemyController.instance.IsSArrest();
        }
	}

	public void isRum()
	{
		m_Animator.SetBool("IsHoverBoard", false);
		m_Animator.SetBool("IsDead", false);
		m_Animator.PlayInFixedTime("Rum");
		if (D3EnemyController.instance)
		{
			D3EnemyController.instance.isRum();
		}
	}

	public void Floating()
	{
		m_Animator.PlayInFixedTime("Floating");
	}

	public void PauseOn()
	{
		m_Animator.SetBool("IsPause", true);
		m_Animator.PlayInFixedTime("Idle");
	}

	public void PauseOff()
	{
		m_Animator.SetBool("IsPause", false);
		m_Animator.PlayInFixedTime("Rum");
	}


    public void IsSpecialLeft()
    {
        m_Animator.PlayInFixedTime("IsSpecialLeft");
    }

    //Turn Right State
    public void IsSpecialRight()
    {
        m_Animator.PlayInFixedTime("IsSpecialRight");
    }

    public void IsSpecialJump()
    {
        m_Animator.PlayInFixedTime("IsSpecialJump");

    }
    public void IsSpecialRoll()
    {
        m_Animator.PlayInFixedTime("IsSpecialScroll");
        IsSliding = true;
        controller.isRoll = true;
    }

    public void IsSArrest()
    {
        m_Animator.SetBool("IsCatch", true);
        m_Animator.PlayInFixedTime("IsCatch");
    }

}
