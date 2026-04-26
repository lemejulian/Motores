using UnityEngine;
using UnityEngine.UI; 
using TMPro;          
using System.Collections;

public class Shot : MonoBehaviour
{
    [Header("Referencias")]
    public Transform shootSpawn;
    public GameObject bulletPrefab;
    public Camera cam; // asigná la cámara del jugador en el Inspector

    [Header("Parámetros de disparo")]
    public float shotForce = 50f;
    public float shotRate = 0.5f;

    [Header("Cargador")]
    public int maxAmmo = 6;
    public int currentAmmo = 3;
    public float reloadTime = 2f;
    private bool isReloading = false;

    private float shotRateTime = 0f;

    [Header("UI")]
    public Text ammoText;        
    public TextMeshProUGUI ammoTMP; 

    void Start()
    {
        UpdateAmmoUI();
    }

    public void Shoot()
    {
        if (isReloading) return;
        if (currentAmmo <= 0) return;

        if (Time.time > shotRateTime)
        {
            GameObject newBullet = Instantiate(bulletPrefab, shootSpawn.position, shootSpawn.rotation);

            Rigidbody bulletRb = newBullet.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                // disparar en la dirección de la cámara
                bulletRb.AddForce(cam.transform.forward * shotForce, ForceMode.Impulse);
            }

            Destroy(newBullet, 5f);

            currentAmmo--;
            shotRateTime = Time.time + shotRate;

            UpdateAmmoUI();
        }
    }

    public void StartReload()
    {
        if (!isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        UpdateAmmoUI();
    }

    private void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = currentAmmo + " / " + maxAmmo;

        if (ammoTMP != null)
            ammoTMP.text = currentAmmo + " / " + maxAmmo;
    }
}
