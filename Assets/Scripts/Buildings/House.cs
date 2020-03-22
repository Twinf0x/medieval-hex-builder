using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : Building
{
    public int bonusPerHouse = 6;
    public int range;

    public override int CalculateProduction(Tile tile)
    {
        int total = baseProduction;
        List<Tile> tilesInRange = tile.GetAllTilesAround(range);
        foreach(Tile tempTile in tilesInRange)
        {
            if(tempTile.placedBuilding == null)
            {
                continue;
            }

            if(tempTile.placedBuilding.GetType().Name == "House")
            {
                total += bonusPerHouse;
            }
        }

        return total;
    }
}
