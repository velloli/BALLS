using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyHealthUIUpdater : MonoBehaviour
{
    public Color healthyColor = Color.green;
    public Color damagedColor = Color.yellow;
    public Color criticalColor = Color.red;

    health healthscript;
    Image healthImage;

    void Start()
    {
        healthscript = GetComponentInParent<health>();
        healthscript.healthUpdated += updateHealthUI;
        healthImage = GetComponent<Image>();
    }

    private void FixedUpdate()
    {
        GetComponentInParent<Canvas>().transform.LookAt(Camera.main.transform.position);
    }

    private void updateHealthUI()
    {
        float healthPercentage = healthscript.currentHealth / healthscript.maxHealth;

        if (healthPercentage > 0.5f)
        {
            healthImage.color = Color.Lerp(damagedColor, healthyColor, (healthPercentage - 0.5f) * 2);
        }
        else
        {
            healthImage.color = Color.Lerp(criticalColor, damagedColor, healthPercentage * 2);
        }

        healthImage.fillAmount = healthPercentage;
    }
}
