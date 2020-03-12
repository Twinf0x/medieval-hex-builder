using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private TileDescription tileDescription;
    [SerializeField] private BuildingDescription buildingDescription;

    private void Awake() 
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void ShowTileDescription(TileDescriptionData data) 
    {
        if(data == null) 
        {
            tileDescription.Hide();
            return;
        }
        tileDescription.Show();
        tileDescription.SetData(data);
    }

    public void ShowBuildingDescription(BuildingDescriptionData data) 
    {
        if(data == null) 
        {
            buildingDescription.Hide();
            return;
        }

        buildingDescription.Show();
        buildingDescription.SetData(data);
    }

    public void HideBuildingDescription() 
    {
        buildingDescription.Hide();
    }
}
