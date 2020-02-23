using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mill : Building
{
    public int collectionRange = 2;

    public override void Produce()
    {
        List<Tile> tilesInRange = locationTile.GetAllTilesAround(collectionRange);
        int collectedFunds = baseProduction;

        foreach(var tile in tilesInRange)
        {
            if(tile.placedBuilding != null && tile.placedBuilding is Field)
            {
                Field field = tile.placedBuilding as Field;
                collectedFunds += field.bonusForMill;
            }
        }

        Treasury.instance.AddMoney(collectedFunds + baseProduction);
    }
}
