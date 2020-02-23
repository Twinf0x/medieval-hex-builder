using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Treasury : MonoBehaviour
{
    public static Treasury instance;

    [Header("Set in Editor")]
    public int baseThreshold = 10;
    public float growthFactor = 0.33f;
    public Slider levelProgressBar;

    [Header("Set during play, Visible for Debug")]
    public int nextThreshold = 0;
    public int previousThreshold = 0;
    public int currentMoney = 0;
    public int currentLevel = 0;
    public List<Building> allPlacedBuildings = new List<Building>();

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            levelProgressBar.value = currentMoney;
            NextLevel();
        }
    }

    public void CollectMoney()
    {
        foreach(Building building in allPlacedBuildings)
        {
            building.Produce();
        }
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        levelProgressBar.value = currentMoney;
        if(currentMoney >= nextThreshold)
        {
            NextLevel();
        }
    }

    public void NextLevel()
    {
        previousThreshold = nextThreshold;
        nextThreshold = GetThresholdForLevel(++currentLevel);

        levelProgressBar.minValue = previousThreshold;
        levelProgressBar.maxValue = nextThreshold;

        if(currentLevel > 1)
            Deck.instance.DrawToHand();
    }

    public int GetThresholdForLevel(int level)
    {
        float power = 1 + ((level - 1) * growthFactor);
        return (int) Mathf.Pow(baseThreshold, power);
    }
}
