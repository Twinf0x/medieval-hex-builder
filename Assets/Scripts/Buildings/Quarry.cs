using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quarry : Building
{
    public int bonusPerMountain = 1;

    public override int CalculateProduction(Tile tile)
    {
        int mountainsInRange = 0;
        List<Tile> tilesInRange = tile.GetAllTilesAround(1);
        foreach(Tile tempTile in tilesInRange)
        {
            if(tempTile.type == TileType.Mountain)
                mountainsInRange++;
        }

        return (mountainsInRange * bonusPerMountain) + baseProduction;
    }
}
