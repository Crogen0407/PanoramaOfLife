using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Managers
    PoolManager _poolManager;
    GameManager _gameManager;

    public BossPattern _bossPattern;

    Vector3 _minPos, _maxPos;
    [SerializeField] private float _speed;
    [SerializeField] private float _shootingDelay = 0.07f;
    private float _h, _v;
    public Vector2 _dir;
    [SerializeField] private float _rushPower;

    [SerializeField] private Transform smokeParticle;
    [SerializeField] private Transform lightGroup;

    float time = 1;

    //Components
    Animator _animator;
    Rigidbody2D _rigid;
    SpriteRenderer _spriteRenderer;
    AudioSource _hitAudioSource;

    public void HalfHp()
	{
        smokeParticle.gameObject.SetActive(true);
    }

    public void Init()
    {
        _poolManager = PoolManager.Instance;
        _gameManager = GameManager.Instance;
        _rigid = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _hitAudioSource = transform.Find("HitAudio").GetComponent<AudioSource>();
        _hitAudioSource.volume = 1 * PlayerPrefs.GetFloat("Sound"); ;
        _minPos = Camera.main.ViewportToWorldPoint(Vector3.zero);
        _maxPos = Camera.main.ViewportToWorldPoint(Vector3.one);
        StartCoroutine(Shooting());

        transform.position = new Vector3(-6, 0);
        
    }

	private void Start()
	{
        _minPos = Camera.main.ViewportToWorldPoint(Vector3.zero);
        _maxPos = Camera.main.ViewportToWorldPoint(Vector3.one);
    }

	void Update()
    {
        
        if (!_gameManager.IsGameOver)
		{
            if(!_gameManager._isFlipY)
			{
                _h = Input.GetAxisRaw("Horizontal");
                _v = Input.GetAxisRaw("Vertical");
            }
            else
			{
                _h = Input.GetAxisRaw("Horizontal") * -1;
                _v = Input.GetAxisRaw("Vertical") * -1;
            }
            
            
            _dir = new Vector3(_h, _v).normalized;
            _rigid.velocity = _dir * _speed;
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, _minPos.x+ 0.767f, _maxPos.x-0.55f), 
                Mathf.Clamp(transform.position.y, _minPos.y+0.35f, _maxPos.y-0.35f));
        }
		else
		{
            _gameManager.CameraShake(10f);
            Falling();     
        }

        if(_gameManager.IsGameOver)
		{
            smokeParticle.up = new Vector3(0, 1);
		}

        _rigid.velocity = _dir * _speed;

    }

    private void Falling()
	{
        lightGroup.gameObject.SetActive(false);
           _dir = Vector3.zero;
        _spriteRenderer.color = Color.black * 0.7f;
        transform.right = new Vector3(1, -0.8f);
        _rigid.gravityScale = 10f;
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
        switch (collision.transform.tag)
        {
            case "EnemyBullet":
                if (_gameManager.islessdamagedMode == false)
                {
                    OnHit(40, 10);
                }
                break;
            case "Enemy":
            case "Laser":
            case "Boss":
                time += Time.deltaTime;
                if (time > 0.5f)
                {
                    OnHit(40, 20);
                    time = 0;
                }
                break;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Laser")
        {
            time += Time.deltaTime;
            if (time > 0.5f)
            {
                OnHit(40, 20);
                time = 0;
            }
        }
    }

	public void OnHit(int input, int hp)
	{
        if(!_gameManager.IsGameOver)
		{
            _animator.SetTrigger("OnHit");
            _gameManager.CameraShake(input);
            _hitAudioSource.Play();
            if (_gameManager.islessdamagedMode == true)
			{
                _gameManager.Hp -= hp / 2;
                return;
            }
            _gameManager.Hp -= hp;
		}
    }

    IEnumerator Shooting()
	{
        while(!_gameManager.IsGameOver)
		{
            if (_gameManager.isPowerUp)
			{
                _poolManager._objectManager.Pop("PlayerBullet_2", transform.position + new Vector3(1, 0.3f));
            }
            else if (!_gameManager.isPowerUp)
            {
                GameObject obj1 = _poolManager._objectManager.Pop("PlayerBullet", transform.position + new Vector3(1, 0f));
                obj1.transform.right = new Vector3(1, 0.1f);
                _poolManager._objectManager.Pop("PlayerBullet", transform.position + new Vector3(1, 0));
                GameObject obj2 = _poolManager._objectManager.Pop("PlayerBullet", transform.position + new Vector3(1, 0f));
                obj2.transform.right = new Vector3(1, -0.1f);

            }
            yield return new WaitForSeconds(_shootingDelay);
            _gameManager.CameraShake(4);
        }
	}
}
