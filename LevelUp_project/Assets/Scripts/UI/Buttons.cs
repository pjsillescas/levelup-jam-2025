using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using TMPro;
//Bajar la prioridad para que se calcule antes el tamaño del texto
public class Buttons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Image buttonImage; // Referencia a la imagen del botón que se anima
    [SerializeField] GameObject buttonText;
    float duration;
    BoxCollider2D col;


    void Start()
    {
        duration = UIManager.instance.GetButtonAnimDuration(); // Obtener la duración de la animación desde el UIManager

        //Calcular tamaños de imagen y collider despues de un frame para asegurarse de que el rectTransform ya está actualizado
        AdjustButton(); // Ajustar el tamaño del botón al texto
    }

    //Método para ajustar el tamaño del botón al texto
    public void AdjustButton()
    {
        StartCoroutine(InitializeCollider()); // Iniciar la corrutina para inicializar el collider
    }

    //Corrutina para inicializar el collider después de un frame
    private IEnumerator InitializeCollider()
    {
        // Esperar un frame adicional para que el ContentSizeFitter termine
        yield return null;
        buttonImage.rectTransform.sizeDelta = Vector2.zero; // Asegurarse de que el tamaño del botón se reinicie antes de ajustarlo
        //Ajustar el tamaño de la imagen del botón al texto del botón
        float textWidth = buttonText.GetComponent<TMP_Text>().renderedWidth * 1.2f;
        float textHeight = buttonText.GetComponent<TMP_Text>().renderedHeight;
        buttonImage.rectTransform.sizeDelta = new Vector2(textWidth, textHeight);

        //Ajustar el tamaño del collider al tamaño del botón
        col = GetComponent<BoxCollider2D>(); // Obtener el Collider2D del botón
        col.size = new Vector2(textWidth, textHeight); // Ajustar el tamaño del collider al tamaño del botón

        buttonImage.fillAmount = 0f; // Asegurarse de que el botón comienza desmarcado
        buttonImage.color = new Color(1f, 1f, 1f, 0.75f); // Cambiar el color del botón a blanco con transparencia al inicio

    }

    #region Button Marking
    //Método para desmarcar el botón
    private void UnmarkButton()
    {
        StartCoroutine(UnmarkButtonCoroutine()); // Iniciar la corrutina para desmarcar el botón
    }

    //Corrutina para desmarcar el botón
    private IEnumerator UnmarkButtonCoroutine()
    {
        float startValue = buttonImage.fillAmount; // Valor inicial
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            buttonImage.fillAmount = Mathf.Lerp(startValue, 0f, t);
            yield return null;
        }
        buttonImage.fillAmount = 0f; // Asegurarse de que el botón termina completamente desmarcado
    }

    //Método para marcar el botón
    private void MarkButton()
    {
        StartCoroutine(MarkButtonCoroutine()); // Iniciar la corrutina para marcar el botón
    }

    //Corrutina para marcar el botón
    private IEnumerator MarkButtonCoroutine()
    {
        float startValue = buttonImage.fillAmount; // Valor inicial
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            buttonImage.fillAmount = Mathf.Lerp(startValue, 1f, t);
            yield return null;
        }
        buttonImage.fillAmount = 1f; // Asegurarse de que el botón termina completamente marcado
    }

    //Método para seleccionar el botón
    private void ClickingButton()
    {
        buttonImage.color = new Color(0.25f, 0.25f, 0.25f, 0.75f); // Cambiar el color del botón a gris al seleccionarlo
    }

    //Método para deseleccionar el botón
    private void UnclickButton()
    {
        buttonImage.color = new Color(1f, 1f, 1f, 0.75f); // Cambiar el color del botón a blanco con transparencia al deseleccionarlo
    }

    //Gestión del pointer enter
    public void OnPointerEnter(PointerEventData eventData)
    {
        MarkButton(); // Marcar el botón cuando el mouse entra en su área
    }

    //Gestión del pointer exit
    public void OnPointerExit(PointerEventData eventData)
    {
        UnmarkButton(); // Desmarcar el botón cuando el mouse sale de su área
    }

    //Gestión del pointer click
    public void OnPointerDown(PointerEventData eventData)
    {
        ClickingButton(); // Marcar el botón como clickeado
    }

    //Gestión al dejar de hacer clic
    public void OnPointerUp(PointerEventData eventData)
    {
        UnclickButton(); // Desmarcar el botón al dejar de hacer clic
    }

    #endregion

    #region Buttons actions
    ///Método de jugar al juego
    public void PlayGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1); // Cargar el primer nivel del juego
    }

    ///Método para salir del juego
    public void ExitGame()
    {
        Application.Quit(); // Salir del juego
        Debug.Log("Game exited"); // Log para verificar que se ha salido del juego
    }

    //Método para abrir el menú de opciones
    public void OpenOptionsMenu()
    {
        UIManager.instance.OpenOptionsMenu(); // Abrir el menú de opciones
    }

    //Método para cerrar el menú de opciones
    public void CloseOptionsMenu()
    {
        UIManager.instance.CloseOptionsMenu(); // Cerrar el menú de opciones
    }


    #endregion
}
