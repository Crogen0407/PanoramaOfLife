using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
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
