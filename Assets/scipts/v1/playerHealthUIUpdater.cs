using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class playerHealthUIUpdater : MonoBehaviour
{
    // Start is called before the first frame update
    health healthscript;
    public GameObject healthUI;
    public GameObject armorUI;

    void Start()
    {
        healthscript = gameObject.GetComponent<health>();
        healthscript.healthUpdated += updateHealthUI;


    }

    private void FixedUpdate()
    {
        //GetComponentInParent<Canvas>().transform.LookAt(Camera.main.transform.position);
    }

    // Update is called once per frame

    private void updateHealthUI()
    {
        //healthUI.GetComponent<Image>().fillAmount = healthscript.currentHealth / healthscript.maxHealth;
        float targetFillAmount = healthscript.currentHealth / healthscript.maxHealth;
        healthUI.GetComponent<Image>().DOFillAmount(targetFillAmount, 0.5f)
            .SetEase(Ease.OutSine);

    }
        
}
