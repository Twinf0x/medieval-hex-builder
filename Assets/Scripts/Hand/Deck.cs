using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Deck : MonoBehaviour
{
    public static Deck instance;

    [ContextMenuItem("Draw", "DrawToHand")]
    public int initialDrawAmount = 5;
    public float drawDuration = 1f;

    public List<Pool> levelPools = new List<Pool>();
    private Queue<GameObject> currentDeck = new Queue<GameObject>();
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

    public void DrawToHand()
    {
        DrawToHand(initialDrawAmount);
    }

    public void DrawToHand(int amount)
    {
        StartCoroutine(DrawCards(amount, drawDuration));
    }

    private IEnumerator DrawCards(int amount, float duration)
    {
        float timeBetweenDraws = duration / amount;

        for(int i = 0; i < amount; i++)
        {
            if(currentDeck.Count <= 0)
            {
                currentDeck = GenerateNextDeck();
            }

            GameObject cardObject = Instantiate(currentDeck.Dequeue());
            Card card = cardObject.GetComponent<Card>();

            Hand.instance.AddCard(card);
            yield return new WaitForSeconds(timeBetweenDraws);
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
