using UnityEngine;

public class ClosedDoor : MonoBehaviour
{
    public GameObject door; // Arrastra aquí el panel o la puerta que quieres activar

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        // Esto se ejecuta cuando algo entra en el trigger
        if (other.CompareTag("Player"))
        {
            door.SetActive(true); // Activa el objeto
        }
    }
}
