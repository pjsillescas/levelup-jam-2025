using UnityEngine;
using System.Collections.Generic;

public class GamepadMenuHandler : MonoBehaviour
{
    public GameObject parentButtons; // Referencia al GameObject padre de los botones
    private List<Buttons> menuButtons;
    int currentButtonIndex;

    void Start()
    {
        currentButtonIndex = 0;
    }

    //Método para marcar un botón
    private void MarkCurrentButton()
    {
        foreach (Buttons button in menuButtons)
        {
            button.UnmarkButton(); // Desmarcar todos los botones
        }
        menuButtons[currentButtonIndex].MarkButton(); // Marcar el botón actual
    }

    //Método para incrementar el índice del botón actual
    public void IncreaseIndex()
    {
        currentButtonIndex++;
        if (currentButtonIndex >= menuButtons.Count) currentButtonIndex = 0; // Volver al primer botón si se supera el índice
        MarkCurrentButton(); // Marcar el nuevo botón
    }

    //Método para decrementar el índice del botón actual
    public void DecreaseIndex()
    {
        currentButtonIndex--;
        if (currentButtonIndex < 0) currentButtonIndex = menuButtons.Count - 1; // Volver al último botón si se supera el índice
        MarkCurrentButton(); // Marcar el nuevo botón
    }

    //Método para usar el metodo del botón actual
    public void UseCurrentButton()
    {
        menuButtons[currentButtonIndex].ClickingButton(); // Llamar al método de clic del botón actual

    }

    //Método para inicializar los botones
    public void InitializeButtons()
    {
        if (menuButtons != null)
        {
            foreach (Buttons button in menuButtons)
            {
                button.OnButtonMarked -= ChangeButtonIndex; // Desuscribirse de eventos previos
            }
        }
        menuButtons = new List<Buttons>();
        foreach (Transform child in parentButtons.transform)
        {
            Buttons button = child.GetComponent<Buttons>();
            if (button != null && child.gameObject.activeInHierarchy)
            {
                menuButtons.Add(button); // Añadir el botón a la lista solo si está activo
                button.OnButtonMarked += ChangeButtonIndex; // Suscribirse al evento de marcado del botón
            }
        }
        MarkCurrentButton(); // Marcar el primer botón al iniciar
    }

    // Método para cambiar el índice del botón al ser marcado
    private void ChangeButtonIndex(Buttons button)
    {
        if(currentButtonIndex == menuButtons.IndexOf(button))
            return; // Si el botón marcado es el actual, no hacer nada
        currentButtonIndex = menuButtons.IndexOf(button); // Actualizar el índice del botón actual
        if (currentButtonIndex < 0) currentButtonIndex = 0; // Asegurarse de que el índice no sea negativo
        MarkCurrentButton(); // Marcar el botón actual
    }

}
