using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TranslucentOnMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image targetImage;
    [Range(0f, 1f)] public float normalAlpha = 1;
    [Range(0f, 1f)] public float mouseOverAlpha = 0.5f;

    private void OnDisable()
    {
        OnPointerExit(null);
    }

    public void OnPointerEnter(PointerEventData data) 
    {
        Color color = targetImage.color;
        color.a = mouseOverAlpha;
        targetImage.color = color;
    }

    public void OnPointerExit(PointerEventData data)
    {
        Color color = targetImage.color;
        color.a = normalAlpha = 1;
        targetImage.color = color;
    }
}
