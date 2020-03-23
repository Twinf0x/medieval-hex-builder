using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brewery : Building
{
    public int bonusForMill = -6;
    
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

            if(tempTile.placedBuilding is Mill)
            {
                Mill mill = tempTile.placedBuilding as Mill;
                collectedFunds += mill.bonusForBrewery;
            }

            if(tempTile.placedBuilding is Field)
            {
                Field field = tempTile.placedBuilding as Field;
                collectedFunds += field.bonusForBrewery;
            }
        }
        
        return collectedFunds + baseProduction;
    }

    internal override int CalculateProductionChanges(Building newNeighbour, Tile neighbourTile)
    {
        if(newNeighbour is Mill)
        {
            Mill mill = newNeighbour as Mill;
            return mill.bonusForBrewery;
        }

        if(newNeighbour is Field)
        {
            Field field = newNeighbour as Field;
            return field.bonusForBrewery;
        }

        return 0;
    }
}
