using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Map Settings")]
    public int xSize = 0;
    public int ySize = 0;

    [Header("Tile Prefabs")]
    public GameObject grasslandPrefab;
    public GameObject forestPrefab;
    public GameObject mountainPrefab;
    public GameObject oceanPrefab;

    [Header("Map Prefab")]
    public GameObject mapPrefab;

    private void Start() 
    {
        CreateMap(xSize, ySize);
    }

    public Map CreateMap(int xSize, int ySize)
    {
        var mapObject = Instantiate(mapPrefab);
        var map = mapObject.GetComponent<Map>();
        map.SetSize(xSize, ySize);

        for(int x = 0; x < xSize; x++)
        {
            for(int y = 0; y < ySize; y++)
            {
                var tileObject = Instantiate(oceanPrefab);
                tileObject.transform.SetParent(mapObject.transform);
                var tile = tileObject.GetComponent<Tile>();
                map.PlaceTile(x, y, tile);
            }
        }

        return map;
    }
}
