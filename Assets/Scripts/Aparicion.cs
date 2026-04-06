using System.Collections;
using UnityEngine;

public class AparicionNina : MonoBehaviour
{
    public GameObject nina;
    public AudioSource audioJugador; // 🔊 voz "te estoy viendo"

    private bool activado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !activado)
        {
            activado = true;

            // 🔊 Reproducir la voz
            audioJugador.Play();

            StartCoroutine(MostrarNina());

            GetComponent<Collider>().enabled = false;
        }
    }

    IEnumerator MostrarNina()
    {
        nina.SetActive(true);
        yield return new WaitForSeconds(5f);
        nina.SetActive(false);
    }
}