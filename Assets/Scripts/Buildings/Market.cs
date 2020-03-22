using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Market : Building
{
    public int collectionRange = 2;
    public int bonusForChurch = -15;

    public override int CalculateProduction()
    {
        List<Tile> tilesInRange = locationTile.GetAllTilesAround(collectionRange);
        int collectedFunds = baseProduction;

        foreach(var tile in tilesInRange)
        {
            if(tile.placedBuilding == null)
            {
                continue;
            }

            if(tile.placedBuilding is House)
            {
                House house = tile.placedBuilding as House;
                collectedFunds += house.CalculateProduction();
            }

            if(tile.placedBuilding is Church)
            {
                Church church = tile.placedBuilding as Church;
                collectedFunds += church.bonusForMarket;
            }
        }

        return collectedFunds;
    }
}
