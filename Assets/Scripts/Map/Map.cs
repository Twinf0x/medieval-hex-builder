using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Tile[,] tiles;

    private bool coordinatesAreValid(int x, int y, int z)
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

        float xPos = (x + y) * (HexMetrics.width * 1.5f);
        float yPos  = (x * HexMetrics.height) - (y * HexMetrics.height);
        tiles[x, y] = tile;
    }
}
