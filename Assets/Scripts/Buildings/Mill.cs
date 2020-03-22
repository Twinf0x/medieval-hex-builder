using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mill : Building
{
    public int collectionRange = 2;
    public int bonusForBakery = 10;
    public int bonusForBrewery = -10;

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

            if(tempTile.placedBuilding is Brewery)
            {
                Brewery brewery = tempTile.placedBuilding as Brewery;
                collectedFunds += brewery.bonusForMill;
            }

            if(tempTile.placedBuilding is Field)
            {
                Field field = tempTile.placedBuilding as Field;
                collectedFunds += field.bonusForMill;
            }
        }

        return collectedFunds;
    }
}
