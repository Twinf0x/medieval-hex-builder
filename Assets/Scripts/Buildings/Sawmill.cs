using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sawmill : Building
{
    public int collectionRange = 2;

    public override void Produce()
    {
        List<Tile> tilesInRange = locationTile.GetAllTilesAround(collectionRange);
        int collectedFunds = baseProduction;

        foreach(var tile in tilesInRange)
        {
            if(tile.placedBuilding != null && tile.placedBuilding is Woodcutter)
            {
                Woodcutter woodcutter = tile.placedBuilding as Woodcutter;
                collectedFunds += woodcutter.TotalProduction;
            }
        }

        GameObject popUpObject = Instantiate(popUpPrefab, transform.position, Quaternion.identity, transform);
        NumberPopUp popUp = popUpObject.GetComponent<NumberPopUp>();
        popUp.text.text = (collectedFunds + baseProduction).ToString();

        Treasury.instance.AddMoney(collectedFunds + baseProduction);
    }
}
