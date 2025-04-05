using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample : MonoBehaviour
{
    public GameObject[] samples;
    public int count = 0;
    float startDelayTime = 0;
    public GameObject text;

    void Update()
    {
        startDelayTime += Time.deltaTime;
        if (startDelayTime > 0.5f && (Input.GetMouseButtonDown(0) || Input.anyKeyDown))
		{
            if(count >= samples.Length)
			{
                text.SetActive(false);
                gameObject.SetActive(false);
            }
            for (int i = 0; i < samples.Length; i++)
			{
                if (i == count)
                {
                    samples[i].SetActive(true);
                    continue;
                }
                samples[i].SetActive(false);
            }
            count++;
            startDelayTime = 0;
        }
    }
}
