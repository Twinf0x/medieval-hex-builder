using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mill : Building
{
    public int collectionRange = 2;
    public int bonusForBakery = 10;
    public int bonusForBrewery = -10;

    public override void Produce()
    {
        List<Tile> tilesInRange = locationTile.GetAllTilesAround(collectionRange);
        int collectedFunds = baseProduction;

        foreach(var tile in tilesInRange)
        {
            if(tile.placedBuilding == null)
            {
                continue;
            }

            if(tile.placedBuilding is Brewery)
            {
                Brewery brewery = tile.placedBuilding as Brewery;
                collectedFunds += brewery.bonusForMill;
            }

            if(tile.placedBuilding is Field)
            {
                Field field = tile.placedBuilding as Field;
                collectedFunds += field.bonusForMill;
            }
        }

        GameObject popUpObject = Instantiate(popUpPrefab, transform.position, Quaternion.identity, transform);
        NumberPopUp popUp = popUpObject.GetComponent<NumberPopUp>();
        popUp.text.text = (collectedFunds + baseProduction).ToString();

        Treasury.instance.AddMoney(collectedFunds + baseProduction);
    }
}
