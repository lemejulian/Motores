using UnityEngine;


public class BossHouseTrigger : MonoBehaviour
{
    [Header("Referencia al texto UI")]
    public GameObject stop;

   
    // Bandera para controlar que solo se muestre una vez
    private bool alreadyShown = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !alreadyShown)
        {
            alreadyShown = true; // marcamos que ya se mostró
            stop.gameObject.SetActive(true);
            StartCoroutine(HideTextAfterSeconds(3f)); // mostrar por 3 segundos
        }
    }

    private System.Collections.IEnumerator HideTextAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        stop.gameObject.SetActive(false);
    }
}
