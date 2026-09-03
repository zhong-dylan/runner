using UnityEngine;

public class D3ItemCoinRotate : MonoBehaviour {
	
	public void PlayCoin(){
		if (GetComponent<Animation>())
		{
			GetComponent<Animation>().playAutomatically=true;
            GetComponent<Animation>().Play();

        }
		else {
			Debug.Log(gameObject.name + " Please add Animation RotateAnimation this gameobject, visit object example for more information");
		}
		
	}	
	
	public void Reset(){
		if (GetComponent<Animation>())
		{
			GetComponent<Animation>().Stop();
		}
	}
	
}
