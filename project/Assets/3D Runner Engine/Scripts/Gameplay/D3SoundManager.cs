using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class D3SoundManager : MonoBehaviour {
	
	[System.Serializable]
	public class SoundGroup{

		public AudioClip audioClip;

		public string soundName;
	}


	public AudioClip BGSound;
    public AudioClip BGSoundSpecial;
    public AudioSource bgmSound;
	public AudioSource SFXSound;

    public List<SoundGroup> sound_List = new List<SoundGroup>();
	
	public static D3SoundManager instance;

	public Slider musicVolumeSlider = null;
	public float musicVolume = 1.0f;

	public Slider SFXVolumeSlider = null;
	public float SFXVolume = 1.0f;

	int FirstLaunch = 0;

	public void Start(){

		FirstLaunch = PlayerPrefs.GetInt("FirstLaunch");
        if (FirstLaunch == 0)
        {
            SFXVolume = 1;
            musicVolume = 1;
            PlayerPrefs.SetFloat("SFXVolume", SFXVolume);
            PlayerPrefs.SetFloat("musicVolume", musicVolume);

            PlayerPrefs.SetInt("FirstLaunch", 1);
        }
        if (FirstLaunch == 1)
        {
            SFXVolume = PlayerPrefs.GetFloat("SFXVolume");
            musicVolume = PlayerPrefs.GetFloat("musicVolume");
        }
        instance = this;
		loadVolumen();
		StartCoroutine(StartBGM());
	}

	public void loadVolumen()
	{
		SFXSound.volume = SFXVolume;
		bgmSound.volume = musicVolume;
		if (SFXVolumeSlider != null)
			SFXVolumeSlider.value = SFXVolume;
        if (musicVolumeSlider != null)
           musicVolumeSlider.value = musicVolume;
    }

	public void OnSFXVolumeSliderUpdated()
	{
		if(SFXVolumeSlider!=null)
		SFXVolume = SFXVolumeSlider.value;
		SFXSound.volume = SFXVolume;
	}

	public void OnMusicVolumeSliderUpdated()
	{
		if(musicVolumeSlider!=null)
		musicVolume = musicVolumeSlider.value;
		bgmSound.volume = musicVolume;
	}

	public void OnAudioDataUpdated()
	{
		PlayerPrefs.SetFloat("SFXVolume", SFXVolume);
		PlayerPrefs.SetFloat("musicVolume", musicVolume);
		loadVolumen();
	}

	public void PlayingSound(string _soundName){
		SFXSound.PlayOneShot(sound_List[FindSound(_soundName)].audioClip);
	}
    public void PlayingAudioClip(AudioClip _soundName)
    {
        SFXSound.PlayOneShot(_soundName);
    }

    private int FindSound(string _soundName){
		int i = 0;
		while( i < sound_List.Count ){
			if(sound_List[i].soundName == _soundName){
				return i;	
			}
			i++;
		}
		return i;
	}
	
	public IEnumerator StartBGM()
	{
		yield return new WaitForSeconds(0.5f);
		if (D3PatternSystem.instance != null)
		{
			while (D3PatternSystem.instance.loadingComplete == false)
			{
				yield return 0;
			}
		}
		
		bgmSound.clip = BGSound;
		bgmSound.Play();
	}
    public IEnumerator StartBGMSpecial()
    {
        yield return new WaitForSeconds(0.5f);
        if (D3PatternSystem.instance != null)
        {
            while (D3PatternSystem.instance.loadingComplete == false)
            {
                yield return 0;
            }
        }

        bgmSound.clip = BGSoundSpecial;
        bgmSound.Play();
    }
}
