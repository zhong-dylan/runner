using UnityEngine;
using UnityEngine.UI;

#if UNITY_ADS && ENABLE_UNITY_ADS
using UnityEngine.Advertisements;
#endif

public class D3ADSVideoReward : MonoBehaviour
{


    public int IDButton;
    public D3TitleCharacter TitleScene;
    public D3GameController GameController;

    void Awake()
    {
        if (GetComponent<Button>())
        {
            if (D3ADSManager.D3AdsManager)
            {
                GetComponent<Button>().onClick.AddListener(ShowRewardedUnityVideo);

            }
        }
    }

    private void Update()
    {
        if (GetComponent<Button>())
        {
            if (!D3ADSManager.D3AdsManager.ADSRewardReady)
            {
                GetComponent<Button>().interactable = false;
            }
            if (D3ADSManager.D3AdsManager.ADSRewardReady)
            {
                GetComponent<Button>().interactable = true;
            }

        }
    }

    void ShowRewardedUnityVideo()
    {
        if (TitleScene != null)
        {
            if (D3ADSManager.D3AdsManager)
            {
#if UNITY_ADS && ENABLE_UNITY_ADS
                D3ADSManager.D3AdsManager.OnUnityAdsRewarded(IDButton);
#endif
#if !UNITY_ADS || !ENABLE_UNITY_ADS
                D3ADSManager.D3AdsManager.OnUnityNoADS(IDButton);
#endif
            }
        }
    }


}
