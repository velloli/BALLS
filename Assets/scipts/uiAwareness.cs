using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class uiAwareness : MonoBehaviour
{
    private Coroutine displayCoroutine;
    private health healthscript;

    private Vector3 healthCanvas_origScale;

    private Vector3 almostZero = new Vector3(0.001f, 0.001f, 0.001f);

    public Canvas healthCanvas;
    // Start is called before the first frame update
    void Start()
    {
        healthscript = gameObject.GetComponent<health>();
        healthCanvas_origScale = healthCanvas.gameObject.transform.localScale;
        healthCanvas.gameObject.transform.localScale = Vector3.zero;
        healthscript.healthUpdated += showHealthBar;
    }

    void showHealthBar()
    {
        
            ShowAndHideCanvas();
        
    }
    public void ShowAndHideCanvas(float displayTime = 5.0f)
    {
        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
        }
        displayCoroutine = StartCoroutine(ShowAndHideCoroutine(displayTime));
    }

    private IEnumerator ShowAndHideCoroutine(float displayTime = 5.0f)
    {
        Debug.Log("Started UI Coroutine");
        healthCanvas.enabled = true;

        healthCanvas.transform.DOScale(healthCanvas_origScale, 0.5f)
            .SetEase(Ease.OutBack);

        yield return new WaitForSeconds(displayTime);

        healthCanvas.transform.DOScale(almostZero, 0.5f)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                healthCanvas.enabled = false;
            });

        // Wait for the second animation to complete before disabling the canvas
        yield return new WaitForSeconds(0.5f);

        healthCanvas.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
