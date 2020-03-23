using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Church : Building
{
    public int bonusForMarket = -15;
    
    public override int CalculateProduction(Tile tile)
    {
        List<Tile> tilesInRange = tile.GetAllTilesAround(collectionRange);
        int collectedFunds = baseProduction;

        foreach(var tempTile in tilesInRange)
        {
            if(tempTile.placedBuilding == null)
            {
                continue;
            }

            if(tempTile.placedBuilding is House)
            {
                House house = tempTile.placedBuilding as House;
                collectedFunds += house.CalculateProduction();
            }

            if(tempTile.placedBuilding is Market)
            {
                Market market = tempTile.placedBuilding as Market;
                collectedFunds += market.bonusForChurch;
            }
        }

        return collectedFunds;
    }

    internal override int CalculateProductionChanges(Building newNeighbour, Tile neighbourTile)
    {
        if(newNeighbour is House)
        {
            House house = newNeighbour as House;
            return house.CalculateProduction(neighbourTile);
        }

        if(newNeighbour is Market)
        {
            Market market = newNeighbour as Market;
            return market.bonusForChurch;
        }

        return 0;
    }
}
