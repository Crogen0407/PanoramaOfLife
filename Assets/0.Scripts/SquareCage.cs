using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SquareCage : MonoBehaviour
{
	[SerializeField] private int direction;
	public Rigidbody2D _rigid;
	PolygonCollider2D _collider;
	float defaultScale;
	private void Awake()
	{
		defaultScale = transform.localScale.x;
		_rigid = GetComponent<Rigidbody2D>();
		_collider = GetComponent<PolygonCollider2D>();
	}

	private void OnEnable()
	{
		_collider.enabled = false;
		transform.localScale = Vector3.zero;
		Sequence seq = DOTween.Sequence();

		seq.Append(transform.DOScale(8, 0.2f));
		seq.AppendCallback(()=>
		{ 
			StartCoroutine(Scaler());
			Invoke("LifeTime", 15);
			_collider.enabled = true;
		});
	}

	private void LifeTime()
	{
		gameObject.SetActive(false);
		StopCoroutine("LifeTime");
	}

	IEnumerator Scaler()
	{
		while(gameObject.activeSelf == true)
		{
			Sequence seq = DOTween.Sequence();
			seq.Append(transform.DOScale(defaultScale-1, 0.5f));
			seq.Append(transform.DOScale(defaultScale, 0.5f));
			seq.Join(transform.DORotate(new Vector3(0, 0, direction * 180f), 5, RotateMode.FastBeyond360));
			seq.Join(transform.DORotate(new Vector3(0, 0, direction * 360), 5, RotateMode.FastBeyond360));
			yield return new WaitForSeconds(1);
		}
	}
}
