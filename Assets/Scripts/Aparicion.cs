using System.Collections;
using UnityEngine;

public class AparicionNina : MonoBehaviour
{
    public GameObject nina;
    public AudioSource audioAtras; // 🔊 sonido detrás

    private bool activado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !activado)
        {
            activado = true;

            // 🔊 reproducir sonido atrás
            audioAtras.Play();

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