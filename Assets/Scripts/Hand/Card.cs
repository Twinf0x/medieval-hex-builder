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
    public TextMeshProUGUI productionText;
    public PlacementRestriction restriction;

    private Building building;

    private void Awake()
    {
        displayText.text = displayName;
        building = buildingPrefab.GetComponent<Building>();
        HideProductionPreview();
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

    public void UpdateProductionPreview(Tile previewTile)
    {
        if(!MeetsPlacementRestriction(previewTile))
        {
            productionText.text = "-";
            return;
        }

        int production = building.CalculateProduction(previewTile);
        productionText.text = $"+{production.ToString()}";
    }

    public void HideProductionPreview()
    {
        productionText.gameObject.SetActive(false);
    }

    public void ShowProductionPreview()
    {
        productionText.gameObject.SetActive(true);
    }
}
