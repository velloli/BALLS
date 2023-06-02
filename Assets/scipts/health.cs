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

    public void TakeDamage(float amount)
    {
        healthUpdated.Invoke();
        if (currentHealth > 0f)
        {
            currentHealth -= amount;

            if (currentHealth <= 0f && !isDead)
            {
                Die();
            }
        }

    }

    public void Heal(float amount)
    {
        healthUpdated.Invoke();

        if (currentHealth > 0f)
        {
            currentHealth += amount;

            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
    }

    public void Die()
    {
        isDead = true;
        // Play death animation or sound effect
        Destroy(gameObject);
    }
}
