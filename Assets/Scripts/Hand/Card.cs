using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerEnterHandler
{
    public string displayName;
    public GameObject buildingPrefab;
    public TextMeshProUGUI displayText;
    public PlacementRestriction restriction;

    private void Awake()
    {
        displayText.text = displayName;
    }

    public void OnPointerEnter(PointerEventData data) 
    {
        StartCoroutine(SimpleAnimations.instance.Wobble(transform, 0.25f, 1, () => transform.localScale = Hand.instance.CardScale ));
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
