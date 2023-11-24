using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tween_scaleBounce : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 originalScale;
    public float scaleAmount = 0.2f;
    public float scaleDuration = 0.5f;

    private void Start()
    {
        originalScale = transform.localScale;
        StartScaling();
    }

    private void StartScaling()
    {
        Vector3 targetScale = originalScale + Vector3.one * scaleAmount;

        transform.DOScale(targetScale, scaleDuration)
            .SetEase(Ease.InOutExpo)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
