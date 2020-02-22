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

    private bool canMoveCamera = true;
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
	
	private void Update ()
    {
        if (!canMoveCamera)
        {
            return;
        }

        var pos = transform.position;
        
        if(Input.GetKey(KeyCode.UpArrow) || Input.mousePosition.y > Screen.height - borderThickness)
        {
            pos.y += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.mousePosition.y < borderThickness)
        {
            pos.y -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x > Screen.width - borderThickness)
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.mousePosition.x < borderThickness)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

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
        Vector2 yLimits = new Vector2(camOffsetY, mapSize.y - camOffsetY);
        Vector2 xLimits = new Vector2(camOffsetX, mapSize.x - camOffsetX);

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
