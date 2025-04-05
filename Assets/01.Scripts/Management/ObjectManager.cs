using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager
{
	Dictionary<string, Queue<GameObject>> poolDic = new Dictionary<string, Queue<GameObject>>();

	public ObjectManager(PoolingPair[] poolingPair)
	{
		for (int i = 0; i < poolingPair.Length; i++)
		{
			poolDic.Add(poolingPair[i].prefabTypeNames, new Queue<GameObject>());
		}
	}

	public void Push(string type, GameObject gameObject)
	{
		gameObject.transform.SetParent(null);
		gameObject.SetActive(false);
		poolDic[type].Enqueue(gameObject);
	}

	public GameObject Pop(string type, Vector2 vec)
	{
		GameObject obj = poolDic[type].Dequeue();
		obj.SetActive(true);
		obj.transform.position = vec;

		return obj;
	}

	public GameObject Pop(string type, Vector2 vec, Quaternion rot)
	{
		GameObject obj = poolDic[type].Dequeue();
		obj.SetActive(true);
		obj.transform.position = vec;
		obj.transform.rotation = rot;
		return obj;
	}

	public GameObject Pop(string type, Transform parentTrm)
	{
		GameObject obj = poolDic[type].Dequeue();
		obj.SetActive(true);
		obj.transform.SetParent(parentTrm);
		obj.transform.position = parentTrm.position;

		return obj;
	}
}
