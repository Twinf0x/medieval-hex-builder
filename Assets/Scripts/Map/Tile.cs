using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType {Ocean, Grassland, Forest, Mountain, Hand, None}

public class Tile : MonoBehaviour
{
    [Header("Set in Editor")]
    public TileType type;
    public GameObject hoverMarker = null;
    public bool RepresentsHand { get { return type == TileType.Hand; } }

    [Header("Set during play, but visible for Debugging")]
    public Vector2 coordinates;
    public Map map;

    [HideInInspector]
    public Placeable localPlaceable = null;
    [HideInInspector]
    public Building placedBuilding = null;

    public bool IsOccupied
    {
        get
        {
            return placedBuilding != null;
        }
    }

    private void OnMouseEnter()
    {
        hoverMarker.SetActive(true);
    }

    private void OnMouseExit()
    {
        hoverMarker.SetActive(false);
    }

    public void PlacePlaceable(Placeable placeable)
    {
        placeable.transform.position = transform.position;
        placeable.transform.SetParent(transform);

        this.localPlaceable = placeable;
        placeable.SetLocation(this);

        if(!RepresentsHand)
        {
            placeable.ReplaceWithBuilding();
        }
    }

    public void PlaceBuilding(Building building)
    {
        building.transform.position = transform.position;
        building.transform.SetParent(transform);

        this.placedBuilding = building;
        building.PlaceOn(this);
    }

    public void Free()
    {
        this.placedBuilding = null;
    }

    public List<Tile> GetAllTilesAround(int maxDistance)
    {
        return map.GetAllTilesAround(this, maxDistance);
    }
}
