using UnityEngine;
using UnityEngine.InputSystem; // Necesario para el New Input System

public class Phone : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Arrastra aquí el panel que quieres desactivar")]
    public GameObject panelPhone;

    private bool isRange = false;

    // Actualizamos el estado de cercanía mediante triggers
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) isRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) isRange = false;
    }

    void Update()
    {
        // Detectamos si el jugador presiona E estando en rango
        if (isRange && Keyboard.current.eKey.wasPressedThisFrame)
        {
            AnswerCall();
        }
    }

    void AnswerCall()
    {
        if (panelPhone != null)
        {
            panelPhone.SetActive(false);
            Debug.Log("Llamada atendida: Panel desactivado correctamente.");
        }
        else
        {
            Debug.LogWarning("El panelCartel no está asignado en el Inspector del objeto Phone.");
        }
    }
}