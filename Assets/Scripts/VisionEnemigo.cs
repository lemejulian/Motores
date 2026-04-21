using UnityEngine;

public class VisionEnemigo : MonoBehaviour
{
    public Transform player;
    public AudioSource audioSource;
    public AudioClip sonidoGrito;   // 🔊 Sonido al verte

    public float visionRange = 8f;
    public float visionAngle = 90f;
    public float attackRange = 2f;
    public float speed = 3f;
    public float attackCooldown = 1f;

    private float lastAttackTime;
    private bool isChasing = false;
    private bool sonidoReproducido = false;

    private Rigidbody rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        Vector3 direction = player.position - transform.position;
        float distance = direction.magnitude;

        if (!isChasing)
        {
            if (distance < visionRange)
            {
                float angle = Vector3.Angle(transform.forward, direction);
                if (angle < visionAngle / 2f)
                {
                    isChasing = true;

                    // 🔊 Grito al verte
                    if (!sonidoReproducido)
                    {
                        audioSource.PlayOneShot(sonidoGrito);
                        sonidoReproducido = true;
                    }

                    animator.SetBool("isChasing", true);
                }
            }
        }

        if (isChasing)
        {
            Vector3 objetivo = player.position;
            objetivo.y = transform.position.y;

            
            Vector3 direccion = player.position - transform.position;
            direccion.y = 0;
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, rotacionObjetivo, 5f * Time.deltaTime));

            if (distance > attackRange)
            {
                rb.MovePosition(transform.position + direccion.normalized * speed * Time.deltaTime);
                animator.SetBool("isAttacking", false);
                animator.SetBool("isWalking", true);
            }
            else
            {
                Attack();
            }

            if (distance > visionRange * 1.5f)
            {
                isChasing = false;
                animator.SetBool("isChasing", false);
                animator.SetBool("isWalking", false);
                animator.SetBool("isAttacking", false);
                sonidoReproducido = false; 
            }
        }
    }

    void Attack()
    {
        if (Time.time > lastAttackTime + attackCooldown)
        {
            Debug.Log("¡Ataque!");

            animator.SetBool("isAttacking", true);
            animator.SetBool("isChasing", false);

            lastAttackTime = Time.time;
        }
    }
}

/*using UnityEngine;

public class VisionEnemigo : MonoBehaviour
{
    // ... (variables públicas iguales)

    private bool isChasing = false;
    private bool isAttacking = false; // Nueva variable de control
    private Rigidbody rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        Vector3 direction = player.position - transform.position;
        float distance = direction.magnitude;

        // 1. Lógica de Detección (Sensor)
        if (!isChasing && distance < visionRange)
        {
            Vector3 direccionPlana = new Vector3(direction.x, 0, direction.z);
            if (Vector3.Angle(transform.forward, direccionPlana) < visionAngle / 2f)
            {
                isChasing = true;
                animator.SetBool("isChasing", true);
            }
        }

        // 2. Lógica de Pérdida de interés
        if (isChasing && distance > visionRange * 1.5f)
        {
            isChasing = false;
            isAttacking = false;
            animator.SetBool("isChasing", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", false);
        }
    }

    void FixedUpdate()
    {
        if (!isChasing) return;

        float distance = Vector3.Distance(player.position, transform.position);
        Vector3 direccion = (player.position - transform.position);
        direccion.y = 0;

        // 3. Control de Estados (Motor)
        if (distance > attackRange)
        {
            // Persecución
            isAttacking = false;
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direccion), 5f * Time.fixedDeltaTime));
            rb.MovePosition(transform.position + direccion.normalized * speed * Time.fixedDeltaTime);
            
            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);
        }
        else
        {
            // Ataque
            isAttacking = true;
            rb.velocity = Vector3.zero; // Frenamos la física
            animator.SetBool("isWalking", false);
            Attack();
        }
    }

    void Attack()
    {
        if (Time.time > lastAttackTime + attackCooldown)
        {
            animator.SetBool("isAttacking", true);
            lastAttackTime = Time.time;
        }
    }
}*/