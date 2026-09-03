using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class D3ActionBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	public Sprite normal;
	public Sprite actived;
	public string levelName;
	

	public void OnPointerDown(PointerEventData eventData)
	{
		GetComponent<Image>().sprite = actived;
		SceneManager.LoadScene(levelName);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		GetComponent<Image>().sprite = normal;

	}

}
