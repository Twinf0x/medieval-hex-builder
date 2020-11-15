using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string displayName;
    public GameObject buildingPrefab;
    public TextMeshProUGUI displayText;
    public TextMeshProUGUI productionText;
    public PlacementRestriction restriction;
    public GameObject invalidTileIndicator;
    public GameObject validTileIndicator;
    public Button clickHandler;

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
        AudioManager.instance?.Play("CardDraw");
        UIManager.instance?.ShowBuildingDescription(building, false);
    }

    public void OnPointerExit(PointerEventData data)
    {
        UIManager.instance?.HideBuildingDescription();
    }

    public void PickFromHand() 
    {
        PlacementController.instance.PickCardUp(this);
        clickHandler.onClick.RemoveAllListeners();
        clickHandler.onClick.AddListener(() => PlacementController.instance.TryPlacingSelectionOnLastClickedTile());
        clickHandler.enabled = false;
        StartCoroutine(SimpleAnimations.instance.Stretch(transform, 0.25f, 1, () => transform.localScale = Hand.instance.CardScale ));
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

    public void ShowInvalidTileIndicator()
    {
        invalidTileIndicator.SetActive(true);
    }

    public void HideInvalidTileIndicator()
    {
        invalidTileIndicator.SetActive(false);
    }

    public void ShowValidTileIndicator()
    {
        validTileIndicator.SetActive(true);
    }

    public void HideValidTileIndicator()
    {
        validTileIndicator.SetActive(false);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        transform.localScale = Hand.instance.CardScale;
    }
}
