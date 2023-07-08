using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float _speed;
    [SerializeField] private int fireDirection;
    public float damage;

    //Components
    Rigidbody2D _rigid;
    AudioSource _audioSource;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        if (!(tag==("PlayerBullet")||tag == ("PlayerBullet_2")))
		{
            _audioSource = GetComponent<AudioSource>();
            _audioSource.volume = 1 * PlayerPrefs.GetFloat("Sound"); ;
        }

    }

    void OnEnable()
    {
        if(!tag.Contains("PlayerBullet"))
		{
            _audioSource.Play();
		}
        
        Invoke("LifeTime", 2);
    }

	private void OnDisable()
	{
        transform.right = Vector3.zero;
        _rigid.velocity = Vector3.zero;

    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if(transform.tag.Contains("Player"))
		{
            if (collision.CompareTag("Enemy"))
            {
                if(transform.name == "PlayerBullet_2")
				{
                    GameManager.Instance.Hp += 0.5f;
				}
                if(!(collision.name == "Enemy_Trap"))
				{
                    collision.GetComponent<Enemy>().Hp -= damage;
				}   

                PoolManager.Instance._objectManager.Pop("Explosion", transform.position);
                LifeTime();
            }
            else if (collision.CompareTag("Boss"))
			{
                GameManager.Instance.BossHp -= damage;
                PoolManager.Instance._objectManager.Pop("Explosion", transform.position);
                LifeTime();
            }
        }
    }

    void Update()
    {
        transform.Translate(Vector2.right * _speed * fireDirection * Time.deltaTime, Space.Self);
        if (transform.position.x > 10 || transform.position.x < -8)
		{
            LifeTime();
        }

    }

    void LifeTime()
	{
        if(gameObject.name == "PlayerBullet_2")
		{
            PoolManager.Instance._objectManager.Push("PlayerBullet_2", gameObject);
            return;
        }
        else if(gameObject.name == "PlayerPowerBullet" && gameObject.activeInHierarchy == true)
		{
            GameManager.Instance.CameraShake(40);
            PoolManager.Instance._objectManager.Pop("HighExplosion", transform.position);
		}
        PoolManager.Instance._objectManager.Push(gameObject.tag, gameObject);
    }
}
