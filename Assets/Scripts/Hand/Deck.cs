using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public static Deck instance;

    public GameObject fieldPrefab;
    public int initialDrawAmount = 5;

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

    private void Start()
    {
        instance.DrawToHand(initialDrawAmount);
    }

    public void DrawToHand(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            GameObject placeableObject = Instantiate(fieldPrefab);
            Placeable placeable = placeableObject.GetComponent<Placeable>();

            Hand.instance.AddPlaceable(placeable);
        }
    }
}
