using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public GameObject handTilePrefab;
    private List<Placeable> placeablesInHand = new List<Placeable>();
    private List<Tile> handTiles = new List<Tile>();

    private float leftmostHandPosition = 0f;
    private float distanceBetweenHandTiles = 0f;

    public void AddPlaceable(Placeable placeable)
    {
        GameObject tileObject = Instantiate(handTilePrefab);
        Tile tile = tileObject.GetComponent<Tile>();
        ArrangeHand();

        PlacementController.instance.PlaceBuildingOnTile(placeable, tile);
    }

    public void RemovePlaceable(Placeable placeable)
    {

    }

    private void ArrangeHand()
    {

    }
}
