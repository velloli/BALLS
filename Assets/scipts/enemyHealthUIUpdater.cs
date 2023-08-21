using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyHealthUIUpdater : MonoBehaviour
{
    // Start is called before the first frame update
    health healthscript;
    void Start()
    {
        healthscript = GetComponentInParent<health>();
        healthscript.healthUpdated += updateHealthUI;


    }

    private void FixedUpdate()
    {
        GetComponentInParent<Canvas>().transform.LookAt(Camera.main.transform.position);
    }

    // Update is called once per frame

    private void updateHealthUI()
    {
        GetComponent<Image>().fillAmount = healthscript.currentHealth / healthscript.maxHealth;
    }
        
}
