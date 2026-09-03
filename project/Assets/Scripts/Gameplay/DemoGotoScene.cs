using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoGotoScene : MonoBehaviour
{

    public void GotoScene(string NameScene)
    {
        D3AddressablesManager.LoadSceneByName(NameScene);
    }
}
