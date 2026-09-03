using UnityEditor;

[System.Serializable]
public class D3LevelSystemData
{
    public int IDLevel;
    public bool LevelLocked = true;
    public int LevelRating;
#if UNITY_EDITOR
    public SceneAsset Scene;
#endif
    public string LevelScene;
}
