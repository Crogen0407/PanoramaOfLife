using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    private Image darkPanel;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        darkPanel = GameObject.Find("Canvas").transform.Find("Panel").GetComponent<Image>();
        PanelActive(true);
    }

    public void PanelActive(bool active)
	{
        darkPanel.gameObject.SetActive(active);
    }

    public IEnumerator PanelfadeIn()
	{
        PanelActive(true);
        float time = 0;
        float timePercent = 5;
        Color currentColor = Color.clear;
        while (time / timePercent < 1)
		{
            darkPanel.color = Color.Lerp(currentColor, Color.black, time / timePercent);
            time += Time.deltaTime;
            yield return null;
        }
        darkPanel.color = Color.black;
        GameManager.Instance.IsGameOver = false;
        SceneManager.LoadScene("StoryScene");
    }

    public IEnumerator PanelfadeOut()
    {
        PanelActive(true);
        float time = 0;
        float timePercent = 1;
        Color currentColor = Color.black;
        while (time / timePercent < 1)
        {
            darkPanel.color = Color.Lerp(currentColor, Color.clear, time / timePercent);
            time += Time.deltaTime;
            yield return null;
        }
        darkPanel.color = Color.clear;
        PanelActive(false);
        yield return null;

    }
}
