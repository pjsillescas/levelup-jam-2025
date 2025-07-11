using UnityEngine;

public class SFXTrigger : MonoBehaviour
{
    private AudioSource SFX;
    private void Awake()
    {
        SFX = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SFX.Play(); // Reproducir el efecto de sonido al entrar en el trigger
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SFX.Stop(); // Detener el efecto de sonido al salir del trigger
        }
    }
}
