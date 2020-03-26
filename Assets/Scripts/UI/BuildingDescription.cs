using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingDescription : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public GameObject productionParent;
    public TextMeshProUGUI productionText;

    public void Show(Building building, bool includeProduction = true) 
    {
        SetData(building, includeProduction);
        gameObject.SetActive(true);
    }

    public void Hide() 
    {
        gameObject.SetActive(false);
    }

    private void SetData(Building building, bool includeProduction)
    {
        titleText.text = building.descriptionData.title;
        descriptionText.text = building.descriptionData.description;
        if(includeProduction)
        {
            productionText.text = "+" + building.CalculateProduction().ToString();
            productionParent.SetActive(true);
        }
        else
        {
            productionParent.SetActive(false);
        }
    }
}
