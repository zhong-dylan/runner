using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


[System.Serializable]
public class D3LevelSystemManager : MonoBehaviour
{

    public static D3LevelSystemManager Instance { get; private set; }

    public List<D3LevelSystemData> LevelSystenData;

    public string LevelDataKey = "LevelSystenData";

    public int CurrentLevel = 0;

    public bool EnabledLevelSystem = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;

            transform.parent = null;

            DontDestroyOnLoad(gameObject);

            LoadDataFronMemory();
        }
    }


    public void LoadDataFronMemory()
    {
        if (PlayerPrefs.HasKey(LevelDataKey))
        {
            string json = PlayerPrefs.GetString(LevelDataKey);

            List<D3LevelSystemData> Data = D3JsonHelper.FromJson<D3LevelSystemData>(json).ToList();

            if (Data == null || Data.Count == 0)
            {
                Debug.Log("Data is == null or count == 0");
                return;
            }

            LevelSystenData = Data;
        }
        else {
            SaveToMemory();
        }
    }
    public void SaveToMemory()
    {
        if (LevelSystenData == null || LevelSystenData.Count == 0)
            return;

        string json = D3JsonHelper.ToJson(LevelSystenData.ToArray());

        PlayerPrefs.SetString(LevelDataKey, json);
    }
    public void ClearMemory()
    {
        PlayerPrefs.DeleteKey(LevelDataKey);
    }

    public void UpdateData(int level, int rating)
    {
        if (rating > 0)
        {
            LevelSystenData[level].LevelLocked = false;
            LevelSystenData[level].LevelRating = rating;

            int newlevel = (CurrentLevel + 1);

            if (newlevel < LevelSystenData.Count)
            {
                LevelSystenData[newlevel].LevelLocked = false;
            }

            SaveToMemory();
        }
    }
    public void CompletedLevel(int level, int rating)
    {
        UpdateData(level, rating);
    }

    public void LoadNewLevel()
    {
        int newlevel = (CurrentLevel +1);
        if (LevelSystenData[newlevel].LevelScene != null)
        {
            string scene = LevelSystenData[newlevel].LevelScene;
            if (SceneUtility.GetBuildIndexByScenePath(scene) == 0)
            {
                Debug.LogError("Could not find scene name '" + scene + "'");
                return;
            }
            else
            {
                CurrentLevel = newlevel;
                SceneManager.LoadScene(scene);
            }
        }
        else
        {
            Debug.LogError("Scene is not assigned to the Level");
            return;
        }
    }
}
