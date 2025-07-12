using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class AudiosSFX
{
    public string audioName; // Nombre del efecto de sonido
    public List<AudioClip> audioClips; // Lista de clips de audio para efectos de sonido
    public AudioSource audioSource; // Fuente de audio para reproducir el efecto de sonido
}
