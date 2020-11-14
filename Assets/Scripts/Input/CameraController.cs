using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public float panSpeed = 20;
    public float borderThickness = 10;

    [HideInInspector]
    public Camera mainCamera;
    [Header("X is minimum, Y is Maximum")]
    public Vector2 xAxisLimits;
    public Vector2 yAxisLimits;
    public Vector2 sizeRestrictions;
    private Vector2 mapSize;

    [SerializeField] private bool canMoveCamera = true;
    private Coroutine scrollCoroutine = null;
    private float desiredSize;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            mainCamera = gameObject.GetComponent<Camera>();
            desiredSize = mainCamera.orthographicSize;
            SetLimits(desiredSize);
        }
    }

    private void OnDestroy()
    {
        if(instance == this)
        {
            instance = null;
        }
    }
	
	private void Update ()
    {
        if (!canMoveCamera || Treasury.instance.gameOver)
        {
            return;
        }

        var pos = transform.position;

        pos += CalculateCameraMovementDesktop();
        pos += CalculateCameraMovementMobile();

        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            ScrollIn();
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            ScrollOut();
        }

        pos.x = Mathf.Clamp(pos.x, xAxisLimits.x, xAxisLimits.y);
        pos.y = Mathf.Clamp(pos.y, yAxisLimits.x, yAxisLimits.y);

        transform.position = pos;
	}

    private Vector3 CalculateCameraMovementMobile()
    {
        Vector3 delta = Vector3.zero;

        if (TouchInput.instance.IsPanning)
        {
            var touch = Input.GetTouch(0);
            var currentTouchPos = mainCamera.ScreenToWorldPoint(touch.position);
            var previousTouchPos = mainCamera.ScreenToWorldPoint(touch.position- touch.deltaPosition);
            var touchDelta = currentTouchPos - previousTouchPos;
            delta.x += touchDelta.x;
            delta.y = touchDelta.y;

            delta *= -1;
        }

        return delta;
    }

    private Vector3 CalculateCameraMovementDesktop()
    {
        Vector3 delta = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Input.mousePosition.y > Screen.height - borderThickness)
        {
            delta.y += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) || Input.mousePosition.y < borderThickness)
        {
            delta.y -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || Input.mousePosition.x > Screen.width - borderThickness)
        {
            delta.x += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || Input.mousePosition.x < borderThickness)
        {
            delta.x -= panSpeed * Time.deltaTime;
        }

        return delta;
    }

    public void SetSizeRestrictions (Vector2 sizeRestrictions)
    {
        this.sizeRestrictions = sizeRestrictions;
    }

    public void SetMapSize (Vector2 mapSize)
    {
        this.mapSize = mapSize;
        SetLimits(desiredSize);
    }

    public void CenterCamera(Vector2 offset) 
    {
        Vector3 position =  new Vector3((mapSize.x + offset.x) / 2f, mapSize.y / 2f, mainCamera.transform.position.z);
        mainCamera.transform.position = position;
    }

    public void ScrollToOrthographicSize (float targetSize)
    {
        if (targetSize < sizeRestrictions.x)
        {
            targetSize = sizeRestrictions.x;
        }
        else if (targetSize > sizeRestrictions.y)
        {
            targetSize = sizeRestrictions.y;
        }

        desiredSize = targetSize;

        if(scrollCoroutine != null)
        {
            StopCoroutine(scrollCoroutine);
        }
        scrollCoroutine = StartCoroutine(Scroll(targetSize));
    }

    private void ScrollIn()
    {
        desiredSize -= 1;
        if(desiredSize < sizeRestrictions.x)
        {
            desiredSize = sizeRestrictions.x;
        }

        if(scrollCoroutine != null)
        {
            StopCoroutine(scrollCoroutine);
        }
        scrollCoroutine = StartCoroutine(Scroll(desiredSize));
    }

    private void ScrollOut()
    {
        desiredSize += 1;
        if (desiredSize > sizeRestrictions.y)
        {
            desiredSize = sizeRestrictions.y;
        }

        if (scrollCoroutine != null)
        {
            StopCoroutine(scrollCoroutine);
        }
        scrollCoroutine = StartCoroutine(Scroll(desiredSize));
    }

    private void SetLimits(float orthographicSize)
    {
        var camOffsetY = orthographicSize;
        var camOffsetX = (camOffsetY / 9f) * 16f;
        Vector2 yLimits = new Vector2(camOffsetY, mapSize.y - camOffsetY + 1);
        Vector2 xLimits = new Vector2(camOffsetX, mapSize.x - camOffsetX + 1);

        xAxisLimits = xLimits;
        yAxisLimits = yLimits;
    }

    private IEnumerator Scroll (float targetSize, float duration = 0.25f)
    {
        instance.SetLimits(targetSize);

        float startSize = mainCamera.orthographicSize;
        float changePerSecond = (targetSize - startSize) / duration;

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            mainCamera.orthographicSize += changePerSecond * Time.deltaTime;
            yield return null;
        }

        mainCamera.orthographicSize = targetSize;
        scrollCoroutine = null;
    }
}
