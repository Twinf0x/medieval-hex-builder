using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Building : MonoBehaviour
{
    public int baseProduction;
    public int collectionRange = 0;
    public GameObject popUpPrefab;
    public TextMeshProUGUI changeIndicatorText;
    public Color positiveChangeColor;
    public Color negativeChangeColor;
    public GameObject changeIndicatorParent;
    public BuildingDescriptionData descriptionData;
    internal Tile locationTile;
    internal Vector3 initialScale;
    
    private void Awake()
    {
        initialScale = transform.localScale;
    }

    public void PlaceOn(Tile tile)
    {
        this.locationTile = tile;
        StartCoroutine(SimpleAnimations.instance.Squash(transform, 0.5f, 1, () => this.transform.localScale = initialScale));
        Treasury.instance.allPlacedBuildings.Add(this);
        Treasury.instance.StartCollectingMoney();
    }

    public int CalculateProduction()
    {
        return CalculateProduction(locationTile);
    }

    public virtual int CalculateProduction(Tile tile)
    {
        return baseProduction;
    }

    public virtual void Produce()
    {
        int production = CalculateProduction();

        GameObject popUpObject = Instantiate(popUpPrefab, transform.position, Quaternion.identity, transform);
        NumberPopUp popUp = popUpObject.GetComponent<NumberPopUp>();
        popUp.text.text = production.ToString();

        Treasury.instance.AddMoney(production);
    }

    internal virtual int CalculateProductionChanges(Building newNeighbour, Tile neighbourTile)
    {
        return 0;
    }

    public void IndicateProductionChanges(Building newNeighbour, Tile neighbourTile)
    {
        int change = CalculateProductionChanges(newNeighbour, neighbourTile);

        if(change == 0)
        {
            HideChangeIndicator();
        }
        else
        {
            ShowChangeIndicator(change);
        }
    }

    public void HideChangeIndicator()
    {
        changeIndicatorParent.SetActive(false);
    }

    private void ShowChangeIndicator(int change)
    {
        if(change > 0)
        {
            changeIndicatorText.text = "+" + change.ToString();
            changeIndicatorText.color = positiveChangeColor;
        }
        else
        {
            changeIndicatorText.text = change.ToString();
            changeIndicatorText.color = negativeChangeColor;
        }

        changeIndicatorParent.SetActive(true);
    }

    public List<Tile> GetTilesInRange()
    {
        if(locationTile != null)
        {
            return locationTile.GetAllTilesAround(collectionRange);
        }

        return new List<Tile>();
    }
}
