using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAnimations : MonoBehaviour
{
    public static SimpleAnimations instance;

    public AnimationCurve wobbleCurve;
    public AnimationCurve stretchCurve;
    public AnimationCurve squashCurve;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public IEnumerator Wobble(Transform transform, float duration, float factor = 1f, Action callback = null)
    {
        float timer = 0f;
        float percentage = timer / duration;
        float curveFactor = wobbleCurve.Evaluate(percentage) * factor;
        var initialScale = transform.localScale;

        transform.localScale = initialScale * curveFactor;

        while(timer < duration)
        {
            timer += Time.deltaTime;
            percentage = timer / duration;
            curveFactor = wobbleCurve.Evaluate(percentage) * factor;
            transform.localScale = initialScale * curveFactor;
            yield return null;
        }

        transform.localScale = initialScale;

        callback?.Invoke();
    }

    public IEnumerator Stretch(Transform transform, float duration, float factor = 1f, Action callback = null)
    {
        float timer = 0f;
        float percentage = timer / duration;
        float stretchFactor = stretchCurve.Evaluate(percentage) * factor;
        float squashFactor = squashCurve.Evaluate(percentage) * factor;

        var initialX = transform.localScale.x;
        var initialY = transform.localScale.y;
        var initialZ = transform.localScale.z;
        var currentScale = new Vector3(initialX * squashFactor, initialY * stretchFactor, initialZ);

        transform.localScale = currentScale;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            percentage = timer / duration;
            stretchFactor = stretchCurve.Evaluate(percentage) * factor;
            squashFactor = squashCurve.Evaluate(percentage) * factor;

            currentScale = new Vector3(initialX * squashFactor, initialY * stretchFactor, initialZ);

            transform.localScale = currentScale;

            yield return null;
        }

        transform.localScale = new Vector3(initialX, initialY, initialZ);

        callback?.Invoke();
    }

    public IEnumerator Squash(Transform transform, float duration, float factor = 1f, Action callback = null)
    {
        float timer = 0f;
        float percentage = timer / duration;
        float stretchFactor = stretchCurve.Evaluate(percentage) * factor;
        float squashFactor = squashCurve.Evaluate(percentage) * factor;

        var initialX = transform.localScale.x;
        var initialY = transform.localScale.y;
        var initialZ = transform.localScale.z;
        var currentScale = new Vector3(initialX * stretchFactor, initialY * squashFactor, initialZ);

        transform.localScale = currentScale;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            percentage = timer / duration;
            stretchFactor = stretchCurve.Evaluate(percentage) * factor;
            squashFactor = squashCurve.Evaluate(percentage) * factor;

            currentScale.x = initialX * stretchFactor;
            currentScale.y = initialY * squashFactor;

            transform.localScale = currentScale;

            yield return null;
        }

        transform.localScale = new Vector3(initialX, initialY, initialZ);

        callback?.Invoke();
    }
}
