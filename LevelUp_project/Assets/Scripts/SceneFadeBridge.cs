using UnityEngine;
using UnityEngine.UI; // Importar para manejar componentes de UI

public class SceneFadeBridge : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Image faderImage;

    [Range(0f, 1f)] // Variable pública con rango para controlar el alpha
    public float alphaValue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Buscar el GameObject UIManager en la escena
        GameObject uiManagerObject = GameObject.Find("UIManager");
        if (uiManagerObject != null)
        {
            uiManager = uiManagerObject.GetComponent<UIManager>();

            // Buscar el GameObject con el tag "Fader"
            GameObject faderObject = GameObject.FindWithTag("Fader");
            if (faderObject != null)
            {
                faderImage = faderObject.GetComponent<Image>();
                if (faderImage == null)
                {
                    Debug.LogError("El GameObject con tag 'Fader' no tiene un componente Image.");
                }
            }
            else
            {
                Debug.LogError("No se encontró un GameObject con el tag 'Fader'.");
            }
        }
        else
        {
            Debug.LogError("UIManager no encontrado en la escena.");
        }
    }

    void Update()
    {
        // Aplicar el valor de alphaValue al componente Image del Fader si está asignado
        if (faderImage != null)
        {
            SetImageAlpha(faderImage, alphaValue);
        }
    }

    // Método público para controlar el alpha de una imagen
    public void SetImageAlpha(Image image, float alpha)
    {
        if (image != null)
        {
            Color color = image.color;
            color.a = Mathf.Clamp01(alpha); // Asegurarse de que el alpha esté entre 0 y 1
            image.color = color;
        }
        else
        {
            Debug.LogError("Image no asignada.");
        }
    }
}
