using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : Building
{
    public int bonusPerHouse = 6;
    public int range;

    public int TotalProduction
    {
        get
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

    public override void Produce()
    {
        int temp = TotalProduction;
        
        GameObject popUpObject = Instantiate(popUpPrefab, transform.position, Quaternion.identity, transform);
        NumberPopUp popUp = popUpObject.GetComponent<NumberPopUp>();
        popUp.text.text = temp.ToString();

        Treasury.instance.AddMoney(temp);
    }
}
