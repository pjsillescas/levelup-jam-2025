using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private List<AudioSource> musicList; // Fuente de audio para la música
    [SerializeField] private List<AudioSource> SFXList; // Fuente de audio para los efectos de sonido
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Mantener el AudioManager entre escenas
        }
        else
        {
            Destroy(gameObject); // Si ya existe una instancia, destruir este objeto
        }
    }

    [SerializeField] private AudioMixer audioMixer; // Mezclador de audio principal

    //Método para establecer el volumen de la música del juego
    public void SetMusicVolume(float newVolume)
    {
        audioMixer.SetFloat("MusicVolume", newVolume);
    }

    //Método para establecer el volumen de los efectos de sonido del juego
    public void SetSFXVolume(float newVolume)
    {
        audioMixer.SetFloat("SFXVolume", newVolume);
    }

    //Método para parar la música
    public void StopMusic()
    {
        foreach (AudioSource music in musicList)
        {
            music.Stop(); // Detener todas las fuentes de música
        }
    }

    //Método para reproducir una música específica
    public void PlayMusic(int musicIndex)
    {
        if (musicIndex < 0 || musicIndex >= musicList.Count)
        {
            Debug.LogWarning("Índice de música fuera de rango: " + musicIndex);
            return;
        }
        if (musicList[musicIndex].isPlaying)
            return; // Si la música ya está reproduciéndose, no hacer nada
        
        StopMusic(); // Detener la música actual
        musicList[musicIndex].Play(); // Reproducir la música seleccionada
    }

    //Método para reproducir un efecto de sonido específico
    public void PlaySFX(int sfxIndex)
    {
        if (sfxIndex < 0 || sfxIndex >= SFXList.Count)
        {
            Debug.LogWarning("Índice de SFX fuera de rango: " + sfxIndex);
            return;
        }
        SFXList[sfxIndex].Play(); // Reproducir el efecto de sonido seleccionado
    }
}
