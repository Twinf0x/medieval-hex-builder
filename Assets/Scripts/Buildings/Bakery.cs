using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bakery : Building
{
    public int collectionRange = 2;

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

            if(tile.placedBuilding is Mill)
            {
                Mill mill = tile.placedBuilding as Mill;
                collectedFunds += mill.bonusForBakery;
            }
        }

        return collectedFunds;
    }
}
