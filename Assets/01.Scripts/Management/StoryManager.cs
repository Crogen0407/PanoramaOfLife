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
                        lastStory = "���� ������ ���������.";
                        break;
                    case 1:
                        lastStory = "�Ҹ��� �鸮�� �ʴ´�.";
                        break;
                    case 2:
                        lastStory = "������ �������� �ʴ´�.";
                        break;
                    case 3:
                        lastStory = "������ �Ѽ����̴�.";
                        break;
                    case 4:
                        lastStory = "���ϰ� ��� �ʹ�.";
                        break;
                    case 5:
                        lastStory = "�Ƹ���� ��� �ʹ�.";
                        break;
                    case 6:
                        lastStory = "����ϸ� ��� �ʹ�.";
                        break;
                    case 7:
                        lastStory = "���ϰ� �װ� �ʹ�.";
                        break;
                    case 8:
                        lastStory = "�Ƹ���� �װ� �ʹ�.";
                        break;
                    case 9:
                        lastStory = "���ϰ� �װ� �ʹ�.";
                        break;
                    case 10:
                        lastStory = "���� �ҿ��� ����� �ʹ� �����Ʊ⿡ ���� �̸� �����ߴ�.|=|����ϰ� ��� �״� ���� �����ƴ� ���ϱ�...";
                        break;
                    default:
                        lastStory = "\n\"����Ǹ� ��������??\"\n";
                        break;
                }
                bossStoryCount++;
            }
			else
			{
                lastStory = "���� ��ﵵ,||| �� ���� ��︶�� �������.|||=|||�������� ����ϴ�.|||=|||�������� ����ߴ�.|||";
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
                lastStory = "�𸣰ڴ�. |||���� ���� ������ �߾����� ����� �ȳ���.";

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
