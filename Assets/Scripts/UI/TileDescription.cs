using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileDescription : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public Image iconImage;

    public void Show() 
    {
        gameObject.SetActive(true);
    }

    public void Hide() 
    {
        gameObject.SetActive(false);
    }

    public void SetData(TileDescriptionData data)
    {
        titleText.text = data.title;
        iconImage.sprite = data.icon;
    }
}
