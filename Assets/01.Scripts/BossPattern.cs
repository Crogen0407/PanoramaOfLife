using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossPattern : MonoBehaviour
{
	Vector3 _minMovePos;
	Vector3 _maxMovePos;

	//Sound
	AudioSource _audioSource;

	[SerializeField] private AudioClip rushSound;

	Transform _bossTrm;
	Transform _patternSign;
	[SerializeField] private Transform _SquareCage;
	[SerializeField] private Vector3 maxSquareCageMovePoint; 
	[SerializeField] private Vector3 minSquareCageMovePoint;
	[SerializeField] private Vector3[] trapLaserPos;
	[SerializeField] private GameObject dieEffectGroup;

	//Components
	SpriteRenderer _spriteRenderer;
	Rigidbody2D _rigid;

	//PowerUp
	[SerializeField] private Transform bossPowerUpEffectGroup;
	[SerializeField] private Transform globalLight;
	private bool _isPowerUp;

	//Managers
	PoolManager _poolManager;
	GameManager _gameManager;

	public Vector3[] enemyTrapPos;

	public bool IsPowerUp
	{
		get => _isPowerUp;
		set
		{
			_isPowerUp = value;
			bossPowerUpEffectGroup.gameObject.SetActive(value);
			globalLight.gameObject.SetActive(!value);
		}
	}

	public void Init(Vector2 minVec, Vector2 maxVec, Transform trm)
	{
		globalLight = GameObject.Find("Global Light 2D").transform;	
		_minMovePos = minVec;
		_maxMovePos = maxVec;
		_gameManager = GameManager.Instance;
		_poolManager = PoolManager.Instance;
		_bossTrm = trm;
		_patternSign = GameObject.Find("PatternSign").transform;
		_audioSource = GetComponent<AudioSource>();
		_audioSource.volume = 1 / PlayerPrefs.GetFloat("Sound");
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_rigid = GetComponent<Rigidbody2D>();
	}

	//DefaultPosition으로 이동
	public IEnumerator ReturnDefaultPosition()
	{
		bool isNotDone = true;

		Sequence seq = DOTween.Sequence();
		seq.Append(_bossTrm.DOMove(new Vector3(12.5f, 0), 1));
		seq.Join(_bossTrm.DORotate(new Vector3(0, 0, 90), 1));


		isNotDone = false;
		yield return new WaitWhile(() => isNotDone);
	}

	//돌진
	public IEnumerator Rush()
	{
		_audioSource.clip = rushSound;
		bool isNotDone = true;
		Sequence seq = DOTween.Sequence();

		seq.Append(_bossTrm.DOMove(new Vector3(23, 0), 3));
		_patternSign.gameObject.SetActive(true);
		_patternSign.position = _bossTrm.position;
		_patternSign.localScale = new Vector2(1, 5);
		float ran = Random.Range(_minMovePos.y, _maxMovePos.y);
		Vector3 vec = new Vector3(_minMovePos.x-50, ran);

		seq.AppendCallback(() => _audioSource.Play());
		seq.AppendCallback(() => _bossTrm.up = vec);
		seq.AppendCallback(() => _patternSign.right = vec);

		
		seq.Append(_patternSign.DOScaleX(100, 2.5f));
		seq.AppendCallback(()=> _patternSign.gameObject.SetActive(false));
		seq.AppendCallback(() => GameManager.Instance.CameraShake(60));
		seq.Append(_bossTrm.DOMove(vec, duration: 7));
		isNotDone = false;
		yield return new WaitForSeconds(12);
		GameManager.Instance.CameraShake(60);
		yield return new WaitWhile(() => isNotDone);
	}

	//총알 난사
	public IEnumerator Shooting()
	{
		int count = 0;
		int n = 15;
		while(count < 30)
		{
			for (int i = 0; i < n; i++)
			{
				GameObject obj = _poolManager._objectManager.Pop("EnemyBullet", _bossTrm.position + new Vector3(-7.5f, 0));
				float r = Mathf.PI * 2 * i / n;
				Vector3 dir = new Vector3(Mathf.Cos(r), Mathf.Sin(r)).normalized;

				Rigidbody2D rigid = obj.GetComponent<Rigidbody2D>();
				rigid.AddForce(dir * 10 * Mathf.Cos(r), ForceMode2D.Impulse);
				
			}
			for (int i = 0; i < n; i++)
			{
				GameObject obj = _poolManager._objectManager.Pop("EnemyBullet", _bossTrm.position + new Vector3(-7.5f, 0));
				float r = Mathf.PI * 2 * i / n;
				Vector3 dir = new Vector3(Mathf.Cos(r),
					Mathf.Sin(r)).normalized;
				Rigidbody2D rigid = obj.GetComponent<Rigidbody2D>();
				rigid.AddForce(dir * 2f, ForceMode2D.Impulse);
			}
			yield return new WaitForSeconds(0.2f);
			count++;
		}
		yield return null;
	}

	//레이저 
	public IEnumerator Laser()
	{
		for (int i = 0; i < 5; i++)
		{
			GameObject obj = _poolManager._objectManager.Pop("EnemyTrap", enemyTrapPos[i]);
			obj.GetComponent<EnemyTrap>().Init();
			yield return new WaitForSeconds(1);
		}
		yield return null;
	}

	//사각형 감옥
	public IEnumerator MakeSquareCage()
	{
		_SquareCage.position = _gameManager._player.transform.position;
		_SquareCage.gameObject.SetActive(true);
		yield return new WaitForSeconds(4);
		while (_SquareCage.gameObject.activeSelf == true)
		{
			float ranX = Random.Range(minSquareCageMovePoint.x, maxSquareCageMovePoint.x);
			float ranY = Random.Range(minSquareCageMovePoint.y, maxSquareCageMovePoint.y);
			_SquareCage.GetComponent<SquareCage>()._rigid.DOMove(new Vector3(ranX, ranY), 5);
			yield return new WaitForSeconds(4);
		}
		yield return null;
	}

	//파워업
	public IEnumerator PowerUp()
	{
		IsPowerUp = true;
		int count = 0;
		StartCoroutine(Rush());

		while (count < 20)
		{
			int ran = Random.Range(0, trapLaserPos.Length);
			Vector2 spawnPos = trapLaserPos[ran];
			GameObject trapLaserSign = _poolManager._objectManager.Pop("TrapLaserSign", spawnPos);
			Vector2 attackdir = _gameManager._player.transform.position - trapLaserSign.transform.position;
			trapLaserSign.transform.up = attackdir;


			Vector2 spawnPos2 = trapLaserPos[ran == trapLaserPos.Length-1 ? 0 : ran + 1];
			GameObject trapLaserSign2 = _poolManager._objectManager.Pop("TrapLaserSign", spawnPos2);
			Vector2 attackdir2 = _gameManager._player.transform.position - trapLaserSign2.transform.position;
			trapLaserSign2.transform.up = attackdir2;

			yield return new WaitForSeconds(1);
			_poolManager._objectManager.Push("TrapLaserSign", trapLaserSign);
			_poolManager._objectManager.Push("TrapLaserSign", trapLaserSign2);

			GameObject trapLaser = _poolManager._objectManager.Pop("TrapLaser", spawnPos);
			GameObject trapLaser2 = _poolManager._objectManager.Pop("TrapLaser", spawnPos2);
			_gameManager.CameraShake(40);
			trapLaser.transform.right = attackdir;
			trapLaser2.transform.right = attackdir2;
			count++;
			yield return null;
		}
		IsPowerUp = false;
		yield return null;
	}

	//보스 처치
	public IEnumerator KillBoss()
	{
		UIManager.Instance.PanelfadeIn();

		DOTween.Sequence().Kill();
		DOTween.KillAll();
		Falling();
		yield return StartCoroutine(_gameManager.GameClear());
	}
	private void Falling()
	{

		dieEffectGroup.SetActive(false);
		_spriteRenderer.color = Color.black * 0.7f;
		transform.up = new Vector3(1, -0.8f);
		_rigid.gravityScale = 0.5f;
	}
}