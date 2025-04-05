using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpawnManager : MonoBehaviour
{
    public float _spawnDelay;
    [SerializeField] private Transform _enemySpawnPosMax;
    [SerializeField] private Transform _enemySpawnPosMin;
    ObjectManager _objectManager;
	GameManager _gameManager;

	public void Init()
	{
		_objectManager = PoolManager.Instance._objectManager;
		_gameManager = GameManager.Instance;
		StartCoroutine(SpawnEnmey());

	}
	IEnumerator SpawnEnmey()
	{
		yield return new WaitForSeconds(2f);

		while (!_gameManager.IsGameOver)
		{
			int ran = Random.Range(0, 11) + 1;
			yield return StartCoroutine("Pattern" + ran);
			yield return new WaitForSeconds(0.5f);
		}
	}

	IEnumerator Pattern1()
	{
		yield return new WaitForSeconds(0.5f);

		for (float i = _enemySpawnPosMax.position.y; i > _enemySpawnPosMin.position.y; i-= 2f)
		{
			GameObject obj = _objectManager.Pop("Enemy_3", new Vector2(_enemySpawnPosMax.position.x, i));
			obj.GetComponent<Enemy>().SetState(speed:8,isBumb: false, isFire:true);
			yield return new WaitForSeconds(0.5f);
		}
		yield return null;
    }

	IEnumerator Pattern2()
	{
		yield return new WaitForSeconds(0.5f);

		int count = 0;

		while (count < 4)
		{
			Vector3 vec = new Vector3();
			switch (count)
			{
				case 0 :
					vec = _enemySpawnPosMax.position;
					break;
				case 1 :
					vec = new Vector3(_enemySpawnPosMax.position.x, _enemySpawnPosMin.position.y);
					break;
				case 2:
					vec = _enemySpawnPosMin.position;
					break;
				case 3:
					vec = new Vector3(_enemySpawnPosMin.position.x, _enemySpawnPosMax.position.y);
					break;
			}
			GameObject obj = _objectManager.Pop("Enemy_3", vec);
			obj.transform.right = -(Vector3.zero - obj.transform.position);
			yield return new WaitForSeconds(0.5f);
			Enemy enemy = obj.GetComponent<Enemy>().SetState(speed: 0, isBumb: false, isFire: false);
			enemy.transform.DOMove(enemy.transform.position - (vec.y > 0 ? new Vector3(0, 1) : new Vector3(0, -1)), 2);
			enemy.SetState(speed: 20, isBumb: false, isFire: false);
			count++;
		}
		DOTween.KillAll();
		yield return null;
	}

	IEnumerator Pattern3()
	{
		Vector2 vec = new Vector2(_enemySpawnPosMax.position.x+1, Random.Range(_enemySpawnPosMax.position.y-1, _enemySpawnPosMin.position.y + 1));

		GameObject obj = _objectManager.Pop("Enemy_1", vec);
		Enemy enemy = obj.GetComponent<Enemy>().SetState(speed: 5, isBumb: false, isFire: true);

		yield return new WaitForSeconds(0.5f);
		GameObject obj2 = _objectManager.Pop("Enemy_1", vec+Vector2.up);
		Enemy enemy2 = obj2.GetComponent<Enemy>().SetState(speed: 5, isBumb: false, isFire: false);

		GameObject obj3 = _objectManager.Pop("Enemy_1", vec + Vector2.down);
		Enemy enemy3 = obj3.GetComponent<Enemy>().SetState(speed: 5, isBumb: false, isFire: false);
	}

	IEnumerator Pattern4()
	{
		Vector2 vec = new Vector2(_enemySpawnPosMax.position.x + 1, Random.Range(_enemySpawnPosMax.position.y - 1, _enemySpawnPosMin.position.y + 1));
		GameObject obj = _objectManager.Pop("Enemy_2", vec+Vector2.up);
		obj.GetComponent<Enemy>().SetState(speed: 4, isBumb: false, isFire: false);
		GameObject obj1 = _objectManager.Pop("Enemy_2", vec + Vector2.down);
		obj1.GetComponent<Enemy>().SetState(speed: 4, isBumb: false, isFire: false);

		yield return new WaitForSeconds(1.5f);
		GameObject obj3 = _objectManager.Pop("Enemy_4", vec);
		obj3.GetComponent<Enemy>().SetState(speed: 4, isBumb: false, isFire: false);
		yield return null;
	}

	IEnumerator Pattern5()
	{
		Vector2 vec = new Vector2(_enemySpawnPosMax.position.x + 1, Random.Range(_enemySpawnPosMax.position.y - 1, _enemySpawnPosMin.position.y + 1));
		GameObject obj3 = _objectManager.Pop("Enemy_2", vec);
		obj3.GetComponent<Enemy>().SetState(speed: 10, isBumb: false, isFire: false);

		yield return new WaitForSeconds(0.3f);
		GameObject obj = _objectManager.Pop("Enemy_3", vec + Vector2.up * 0.4f);
		obj.GetComponent<Enemy>().SetState(speed: 10, isBumb: false, isFire: false);
		GameObject obj1 = _objectManager.Pop("Enemy_3", vec + Vector2.down * 0.4f);
		obj1.GetComponent<Enemy>().SetState(speed: 10, isBumb: false, isFire: false);
		
		yield return null;
	}

	IEnumerator Pattern6()
	{
		for (int i = 0; i < 2; i++)
		{
			Vector3 vec = _enemySpawnPosMax.position + new Vector3(1, -1);
			for (int j = 0; j < 5; j++)
			{
				GameObject obj = _objectManager.Pop("Enemy_3", vec + new Vector3(0, -j));
				obj.GetComponent<Enemy>().SetState(speed: 10, isBumb: false, isFire: false);
			}
			yield return new WaitForSeconds(0.8f);

			vec = new Vector3(_enemySpawnPosMax.position.x, _enemySpawnPosMin.position.y) + Vector3.one;
			for (int j = 0; j < 5; j++)
			{
				GameObject obj = _objectManager.Pop("Enemy_3", vec + new Vector3(0, j));
				obj.GetComponent<Enemy>().SetState(speed: 10, isBumb: false, isFire: false);
			}
			yield return new WaitForSeconds(0.8f);
		}
		yield return null;
	}

	IEnumerator Pattern7()
	{
		Vector2 vec = new Vector2(_enemySpawnPosMax.position.x + 1, Random.Range(_enemySpawnPosMax.position.y - 1, _enemySpawnPosMin.position.y + 1));
		GameObject obj1 = _objectManager.Pop("Enemy_2", vec);
		obj1.GetComponent<Enemy>().SetState(speed: 6, isBumb: false, isFire: false);
		yield return new WaitForSeconds(0.11f);
		GameObject obj2 = _objectManager.Pop("Enemy_1", vec + Vector2.up * 2f);
		obj2.GetComponent<Enemy>().SetState(speed: 6, isBumb: true, isFire: true);
		GameObject obj3 = _objectManager.Pop("Enemy_1", vec + Vector2.down * 2);
		obj3.GetComponent<Enemy>().SetState(speed: 6, isBumb: true, isFire: true);
		yield return new WaitForSeconds(0.11f);

		GameObject obj4 = _objectManager.Pop("Enemy_2", vec);
		obj4.GetComponent<Enemy>().SetState(speed: 6, isBumb: true, isFire: false);

		yield return null;
	}

	IEnumerator Pattern8()
	{
		for (float i = _enemySpawnPosMax.position.x; i > _enemySpawnPosMin.position.x; i -= 2f)
		{
			GameObject obj = _objectManager.Pop("Enemy_3", new Vector2(i, _enemySpawnPosMin.position.y));
			obj.GetComponent<Enemy>().SetState(speed: 8, isBumb: true, isFire: false);
			obj.transform.right = Vector3.down;
			yield return new WaitForSeconds(0.5f);
		}

		yield return null;
	}

	IEnumerator Pattern9()
	{
		yield return new WaitForSeconds(0.5f);

		for (float i = _enemySpawnPosMin.position.y; i < _enemySpawnPosMax.position.y; i += 2f)
		{
			GameObject obj = _objectManager.Pop("Enemy_3", new Vector2(_enemySpawnPosMax.position.x, i));
			obj.GetComponent<Enemy>().SetState(speed: 8, isBumb: false, isFire: true);
			yield return new WaitForSeconds(0.5f);
		}
		yield return null;
	}

	IEnumerator Pattern10()
	{
		for (float i = _enemySpawnPosMax.position.x; i > _enemySpawnPosMin.position.x; i -= 2f)
		{
			GameObject obj = _objectManager.Pop("Enemy_3", new Vector2(i, _enemySpawnPosMax.position.y));
			obj.GetComponent<Enemy>().SetState(speed: 8, isBumb: true, isFire: false);
			obj.transform.right = Vector3.up;
			yield return new WaitForSeconds(0.5f);
		}

		yield return null;
	}

	IEnumerator Pattern11()
	{
		yield return new WaitForSeconds(0.5f);
		GameObject obj = _objectManager.Pop("Enemy_1", new Vector2(_enemySpawnPosMax.position.x, 0));
		obj.GetComponent<Enemy>().SetState(speed: 5, isBumb: false, isFire: true);
		yield return new WaitForSeconds(0.5f);

		obj = _objectManager.Pop("Enemy_1", new Vector2(_enemySpawnPosMax.position.x, -1.5f));
		obj.GetComponent<Enemy>().SetState(speed: 5, isBumb: false, isFire: true);
		obj = _objectManager.Pop("Enemy_1", new Vector2(_enemySpawnPosMax.position.x, 1.5f));
		obj.GetComponent<Enemy>().SetState(speed: 5, isBumb: false, isFire: true);
		yield return new WaitForSeconds(0.5f);

		obj = _objectManager.Pop("Enemy_1", new Vector2(_enemySpawnPosMax.position.x, -3));
		obj.GetComponent<Enemy>().SetState(speed: 5, isBumb: false, isFire: true);
		obj = _objectManager.Pop("Enemy_1", new Vector2(_enemySpawnPosMax.position.x, 3));
		obj.GetComponent<Enemy>().SetState(speed: 5, isBumb: false, isFire: true);
		yield return null;
	}
}
