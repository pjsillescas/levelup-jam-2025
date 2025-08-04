using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadOptionsManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject optionsPanel;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference navigateAction;
    [SerializeField] private InputActionReference submitAction;

    [Header("Delay Settings")]
    [SerializeField] private float initialDelayBetweenInputs = 0.5f;
    [SerializeField] private float finalDelayBetweenInputs = 0.2f;
    [SerializeField] private float delayDecreaseSpeed; // velocidad de reducción del delay

    [Header("Options and Buttons")]
    [SerializeField] private List<Option> options;
    [SerializeField] private Transform buttonContainer; // Contenedor de botones en el panel de opciones
    private List<Buttons> optionButtons = new List<Buttons>();

    float nextInputTime = 0f; // Tiempo del último input
    float currentDelayBetweenInputs;
    bool isHoldingNav = false;

    private int generalIndex = 0;
    private int buttonIndex = 0;
    public bool canNavigate = true;

    public static GamepadOptionsManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentDelayBetweenInputs = initialDelayBetweenInputs; // Inicializar el delay actualç
        canNavigate = true; // Permitir la navegación al inicio
    }

    private void OnDisable()
    {
        submitAction.action.performed -= SubmitControl; // Desuscribirse del evento de submit
    }

    void OnEnable()
    {
        submitAction.action.performed += SubmitControl;
    }

    //Método para llamar a subir index
    public void IncreaseIndex()
    {
        // Si todavía hay más opciones antes de la última, avanzamos en la lista de opciones
        if (generalIndex < options.Count - 1)
        {
            generalIndex++;
        }
        // Si estamos en la última opción, saltamos al modo de botones
        else if (generalIndex == options.Count - 1)
        {
            generalIndex = options.Count; // Entrar en la zona de botones
            buttonIndex = 0;
        }
        // Si ya estábamos en modo botones, volvemos al primer índice de opciones
        else
        {
            generalIndex = 0;
            buttonIndex = 0;
        }

        MarkCurrentOption();
    }

    //Método para llamar a bajar index
    public void DecreaseIndex()
    {
        if (generalIndex > 0)
        {
            generalIndex--;
        }
        else
        {
            generalIndex = options.Count; // Volver al último índice si se supera el mínimo
            buttonIndex = 0;
        }

        MarkCurrentOption();
    }

    //Método para incrementar el índice del botón actual
    public void IncreaseButtonIndex()
    {
        if (generalIndex != options.Count)
            return; // No incrementar el índice de botón si no estamos en el modo de botones


        buttonIndex++;
        if (buttonIndex >= optionButtons.Count) buttonIndex = 0; // Volver al primer botón si se supera el índice
        MarkCurrentOption();
    }

    //Método para decrementar el índice del botón actual
    public void DecreaseButtonIndex()
    {
        if (generalIndex != options.Count)
            return; // No decrementar el índice de botón si no estamos en el modo de botones

        buttonIndex--;
        if (buttonIndex < 0) buttonIndex = optionButtons.Count - 1; // Volver al último botón si se supera el índice
        MarkCurrentOption();
    }

    //Método para llamar al método de acción del botón actual
    public void SubmitControl(InputAction.CallbackContext context)
    {
        // Ignorar submit si panel cerrado o navegación deshabilitada
        if (!optionsPanel.activeSelf)
        {
            nextInputTime = Time.time + currentDelayBetweenInputs;
            return;
        }
        // Ignorar si aún no ha pasado el retardo entre inputs
        if (Time.time < nextInputTime)
            return;
        // Actualizar próximo tiempo de input válido
        nextInputTime = Time.time + currentDelayBetweenInputs;

        if (generalIndex == options.Count)
        {
            optionButtons[buttonIndex].ClickingButton(); // Llamar al método de clic del botón actual
        }
        else
        {
            options[generalIndex].OnOptionSelected(); // Llamar al método de selección de la opción actual
        }

    }

    void Update()
    {
        if (!optionsPanel.activeSelf || !canNavigate)
            return;


        Vector2 nav = navigateAction.action.ReadValue<Vector2>();

        bool navUp = nav.y > 0.5f;
        bool navDown = nav.y < -0.5f;
        bool navLeft = nav.x < -0.5f;
        bool navRight = nav.x > 0.5f;

        if (navUp || navDown || navLeft || navRight)
        {
            isHoldingNav = true;
            if (Time.time >= nextInputTime)
            {
                if (navDown)
                {
                    IncreaseIndex();
                }
                else if (navUp)
                {
                    DecreaseIndex();
                }
                if (navLeft)
                {
                    DecreaseButtonIndex();
                    HandleLeftOptionAction();
                }
                else if (navRight)
                {
                    IncreaseButtonIndex();
                    HandleRightOptionAction();
                }

                // Disminuir el delay progresivamente hasta el mínimo
                currentDelayBetweenInputs = Mathf.Max(finalDelayBetweenInputs, currentDelayBetweenInputs - delayDecreaseSpeed * Time.deltaTime);
                nextInputTime = Time.time + currentDelayBetweenInputs;
            }
        }
        else
        {
            // Si se suelta el botón, reiniciar el delay
            if (isHoldingNav)
            {
                currentDelayBetweenInputs = initialDelayBetweenInputs;
                isHoldingNav = false;
            }
            nextInputTime = 0f;
        }
    }

    //Método para inicializar los botones
    public void InitializeButtons()
    {
        canNavigate = true; // Permitir la navegación al inicializar los botones
        if (optionButtons.Count > 0)
        {
            foreach (Buttons button in optionButtons)
            {
                button.OnButtonMarked -= ChangeIndex; // Desuscribirse de eventos previos
            }

            optionButtons.Clear(); // Limpiar la lista de botones
        }

        // Usar el contenedor específico de botones en lugar del panel completo
        foreach (Transform child in buttonContainer)
        {
            Buttons button = child.GetComponent<Buttons>();
            if (button != null && child.gameObject.activeInHierarchy)
            {
                optionButtons.Add(button); // Añadir el botón a la lista solo si está activo
                button.OnButtonMarked += ChangeIndex; // Suscribirse al evento de marcado del botón
            }
        }
        buttonIndex = 0; // Reiniciar el índice del botón
        generalIndex = 0; // Reiniciar el índice general
        MarkCurrentOption(); // Marcar el primer botón al iniciar
    }

    //Método para marcar la opción actual
    private void MarkCurrentOption()
    {
        if (generalIndex != options.Count)
        {
            foreach (Option option in options)
            {
                option.UnmarkOption(); // Desmarcar todas las opciones
            }
            options[generalIndex].MarkOption();
            foreach (Buttons button in optionButtons)
            {
                button.UnmarkButton();
            }
        }
        else
        {
            foreach (Buttons button in optionButtons)
            {
                button.UnmarkButton();
            }
            optionButtons[buttonIndex].MarkButton(); // Marcar el botón actual
            foreach (Option option in options)
            {
                option.UnmarkOption(); // Desmarcar todas las opciones
            }
        }
    }

    //Método para cambiar el índice al marcar un botón
    private void ChangeIndex(Buttons button)
    {
        if (buttonIndex == optionButtons.IndexOf(button))
            return; // Si el botón marcado es el actual, no hacer nada
        buttonIndex = optionButtons.IndexOf(button);
        generalIndex = options.Count; // Cambiar al modo de botones
        MarkCurrentOption();
    }

    // Método para manejar la acción de izquierda en las opciones
    private void HandleLeftOptionAction()
    {
        if (generalIndex == options.Count)
            return;

        DecreaseButtonIndex();
        options[generalIndex].OnLeftOptionAction(); // Llamar al método de acción de izquierda de la opción actual
    }

    // Método para manejar la acción de derecha en las opciones
    private void HandleRightOptionAction()
    {
        if (generalIndex == options.Count)
            return;

        IncreaseButtonIndex();
        options[generalIndex].OnRightOptionAction(); // Llamar al método de acción de derecha de la opción actual
    }

}
