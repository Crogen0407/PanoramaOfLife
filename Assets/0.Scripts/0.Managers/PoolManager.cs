using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    //Prefabs
    public PoolingBase poolingBase;

    public ObjectManager _objectManager;

    public void Init()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        MakeObj();
    }

    void MakeObj()
    {
        PoolingPair[] poolingPairs = poolingBase.pairs.ToArray();
        _objectManager = new ObjectManager(poolingPairs);
		for (int i = 0; i < poolingPairs.Length; i++)
		{

            for (int j = 0; j < poolingPairs[i].poolCounts; j++)
			{
                GameObject poolObject = Instantiate(poolingPairs[i].prefabs, Vector3.zero, Quaternion.identity);
                poolObject.name = poolObject.name.Replace("(Clone)","");
                _objectManager.Push(poolingPairs[i].prefabTypeNames, poolObject);
			}
        }
    }

}
