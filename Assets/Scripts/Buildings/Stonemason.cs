using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stonemason : Building
{
    public int collectionRange = 2;

    public override int CalculateProduction()
    {
        List<Tile> tilesInRange = locationTile.GetAllTilesAround(collectionRange);
        int collectedFunds = baseProduction;

        foreach(var tile in tilesInRange)
        {
            if(tile.placedBuilding != null && tile.placedBuilding is Quarry)
            {
                Quarry quarry = tile.placedBuilding as Quarry;
                collectedFunds += quarry.CalculateProduction();
            }
        }

        return collectedFunds;
    }
}
