using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public static Hand instance;

    public GameObject handTilePrefab;
    private List<Placeable> placeablesInHand = new List<Placeable>();
    private List<Tile> handTiles = new List<Tile>();

    [Header("X is Minimum, Y is Maximum")]
    public Vector2 handScales = new Vector2(0.6f, 1.4f);
    public Vector2 handTileDistances = new Vector2(2f, 0.3f);
    public Vector2 handTilesForDistances = new Vector2(2f, 8f);
    private float leftmostHandPosition = 0f;
    private float handTileY = -1.5f;
    private float distanceBetweenHandTiles = 0.5f;
    private float currentHandScale = 1f;

    public bool IsEmptyAfterPlacement { get{ return placeablesInHand.Count <= 1; } }

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
        float handFillPercentage = Mathf.InverseLerp(handTilesForDistances.x, handTilesForDistances.y, handTiles.Count);
        float unscaledDistance = Mathf.Lerp(handTileDistances.x, handTileDistances.y, handFillPercentage);
        distanceBetweenHandTiles = unscaledDistance * currentHandScale;
        leftmostHandPosition = ((handTiles.Count - 1) / 2f) * distanceBetweenHandTiles * -1;

        for(int i = 0; i < handTiles.Count; i++)
        {
            Vector3 temp = handTiles[i].transform.localPosition;
            temp = new Vector3(leftmostHandPosition + (i * distanceBetweenHandTiles), handTileY, temp.z);
            handTiles[i].transform.localPosition = temp;
        }
    }

    public void AdjustToCameraSize(float size)
    {
        handTileY = -0.8f * size;
        float scrollFactor = Mathf.InverseLerp(CameraController.instance.sizeRestrictions.x, CameraController.instance.sizeRestrictions.y, size);
        float handScale = Mathf.Lerp(handScales.x, handScales.y, scrollFactor);
        currentHandScale = handScale;
        foreach(Tile tile in handTiles)
        {
            tile.transform.localScale = new Vector3(handScale, handScale, handScale);
        }

        ArrangeHand();
    }
}
