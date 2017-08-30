using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour 
{

	public static Music instance;
	
	public AudioClip audioClamMusic;
	public AudioClip audioBossMusic;
	
	AudioSource audioPlayer;
		
	void Start () 
	{
		instance = this;
		audioPlayer = gameObject.GetComponent<AudioSource>();
	}
	
	public void PlayCalmMusic()
	{
		audioPlayer.clip = audioClamMusic;
		audioPlayer.Play();
	}
	
	public void PlayBossMusic()
	{
		audioPlayer.clip = audioBossMusic;
		audioPlayer.volume = 0.8f;
		audioPlayer.Play();
	}
}
