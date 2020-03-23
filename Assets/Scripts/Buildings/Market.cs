using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Market : Building
{
    public int bonusForChurch = -15;

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

            if(tempTile.placedBuilding is Church)
            {
                Church church = tempTile.placedBuilding as Church;
                collectedFunds += church.bonusForMarket;
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

        if(newNeighbour is Church)
        {
            Church church = newNeighbour as Church;
            return church.bonusForMarket;
        }

        return 0;
    }
}
