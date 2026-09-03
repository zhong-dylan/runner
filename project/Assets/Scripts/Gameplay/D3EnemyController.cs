using System.Threading;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class D3EnemyController : MonoBehaviour
{
    public static D3EnemyController instance;



    public int decreaseLife = 1; //decrease life if item = obstacle 
    public float DistanceEnemy = 2.5f;
    public float FirsHitPlayerDistanceEnemy = 3.5f;
    public float SecondHitPlayerDistanceEnemy = 2.5f;
    public float ThirdHitPlayerDistanceEnemy = 0f;
    public float DistanceAnimationCamera = -6;
    public Vector3 PosEnemyWhenArrestPlayer = new Vector3(0.1f,0f,0f);
    public AudioClip FarPolice, ArrestPlayer;




    private float SpeedEnemy;
    public bool mesh = true;
    private Vector3 posEnemy;
    private float DurationEnemy = 20;
    private float Count;
    private float distance;
    private float hideMesh = 5;
    private int hitPlayer ;
    public bool statusPlay = true;
    private float PlayerDist;
    private float DefaultDistanceCamera;
    public float ResetTime = 10;
    private float countReset = 0;
    private bool EnabledResetTime = false;
    void Start()
    {
        instance = this;
        ShowMesh();
        SpeedEnemy = D3GameAttribute.gameAttribute.speed;
        Count = DurationEnemy;
        distance = DistanceEnemy;
        if (D3CameraFollow.instace)
        {
           DefaultDistanceCamera = D3CameraFollow.instace.distance;
           D3CameraFollow.instace.distance = DistanceAnimationCamera;
        }
        if (D3SoundManager.instance && !D3Controller.instace.isSprint && !D3Controller.instace.IsSpecial && statusPlay)
        {
            if (FarPolice !=null)
            {
                D3SoundManager.instance.PlayingAudioClip(FarPolice);
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Item" && statusPlay)
        {
            if (col.GetComponent<D3Item>().typeItem == D3Item.TypeItem.Obstacle || col.GetComponent<D3Item>().typeItem == D3Item.TypeItem.Moving_Obstacle)
            {
                Jump();
            }
            if (col.GetComponent<D3Item>().typeItem == D3Item.TypeItem.Obstacle_Roll)
            {
                Roll();
            }
        }
    }

    public void UpdateResetTime()
    {
        countReset = ResetTime;
        EnabledResetTime = true;
    }

    public void ReStart()
    {
        ShowMesh();

        transform.position = new Vector3(D3GameController.instace.posStart.x +1.8f, D3GameController.instace.posStart.y + 2, D3GameController.instace.posStart.z + 505);

        if (D3SoundManager.instance && !D3Controller.instace.isSprint && !D3Controller.instace.IsSpecial && statusPlay)
        {
            if (FarPolice != null)
            {
                D3SoundManager.instance.PlayingAudioClip(FarPolice);
            }
        }
        if (D3CameraFollow.instace)
        {
            DefaultDistanceCamera = D3CameraFollow.instace.distance;
            D3CameraFollow.instace.distance = DistanceAnimationCamera;
        }
        statusPlay = true;
        SpeedEnemy = D3GameAttribute.gameAttribute.speed;
        Count = DurationEnemy;
        distance = DistanceEnemy;
        GetComponent<Animator>().SetBool("IsCatch", false);
        isRum();

    }
    public void Jump()
    {
        GetComponent<Animator>().PlayInFixedTime("Jump");
    }

    public void Idle()
    {
        GetComponent<Animator>().PlayInFixedTime("Idle");
    }

    public void TurnLeft()
    {
        GetComponent<Animator>().PlayInFixedTime("Left");
    }

    //Turn Right State
    public void TurnRight()
    {
        GetComponent<Animator>().PlayInFixedTime("Right");
    }

    public void Roll()
    {
        GetComponent<Animator>().PlayInFixedTime("Sliding");
    }

    public void isRum()
    {
        GetComponent<Animator>().PlayInFixedTime("Run");
    }

    public void ResetHit()
    {
        hitPlayer = 0;
    }

    public void IsSArrest()
    {
        GetComponent<Animator>().SetBool("IsCatch", true);
        GetComponent<Animator>().PlayInFixedTime("IsCatch");
    }
    public void Arrest()
    {
        statusPlay = false;
        if (D3SoundManager.instance && !D3Controller.instace.isSprint && !D3Controller.instace.IsSpecial && statusPlay)
        {
            if (ArrestPlayer != null)
            {
                D3SoundManager.instance.PlayingAudioClip(ArrestPlayer);
            }
        }
        IsSArrest();
        transform.position = Vector3.MoveTowards(transform.position, D3GameController.instace.Player.transform.position + PosEnemyWhenArrestPlayer, SpeedEnemy);
        D3GameAttribute.gameAttribute.life -= decreaseLife;
        D3GameAttribute.gameAttribute.ActiveShakeCamera();
    }
    public void ResetCount() {
        if (statusPlay)
        {
            SpeedEnemy = D3GameAttribute.gameAttribute.speed;

            if (Count >= 6 && Count <= 20)
            {
                if (hitPlayer == 2)
                {
                    if (distance > 1.2)
                    {
                        distance = ThirdHitPlayerDistanceEnemy;
                    }
                    hitPlayer = 0;
                }
                if (hitPlayer == 1)
                {
                    if (distance > 2.2)
                    {
                        distance = SecondHitPlayerDistanceEnemy;
                    }
                    hitPlayer = 2;
                }
                if (hitPlayer == 0)
                {
                    if (distance > 3.2)
                    {

                        if (D3SoundManager.instance && !D3Controller.instace.isSprint && !D3Controller.instace.IsSpecial && statusPlay)
                        {
                            if (FarPolice != null)
                            {
                                D3SoundManager.instance.PlayingAudioClip(FarPolice);
                            }
                        }
                        distance = FirsHitPlayerDistanceEnemy;
                        if (D3CameraFollow.instace)
                        {
                            DefaultDistanceCamera = D3CameraFollow.instace.distance;
                            D3CameraFollow.instace.distance = DistanceAnimationCamera;
                        }
                    }
                    hitPlayer = 1;
                }
            }
            if (Count <= 2 )
            {
                Count = DurationEnemy;

            }
        }
    }

    void Update()
    {
        if (EnabledResetTime)
        {
            if (countReset > 1)
            {
                countReset -= Time.deltaTime;
            }
            if (countReset <= 0)
            { 
                EnabledResetTime = false;
                statusPlay = true;

            }
        }

        if (!D3GameAttribute.gameAttribute.pause && !D3Controller.instace.isSprint && !D3Controller.instace.IsSpecial && statusPlay)
        {
            PlayerDist = Vector3.Distance(D3GameController.instace.Player.transform.position, transform.position);

            posEnemy.x = Mathf.Lerp(posEnemy.x, D3GameController.instace.Player.transform.position.x, 15 * Time.deltaTime);
            posEnemy.y = Mathf.Lerp(posEnemy.y, D3GameController.instace.Player.transform.position.y, 10 * Time.deltaTime);
            posEnemy.z = Mathf.Lerp(posEnemy.z, D3GameController.instace.Player.transform.position.z - distance, 5 * Time.deltaTime); //* Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, posEnemy, SpeedEnemy);

            if (Count > 1)
            {
                Count -= Time.deltaTime;
                if (Count < 5)
                {
                    if (D3CameraFollow.instace)
                    {
                        if (D3CameraFollow.instace.distance != DefaultDistanceCamera)
                        {
                            D3CameraFollow.instace.distance = DefaultDistanceCamera;
                        }
                    }
                    if (SpeedEnemy > 1)
                    {
                        SpeedEnemy -= Time.deltaTime;
                    }
                    distance += Time.deltaTime;
                }
            }
            if (PlayerDist > hideMesh)
            {
                HideMesh();
            }
            else {
                ShowMesh();

            }
        }
    }

    public void ShowMesh()
    {
        if (!mesh && !D3GameAttribute.gameAttribute.pause && !D3Controller.instace.isSprint && !D3Controller.instace.IsSpecial)
        {
            mesh = true;
            foreach (Transform tran in transform) tran.gameObject.SetActive(true);
        }
    }

    public void HideMesh()
    {
        if (mesh )
        {
            mesh = false;
            foreach (Transform tran in transform) tran.gameObject.SetActive(false);
        }
    }
}
