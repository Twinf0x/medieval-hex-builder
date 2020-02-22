using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public static Hand instance;

    public GameObject handTilePrefab;
    private List<Placeable> placeablesInHand = new List<Placeable>();
    private List<Tile> handTiles = new List<Tile>();

    private float leftmostHandPosition = 0f;
    private float handTileY = -1.5f;
    private float distanceBetweenHandTiles = 0.5f;

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

    public void AddPlaceable(Placeable placeable)
    {
        GameObject tileObject = Instantiate(handTilePrefab, transform.position, Quaternion.identity, transform);
        Tile tile = tileObject.GetComponent<Tile>();
        handTiles.Add(tile);
        ArrangeHand();

        PlacementController.instance.PlaceBuildingOnTile(placeable, tile);
        //placeable.transform.SetParent(tile.transform);
        placeablesInHand.Add(placeable);
    }

    public void RemovePlaceable(Placeable placeable)
    {
        int index = placeablesInHand.IndexOf(placeable);
        if(index < 0)
        {
            return;
        }

        placeablesInHand.RemoveAt(index);
        Tile handTile = handTiles[index];
        handTiles.Remove(handTile);
        Destroy(handTile);

        ArrangeHand();
    }

    private void ArrangeHand()
    {
        leftmostHandPosition = ((handTiles.Count - 1) / 2f) * distanceBetweenHandTiles * -1;

        for(int i = 0; i < handTiles.Count; i++)
        {
            Vector3 temp = handTiles[i].transform.position;
            temp = new Vector3(leftmostHandPosition + (i * distanceBetweenHandTiles), handTileY, temp.z);
            handTiles[i].transform.position = temp;
        }
    }
}
