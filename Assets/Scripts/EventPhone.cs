using UnityEngine;

public class EventPhone : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject panelPhone; // Arrastra aquí el panel

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            panelPhone.SetActive(true); // Activa el cartel al entrar
        }
    }
}
