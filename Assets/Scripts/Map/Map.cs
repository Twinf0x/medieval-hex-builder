using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private int xSize;
    private int ySize;
    private Tile[,] tiles;

    public void SetSize(int x, int y)
    {
        xSize = x;
        ySize = y;
        tiles = new Tile[x, y];
    }

    public Tile GetTile(int x, int y)
    {
        if(!coordinatesAreValid(x, y))
        {
            return null;
        }

        return tiles[x, y];
    }

    private bool coordinatesAreValid(int x, int y)
    {
        if(x >= tiles.GetLength(0))
        {
            Debug.LogWarning($"Trying to access x-index \"{x}\" of the map, but it is out of bounds.");
            return false;
        }

        if(y >= tiles.GetLength(1))
        {
            Debug.LogWarning($"Trying to access y-index \"{y}\" of the map, but it is out of bounds.");
            return false;
        }

        return true;
    }

    public void ReplaceTile(int x, int y, Tile replacement) 
    {
        if(!coordinatesAreValid(x, y))
        {
            return;
        }

        RemoveTile(x, y);
        PlaceTile(x, y, replacement);
    }

    public void RemoveTile(int x, int y)
    {
        if(!coordinatesAreValid(x, y))
        {
            return;
        }

        Destroy(tiles[x, y].gameObject);
        tiles[x, y] = null;
    }

    public void PlaceTile(int x, int y, Tile tile)
    {
        if(!coordinatesAreValid(x, y))
        {
            return;
        }

        float xPos = (x + y) * HexMetrics.xOffset;
        float yPos  = (x - y) * HexMetrics.yOffset;
        tile.transform.position = new Vector3(xPos, yPos, 0f);

        tiles[x, y] = tile;
        tile.coordinates = new Vector2(x, y);
    }

    public void PlaceTileGroup(int minTileAmount, int maxTileAmount, GameObject tilePrefab)
    {
        int size = Random.Range(minTileAmount, maxTileAmount + 1);
        List<Vector2> positions = GetGroupPositions(size);

        foreach(Vector2 position in positions)
        {
            GameObject tileObject = Instantiate(tilePrefab);
            Tile tile = tileObject.GetComponent<Tile>();
            ReplaceTile(position.x, position.y, tile);
        }
    }

    private List<Vector2> GetGroupPositions(int groupSize)
    {

    }
}
