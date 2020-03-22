using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Building : MonoBehaviour
{
    public int baseProduction;
    public GameObject popUpPrefab;
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

    public virtual int CalculateProduction()
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
}
