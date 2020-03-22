using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hand : MonoBehaviour
{
    public static Hand instance;

    private List<Card> cardsInHand = new List<Card>();

    [Header("X is Minimum, Y is Maximum")]
    public Vector2 handScales = new Vector2(1f, 0.5f);
    public Vector2 handTileDistances = new Vector2(2f, 1f);
    public Vector2 handTilesForDistances = new Vector2(2f, 10f);
    public float handTileBaseWidth = 250f;
    private float leftmostHandPosition = 0f;
    private float handTileY = -1.5f;
    private float distanceBetweenHandTiles = 0.5f;
    private float currentHandScale = 1f;

    public bool IsEmptyAfterPlacement { get{ return cardsInHand.Count <= 0; } }
    public Vector3 CardScale { get; private set; }

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

    public void AddCard(Card card)
    {
        card.transform.SetParent(transform);
        cardsInHand.Add(card);
        ArrangeHand();
    }

    public void RemoveCard(Card card)
    {
        int index = cardsInHand.IndexOf(card);
        if(index < 0)
        {
            return;
        }

        cardsInHand.RemoveAt(index);

        ArrangeHand();
    }

    private void ArrangeHand()
    {
        float handFillPercentage = Mathf.InverseLerp(handTilesForDistances.x, handTilesForDistances.y, cardsInHand.Count);
        float handTileScale = Mathf.Lerp(handScales.x, handScales.y, handFillPercentage);
        CardScale = new Vector3(handTileScale, handTileScale, handTileScale);

        distanceBetweenHandTiles = Mathf.Lerp(handTileDistances.x, handTileDistances.y, handFillPercentage) * handTileBaseWidth * handTileScale;
        leftmostHandPosition = ((cardsInHand.Count - 1) / 2f) * distanceBetweenHandTiles * -1;

        for(int i = 0; i < cardsInHand.Count; i++)
        {
            Vector3 temp = cardsInHand[i].transform.localPosition;
            temp = new Vector3(leftmostHandPosition + (i * distanceBetweenHandTiles), handTileY, temp.z);
            cardsInHand[i].transform.localPosition = temp;

            cardsInHand[i].transform.localScale = CardScale;
        }
    }

    public void DeactivateHand()
    {
        foreach(Card card in cardsInHand)
        {
            Button cardButton = card.gameObject.GetComponent<Button>();
            cardButton.interactable = false;
        }
    }

    public void ActivateHand()
    {
        foreach(Card card in cardsInHand)
        {
            Button cardButton = card.gameObject.GetComponent<Button>();
            cardButton.interactable = true;
        }
    }
}
