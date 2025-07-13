using UnityEngine;
using UnityEngine.SceneManagement;

public class HiderOnScene : MonoBehaviour
{
    [SerializeField]
    private string targetSceneName; // Nombre de la escena asignable desde el editor
    void Awake()
    {
        if (SceneManager.GetActiveScene().name == targetSceneName)
        {
            gameObject.SetActive(false); // Esconde el GameObject
        }
        else
        {
            gameObject.SetActive(true); // Asegura que el GameObject esté activo en otras escenas
        }
    }
}
