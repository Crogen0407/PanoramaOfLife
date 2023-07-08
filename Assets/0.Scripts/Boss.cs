using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Boss : MonoBehaviour
{
    BossPattern _bossPattern;

    public Transform _minMovePos;
    public Transform _maxMovePos;

    GameManager _gameManager;
    PoolManager _poolManager;

    Animator _animator;

    public void Init()
    {
        _gameManager = GameManager.Instance;
        _poolManager = PoolManager.Instance;
        _bossPattern = GetComponent<BossPattern>();
        _animator = GetComponent<Animator>();
        _bossPattern.Init(_minMovePos.position, _maxMovePos.position, transform);
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.tag == "PlayerBullet")
		{
            _animator.SetTrigger("OnHit");
        }
        if (collision.tag == "PlayerPowerBullet")
        {
            _animator.SetTrigger("OnHit");
            _gameManager.CameraShake(100);
            _gameManager.GetComponent<AudioSource>().Play();
        }
    }

	private void Update()
	{
		if (_gameManager.IsGameClear)
		{
            StopAllCoroutines();
		}
	}

	private void Start()
	{
        //_gameManager.IsGameClear = true;
        StartCoroutine(Pattern());
    }

    IEnumerator Pattern()
	{
		while (true)
		{
            yield return StartCoroutine(_bossPattern.ReturnDefaultPosition());
            yield return new WaitForSeconds(3);
			yield return StartCoroutine(_bossPattern.Rush());
			yield return StartCoroutine(_bossPattern.ReturnDefaultPosition());
			yield return new WaitForSeconds(1);
			yield return StartCoroutine(_bossPattern.MakeSquareCage());
			yield return new WaitForSeconds(1);
			yield return StartCoroutine(_bossPattern.Shooting());
			yield return new WaitForSeconds(1);
			yield return StartCoroutine(_bossPattern.Laser());
			yield return new WaitForSeconds(6);
			yield return StartCoroutine(_bossPattern.PowerUp());

		}
    }
}