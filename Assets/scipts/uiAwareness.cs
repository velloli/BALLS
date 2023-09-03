using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class uiAwareness : MonoBehaviour
{

    private Coroutine displayCoroutine;
    private health healthscript;
    private GameObject currentHIghlighting;

    private Vector3 healthCanvas_origScale;

    private Vector3 almostZero = new Vector3(0.001f, 0.001f, 0.001f);

    public Canvas healthCanvas;
    public GameObject enemyHighlightUI_Dash;
    // Start is called before the first frame update
    void Start()
    {
        healthscript = gameObject.GetComponent<health>();
        healthCanvas_origScale = healthCanvas.gameObject.transform.localScale;
        healthCanvas.gameObject.transform.localScale = Vector3.zero;
        healthscript.healthUpdated += showHealthBar;
        enemyHighlightUI_Dash = Instantiate(enemyHighlightUI_Dash, new Vector3(0, 0, 0), Quaternion.identity);
        enemyHighlightUI_Dash.SetActive(false);
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

    public void highlightEnemy_Dash(GameObject enemy)
    {
        if(!currentHIghlighting ||  currentHIghlighting != enemy)
        {
            Debug.Log("Currently Highlighting "+ enemy.name);
            enemyHighlightUI_Dash.transform.rotation = Quaternion.identity;
            enemyHighlightUI_Dash.transform.position = enemy.transform.position;
            //enemyHighlightUI_Dash.transform.forward= enemy.transform.forward;
            enemyHighlightUI_Dash.SetActive(true);
            currentHIghlighting = enemy;
        }
        else
        {
            //still highlighting the same enemy
            enemyHighlightUI_Dash.transform.position = enemy.transform.position;
            enemyHighlightUI_Dash.transform.LookAt(GameManager.Instance.player.transform);
            //enemyHighlightUI_Dash

        }


    }

    private IEnumerator ShowAndHideCoroutine(float displayTime = 5.0f)
    {
        //Debug.Log("Started UI Coroutine");
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
