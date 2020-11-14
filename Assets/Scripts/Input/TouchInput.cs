using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    #region Singleton
    public static TouchInput instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    #region Properties
    public bool IsPanning
    {
        get { return touchCount == 2; }
    }
    #endregion

    private int touchCount;

    private void Update()
    {
        touchCount = 0;
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
            {
                touchCount++;
            }
        }
    }
}
