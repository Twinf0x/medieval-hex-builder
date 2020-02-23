using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Building : MonoBehaviour
{
    public int baseProduction;
    internal Tile locationTile;

    public void PlaceOn(Tile tile)
    {
        this.locationTile = tile;
        Treasury.instance.allPlacedBuildings.Add(this);
        Treasury.instance.CollectMoney();
    }

    public virtual void Produce()
    {
        Treasury.instance.AddMoney(baseProduction);
    }
}
