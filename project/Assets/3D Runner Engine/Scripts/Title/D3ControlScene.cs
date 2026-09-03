using UnityEngine;
using UnityEngine.SceneManagement;

public class D3ControlScene : MonoBehaviour 
{

	public AudioClip sfxButton;
	
	private bool oneshotSfx;
	
	void Update () {
		
		if(Input.anyKeyDown)
		{
			if(!oneshotSfx)
			{
				AudioSource.PlayClipAtPoint(sfxButton,Vector3.zero);
				Invoke("LoadScene",0.5f);
				oneshotSfx = true;
			}
			
			
		}
	
	}
	
	void LoadScene()
	{
		SceneManager.LoadScene("Shop");
	}
	
}