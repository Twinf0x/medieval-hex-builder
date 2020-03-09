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

    [Header("Camera Restrictions")]
    public float paddingAroundIsland = 2f;
    [Header("Animation")]
    public float animationDuration = 2f;
    public float dropDuration = 1.5f;

    [Header("Tile Prefabs")]
    public GameObject grasslandPrefab;
    public GameObject forestPrefab;
    public GameObject mountainPrefab;
    public GameObject oceanPrefab;

    [Header("Map Prefab")]
    public GameObject mapPrefab;

    [HideInInspector]
    public Vector2 viewPortOffset;

    private void Start() 
    {
        viewPortOffset = CalculateViewportOffset(ySize, paddingAroundIsland);
        Vector2 mapSize = CalculateMapSizeForCamera(xSize, ySize, zSize, paddingAroundIsland);
        CameraController.instance.SetMapSize(mapSize);
        CameraController.instance.CenterCamera(viewPortOffset);

        Map oceanMap = CreateOcean();
        oceanMap.transform.Translate(viewPortOffset.x, viewPortOffset.y, 0);

        Map islandMap = CreateMap(xSize, ySize, zSize, grasslandPrefab);
        AddMapFeatures(islandMap);
        StartCoroutine(AnimateMapCreation(islandMap, animationDuration));
        islandMap.transform.Translate(viewPortOffset.x, viewPortOffset.y, 0);
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

    private void AddMapFeatures(Map map)
    {
        for(int i = 0; i < numberOfMountainranges; i++)
        {
            map.PlaceTileGroup(minMountainrangeSize, maxMountainrangeSize, mountainPrefab);
        }

        for(int i = 0; i < numberOfForests; i++)
        {
            map.PlaceTileGroup(minForestSize, maxForestSize, forestPrefab);
        }
    }

    private IEnumerator AnimateMapCreation(Map map, float duration) 
    {
        float yOffset = 10f;
        float timeBetweenTiles = duration / map.tiles.Count;

        foreach(var tile in map.tiles.Values)
        {
            Vector3 tempPos = tile.transform.localPosition;
            tile.transform.Translate(0f, yOffset, 0f);
        }

        foreach(var tile in map.tiles.Values)
        {
            StartCoroutine(SimpleAnimations.instance.Translate(tile.transform, dropDuration, new Vector3(0f, -1 * yOffset, 0f)));
            yield return new WaitForSeconds(timeBetweenTiles);
        }
    }

    public Map CreateOcean()
    {
        Map oceanMap = CreateMap(xSize + oceanOffset, ySize + oceanOffset, zSize + oceanOffset, oceanPrefab);
        oceanMap.transform.Translate(-1 * HexMetrics.xOffset * oceanOffset, -1 * HexMetrics.yOffset * oceanOffset, 0);

        return oceanMap;
    }

    private Vector2 CalculateMapSizeForCamera(int x, int y, int z, float padding)
    {
        float width = (x + y) * HexMetrics.xOffset;
        width += HexMetrics.totalWidthInUnits;
        width+= padding;
        float height = (2 * z) * HexMetrics.yOffset * 2f;
        height += HexMetrics.surfaceHeightInUnits;
        height += padding;
        
        return new Vector2(width, height);
    }

    private Vector2 CalculateViewportOffset(int y, float padding)
    {
        float xOffset = HexMetrics.xOffset + padding;
        float yOffset = (y * HexMetrics.yOffset) + padding;

        return new Vector2(xOffset, yOffset);
    }
}
