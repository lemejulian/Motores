using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Parámetros de vida")]
    public int maxHealth = 3;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("⚔️ " + gameObject.name + " recibió " + amount + " de daño. Vida restante: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("☠️ " + gameObject.name + " murió.");
        Destroy(gameObject);
    }
}

