using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CreditsManager : MonoBehaviour
{
    // Velocidad de desplazamiento de los créditos
    [SerializeField] float scrollSpeed;
    public GameObject panelCredits; // Referencia al panel de créditos
    public GameObject creditsContainer; // Referencia al container de créditos
    RectTransform rt; // Referencia al RectTransform del container de créditos

    float endYPosition; // Posición final en Y (anchoredPosition)
    Vector2 startPosition; // Posición inicial en Y (anchoredPosition)

    public static CreditsManager instance; // Instancia estática para acceso global
    private void Awake()
    {
        // Asegurarse de que solo haya una instancia de CreditsManager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Referencia al RectTransform para acceder fácilmente a sus propiedades
        rt = creditsContainer.GetComponent<RectTransform>();
        startPosition = rt.anchoredPosition; // Guardar la posición inicial

        //Ocultar el panel de créditos al inicio
        panelCredits.SetActive(false);

    }

    //Método para iniciar el scroll de los créditos
    public void StartCredits()
    {
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
        float realScrollSpeed;
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

        //Devolvemos al inicio por si se quieren volver a ver los créditos
        rt.anchoredPosition = startPosition; // Volver a la posición inicial


        //Desactivamos el panel
        panelCredits.SetActive(false);
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
        endYPosition = startY + totalHeight + 100f; // Añadimos un margen de 100 para que no se corte
    }

    //Método para cerrar los créditos
    public void CloseCredits()
    {
        StopAllCoroutines(); // Detener todas las corrutinas relacionadas con los créditos
        panelCredits.SetActive(false); // Desactivar el panel de créditos
    }

}
