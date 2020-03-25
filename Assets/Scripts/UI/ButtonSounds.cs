using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class ButtonSounds : MonoBehaviour, IPointerEnterHandler
{
    public string mouseOverSound;
    public string clickSound;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => AudioManager.instance?.Play(clickSound));
    }

    public void OnPointerEnter(PointerEventData data)
    {
        AudioManager.instance?.Play(mouseOverSound);
    }
}
