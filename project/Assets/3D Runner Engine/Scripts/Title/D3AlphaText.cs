using UnityEngine;
using UnityEngine.UI;

public class D3AlphaText : MonoBehaviour {
	
	public float speedFade;
	private float count;
	void Update () {
		count += speedFade * Time.deltaTime;
		GetComponent<Image>().color = new Color(0.5f,0.5f,0.5f,Mathf.Sin(count)*0.5f);
	}
}
