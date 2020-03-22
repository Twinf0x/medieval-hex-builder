using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Woodcutter : Building
{
    public int bonusPerForest = 1;

    public override int CalculateProduction()
    {
        int forestsInRange = 0;
        List<Tile> tilesInRange = locationTile.GetAllTilesAround(1);
        foreach(Tile tile in tilesInRange)
        {
            if(tile.type == TileType.Forest)
                forestsInRange++;
        }

        return (forestsInRange * bonusPerForest) + baseProduction;
    }
}
