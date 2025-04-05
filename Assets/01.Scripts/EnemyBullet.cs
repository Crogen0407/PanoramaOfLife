using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int fireDirection;

    //Components
    Rigidbody2D _rigid;

    private bool _isFirstCreate = true;

    GameManager _gameManager;
    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        if (_isFirstCreate == true)
        {
            _isFirstCreate = false;
        }
        else if(_isFirstCreate == false)
		{
            StartCoroutine(LifeTime());
		}
        _rigid.AddForce(_speed * Vector2.right * fireDirection, ForceMode2D.Impulse);
        _gameManager = GameManager.Instance;
    }

	private void OnDisable()
	{
        StopCoroutine("LifeTime");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(GameManager.Instance.islessdamagedMode == true)
			{
                Transform trm = PoolManager.Instance._objectManager.Pop("PlayerBullet", transform.position).transform;
                trm.right = transform.position;
            }
            else if (gameObject.tag == "DisturbBullet")
            {
                GameManager.Instance.FlipScreen();
                LifeTime();
            }
        }
    }

    void Update()
    {
        if (transform.position.x > 10 || transform.position.x < -10)
        {
            LifeTime();
        }

        if (_gameManager.IsGameClear == true)
        {
            PoolManager.Instance._objectManager.Push("EnemyTrap", gameObject);
        }
    }

    IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(2);
        PoolManager.Instance._objectManager.Push(tag, gameObject);
    }
}
