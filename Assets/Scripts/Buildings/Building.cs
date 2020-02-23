using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Building : MonoBehaviour
{
    public int baseProduction;
    public GameObject popUpPrefab;
    internal Tile locationTile;

    public void PlaceOn(Tile tile)
    {
        this.locationTile = tile;
        Treasury.instance.allPlacedBuildings.Add(this);
        Treasury.instance.CollectMoney();
    }

    public virtual void Produce()
    {
        GameObject popUpObject = Instantiate(popUpPrefab, transform.position, Quaternion.identity, transform);
        NumberPopUp popUp = popUpObject.GetComponent<NumberPopUp>();
        popUp.text.text = baseProduction.ToString();

        Treasury.instance.AddMoney(baseProduction);
    }
}
