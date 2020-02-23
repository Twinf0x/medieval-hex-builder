using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quarry : Building
{
    public int bonusPerMountain = 1;
    public int TotalProduction 
    {
        get 
        {
            int mountainsInRange = 0;
            List<Tile> tilesInRange = locationTile.GetAllTilesAround(1);
            foreach(Tile tile in tilesInRange)
            {
                if(tile.type == TileType.Mountain)
                    mountainsInRange++;
            }

            return (mountainsInRange * bonusPerMountain) + baseProduction;
        }
    }

    public override void Produce()
    {
        Treasury.instance.AddMoney(TotalProduction);
    }
}
