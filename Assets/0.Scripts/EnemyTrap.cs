using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyTrap : MonoBehaviour
{
	[SerializeField] private Transform _laser;
	GameManager _gameManager;

	public void Init()
    {
		_gameManager = GameManager.Instance;
		Sequence seq = DOTween.Sequence();

		seq.Append(transform.DOMoveY(4, 0.5f));
		seq.AppendCallback(() => _laser.gameObject.SetActive(true));
		seq.Append(_laser.DOScaleY(0, 0f));
		seq.Append(_laser.DOScaleY(1, 0.2f));
		seq.AppendInterval(5);
		seq.Append(_laser.DOScaleX(0, 0.5f));
		seq.AppendCallback(() => _laser.gameObject.SetActive(false));
		seq.AppendInterval(1);
		seq.Append(transform.DOMoveY(-9, 0.5f));
		seq.AppendCallback(() => PoolManager.Instance._objectManager.Push("EnemyTrap", gameObject));
	}

	private void Update()
	{
		if(_gameManager.IsGameClear == true)
		{
			PoolManager.Instance._objectManager.Push("EnemyTrap", gameObject);
		}
	}
}
