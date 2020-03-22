using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stonemason : Building
{
    public int collectionRange = 2;

    public override int CalculateProduction(Tile tile)
    {
        List<Tile> tilesInRange = tile.GetAllTilesAround(collectionRange);
        int collectedFunds = baseProduction;

        foreach(var tempTile in tilesInRange)
        {
            if(tempTile.placedBuilding != null && tempTile.placedBuilding is Quarry)
            {
                Quarry quarry = tempTile.placedBuilding as Quarry;
                collectedFunds += quarry.CalculateProduction();
            }
        }

        return collectedFunds;
    }
}
