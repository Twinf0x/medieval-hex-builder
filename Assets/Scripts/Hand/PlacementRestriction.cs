using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlacementRestriction", menuName = "Custom/PlacementRestriction")]
public class PlacementRestriction : ScriptableObject
{
    public TileType tileRestriction = TileType.None;
    public string buildingRestriction = "";
    
    public bool MeetsPlacementRestrictions(Tile tile)
    {
        if(tileRestriction != TileType.None && tile.type != tileRestriction)
        {
            return false;
        }

        if(buildingRestriction == "" && tile.placedBuilding != null)
        {
            return false;
        }

        if(buildingRestriction != "" && tile.placedBuilding == null)
        {
            return false;
        }

        if(buildingRestriction != "" && tile.placedBuilding != null && !(tile.placedBuilding.GetType().Name == buildingRestriction))
        {
            return false;
        }

        return true;
    }
}
