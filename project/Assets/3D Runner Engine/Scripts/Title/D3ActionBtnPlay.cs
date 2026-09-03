using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class D3ActionBtnPlay : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	public Sprite normal;
	public Sprite actived;
	public string levelName;
	public GameObject WindowNoLife;
	int Life;

	

    public void Start()
    {
		WindowNoLife.SetActive(false);
    }

    private void Update()
    {
        Life = D3GameData.LoadLife();
	}

    public void OnPointerDown(PointerEventData eventData)
	{
		GetComponent<Image>().sprite = actived;
		if (Life > 0)
		{
			SceneManager.LoadScene(levelName);
		}

		if (Life <= 0)
		{
			WindowNoLife.SetActive(true);
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		GetComponent<Image>().sprite = normal;

	}
}
