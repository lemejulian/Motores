using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sonidoGrito;   // 🔊 Sonido al verte
    private bool sonidoReproducido = false;

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
    //private bool isWalking = false; // Estado para cuando sale del rango

   
    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        DetectPlayer();
    }

    void FixedUpdate()
    {
        HandleEnemyState();
    }

   
    private void DetectPlayer()
    {
        Vector3 direction = player.position - transform.position;
        float distance = direction.magnitude;

        // Si el jugador está cerca y dentro del ángulo de visión
        if (distance < visionRange)
        {
            Vector3 directionFlat = new Vector3(direction.x, 0, direction.z);
            if (Vector3.Angle(transform.forward, directionFlat) < visionAngle / 2f)
            {
                isChasing = true;
                // 🔊 Grito al verte
                if (!sonidoReproducido)
                {
                    audioSource.PlayOneShot(sonidoGrito);
                    sonidoReproducido = true;
                }
                return;
            }
        }

        // Si sale de rango de persecución, desactivamos persecución
        if (distance > visionRange * 1.2f)
        {
            isChasing = false;
            isAttacking = false;
            sonidoReproducido = false;
        }
    }

    private void HandleEnemyState()
    {
        
        if (!isChasing)
        {
            SetAnimationStates(false, false, true); // Idle/Walk
            rb.linearVelocity = Vector3.zero;
            return;
        }

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

    private void SetAnimationStates(bool chasing, bool attacking, bool walking)
    {
        // Solo enviamos cambios si el valor es distinto para optimizar
        if (animator.GetBool("isChasing") != chasing) animator.SetBool("isChasing", chasing);
        if (animator.GetBool("isAttacking") != attacking) animator.SetBool("isAttacking", attacking);
        //if (animator.GetBool("isWalking") != walking) animator.SetBool("isWalking", walking);
    }
}

