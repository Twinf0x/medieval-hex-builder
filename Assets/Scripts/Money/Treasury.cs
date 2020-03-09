using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Treasury : MonoBehaviour
{
    public static Treasury instance;

    [Header("Set in Editor")]
    public int baseThreshold = 10;
    public float growthFactorOne = 1f;
    public float growthFactorTwo = 0.33f;
    public Slider levelProgressBar;
    public Transform moneyIndicator;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI levelText;

    public GameObject gameOverScreen;
    public TextMeshProUGUI gameOverMoneyText;


    [Header("Set during play, Visible for Debug")]
    public int nextThreshold = 0;
    public int previousThreshold = 0;
    public int currentMoney = 0;
    public int currentLevel = 0;
    public List<Building> allPlacedBuildings = new List<Building>();

    private Vector3 indicatorScale;

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
            indicatorScale = moneyIndicator.localScale;
            NextLevel();
        }
    }

    public void CollectMoney()
    {
        foreach(Building building in allPlacedBuildings)
        {
            building.Produce();
            AudioManager.instance.Play("Coins");
        }

        moneyText.text = currentMoney.ToString();
        StartCoroutine(SimpleAnimations.instance.Wobble(moneyIndicator, 0.25f, 1, () => moneyIndicator.localScale = indicatorScale));

        if(Hand.instance.IsEmptyAfterPlacement)
        {
            Lose();
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

        levelText.text = currentLevel.ToString();

        if(currentLevel > 1)
            Deck.instance.DrawToHand();
    }

    public int GetThresholdForLevel(int level)
    {
        float power = 1 + (Mathf.Min((level - 1), 3) * growthFactorOne) + (Mathf.Max((level - 4), 0) * growthFactorTwo);
        return (int) Mathf.Pow(baseThreshold, power);
    }

    private void Lose()
    {
        gameOverMoneyText.text = $"You earned {currentMoney.ToString()} Gold!";
        gameOverScreen.SetActive(true);
    }
}
