using UnityEngine;

public class D3DestroyEffect : MonoBehaviour {

	public float time;
	
	void Start(){
		Destroy(gameObject, time);	
	}
}
