using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasury : MonoBehaviour
{
    public static Treasury instance;

    [Header("Set in Editor")]
    public int baseThreshold = 10;
    public float growthFactor = 0.33f;

    [Header("Set during play, Visible for Debug")]
    public int nextThreshold = 0;
    public int previousThreshold = 0;
    public int currentMoney = 0;
    public int currentLevel = 0;

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

    public void NextLevel()
    {
        previousThreshold = nextThreshold;
        nextThreshold = GetThresholdForLevel(++currentLevel);
    }

    public int GetThresholdForLevel(int level)
    {
        float power = 1 + ((level - 1) * growthFactor);
        return (int) Mathf.Pow(baseThreshold, power);
    }
}
