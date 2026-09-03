using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class D3LevelObject : MonoBehaviour
{
    [SerializeField] public Text LevelText;
    [SerializeField] public Button LevelButton;
    [SerializeField] public GameObject LockedImg;

    [SerializeField] public Image[] Star_Icons = new Image[3];
    [SerializeField] public Sprite Star_NoFill;
    [SerializeField] public Sprite Star_Fill;

    public int LevelID;
    public void SetupLevelObject(int level, int rating, bool isLocked)
    {
        LevelText.text = level.ToString();
        LevelID = level;
        SetupStart(rating, isLocked);

        LevelButton.interactable = !isLocked;

        LevelButton.onClick.RemoveAllListeners();
        LevelButton.onClick.AddListener(() =>
        {
            OnClick_LoadLevel();
        });
    }

    public void OnClick_LoadLevel()
    {
        if (D3LevelSystemManager.Instance.LevelSystenData[LevelID - 1].LevelScene != null)
        {
            string scene = D3LevelSystemManager.Instance.LevelSystenData[(LevelID - 1)].LevelScene;

            D3LevelSystemManager.Instance.CurrentLevel = (LevelID - 1);

            if (string.IsNullOrEmpty(scene))
            {
                Debug.LogError("Could not find scene name '" + scene + "'");
                return;
            }
            else
            {

                D3AddressablesManager.LoadSceneByName(scene);
            }
        }
        else
        {
            Debug.LogError("Scene is not assigned to the Level");
            return;
        }
    }

    private void SetupStart(int rating, bool islocked)
    {
        if (!islocked)
        {
            LockedImg.SetActive(false);
            if (rating == 0)
            {
                for (int i = 0; i < Star_Icons.Length; i++)
                {
                    Star_Icons[i].gameObject.SetActive(true);
                    Star_Icons[i].sprite = Star_NoFill;
                }
            }
            else
            {
                for (int i = 0; i < Star_Icons.Length; i++)
                {
                    Star_Icons[i].gameObject.SetActive(true);

                    if (i < rating)
                    {
                        Star_Icons[i].sprite = Star_Fill;
                    }
                    else
                    {
                        Star_Icons[i].sprite = Star_NoFill;
                    }
                }
            }

        }
        else {
            LockedImg.SetActive(true);
            for (int i = 0; i < Star_Icons.Length; i++)
            {
                Star_Icons[i].gameObject.SetActive(false);
                Star_Icons[i].sprite = Star_NoFill;
            }
        }
        
    }
}
