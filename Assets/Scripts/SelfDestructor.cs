using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructor : MonoBehaviour
{
    public float timer = 1f;

    private void Start()
    {
        StartCoroutine(SelfDestruction(timer));
    }

    private IEnumerator SelfDestruction(float timer)
    {
        yield return new WaitForSeconds(timer);

        Destroy(this.gameObject);
    }
}
