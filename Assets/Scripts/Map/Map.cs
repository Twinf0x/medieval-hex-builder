using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map : MonoBehaviour
{
    Dictionary<Vector2, Tile> tiles;

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

    private List<Vector2> GetGroupPositions(int groupSize)
    {
        List<Vector2> result = new List<Vector2>();
        Vector2 startPosition = Helpers.RandomKeys(tiles).FirstOrDefault();

        return result;
    }
}
