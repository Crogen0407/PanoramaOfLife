using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PoolingPair
{
    public GameObject prefabs;
    public string prefabTypeNames;
    public int poolCounts;
}

[CreateAssetMenu (menuName = "SO/PoolingBase")]
public class PoolingBase : ScriptableObject
{
    public List<PoolingPair> pairs;
}
