using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Building : MonoBehaviour
{
    public int baseProduction;
    public List<UnityEvent> productionModificators = new List<UnityEvent>();
    private Tile locationTile;

    public void PlaceOn(Tile tile)
    {
        this.locationTile = tile;
        Treasury.instance.allPlacedBuildings.Add(this);
        Treasury.instance.CollectMoney();
    }

    public void Produce()
    {
        Treasury.instance.AddMoney(baseProduction);
    }
}
