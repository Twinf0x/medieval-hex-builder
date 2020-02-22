using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    public GameObject buildingPrefab;
    public GameObject hoverMarker;
    public Vector3 mouseOffset = new Vector3(1, 1, 0);
    private bool isPickedUp = false;
    private Vector3 initialPosition = Vector3.zero;
    private Vector3 initialScale;
    [HideInInspector]
    public bool isInHand = true;

    private void Awake()
    {
        initialScale = transform.localScale;
    }

    private void Update()
    {
        if (isPickedUp)
        {
            MoveNextToCursor();
        }
    }

    private void OnMouseEnter()
    {
        if (isPickedUp)
            return;

        hoverMarker.SetActive(true);
        StartCoroutine(SimpleAnimations.instance.Wobble(transform, 0.25f, 1, () => this.transform.localScale = initialScale));
    }

    private void OnMouseExit()
    {
        if (isPickedUp)
            return;

        hoverMarker.SetActive(false);
    }

    public void PickUp()
    {
        hoverMarker.SetActive(false);
        initialPosition = transform.position;
        isPickedUp = true;
        StartCoroutine(SimpleAnimations.instance.Stretch(transform, 0.25f, 1, () => this.transform.localScale = initialScale));
    }

    public void Place()
    {
        isPickedUp = false;
        StartCoroutine(SimpleAnimations.instance.Squash(transform, 0.25f, 1, () => this.transform.localScale = initialScale));
    }

    public void Reset()
    {
        transform.position = initialPosition;
        Place();
    }

    private void MoveNextToCursor()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var newPosition = new Vector3(mousePosition.x + mouseOffset.x, mousePosition.y + mouseOffset.y, 0);
        transform.position = newPosition;
    }

    public void ReplaceWithBuilding()
    {
        if(!isInHand)
        {
            var buildingObj = Instantiate(buildingPrefab, transform.position, Quaternion.identity, transform.parent);
            var building = buildingObj.GetComponent<Building>();
        }
        Debug.Log($"Destroying {this.gameObject.name}");
        Destroy(this.gameObject);
    }
}
