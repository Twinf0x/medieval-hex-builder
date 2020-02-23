using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementController : MonoBehaviour
{
    public static PlacementController instance;

    private Placeable selectedPlaceable = null;
    private Tile pickedUpFromTile = null;
    private float lastPickUpTime = 0;
    private float dragThreshold = 0.3f;

    private void Awake() 
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Update()
    {
        CheckMouseActivity();
    }

    private void CheckMouseActivity()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UpdateSelection();
        }

        if (Input.GetMouseButtonUp(0) && selectedPlaceable != null && (Time.time - lastPickUpTime) > dragThreshold)
        {
            TryPlacingSelection();
        }
    }

    private void UpdateSelection()
    {
        var tempPlaceable = GetPlaceableFromMousePosition();
        if(tempPlaceable == null)
        {
            return;
        }
        var tempTile = GetTileFromPlaceablePosition(tempPlaceable);

        if (selectedPlaceable == null)
        {
            SelectPlaceable(tempPlaceable, tempTile);
        }
        else
        {
            if(tempTile == null)
            {
                selectedPlaceable.Reset();
                SelectPlaceable(tempPlaceable, tempTile);
            }
        }
    }

    private Placeable GetPlaceableFromMousePosition()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var buildingsUnderMouse = Physics2D.OverlapAreaAll(mousePosition, mousePosition, LayerMask.GetMask("Building"));

        foreach(var buildingCollider in buildingsUnderMouse)
        {
            var placeable = buildingCollider.gameObject.GetComponent<Placeable>();
            if(placeable != null)
            {
                return placeable;
            }
        }

        return null;
    }

    private void SelectPlaceable(Placeable placeable, Tile tile)
    {
        selectedPlaceable = placeable;
        pickedUpFromTile = tile;
        selectedPlaceable.PickUp();
        lastPickUpTime = Time.time;
    }

    private void TryPlacingSelection()
    {
        if(selectedPlaceable == null)
        {
            return;
        }

        var hoverTile = GetTileFromMousePosition();
        if(hoverTile == null)
        {
            return;
        }

        if(selectedPlaceable.MeetsPlacementRestrictions(hoverTile))
        {
            PlaceBuildingOnTile(selectedPlaceable, hoverTile);
        }
    }

    public bool PlaceBuildingOnTile(Placeable building, Tile tile)
    {
        pickedUpFromTile?.Free();
        pickedUpFromTile = null;
        
        selectedPlaceable = tile.localPlaceable;
        if(selectedPlaceable != null)
        {
            selectedPlaceable.PickUp();
            selectedPlaceable.isInHand = true;
        }

        Building buildingOnTile = tile.placedBuilding;
        if(buildingOnTile != null)
        {
            Treasury.instance.allPlacedBuildings.Remove(buildingOnTile);
            tile.placedBuilding = null;
            Destroy(buildingOnTile.gameObject);
        }
        
        building.Place();
        tile.PlacePlaceable(building);
        Hand.instance.RemovePlaceable(building);
        building.isInHand = tile.RepresentsHand;
        return true;
    }

    private Tile GetTileFromMousePosition()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return GetTileFromWorldPosition(mousePosition);
    }

    private Tile GetTileFromPlaceablePosition(Placeable placeable)
    {
        var pos = placeable.transform.position;
        return GetTileFromWorldPosition(pos);
    }

    private Tile GetTileFromWorldPosition(Vector3 position)
    {
        var tilesAtPosition = Physics2D.OverlapAreaAll(position, position, LayerMask.GetMask("Floor"));

        foreach (var tileCollider in tilesAtPosition)
        {
            var tile = tileCollider.gameObject.GetComponent<Tile>();
            if (tile != null)
            {
                return tile;
            }
        }

        return null;
    }
}
