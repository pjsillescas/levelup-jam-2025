using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Asegurarse de que el UIManager persista entre escenas
        }
        else
        {
            Destroy(gameObject); // Asegurarse de que solo haya una instancia
        }
    }

    [SerializeField] float buttonAnimDuration;
    [SerializeField] private OptionsManager optionsManager;

    void Start()
    {
        CloseOptionsMenu(); // Asegurarse de que el menú de opciones esté cerrado al inicio
    }

    void Update()
    {
        // Aquí se pueden manejar eventos de UI globales, como abrir el menú de opciones con una tecla
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionsManager.IsMenuOpen())
            {
                CloseOptionsMenu();
            }
            else
            {
                OpenOptionsMenu();
            }
        }
    }

    //Método para obtener la duración de la animación del botón
    public float GetButtonAnimDuration()
    {
        return buttonAnimDuration; // Retornar la duración de la animación del botón
    }

    //Método para abrir el menú de opciones
    public void OpenOptionsMenu()
    {
        optionsManager.OpenMenu(); // Activar el menú de opciones
    }

    //Método para cerrar el menú de opciones
    public void CloseOptionsMenu()
    {
        optionsManager.CloseMenu(); // Desactivar el menú de opciones
    }

}
