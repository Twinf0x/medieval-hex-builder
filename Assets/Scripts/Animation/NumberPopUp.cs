using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumberPopUp : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image coin; 
    public AnimationCurve yOffsetCurve;
    public AnimationCurve alphaCurve;
    public float duration;

    private void Start()
    {
        StartCoroutine(Animate());
    }
    
    public IEnumerator Animate()
    {
        float timer = 0f;
        float percentage = timer / duration;

        float yOffset = yOffsetCurve.Evaluate(percentage);
        float alpha = alphaCurve.Evaluate(percentage);
        transform.localPosition = new Vector3(transform.localPosition.x, yOffset, transform.localPosition.z);
        Color tempColor = text.color;
        tempColor.a = alpha;
        text.color = tempColor;
        tempColor = coin.color;
        tempColor.a = alpha;
        coin.color = tempColor;

        while(timer < duration)
        {
            timer += Time.deltaTime;
            percentage = timer / duration;

            yOffset = yOffsetCurve.Evaluate(percentage);
            alpha = alphaCurve.Evaluate(percentage);
            transform.localPosition = new Vector3(transform.localPosition.x, yOffset, transform.localPosition.z);
            tempColor = text.color;
            tempColor.a = alpha;
            text.color = tempColor;
            tempColor = coin.color;
            tempColor.a = alpha;
            coin.color = tempColor;

            yield return null;
        }

        Destroy(this.gameObject);
    }
}
