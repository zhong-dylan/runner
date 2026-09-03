using UnityEngine;

[System.Serializable]
public class D3LevelSystem : MonoBehaviour
{
    [SerializeField] public GameObject TemplateLevelPrefab;
    [SerializeField] public Transform RootCanvasLevelsParent;

    void Start()
    {
        if (D3LevelSystemManager.Instance)
        {
            if (D3LevelSystemManager.Instance.LevelSystenData.Count > 0)
            {
                for (int i = 0; i < D3LevelSystemManager.Instance.LevelSystenData.Count; i++)
                {
                    GameObject LevelObj = Instantiate(TemplateLevelPrefab, RootCanvasLevelsParent, false);

                    D3LevelObject prefabData = LevelObj.GetComponent<D3LevelObject>();
                    prefabData.SetupLevelObject(D3LevelSystemManager.Instance.LevelSystenData[i].IDLevel, D3LevelSystemManager.Instance.LevelSystenData[i].LevelRating, D3LevelSystemManager.Instance.LevelSystenData[i].LevelLocked);
                }
            }
        }
        else {

            Debug.Log("Error Reading Level System");
        }
    }
    public void CloseWindows()
    {
        if (D3TitleCharacter.instance)
        {
            D3TitleCharacter.instance.CloseLevelSystemWindow();
        }
    }
}
