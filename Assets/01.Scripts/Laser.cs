using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Laser : MonoBehaviour
{
	AudioSource audioSource;

	bool isSucceedAttack = false;
	GameManager _gameManager;
	private void Awake()
	{
		transform.DOScale(new Vector3(0, 1, 1),0);
		audioSource = GetComponent<AudioSource>();
		audioSource.volume = 1 *PlayerPrefs.GetFloat("Sound");
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(isSucceedAttack == true && collision.tag == "Player")
		{
			GameManager.Instance._player.OnHit(60, 15);
		}
	}

	private void OnEnable()
	{
		transform.DOScale(new Vector3(1, 1, 1), 0.5f);
		isSucceedAttack = true;
		audioSource.Play();

		StartCoroutine(DestoryDelay());
		_gameManager = GameManager.Instance;
	}

	private void Update()
	{
		if (_gameManager.IsGameClear == true)
		{
			PoolManager.Instance._objectManager.Push(tag, gameObject);
		}
	}

	IEnumerator DestoryDelay()
	{
		yield return new WaitForSeconds(0.15f);
		isSucceedAttack = false;
		yield return new WaitForSeconds(0.6f);
		if(tag == "TrapLaser")
		{
			yield return new WaitForSeconds(0.1f);
		}
		else
		{
			yield return new WaitForSeconds(2f);
		}
		transform.DOScale(new Vector3(1, 0, 1), 0.5f);
		yield return new WaitForSeconds(0.5f);
		PoolManager.Instance._objectManager.Push(tag, gameObject);
	}
}
