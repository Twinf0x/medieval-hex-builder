using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public float defaultTimeBetweenCollections = 0.25f;
    public float maxMoneyCollectionDuration = 2.5f;
    public Slider levelProgressBar;
    public Transform treasureChest;
    public Transform moneyIndicator;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI levelText;

    public GameObject gameOverScreen;
    public TextMeshProUGUI gameOverMoneyText;

    public GameObject collectEffectPrefab;
    public GameObject levelUpEffectPrefab;


    [Header("Set during play, Visible for Debug")]
    public int nextThreshold = 0;
    public int previousThreshold = 0;
    public int currentMoney = 0;
    public int currentLevel = 0;
    public List<Building> allPlacedBuildings = new List<Building>();

    private Vector3 indicatorScale;
    private Vector3 treasureChestScale;

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
            treasureChestScale = treasureChest.localScale;
            NextLevel();
        }
    }

    private void Start()
    {
        AudioManager.instance?.StopAll();
        AudioManager.instance?.Play("GameTheme");
    }

    public void StartCollectingMoney()
    {
        if(allPlacedBuildings.Count == 1)
        {
            allPlacedBuildings.First().Produce();
            AudioManager.instance?.Play("Coins");
            moneyText.text = currentMoney.ToString();
            StartCoroutine(SimpleAnimations.instance.Wobble(moneyIndicator, 0.25f, 1, () => moneyIndicator.localScale = indicatorScale));

            if(Hand.instance.IsEmptyAfterPlacement)
            {
                Lose();
            }
        }
        else
        {
            float moneyCollectionDuration = Mathf.Min(allPlacedBuildings.Count * defaultTimeBetweenCollections, maxMoneyCollectionDuration);
            StartCoroutine(CollectMoney(moneyCollectionDuration));
        }
    }

    public IEnumerator CollectMoney(float duration)
    {
        if(allPlacedBuildings.Count == 1)
        {
            yield break;
        }

        float timeBetweenCollections = duration / (allPlacedBuildings.Count - 1);

        Hand.instance.DeactivateHand();
        foreach(Building building in allPlacedBuildings)
        {
            building.Produce();
            AudioManager.instance?.Play("Coins");

            moneyText.text = currentMoney.ToString();
            StartCoroutine(SimpleAnimations.instance.Wobble(moneyIndicator, timeBetweenCollections, 1, () => moneyIndicator.localScale = indicatorScale));
            yield return new WaitForSeconds(timeBetweenCollections);
        }
        Hand.instance.ActivateHand();

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
        {
            AudioManager.instance?.Play("LevelUp");
            StartCoroutine(SimpleAnimations.instance.Wobble(treasureChest, 0.5f, 1.25f, () => treasureChest.localScale = treasureChestScale));
            Vector3 position = Camera.main.ScreenToWorldPoint(levelText.transform.position);
            position.z = -1 * Camera.main.transform.position.z;
            Instantiate(levelUpEffectPrefab, position, Quaternion.identity, Camera.main.transform);
            Deck.instance.DrawToHand();
        }
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
        AudioManager.instance?.Stop("GameTheme");
        AudioManager.instance?.Play("GameOver");
        AudioManager.instance?.Play("Wind");
    }
}
