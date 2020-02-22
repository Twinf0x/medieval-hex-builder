using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Map Settings")]
    public int xSize = 0;
    public int ySize = 0;
    public int numberOfForests = 3;
    public int maxForestSize = 8;
    public int minForestSize = 4;
    public int numberOfMountainranges = 3;
    public int maxMountainrangeSize = 8;
    public int minMountainrangeSize = 4;

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
        GameObject mapObject = Instantiate(mapPrefab);
        Map map = mapObject.GetComponent<Map>();
        map.SetSize(xSize, ySize);

        for(int x = 0; x < xSize; x++)
        {
            for(int y = 0; y < ySize; y++)
            {
                GameObject tileObject = Instantiate(oceanPrefab);
                tileObject.transform.SetParent(mapObject.transform);
                Tile tile = tileObject.GetComponent<Tile>();
                map.PlaceTile(x, y, tile);
            }
        }

        for(int i = 0; i < numberOfForests; i++)
        {
            map.PlaceTileGroup(minForestSize, maxForestSize, forestPrefab);
        }

        for(int i = 0; i < numberOfMountainranges; i++)
        {
            map.PlaceTileGroup(minMountainrangeSize, maxMountainrangeSize, mountainPrefab);
        }

        return map;
    }
}
