using UnityEngine;
using UnityEngine.InputSystem;

public class ShooterInput : MonoBehaviour
{
    [Header("Configuración")]
    public Transform shootSpawn;
    public GameObject bulletPrefab;
    public Camera mainCam;
    public float bulletSpeed = 20f;
    public float fireRate = 0.5f;

    // --- NUEVO ---
    [Header("Físicas")]
    public LayerMask layerToIgnore; // Aquí seleccionaremos la capa "Player"
                                    // -------------

    private float nextFireTime;

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed && Time.time >= nextFireTime)
        {
            PerformShoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void PerformShoot()
    {
        // 1. Raycast desde el centro de la cámara
        Ray ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 targetPoint;

        // MODIFICADO: Añadimos ~layerToIgnore para que el rayo NO toque esa capa
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, ~layerToIgnore))
        {
            targetPoint = hit.point;
        }
        else
        {
            // Si no toca nada, disparar al horizonte
            targetPoint = ray.GetPoint(50f);
        }

        // 2. Calculamos la dirección de forma segura
        Vector3 dir = (targetPoint - shootSpawn.position).normalized;

        // --- SOLUCIÓN EXTRA ---
        // Si la bala sale "hacia abajo", forzamos a que no pueda tener ángulo negativo excesivo
        if (dir.y < -0.9f) dir = (mainCam.transform.forward).normalized;
        // -----------------------

        GameObject bullet = Instantiate(bulletPrefab, shootSpawn.position, Quaternion.LookRotation(dir));

        if (bullet.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.linearVelocity = dir * bulletSpeed;
        }

        Destroy(bullet, 3f);
    }
}