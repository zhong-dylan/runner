using UnityEngine;
[System.Serializable]
public class D3SetFPS : MonoBehaviour {

	[HideInInspector]
	public int fpsTarget;
	
	void Start () {
		Application.targetFrameRate = fpsTarget;
	}
}
