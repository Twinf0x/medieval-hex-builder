using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlacementController : MonoBehaviour
{
    public static PlacementController instance;

    public Vector3 selectedCardOffset;

    private Card selectedCard = null;
    private bool justPickedUpACard = false;

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
        if(selectedCard != null)
        {
            PlaceCardNextToCursor(selectedCard);
        }
    }

    private void CheckMouseActivity()
    {
        if(justPickedUpACard)
        {
            justPickedUpACard = false;
            return;
        }

        if (Input.GetMouseButtonUp(0) && selectedCard != null)
        {
            if(!Helpers.IsMouseOverUI())
            {
                TryPlacingSelection();
            }
            else
            {
                PutCardBack();
            }
        }
    }

    private void TryPlacingSelection()
    {
        if(selectedCard == null)
        {
            return;
        }

        var hoverTile = GetTileFromMousePosition();
        if(hoverTile == null)
        {
            return;
        }

        if(selectedCard.MeetsPlacementRestriction(hoverTile))
        {
            GameObject buildingObj = Instantiate(selectedCard.buildingPrefab);
            Building building = buildingObj.GetComponent<Building>(); 
            PlaceBuildingOnTile(building, hoverTile);
            Destroy(selectedCard.gameObject);
            selectedCard = null;
        }
    }

    public void PlaceBuildingOnTile(Building building, Tile tile)
    {
        Building buildingOnTile = tile.placedBuilding;
        if(buildingOnTile != null)
        {
            Treasury.instance.allPlacedBuildings.Remove(buildingOnTile);
            tile.placedBuilding = null;
            Destroy(buildingOnTile.gameObject);
        }
        
        tile.PlaceBuilding(building);
        AudioManager.instance?.Play("Build");

        foreach(Building tempBuilding in Treasury.instance.allPlacedBuildings)
        {
            tempBuilding.HideChangeIndicator();
        }
    }

    public void PickCardUp(Card card)
    {
        if(selectedCard != null)
        {
            PutCardBack();
        }
        
        Hand.instance.RemoveCard(card);
        selectedCard = card;
        card.ShowProductionPreview();
        justPickedUpACard = true;
    }

    private Tile GetTileFromMousePosition()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return GetTileFromWorldPosition(mousePosition);
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

    private void PutCardBack()
    {
        Hand.instance.AddCard(selectedCard);
        selectedCard.HideProductionPreview();
        selectedCard = null;

        foreach(Building tempBuilding in Treasury.instance.allPlacedBuildings)
        {
            tempBuilding.HideChangeIndicator();
        }
    }

    public void PlaceCardNextToCursor(Card card)
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 newPosition = mousePosition + selectedCardOffset;

        card.transform.position = newPosition;
    }

    public void UpdateCardProduction(Tile hoverTile)
    {
        if(selectedCard == null)
        {
            return;
        }

        selectedCard.UpdateProductionPreview(hoverTile);
    }

    public void IndicateProductionChanges(Tile hoverTile)
    {
        if(selectedCard == null)
        {
            return;
        }

        Building potentialNewBuilding = selectedCard.buildingPrefab.GetComponent<Building>();

        foreach(Building building in Treasury.instance.allPlacedBuildings)
        {
            List<Tile> tilesInRange = building.GetTilesInRange();
            if(!tilesInRange.Contains(hoverTile))
            {
                building.HideChangeIndicator();
                continue;
            }

            building.IndicateProductionChanges(potentialNewBuilding, hoverTile);
        }
    }
}
