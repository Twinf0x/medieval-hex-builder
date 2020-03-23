using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sawmill : Building
{
    public override int CalculateProduction(Tile tile)
    {
        List<Tile> tilesInRange = tile.GetAllTilesAround(collectionRange);
        int collectedFunds = baseProduction;

        foreach(var tempTile in tilesInRange)
        {
            if(tempTile.placedBuilding != null && tempTile.placedBuilding is Woodcutter)
            {
                Woodcutter woodcutter = tempTile.placedBuilding as Woodcutter;
                collectedFunds += woodcutter.CalculateProduction();
            }
        }

        return collectedFunds;
    }

    internal override int CalculateProductionChanges(Building newNeighbour, Tile neighbourTile)
    {
        if(newNeighbour is Woodcutter)
        {
            Woodcutter woodcutter = newNeighbour as Woodcutter;
            return woodcutter.CalculateProduction(neighbourTile);
        }

        return 0;
    }
}
