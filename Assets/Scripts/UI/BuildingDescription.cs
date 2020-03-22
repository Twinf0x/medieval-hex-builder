using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingDescription : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI productionText;

    public void Show() 
    {
        gameObject.SetActive(true);
    }

    public void Hide() 
    {
        gameObject.SetActive(false);
    }

    public void SetData(Building building)
    {
        titleText.text = building.descriptionData.title;
        descriptionText.text = building.descriptionData.description;
        productionText.text = "+" + building.CalculateProduction().ToString();
    }
}
