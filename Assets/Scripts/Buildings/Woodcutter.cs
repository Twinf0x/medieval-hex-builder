using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Woodcutter : Building
{
    public int bonusPerForest = 1;

    public override int CalculateProduction(Tile tile)
    {
        int forestsInRange = 0;
        List<Tile> tilesInRange = tile.GetAllTilesAround(collectionRange);
        foreach(Tile tempTile in tilesInRange)
        {
            if(tempTile.type == TileType.Forest)
                forestsInRange++;
        }

        return (forestsInRange * bonusPerForest) + baseProduction;
    }
}
