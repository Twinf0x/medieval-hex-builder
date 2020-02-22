using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType {Ocean, Grassland, Forest, Mountain, Hand}

public class Tile : MonoBehaviour
{
    [Header("Set in Editor")]
    public TileType type;
    public GameObject hoverMarker = null;
    public bool RepresentsHand { get { return type == TileType.Hand; } }

    [Header("Set during play")]
    public Vector2 coordinates;

    [HideInInspector]
    public Placeable placedBuilding = null;

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

    public void PlaceBuilding(Placeable building)
    {
        this.placedBuilding = building;
        building.transform.position = transform.position;
        building.transform.SetParent(transform);
    }

    public void Free()
    {
        this.placedBuilding = null;
    }
}
