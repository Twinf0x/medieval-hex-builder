﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bakery : Building
{
    public int collectionRange = 2;

    public override void Produce()
    {
        List<Tile> tilesInRange = locationTile.GetAllTilesAround(collectionRange);
        int collectedFunds = baseProduction;

        foreach(var tile in tilesInRange)
        {
            if(tile.placedBuilding == null)
            {
                continue;
            }

            if(tile.placedBuilding is Mill)
            {
                Mill mill = tile.placedBuilding as Mill;
                collectedFunds += mill.bonusForBakery;
            }
        }

        GameObject popUpObject = Instantiate(popUpPrefab, transform.position, Quaternion.identity, transform);
        NumberPopUp popUp = popUpObject.GetComponent<NumberPopUp>();
        popUp.text.text = (collectedFunds + baseProduction).ToString();

        Treasury.instance.AddMoney(collectedFunds + baseProduction);
    }
}
