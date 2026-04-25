using UnityEngine;
using TMPro;

public class BossHouseTrigger : MonoBehaviour
{
    [Header("Referencia al texto UI")]
    public TextMeshProUGUI dialogueText;

    [Header("Mensaje a mostrar")]
    public string message = "¿Dónde está este boludo?";

    // Bandera para controlar que solo se muestre una vez
    private bool alreadyShown = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !alreadyShown)
        {
            alreadyShown = true; // marcamos que ya se mostró
            dialogueText.gameObject.SetActive(true);
            dialogueText.text = message;
            StartCoroutine(HideTextAfterSeconds(3f)); // mostrar por 3 segundos
        }
    }

    private System.Collections.IEnumerator HideTextAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        dialogueText.gameObject.SetActive(false);
    }
}
