using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : Building
{
    public int bonusPerHouse = 6;
    public int range;

    public override int CalculateProduction()
    {
        int total = baseProduction;
        List<Tile> tilesInRange = locationTile.GetAllTilesAround(range);
        foreach(Tile tile in tilesInRange)
        {
            if(tile.placedBuilding == null)
            {
                continue;
            }

            if(tile.placedBuilding.GetType().Name == "House")
            {
                total += bonusPerHouse;
            }
        }

        return total;
    }
}
