using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class D3PanelRewardADS : MonoBehaviour
{
    public static D3PanelRewardADS instance;
    public Animator m_Animator;
    public Button RewardButton;
    public TextMeshProUGUI TextReward;
    public Image ImgReward;
    float TimeActivatePanel = 15;
    float TimeSelect = 0;
    public bool EnabledPanelRewardADS = true;
    bool Animation1 = false;
    bool Animation2 = false;

    void Start()
    {
        instance = this;
        TimeSelect = TimeActivatePanel;
#if UNITY_ADS && ENABLE_UNITY_ADS
        if (D3GameController.instace)
        {
            if (D3GameController.instace.ListRandomRewardedAD.Count > 0)
            {

                for (int i = 0; D3GameController.instace.ListRandomRewardedAD.Count > i; i++)
                {
                    if (D3GameController.instace.ListRandomRewardedAD[i] != null)
                    {
                        D3GameController.instace.ListRandomRewardedAD[i].IdButton = i;
                    }
                }
            }
        }
        else {
            DisablePanel();
        }

        if (RewardButton)
        {
            if (D3ADSManager.D3AdsManager)
            {
                RewardButton.onClick.AddListener(ShowRewardedUnityVideo);
            }
        }
#endif
    }

    void ShowRewardedUnityVideo()
    {
#if UNITY_ADS && ENABLE_UNITY_ADS
        if (D3ADSManager.D3AdsManager)
        {
            D3Controller.instace.PauseOn();
            D3GameAttribute.gameAttribute.Pause(true);
            D3ADSManager.D3AdsManager.OnUnityAdsRewarded(0);
        }
#endif
    }

    public void EnabledPanel()
    {
        EnabledPanelRewardADS = true;
        if (!Animation1)
        {
            if (m_Animator.gameObject.activeSelf)
            {
                m_Animator.PlayInFixedTime("ExitPanel");
                Animation1 = true;
            }
        }
        if (Animation2)
        {
            Animation2 = false;
        }
    }
    public void DisablePanel()
    {
        EnabledPanelRewardADS = false;
    }

    void SelectRandomReward()
    {
#if UNITY_ADS && ENABLE_UNITY_ADS
        RewardButton.gameObject.SetActive(true);
        if (D3GameController.instace.ListRandomRewardedAD.Count > 0)
        {
            int SelectRandom = Random.Range(0, D3GameController.instace.ListRandomRewardedAD.Count);

            for (int i = 0; D3GameController.instace.ListRandomRewardedAD.Count > i; i++)
            {
                if (SelectRandom == D3GameController.instace.ListRandomRewardedAD[i].IdButton)
                {
                    D3GameController.instace.RewardSelect = D3GameController.instace.ListRandomRewardedAD[i].IdButton;
                    ImgReward.sprite = D3GameController.instace.ListRandomRewardedAD[i].ImgReward;

                    if (TextReward != null && D3GameController.instace.ListRandomRewardedAD[i].TypeReward == D3ADSRandomReward.D3TypeRandomReward.Null)
                    {
                        RewardButton.gameObject.SetActive(false);
                    }
                    if (TextReward != null && D3GameController.instace.ListRandomRewardedAD[i].TypeReward == D3ADSRandomReward.D3TypeRandomReward.Life)
                    {
                        TextReward.text = "Life + " + D3GameController.instace.ListRandomRewardedAD[i].CantReward.ToString();
                    }
                    if (TextReward != null && D3GameController.instace.ListRandomRewardedAD[i].TypeReward == D3ADSRandomReward.D3TypeRandomReward.HoverboardKeyUse)
                    {
                        TextReward.text = "Hoverboard Key Use + " + D3GameController.instace.ListRandomRewardedAD[i].CantReward.ToString();
                    }
                    if (TextReward != null && D3GameController.instace.ListRandomRewardedAD[i].TypeReward == D3ADSRandomReward.D3TypeRandomReward.Coin)
                    {
                        TextReward.text = "Coin + " + D3GameController.instace.ListRandomRewardedAD[i].CantReward.ToString();
                    }
                    if (TextReward != null && D3GameController.instace.ListRandomRewardedAD[i].TypeReward == D3ADSRandomReward.D3TypeRandomReward.ItemJump)
                    {
                        TextReward.text = "Item Jump";
                    }
                    if (TextReward != null && D3GameController.instace.ListRandomRewardedAD[i].TypeReward == D3ADSRandomReward.D3TypeRandomReward.ItemMagnet)
                    {
                        TextReward.text = "Item Magnet";
                    }
                    if (TextReward != null && D3GameController.instace.ListRandomRewardedAD[i].TypeReward == D3ADSRandomReward.D3TypeRandomReward.ItemMultiply)
                    {
                        TextReward.text = "Item Multiply";
                    }
                    if (TextReward != null && D3GameController.instace.ListRandomRewardedAD[i].TypeReward == D3ADSRandomReward.D3TypeRandomReward.ItemShield)
                    {
                        TextReward.text = "Item Shield";
                    }
                    
                }
            }
        }
#endif
    }

    void Update()
    {
        if (TimeActivatePanel > 0 && EnabledPanelRewardADS && !D3GameAttribute.gameAttribute.pause)
        {

            TimeActivatePanel -= Time.deltaTime;
            
        }
        if (TimeActivatePanel <= 0 && EnabledPanelRewardADS && !D3GameAttribute.gameAttribute.pause)
        {
            if (Animation2)
            {
                EnabledPanel();
            }else{
                SelectRandomReward();
                if (Animation1)
                {
                    Animation1 = false;
                }
                m_Animator.PlayInFixedTime("EnabledPanel");
                Animation2 = true;
                TimeActivatePanel = TimeSelect;
            }
           
        }
        if (RewardButton != null)
        {
            if (D3ADSManager.D3AdsManager)
            {
                if (!D3ADSManager.D3AdsManager.ADSRewardReady)
                {
                    RewardButton.gameObject.SetActive(false);
                }
                if (D3ADSManager.D3AdsManager.ADSRewardReady)
                {
                    RewardButton.gameObject.SetActive(true);
                }

            }
            
        }
    }
}
