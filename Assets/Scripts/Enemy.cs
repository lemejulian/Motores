using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class Enemy : MonoBehaviour
{

    [Header("Parámetros de vida")]
    public int maxHealth = 3;
    private int currentHealth;
    // 1. Añadimos esta variable para conectar el otro script
    public EnemyController controller;

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
            // 2. Ahora sí, esto llamará a la función Die que está en el otro script
            controller.Die();
                    
        }
    }

    
}

