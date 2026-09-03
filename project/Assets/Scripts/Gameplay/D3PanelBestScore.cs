using TMPro;
using UnityEngine;

public class D3PanelBestScore : MonoBehaviour
{
    public static D3PanelBestScore instance;
    public Animator m_Animator;
    public TextMeshProUGUI TextCountBestScore;

    float BestScore;
    public float ScoreToActivatePanel = 300;
    bool Enabled = true;
    bool Animation1= false;
    bool Animation2 = false;

    void Start()
    {
        instance = this;
        Enabled = true;
        BestScore = D3GameData.LoadBestScore();
        TextCountBestScore.text = BestScore.ToString();
        if (!Animation1)
        {
            m_Animator.PlayInFixedTime("ExitPanelBestScore");
            Animation1 = true;
        }
        
    }

    public void ResetPanel()
    {
        Animation1 = false;
        Animation2 = false;
        Enabled = true;
        BestScore = D3GameData.LoadBestScore();
        TextCountBestScore.text = BestScore.ToString();
        if (!Animation1)
        {
            m_Animator.PlayInFixedTime("ExitPanelBestScore");
            Animation1 = true;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (BestScore > 0 && Enabled && !D3GameAttribute.gameAttribute.pause)
        {
            BestScore -= D3GameAttribute.gameAttribute.speed * Time.deltaTime;
            if (BestScore <= ScoreToActivatePanel)
            {
                if (!Animation2)
                {
                    m_Animator.PlayInFixedTime("PanelBestScore");
                    Animation2 = true;
                }
                TextCountBestScore.text = BestScore.ToString("00.");
            }
            
            
        }
        if (BestScore <= 0 && Enabled && !D3GameAttribute.gameAttribute.pause)
        { 
            BestScore = D3GameData.LoadBestScore();
            TextCountBestScore.text = BestScore.ToString();
            if (Animation1)
            {
                m_Animator.PlayInFixedTime("ExitPanelBestScore");
                Animation1 = false;
            }
            Enabled = false;
        }
    }
}
