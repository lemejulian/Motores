using UnityEngine;
using FMODUnity;

public class PlayerFootsteps : MonoBehaviour
{
    [Header("FMOD Events")]
    public string footstepWoodEvent = "event:/FootstepsWood";
    public string footstepGravelEvent = "event:/FootstepsGravel";

    [Header("Ground Detection")]
    public LayerMask groundMask;
    public float rayDistance = 2f; // más largo para pisos interiores
    private float rayOffset = 0.3f; // offset desde pivot

    [Header("Step Timing")]
    public float stepInterval = 0.5f;
    private float stepTimer;

    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        HandleFootsteps();
    }

    void HandleFootsteps()
    {
        if (!IsGrounded()) return;

        // Calcula velocidad real del jugador
        float speed = (transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = transform.position;

        if (speed > 0.1f)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                PlayFootstep();
                stepTimer = stepInterval / Mathf.Clamp(speed, 1f, 6f);
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    void PlayFootstep()
    {
        string surface = GetSurface();

        // Debug para confirmar superficie y evento
        Debug.Log("Detected Surface: " + surface);

        string evt = (surface == "Wood") ? footstepWoodEvent : footstepGravelEvent;
        Debug.Log("Playing Event: " + evt);

        RuntimeManager.PlayOneShot(evt, transform.position);
    }

    bool IsGrounded()
    {
        return Physics.Raycast(
            transform.position + Vector3.up * rayOffset,
            Vector3.down,
            rayDistance,
            groundMask
        );
    }

    string GetSurface()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position + Vector3.up * rayOffset, Vector3.down, out hit, rayDistance, groundMask))
        {
            if (hit.collider.CompareTag("Wood"))
                return "Wood";
            if (hit.collider.CompareTag("Gravel"))
                return "Gravel";

            Debug.Log("Hit object with tag: " + hit.collider.tag);
        }

        return "Gravel"; // fallback
    }
}