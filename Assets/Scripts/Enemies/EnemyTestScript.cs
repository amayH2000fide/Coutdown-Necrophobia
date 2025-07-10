using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTestScript : MonoBehaviour
{
    [SerializeField] private int maxHealth = 200;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has been destroyed.");
        Destroy(gameObject);
    }
}
