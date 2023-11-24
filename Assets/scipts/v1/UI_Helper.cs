using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Helper : MonoBehaviour
{
    public GameObject Debug_IsTouchingEnemy;
    public GameObject Debug_IsGrounded;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug_IsTouchingEnemy.GetComponent<TextMeshProUGUI>().text = "Is Touching Enemy: " + GameManager.Instance.player.GetComponent<Schmovement>().isTouchingEnemy;
        Debug_IsGrounded.GetComponent<TextMeshProUGUI>().text ="Is Grounded: "+ GameManager.Instance.player.GetComponent<Schmovement>().isGrounded;
    }
}
