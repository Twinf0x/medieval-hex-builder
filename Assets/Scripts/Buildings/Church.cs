using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Church : Building
{
    public int collectionRange = 2;
    public int bonusForMarket = -15;
    
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

            if(tile.placedBuilding is House)
            {
                House house = tile.placedBuilding as House;
                collectedFunds += house.TotalProduction;
            }

            if(tile.placedBuilding is Market)
            {
                Market market = tile.placedBuilding as Market;
                collectedFunds += market.bonusForChurch;
            }
        }

        Treasury.instance.AddMoney(collectedFunds + baseProduction);
    }
}
