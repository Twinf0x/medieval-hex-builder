using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brewery : Building
{
    public int collectionRange = 2;
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
}
