using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject hoverMarker = null;

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
    }

    public void Free()
    {
        this.placedBuilding = null;
    }
}
