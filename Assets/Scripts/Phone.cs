using UnityEngine;
using UnityEngine.InputSystem;

public class Phone : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Arrastra aquí el panel que quieres desactivar")]
    public GameObject panelPhone;

    private bool isRange = false;

    private void OnTriggerEnter(Collider other)
    {
        // Asegúrate de que el jugador tenga el tag "Player"
        if (other.CompareTag("Player")) isRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) isRange = false;
    }

    void Update()
    {
        // Validación de null para evitar errores si el script está activo sin panel
        if (isRange && Keyboard.current.eKey.wasPressedThisFrame)
        {
            AnswerCall();
        }
    }

    void AnswerCall()
    {
        if (panelPhone != null)
        {
            // Usamos ! para invertir el estado, por si quieres que también se vuelva a activar
            panelPhone.SetActive(!panelPhone.activeSelf);
            Debug.Log("Interacción con el teléfono ejecutada.");
        }
        else
        {
            Debug.LogWarning("No asignaste el panel en el Inspector.");
        }
    }
}