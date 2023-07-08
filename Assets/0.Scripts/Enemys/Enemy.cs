using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Sprite defualtSprite;
    public float _speed;
    [SerializeField] private int enemyCode;
    [SerializeField] private float hp;
    [SerializeField] private bool _isBumb = false;
    [SerializeField] private int selfScore;
    [SerializeField] private GameObject _shockEffect;
    [SerializeField] private float defualtHp;
    
    //Component
    Rigidbody2D _rigid;
    AudioSource _audioSource;
    [SerializeField] private AudioSource _fireAudioSource;
    SpriteRenderer _spriteRenderer;
    PoolManager _poolManager;

    public float Hp
	{
        get => hp;
        set
        {
            hp = value; 
            if(hp<0.1f)
			{
                _audioSource.Play();
                _poolManager._objectManager.Pop("EnemyExplosion", transform.position);
                _poolManager._objectManager.Push("Enemy_" + enemyCode, gameObject);
                hp = defualtHp;
                _spriteRenderer.sprite = defualtSprite;
                if (_isBumb)
				{
                    _shockEffect.SetActive(false);
                    _poolManager._objectManager.Pop("Laser", new Vector3(0,transform.position.y));
				}
                _isBumb = false;
            }
        }
	}

	private void OnEnable()
	{
        hp = defualtHp;
        _poolManager = PoolManager.Instance;
        transform.right = Vector3.right;
        _shockEffect.SetActive(false);
    }

    void Start()
    {
        defualtHp = hp;
        _poolManager = PoolManager.Instance;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();
        _fireAudioSource = GetComponent<AudioSource>();
        _fireAudioSource.volume = 1 * PlayerPrefs.GetFloat("Sound") * 0.25f;
        _audioSource = GameObject.Find("SpawnManager").GetComponent<AudioSource>();
        _audioSource.volume = 1 * PlayerPrefs.GetFloat("Sound");
    }

    public Enemy SetState(int speed, bool isBumb, bool isFire)
    {
        _speed = speed;
        _isBumb = isBumb;
        _shockEffect.SetActive(isBumb);
        if (isFire == true)
        {
            StartCoroutine(Shooting());
        }

        return this;
    }

	void Update()
    {
        _rigid.velocity = -transform.right * _speed;

        if(transform.position.x < -10 || transform.position.x > 10 || transform.position.y > 10 || transform.position.y < -10)
		{
            _poolManager._objectManager.Push("Enemy_"+ enemyCode, gameObject);
		}
    }

    IEnumerator Shooting()
	{
        while(gameObject.activeSelf == true)
		{
            _poolManager._objectManager.Pop("DisturbBullet", transform.position);
            _fireAudioSource.Play();
            yield return new WaitForSeconds(2);
        }
    }
}