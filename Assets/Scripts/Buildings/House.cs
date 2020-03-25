using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : Building
{
    public int bonusPerHouse = 6;

    public override int CalculateProduction(Tile tile)
    {
        int total = baseProduction;
        List<Tile> tilesInRange = tile.GetAllTilesAround(collectionRange);
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

        //Add the house itself if it isn't placed yet for preview purposes
        if(this.locationTile == null)
        {
            total += bonusPerHouse;
            if(tile.placedBuilding != null && tile.placedBuilding is House)
            {
                total -= bonusPerHouse;
            }
        }

        return total;
    }

    internal override int CalculateProductionChanges(Building newNeighbour, Tile neighbourTile)
    {
        int change = 0;

        if(newNeighbour is House)
        {
            change += bonusPerHouse;
        }

        if(neighbourTile.placedBuilding != null && neighbourTile.placedBuilding is House)
        {
            change -= bonusPerHouse;
        }

        return change;
    }
}
