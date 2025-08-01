using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Option : MonoBehaviour
{
    [SerializeField] private TMP_Text optionText; // Texto de la opción
    [SerializeField] private GameObject optionSetting; // Configurable de la opción
    [SerializeField] private float stepValue = 0.1f; // Valor de paso para sliders
    [SerializeField] private bool isActive; // Indica si la opción está activa
    enum OptionType { Scrollbar, Dropdown } // Tipos de opciones
    [SerializeField] private OptionType optionType; // Tipo de opción

    void Start()
    {
        isActive = false; // Inicialmente la opción no está activa
    }

    //Método para marcar la opción
    public virtual void MarkOption()
    {
        optionText.color = Color.yellow; // Cambiar el color del texto al seleccionar la opción
    }

    //Método para desmarcar la opción
    public virtual void UnmarkOption()
    {
        optionText.color = Color.white; // Cambiar el color del texto al desmarcar la opción
    }

    //Método para gestionar la acción de izquierda
    public virtual void OnLeftOptionAction()
    {
        switch (optionType)
        {
            case OptionType.Scrollbar:
                Scrollbar scrollbar = optionSetting.GetComponent<Scrollbar>();
                scrollbar.value -= stepValue; // Reducir el valor del scrollbar
                scrollbar.value = Mathf.Clamp(scrollbar.value, 0f, 1f); // Asegurar que el valor esté dentro del rango

                break;
            case OptionType.Dropdown:
                break;
        }
    }

    //Método para gestionar la acción de derecha
    public virtual void OnRightOptionAction()
    {
        switch (optionType)
        {
            case OptionType.Scrollbar:
                Scrollbar scrollbar = optionSetting.GetComponent<Scrollbar>();
                scrollbar.value += stepValue; // Aumentar el valor del scrollbar
                scrollbar.value = Mathf.Clamp(scrollbar.value, 0f, 1f); // Asegurar que el valor esté dentro del rango

                break;
            case OptionType.Dropdown:

                break;
        }
    }

    //Método para gestionarla accion de abajo
    public virtual void OnDownOptionAction()
    {
        switch (optionType)
        {
            case OptionType.Scrollbar:
                break;
            case OptionType.Dropdown:
                TMP_Dropdown dropdown = optionSetting.GetComponent<TMP_Dropdown>();
                if (dropdown != null && dropdown.options.Count > 0)
                {
                    int nextIndex = (dropdown.value + 1) % dropdown.options.Count;
                    dropdown.value = nextIndex;
                    dropdown.RefreshShownValue(); // Actualizar el texto mostrado
                }
                break;
        }
    }

    //Método para gestionar la acción de arriba en dropdown
    public virtual void OnUpOptionAction()
    {
        switch (optionType)
        {
            case OptionType.Dropdown:
                TMP_Dropdown dropdown = optionSetting.GetComponent<TMP_Dropdown>();
                if (dropdown != null && dropdown.options.Count > 0)
                {
                    int prevIndex = (dropdown.value - 1 + dropdown.options.Count) % dropdown.options.Count;
                    dropdown.value = prevIndex;
                    dropdown.RefreshShownValue();
                }
                break;
        }
    }

    // Indica si el dropdown está abierto y activo desde el menú
    public bool IsActiveDropdown => optionType == OptionType.Dropdown && isActive;

    // Método para gestionar la acción de clic
    public virtual void OnOptionSelected()
    {
        Debug.Log("Opción seleccionada: " + optionText.text);
        switch (optionType)
        {
            case OptionType.Scrollbar:
                break;
            case OptionType.Dropdown:
                TMP_Dropdown dropdown = optionSetting.GetComponent<TMP_Dropdown>();
                
                if (isActive)
                {
                    dropdown.Show(); // Mostrar el dropdown si está activo
                    isActive = false; // Cambiar el estado a inactivo
                    GamepadOptionsManager.instance.canNavigate = true; // Deshabilitar la navegación mientras el dropdown está activo
                }
                else
                {
                    dropdown.Hide(); // Ocultar el dropdown si no está activo
                    isActive = true; // Cambiar el estado a activo
                    GamepadOptionsManager.instance.canNavigate = false; // Permitir la navegación nuevamente
                }

                break;
        }
    }
}
