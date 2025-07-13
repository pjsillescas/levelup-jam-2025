using UnityEngine;
using System.Collections;

public class SFXTrigger : MonoBehaviour
{
    private AudioSource SFX;
    private Coroutine fadeCoroutine;
    public float fadeDuration = 1.0f; // Duración del fade in/out
    public float maxVolume = 1.0f; // Volumen máximo configurable

    private void Awake()
    {
        SFX = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeVolume(1.0f)); // Fade in al volumen máximo
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeVolume(0.0f)); // Fade out al volumen mínimo
        }
    }

    private IEnumerator FadeVolume(float targetVolume)
    {
        float startVolume = SFX.volume;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            SFX.volume = Mathf.Lerp(startVolume, targetVolume * maxVolume, elapsedTime / fadeDuration);
            yield return null;
        }

        SFX.volume = targetVolume * maxVolume;

        if (targetVolume == 0.0f)
        {
            SFX.Stop(); // Detener el audio completamente si el volumen es 0
        }
        else if (!SFX.isPlaying)
        {
            SFX.Play(); // Reproducir el audio si no está sonando
        }
    }
}
