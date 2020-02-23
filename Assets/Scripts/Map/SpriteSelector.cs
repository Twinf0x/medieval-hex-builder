using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteSelector : MonoBehaviour
{
    public List<Sprite> options;

    private void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = options[Random.Range(0, options.Count)];
        Destroy(this);
    }
}
