using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Settings")]
    public float visionRange = 10f;
    public float visionAngle = 90f;
    public float attackRange = 2f;
    public float speed = 3.5f;

    [Header("References")]
    public Transform player;
    public Animator animator;
    public Rigidbody rb;

    private bool isChasing = false;
    private bool isAttacking = false;
    public bool isDead = false;
    //private bool isWalking = false; // Estado para cuando sale del rango

    void Update()
    {
        DetectPlayer();
        if (isDead) return; // Si está muerto, no hagas nada más
        DetectPlayer();
    }

    void FixedUpdate()
    {
        HandleEnemyState();
        if (isDead) return; // Si está muerto, no hagas nada más
        DetectPlayer();
    }

    private void DetectPlayer()
    {
        // 1. Prioridad absoluta: Si está muerto, bloqueamos todo
        if (isDead)
        {
            isChasing = false;
            isAttacking = false;
            rb.linearVelocity = Vector3.zero;
            return; // Salimos de la función aquí mismo
        }

        Vector3 direction = player.position - transform.position;
        float distance = direction.magnitude;

        // 2. Lógica de persecución (solo si está vivo)
        if (distance < visionRange)
        {
            Vector3 directionFlat = new Vector3(direction.x, 0, direction.z);
            if (Vector3.Angle(transform.forward, directionFlat) < visionAngle / 2f)
            {
                isChasing = true;
                return;
            }
        }

        // 3. Lógica de salida de rango
        if (distance > visionRange * 1.2f)
        {
            isChasing = false;
            isAttacking = false;
            // OJO: NUNCA pongas isDead = false aquí. 
            // Si el enemigo murió, isDead debe mantenerse en true hasta que el objeto se destruya.
        }
    }

    private void HandleEnemyState()
    {
        // 1. BLINDAJE: Si está muerto, no hacemos nada de lógica de movimiento
        if (isDead)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        // 2. Lógica de Idle (cuando no persigue)
        if (!isChasing)
        {
            SetAnimationStates(false, false, false);
            rb.linearVelocity = Vector3.zero;
            return;
        }

        // 3. Lógica de Persecución y Ataque
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance > attackRange)
        {
            // Persecución
            isAttacking = false;
            Vector3 dir = (player.position - transform.position).normalized;
            dir.y = 0;

            rb.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 5f * Time.fixedDeltaTime));
            rb.MovePosition(transform.position + dir * speed * Time.fixedDeltaTime);

            SetAnimationStates(true, false, false);
        }
        else
        {
            // Ataque
            isAttacking = true;
            rb.linearVelocity = Vector3.zero;
            SetAnimationStates(true, true, false);
        }
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        // 1. Limpiamos las variables de persecución/ataque del Animator
        animator.SetBool("isChasing", false);
        animator.SetBool("isAttacking", false);

        // 2. Activamos la muerte
        animator.SetBool("isDead", true);

        // 3. Detenemos física
        rb.linearVelocity = Vector3.zero;
        Destroy(gameObject, 0.5f);
    }

    private void SetAnimationStates(bool chasing, bool attacking, bool walking)
    {
        // Solo enviamos cambios si el valor es distinto para optimizar
        if (animator.GetBool("isChasing") != chasing) animator.SetBool("isChasing", chasing);
        if (animator.GetBool("isAttacking") != attacking) animator.SetBool("isAttacking", attacking);
        //if (animator.GetBool("isWalking") != walking) animator.SetBool("isWalking", walking);
    }
}
