using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class health : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxHealth = 100f;
    public float currentHealth;
    public float regenRate = 0f; //health points per second
    public float damageRate = 0f; //health points per second

    public delegate void EnemyHealthHandler();
    public event EnemyHealthHandler healthUpdated;



    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        //Reduce health over time
        if (damageRate > 0f && currentHealth > 0f)
        {
            currentHealth -= damageRate * Time.deltaTime;

            if (currentHealth <= 0f && !isDead)
            {
                //TODO this is just for debugging purposes
                if(!gameObject.CompareTag("Player"))
                Die();
            }
        }

        //Increase health over time
        if (regenRate > 0f && currentHealth < maxHealth)
        {
            currentHealth += regenRate * Time.deltaTime;

            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
    }

    public float TakeDamage(float amount)
    {
        if (currentHealth > 0f)
        {
            currentHealth -= amount;

            if (currentHealth <= 0f && !isDead)
            {
                if (!gameObject.CompareTag("Player"))
                    Die();
                return 0.0f;
            }
        }
        healthUpdated.Invoke();
        return currentHealth;

    }

    public void Heal(float amount)
    {

        if (currentHealth > 0f)
        {
            currentHealth += amount;

            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
        healthUpdated.Invoke();
    }

    public void Die()
    {
        isDead = true;
        // Play death animation or sound effect
        Destroy(gameObject);
    }
}
