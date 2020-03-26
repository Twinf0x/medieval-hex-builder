using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    public List<Transform> movementPoints;
    public float panSpeed;
    public float distanceThreshold;

    private void Start()
    {
        Vector3 temp = movementPoints.First().position;
        transform.position = new Vector3(temp.x, temp.y, transform.position.z);

        StartCoroutine(MoveAround());
    }

    private IEnumerator MoveAround()
    {
        Vector3 nextTarget = transform.position;
        Vector3 direction = Vector3.zero;
        float distance = 0f;

        while(true)
        {
            distance = (nextTarget - transform.position).magnitude;

            if(distance < distanceThreshold)
            {
                nextTarget = movementPoints.ElementAt(Random.Range(0, movementPoints.Count)).position;
                direction = (nextTarget - transform.position).normalized;
            }

            transform.Translate(direction * panSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
