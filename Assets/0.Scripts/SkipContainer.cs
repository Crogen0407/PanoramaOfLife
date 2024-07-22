using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkipContainer : MonoBehaviour
{
	[SerializeField] private Image _fillGauge;
	[SerializeField] private StoryManager _storyManager;

	private float _currentSkipAmount = 0;
	private bool _isFilling;


	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			_isFilling = true;
		if (Input.GetKeyUp(KeyCode.Escape))
			_isFilling = false;


		if(_isFilling)
		{
			_currentSkipAmount += Time.deltaTime;
			_fillGauge.fillAmount = _currentSkipAmount/3;
			if(_currentSkipAmount/3 > 1)
			{
				_storyManager.StorySkip();
				gameObject.SetActive(false);
			}
		}
		else
		{
			_currentSkipAmount = 0;
			_fillGauge.fillAmount = 0;
		}
	}
}
