using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bakery : Building
{
    public int collectionRange = 2;

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
                collectedFunds += mill.bonusForBakery;
            }
        }

        return collectedFunds;
    }
}
