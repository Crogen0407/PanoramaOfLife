using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class LightControl : MonoBehaviour
{
    [SerializeField] private float _minIntensity;
    [SerializeField] private float _maxIntensity;
    [SerializeField] private float _delayTime;

    private Light2D _light;

    void Awake()
    {
        _light = GetComponent<Light2D>();
        StartCoroutine(DelayTime());
    }

    IEnumerator DelayTime()
	{
        while(true)
		{
            float ran = Random.Range(_minIntensity, _maxIntensity);
            _light.intensity = ran;
            yield return new WaitForSeconds(_delayTime);
		}
	}
}
