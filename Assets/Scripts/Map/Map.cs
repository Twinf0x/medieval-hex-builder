using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map : MonoBehaviour
{
    private Dictionary<Vector2, Tile> tiles;
    private Vector2 invalidPosition = new Vector2(-100, -100);

    public void Initialize()
    {
        tiles = new Dictionary<Vector2, Tile>();
    }

    public Tile GetTile(Vector2 coordinates)
    {
        if(!coordinatesAreValid(coordinates))
        {
            return null;
        }

        return tiles[coordinates];
    }

    private bool coordinatesAreValid(Vector2 coordinates)
    {
        return tiles.ContainsKey(coordinates);
    }

    public void ReplaceTile(Vector2 coordinates, Tile replacement) 
    {
        if(!coordinatesAreValid(coordinates))
        {
            return;
        }

        RemoveTile(coordinates);
        PlaceTile(coordinates, replacement);
    }

    public void RemoveTile(Vector2 coordinates)
    {
        if(!coordinatesAreValid(coordinates))
        {
            return;
        }

        Destroy(tiles[coordinates].gameObject);
        tiles.Remove(coordinates);
    }

    public void PlaceTile(Vector2 coordinates, Tile tile)
    {
        float xPos = (coordinates.x + coordinates.y) * HexMetrics.xOffset;
        float yPos  = (coordinates.x - coordinates.y) * HexMetrics.yOffset;
        tile.transform.position = new Vector3(xPos, yPos, 0f);

        if(tiles.ContainsKey(coordinates))
        {
            RemoveTile(coordinates);
        }
        tiles.Add(coordinates, tile);
        tile.coordinates = coordinates;
    }

    public void PlaceTileGroup(int minTileAmount, int maxTileAmount, GameObject tilePrefab)
    {
        int size = Random.Range(minTileAmount, maxTileAmount + 1);
        List<Vector2> positions = GetGroupPositions(size);

        foreach(Vector2 position in positions)
        {
            GameObject tileObject = Instantiate(tilePrefab);
            Tile tile = tileObject.GetComponent<Tile>();
            PlaceTile(position, tile);
        }
    }

    private List<Vector2> GetGroupPositions(int groupSize, int maxFailures = 5)
    {
        List<Vector2> result = new List<Vector2>();
        System.Random random = new System.Random();
        int failures = 0;

        Vector2 startPosition = Helpers.RandomKeys(tiles).Take(1).FirstOrDefault();
        result.Add(startPosition);
        
        while(result.Count < groupSize)
        {
            Vector2 groupPosition = result[random.Next(result.Count)];
            Vector2 newPosition = GetValidNeighbour(groupPosition);
            if(newPosition != invalidPosition)
            {
                result.Add(newPosition);
            }
            else
            {
                if(++failures > maxFailures)
                {
                    break;
                }
            }
        }

        return result;
    }

    private Vector2 GetValidNeighbour(Vector2 position, int maxTries = 5)
    {
        int failedTries = 0;
        while(failedTries < maxTries)
        {
            int xOffset = Random.Range(-1, 2);
            int yOffset = Random.Range(-1, 2);

            Vector2 neighbour = new Vector2(position.x + xOffset, position.y + yOffset);
            if(IsValidPosition(neighbour))
            {
                return neighbour;
            }

            failedTries++;
        }

        return invalidPosition;
    }

    private bool IsValidPosition(Vector2 position)
    {
        if(!coordinatesAreValid(position))
        {
            return false;
        }

        return tiles[position].type != TileType.Ocean;
    }
}
