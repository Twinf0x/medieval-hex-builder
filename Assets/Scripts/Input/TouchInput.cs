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
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    #region Inspector
    [SerializeField, Range(0.001f, 1f)] private float zoomSensitivity = 0.8f;
    #endregion

    #region Properties
    public bool IsPanning
    {
        get { return touchCount == 2; }
    }

    public Vector2 PanPosition
    {
        get 
        {
            mainTouch = Input.GetTouch(0);
            secondaryTouch = Input.GetTouch(1);
            return mainTouch.position + ((secondaryTouch.position - mainTouch.position) / 2f); 
        }
    }

    public Vector2 OldPanPosition
    {
        get 
        {
            mainTouch = Input.GetTouch(0);
            secondaryTouch = Input.GetTouch(1);
            mainOldPosition = mainTouch.position - mainTouch.deltaPosition;
            secondaryOldPosition = secondaryTouch.position - secondaryTouch.deltaPosition;
            return mainOldPosition + ((secondaryOldPosition - mainOldPosition) / 2); 
        }
    }
    public float ZoomDelta
    {
        get
        {
            mainTouch = Input.GetTouch(0);
            secondaryTouch = Input.GetTouch(1);
            mainOldPosition = mainTouch.position - mainTouch.deltaPosition;
            secondaryOldPosition = secondaryTouch.position - secondaryTouch.deltaPosition;
            var rawDelta = Vector2.Distance(Camera.main.ScreenToWorldPoint(mainTouch.position), Camera.main.ScreenToWorldPoint(secondaryTouch.position)) - Vector2.Distance(Camera.main.ScreenToWorldPoint(mainOldPosition), Camera.main.ScreenToWorldPoint(secondaryOldPosition));
            rawDelta *= -1;
            rawDelta *= zoomSensitivity;
            return rawDelta;
        }
    }
    #endregion

    private int touchCount;
    private Touch mainTouch;
    private Touch secondaryTouch;
    private Vector2 mainOldPosition;
    private Vector2 secondaryOldPosition;

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
