using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TileView : MonoBehaviour
{
    public int id;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }


    public void SetValue(int value)
    {
        id = value;

        sr.color = value switch
        {
            0 => Color.black,
            1 => new Color(1f, 0.2f, 0.2f),   
            2 => new Color(1f, 0.9f, 0.2f),  
            3 => new Color(0.2f, 1f, 0.2f),   
            4 => new Color(0.2f, 0.6f, 1f),  
            _ => Color.white
        };
    }

    public void MoveToLocal(Vector3 targetLocalPos, float duration = 0.25f)
    {
        StopAllCoroutines();
        StartCoroutine(MoveCoroutine(targetLocalPos, duration));
    }

    private IEnumerator MoveCoroutine(Vector3 targetPos, float duration)
    {
        Vector3 startPos = transform.localPosition;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(startPos, targetPos, t / duration);
            yield return null;
        }

        transform.localPosition = targetPos;
    }
}
