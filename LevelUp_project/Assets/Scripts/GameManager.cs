using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private int targetFPS;
    public static GameManager instance;


    public static bool isGamePaused;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
            
        instance = this;
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = targetFPS;
    }
}
