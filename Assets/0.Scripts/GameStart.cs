using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class GameStart : MonoBehaviour
{
    public Light2D blueLight;
    public Light2D radLight;
	private AudioSource audioSource;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
        PlayerPrefs.SetFloat("Sound", 1);

	}

	public void StartGame()
	{
        SceneManager.LoadScene("StoryScene");
	}

    public void GameQuit()
	{
        Application.Quit();
	}

    public void SetSound(float volume)
	{
        PlayerPrefs.SetFloat("Sound", volume);
	}

	public void SetSoundVolume()
	{
		audioSource.volume = 1 * PlayerPrefs.GetFloat("Sound");
	}
}
