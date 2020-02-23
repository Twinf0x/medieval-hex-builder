using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PoolItem 
{
    public GameObject prefab;
    public Vector2 amount;
}

[CreateAssetMenu(fileName = "BuldingPool", menuName = "Custom/BuildingPool")]
public class Pool : ScriptableObject
{
    public List<PoolItem> items;
}
