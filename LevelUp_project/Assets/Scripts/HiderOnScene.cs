using UnityEngine;
using UnityEngine.SceneManagement;

public class HiderOnScene : MonoBehaviour
{
    [SerializeField]
    private string targetSceneName; // Nombre de la escena asignable desde el editor
    void Start()
    {
        if (SceneManager.GetActiveScene().name == targetSceneName)
        {
            gameObject.SetActive(false); // Esconde el GameObject
        }
    }
}
