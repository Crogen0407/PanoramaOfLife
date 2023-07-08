using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    PoolManager poolManager;

    void Start()
    {
        poolManager = PoolManager.Instance;
    }

    public void OffSet()
	{
        poolManager._objectManager.Push(tag,gameObject);
    }
}
