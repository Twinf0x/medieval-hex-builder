using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    public string displayName;
    public GameObject buildingPrefab;
    public TextMeshProUGUI displayText;
    public PlacementRestriction restriction;

    private void Awake()
    {
        displayText.text = displayName;
    }

    private void OnMouseEnter() 
    {

        StartCoroutine(SimpleAnimations.instance.Wobble(transform, 0.25f, 1, null));
    }

    public void PickFromHand() 
    {
        PlacementController.instance.PickCardUp(this);
    }

    public bool MeetsPlacementRestriction(Tile tile) 
    {
        return restriction.MeetsPlacementRestrictions(tile);
    }
}
