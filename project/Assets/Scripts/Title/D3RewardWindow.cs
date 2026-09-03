using UnityEngine;
using UnityEngine.UI;

public class D3RewardWindow : MonoBehaviour
{
    public Text TextReward;
    public Image ImageReward;

    public bool AutoClose = false;
    float  TimeToClose = 3;
    float TimeSelect = 0;

    private void Start()
    {
        TimeSelect = TimeToClose;
    }
    public void Update()
    {
        if (AutoClose)
        {
            if (TimeToClose > 0 && AutoClose)
            {

                TimeToClose -= Time.deltaTime;
                
            }
            if (TimeToClose <= 0 && AutoClose && !D3GameAttribute.gameAttribute.pause)
            {
                if (D3GUIManager.instance)
                {
                    TimeToClose = TimeSelect;
                    D3GUIManager.instance.CloseRewardWindow();
                }

            }

        }
    }



}
