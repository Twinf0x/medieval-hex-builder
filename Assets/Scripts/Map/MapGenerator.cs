using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Map Settings")]
    [Header("Map Size")]
    public int xSize = 0;
    public int ySize = 0;
    public int zSize = 0;

    [Header("Ocean")]
    public int oceanOffset = 3;

    [Header("Forests")]
    public int numberOfForests = 3;
    public int maxForestSize = 8;
    public int minForestSize = 4;

    [Header("Mountains")]
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
        CreateOcean();
        Map islandMap = CreateMap(xSize, ySize, zSize, grasslandPrefab);
        AddMapFeatures(islandMap);
    }

    public Map CreateMap(int xSize, int ySize, int zSize, GameObject basePrefab)
    {
        GameObject mapObject = Instantiate(mapPrefab);
        Map map = mapObject.GetComponent<Map>();
        map.Initialize();

        //Create Base
        for(int x = 0; x < xSize; x++)
        {
            for(int y = 0; y < ySize; y++)
            {
                GameObject tileObject = Instantiate(basePrefab);
                tileObject.transform.SetParent(mapObject.transform);
                Tile tile = tileObject.GetComponent<Tile>();
                Vector2 coordinates = new Vector2(x, y);
                map.PlaceTile(coordinates, tile);
            }
        }

        //Create Rows with Z-Offset
        for(int z = 0; z < zSize; z++)
        {
            int xOffset = z;
            int yOffset = -1 * z;

            for(int x = 0; x < xSize; x++)
            {
                GameObject tileObject = Instantiate(basePrefab);
                tileObject.transform.SetParent(mapObject.transform);
                Tile tile = tileObject.GetComponent<Tile>();
                Vector2 coordinates = new Vector2(x + xOffset, yOffset);
                map.PlaceTile(coordinates, tile);
            }

            for(int y = 0; y < ySize; y++)
            {
                GameObject tileObject = Instantiate(basePrefab);
                tileObject.transform.SetParent(mapObject.transform);
                Tile tile = tileObject.GetComponent<Tile>();
                Vector2 coordinates = new Vector2(xSize - 1 + xOffset, y + yOffset);
                map.PlaceTile(coordinates, tile);
            }
        }

        return map;
    }

    public void AddMapFeatures(Map map)
    {
        for(int i = 0; i < numberOfForests; i++)
        {
            map.PlaceTileGroup(minForestSize, maxForestSize, forestPrefab);
        }

        for(int i = 0; i < numberOfMountainranges; i++)
        {
            map.PlaceTileGroup(minMountainrangeSize, maxMountainrangeSize, mountainPrefab);
        }
    }

    public Map CreateOcean()
    {
        Map oceanMap = CreateMap(xSize + oceanOffset, ySize + oceanOffset, zSize + oceanOffset, oceanPrefab);
        oceanMap.transform.Translate(-1 * HexMetrics.xOffset * oceanOffset, -1 * HexMetrics.yOffset * oceanOffset, 0);

        return oceanMap;
    }
}
