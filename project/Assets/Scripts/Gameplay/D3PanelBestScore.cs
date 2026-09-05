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
    bool isPanelShown = false;

    void Start()
    {
        instance = this;
        Enabled = true;
        BestScore = D3GameData.LoadBestScore();
        TextCountBestScore.text = BestScore.ToString();
        PlayExit();
    }

    public void ResetPanel()
    {
        isPanelShown = false;
        Enabled = true;
        BestScore = D3GameData.LoadBestScore();
        TextCountBestScore.text = BestScore.ToString();
        PlayExit();
    }


    // Update is called once per frame
    void Update()
    {
        if (!Enabled)
        {
            return;
        }

        var gameAttribute = D3GameAttribute.gameAttribute;
        if (gameAttribute == null)
        {
            return;
        }

        if (isPanelShown && (gameAttribute.pause || !gameAttribute.isPlaying))
        {
            HidePanel();
            return;
        }

        if (BestScore > 0 && !gameAttribute.pause)
        {
            BestScore -= gameAttribute.speed * Time.deltaTime;
            if (BestScore <= ScoreToActivatePanel)
            {
                if (!isPanelShown)
                {
                    PlayShow();
                }
                TextCountBestScore.text = BestScore.ToString("00.");
            }
        }

        if (BestScore <= 0)
        { 
            BestScore = D3GameData.LoadBestScore();
            TextCountBestScore.text = BestScore.ToString();
            HidePanel();
        }
    }

    private void PlayShow()
    {
        if (m_Animator != null)
        {
            m_Animator.PlayInFixedTime("PanelBestScore", 0, 0f);
        }

        isPanelShown = true;
    }

    private void HidePanel()
    {
        if (isPanelShown)
        {
            PlayExit();
        }

        Enabled = false;
    }

    private void PlayExit()
    {
        if (m_Animator != null)
        {
            m_Animator.PlayInFixedTime("ExitPanelBestScore", 0, 0f);
        }

        isPanelShown = false;
    }
}
