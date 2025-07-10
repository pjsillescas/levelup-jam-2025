using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CreditsManager : MonoBehaviour
{
    // Velocidad de desplazamiento de los créditos
    public float scrollSpeed;
    // Objeto “Gracias por jugar” (inactivo al iniciar)
    public GameObject thanksTextObject;
    public GameObject panelCredits; // Referencia al panel de créditos
    public GameObject creditsContainer; // Referencia al container de créditos
    public GameObject quitButton, playAgainButton; // Botones de salir y jugar de nuevo
    RectTransform rt; // Referencia al RectTransform del container de créditos

    float endYPosition; // Posición final en Y (anchoredPosition)

    void Start()
    {
        // Referencia al RectTransform para acceder fácilmente a sus propiedades
        rt = creditsContainer.GetComponent<RectTransform>();

        //Ocultar el texto de gracias por jugar
        thanksTextObject.SetActive(false);

        //Ocultar el panel de créditos al inicio
        panelCredits.SetActive(false);

        //Desactivar los botones de salir y jugar de nuevo
        quitButton.SetActive(false);
        playAgainButton.SetActive(false);

    }

    //Método para iniciar el scroll de los créditos
    public void StartCredits()
    {
        //!Pausar el input del jugador
        //GameManager.instance.StartDialogue(); // Iniciar el diálogo del GameManager
        //Abrir el panel de créditos
        panelCredits.SetActive(true);

        //Calcular la posición final de los créditos
        StartCoroutine(CalculateEndYPosition());

        // Iniciar la corrutina de scroll
        StartCoroutine(ScrollCredits());
    }

    public IEnumerator ScrollCredits()
    {
        float realScrollSpeed = scrollSpeed;
        //Mientras no lleguemos al endYPosition...
        while (rt.anchoredPosition.y < endYPosition)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                // Si se pulsa espacio, se acelera el scroll
                realScrollSpeed = scrollSpeed * 4f;
            }
            else
            {
                realScrollSpeed = scrollSpeed; // Si no, se mantiene la velocidad normal
            }
            rt.anchoredPosition += Vector2.up * realScrollSpeed * Time.deltaTime;
            yield return null;
        }

        //Desactivamos el container y mostramos “gracias”
        creditsContainer.SetActive(false);
        thanksTextObject.SetActive(true);
        //Activar los botones de salir y jugar de nuevo
        quitButton.SetActive(true);
        playAgainButton.SetActive(true);
    }

    //Método para calcular la posición final de los créditos
    public IEnumerator CalculateEndYPosition()
    {
        //Lista de los hijos del container de créditos
        RectTransform[] children = creditsContainer.GetComponentsInChildren<RectTransform>();
        float totalHeight = 0f;

        //Esperamos un frame para que se pueda leer las alturas correctamente ya que credits necesita calcularse
        yield return null;

        foreach (RectTransform child in children)
        {
            //Sumamos la altura de cada hijo al total
            totalHeight += child.rect.height;
        }

        totalHeight += panelCredits.GetComponent<RectTransform>().rect.height; // Añadimos la altura del panel de créditos

        // Calculo de la Y final en anchoredPosition
        float startY = rt.anchoredPosition.y;
        endYPosition = startY + totalHeight + 10f; // Añadimos un margen de 10 para que no se corte
    }

    //!TESTING
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCredits(); // Iniciar los créditos al pulsar C
        }
    }

}
