using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    // Creamos la instancia para el Singleton
    private static OptionsManager instance;

    [SerializeField] float musicVolume = 1.0f; // Volumen del juego

    [SerializeField] float SFXVolume = 1.0f; // Volumen del juego

    [SerializeField] Scrollbar musicVolumeSlider, SFXVolumeSlider; // Sliders para ajustarlo al valor inicial
    [SerializeField] List<Buttons> panelButtons; // Lista de botones del panel de opciones

    // Creamos la instancia Singleton
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Si ya existe una instancia, destruir este objeto
        }


    }

    void Start()
    {
        SetSliders(); // Establecer los sliders y el dropdown al inicio
    }


    //Método para establecer el volumen de la música del juego y transmitirlo a AudioManager
    public void SetMusicVolume(float newVolume)
    {
        musicVolume = newVolume;
        // Convertir de forma logarítmica el valor del slider (0-1) a decibelios (-80 a 0)
        float volumeInDecibels = musicVolume > 0 ? 20f * Mathf.Log10(musicVolume) : -80f;
        AudioManager.instance.SetMusicVolume(volumeInDecibels); // Transmitir el volumen al AudioManager
    }

    //Método para establecer el volumen de la música del juego y transmitirlo a AudioManager
    public void SetFXVolume(float newVolume)
    {
        SFXVolume = newVolume;
        // Convertir de forma logarítmica el valor del slider (0-1) a decibelios (-80 a 0)
        float volumeInDecibels = SFXVolume > 0 ? 20f * Mathf.Log10(SFXVolume) : -80f;
        AudioManager.instance.SetSFXVolume(volumeInDecibels); // Transmitir el volumen al AudioManager
    }

    //Método para setear los sliders de las opciones en el menú y la opción de dificultad
    public void SetSliders()
    {
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = musicVolume;
        }
        else
        {
            Debug.LogWarning("Music Volume Slider is not assigned in the inspector.");
        }

        if (SFXVolumeSlider != null)
        {
            SFXVolumeSlider.value = SFXVolume;
        }
        else
        {
            Debug.LogWarning("SFX Volume Slider is not assigned in the inspector.");
        }
    }

    //Método para ajustar bien los botones del panel de opciones
    public void AdjustButtons()
    {
        foreach (Buttons button in panelButtons)
        {
            button.AdjustButton(); // Ajustar cada botón del panel de opciones
        }
    }

    //Método para cerrar el menú de opciones
    public void CloseMenu()
    {
        gameObject.SetActive(false); // Desactivar el menú de opciones
    }

    //Método para abrir el menú de opciones
    public void OpenMenu()
    {
        gameObject.SetActive(true); // Activar el menú de opciones
        AdjustButtons(); // Ajustar los botones del panel de opciones
        SetSliders(); // Establecer los sliders al valor actual
    }

    //Método para obtener si el menú de opciones está abierto
    public bool IsMenuOpen()
    {
        return gameObject.activeSelf; // Retornar si el menú de opciones está activo
    }
}
