using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{
    static int bossStoryCount=0;
    public TextMeshProUGUI storyText;
    public TextAsset storyTextFile;
    public static string[] text;
    public static int _storyIndex = 0;
    public string lastStory = "";
    [SerializeField] private Image darkPanel;
    private static bool isBossStage;
    private static bool isGameClear;
    //Sounds
    [SerializeField] private AudioClip[] storySounds;

    //Components
    [SerializeField] private AudioSource audioSource_soundeffect;
    [SerializeField] private AudioSource audioSource_Backgroundmusic;
    void Start()
    {
        isGameClear = GameManager.Instance.IsGameClear;
        if (text == null)
		{
            text = storyTextFile.text.Split('\n');
		}
        StartCoroutine(TextRendering());
        audioSource_Backgroundmusic.clip = storySounds[_storyIndex];
        audioSource_Backgroundmusic.Play();
        float sound = PlayerPrefs.GetFloat("Sound");
        audioSource_soundeffect.volume *= sound;
        audioSource_Backgroundmusic.volume = 1 * PlayerPrefs.GetFloat("Sound");
    }

    public void StorySkip()
	{
        StopAllCoroutines();
        StartCoroutine(PanelfadeIn(1));
    }

    IEnumerator TextRendering()
	{
        yield return new WaitForSeconds(1f);

        if (isBossStage == true)
		{
            if(isGameClear == false)
			{
				switch (bossStoryCount)
				{
					case 0:
                        lastStory = "팔의 감각이 사라져간다.";
                        break;
                    case 1:
                        lastStory = "소리가 들리지 않는다.";
                        break;
                    case 2:
                        lastStory = "냄새가 느껴지지 않는다.";
                        break;
                    case 3:
                        lastStory = "폭격은 한순간이다.";
                        break;
                    case 4:
                        lastStory = "편하게 살고 싶다.";
                        break;
                    case 5:
                        lastStory = "아름답게 살고 싶다.";
                        break;
                    case 6:
                        lastStory = "사랑하며 살고 싶다.";
                        break;
                    case 7:
                        lastStory = "편하게 죽고 싶다.";
                        break;
                    case 8:
                        lastStory = "아름답게 죽고 싶다.";
                        break;
                    case 9:
                        lastStory = "편하게 죽고 싶다.";
                        break;
                    case 10:
                        lastStory = "나의 소원은 욕심이 너무 지나쳤기에 신은 이를 무시했다.|=|평범하게 살고 죽는 것이 지났쳤던 것일까...";
                        break;
                    default:
                        lastStory = "\n\"이쯤되면 포기하지??\"\n";
                        break;
                }
                bossStoryCount++;
            }
			else
			{
                lastStory = "좋은 기억도,||| 안 좋은 기억마저 사라진다.|||=|||마지막은 편안하다.|||=|||마지막은 편안했다.|||";
            }

        }
		else if(!isBossStage)
		{
            if (!GameManager.Instance.isLastStage)
            {
                yield return new WaitForSeconds(1f);
                if (isBossStage == false)
                {
                    lastStory = text[_storyIndex];
                    _storyIndex++;
                }
            }
            else if (GameManager.Instance.isLastStage && isBossStage == false)
            {
                lastStory = "모르겠다. |||내가 무슨 생각을 했었는지 기억이 안난다.";

                yield return new WaitForSeconds(1f);
                isBossStage = true;
            }
        }
        for (int i = 0; i < lastStory.Length; i++)
        {
            storyText.transform.localScale = Vector3.one;

            if (lastStory[i] == '=')
            {
                yield return new WaitForSeconds(1f);
                int curScale = 1;
                float curTime = 0;
                float timePersent = 0.5f;
                while (curTime / timePersent < 1)
				{
                    storyText.transform.localScale = Vector3.Lerp(new Vector3(1, curScale), new Vector3(1, 0), curTime / timePersent);
                    curTime += Time.deltaTime;
                    yield return null;
                }
                storyText.transform.localScale = Vector3.zero;
                storyText.text = "";
            }
            else if (lastStory[i] == '|')
            {
                yield return new WaitForSeconds(0.5f);
            }
            else if (lastStory[i] == '\\')
            {
                storyText.text += '\n';
            }
            else
            {
                audioSource_soundeffect.Play();
                yield return new WaitForSeconds(0.12f);
                storyText.text += lastStory[i];
            }
        }
        yield return new WaitForSeconds(5);
        StartCoroutine(PanelfadeIn());
        yield return null;
    }

    public IEnumerator PanelfadeIn(float timePercent = 5)
    {
        darkPanel.gameObject.SetActive(true);
        float currentVulume = audioSource_Backgroundmusic.volume;
        float time = 0;
        Color currentColor = darkPanel.color;
        while (time / timePercent < 1)
        {
            audioSource_Backgroundmusic.volume = Mathf.Lerp(currentVulume, 0, time / timePercent);
            darkPanel.color = Color.Lerp(currentColor, Color.black, time / timePercent);
            time += Time.deltaTime;
            yield return null;
        }
        darkPanel.color = Color.black;
        if (GameManager.Instance.IsGameClear)
        {
            Application.Quit();
        }
        else if (text.Length == _storyIndex)
		{
            GameManager.Instance.isLastStage = true;
            isBossStage = true;
        }
        SceneManager.LoadScene("GameScene");
    }
}
