using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public static Deck instance;

    public int initialDrawAmount = 5;

    public List<Pool> levelPools = new List<Pool>();
    private Queue<GameObject> currentTileDeck = new Queue<GameObject>();
    private int currentLevel = 0;

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
            if(currentTileDeck.Count <= 0)
            {
                currentTileDeck = GenerateNextDeck();
            }

            GameObject placeableObject = Instantiate(currentTileDeck.Dequeue());
            Placeable placeable = placeableObject.GetComponent<Placeable>();

            Hand.instance.AddPlaceable(placeable);
        }
    }

    private Queue<GameObject> GenerateNextDeck()
    {
        Pool pool;
        if(currentLevel >= levelPools.Count)
        {
            pool = levelPools[levelPools.Count - 1];
        }
        else
        {
            pool = levelPools[currentLevel];
        }
        currentLevel++;

        List<GameObject> newDeck = new List<GameObject>();

        foreach(var item in pool.items)
        {
            int amountToAdd = Random.Range((int)item.amount.x, (int)item.amount.y +1);
            for(int i = 0; i < amountToAdd; i++)
            {
                newDeck.Add(item.prefab);
            }
        }
        newDeck.Shuffle();

        return new Queue<GameObject>(newDeck);
    }
}
