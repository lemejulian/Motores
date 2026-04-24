using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponController : MonoBehaviour
{
    public Transform shootSpawn;
    public bool shooting = false;

    public GameObject bulletPrefab; // 

    void Update()
    {
        Debug.DrawLine(shootSpawn.position, shootSpawn.forward * 10f, Color.red);
        Debug.DrawLine(Camera.main.transform.position, Camera.main.transform.forward * 10f, Color.blue);

        RaycastHit cameraHit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out cameraHit))
        {
            Vector3 shootDirection = cameraHit.point - shootSpawn.position;
            shootSpawn.rotation = Quaternion.LookRotation(shootDirection); // ✅ corregido

            if (Input.GetKeyDown(KeyCode.Mouse0)) // ✅ corregido
            {
                Shoot();
            }
        }
    }

    public void Shoot()
    {
        Instantiate(bulletPrefab, shootSpawn.position, shootSpawn.rotation); // ✅ corregido
    }
}
