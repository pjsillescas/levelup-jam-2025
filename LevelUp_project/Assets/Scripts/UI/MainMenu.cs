using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class MainMenu : MonoBehaviour
{
	[Header("UI Panels")]
	[SerializeField] private GameObject optionsPanel;
	[SerializeField] private GameObject creditsPanel;

	[Header("Gamepad Menu Handler")]
	[SerializeField] private GamepadMenuHandler gamepadMenuHandler;

	[Header("Input Actions")]
	[SerializeField] private InputActionReference navigateAction;
	[SerializeField] private InputActionReference submitAction;

	[Header("Delay Settings")]
	[SerializeField] private float initialDelayBetweenInputs = 0.5f;
	[SerializeField] private float finalDelayBetweenInputs = 0.2f;
	[SerializeField] private float delayDecreaseSpeed; // velocidad de reducción del delay

	float nextInputTime = 0f; // Tiempo del último input
	float currentDelayBetweenInputs;
	bool isHoldingNav = false;

	void Start()
	{
		AudioManager.instance.PlayMusic(1);
		currentDelayBetweenInputs = initialDelayBetweenInputs; // Inicializar el delay actual
		gamepadMenuHandler.InitializeButtons(); // Inicializar los botones del menú
	}

	private void OnDestroy()
	{
		AudioManager.instance.StopMusic();
		submitAction.action.performed -= SubmitControl; // Desuscribirse del evento de submit
	}

	void OnEnable()
	{
		// Solo suscribimos el submit, la navegación se gestiona en Update
		submitAction.action.performed += SubmitControl;
	}

	//Método para llamar a subir index
	public void IncreaseIndex()
	{
		gamepadMenuHandler.IncreaseIndex();
	}

	//Método para llamar a bajar index
	public void DecreaseIndex()
	{
		gamepadMenuHandler.DecreaseIndex();
	}

	//Método para llamar al método de acción del botón actual
	public void SubmitControl(InputAction.CallbackContext context)
	{
		if (creditsPanel.activeSelf || optionsPanel.activeSelf || Time.time < nextInputTime)
		{
			nextInputTime = Time.time + currentDelayBetweenInputs;
			return; // No hacer nada si los créditos o las opciones están abiertas
		}
		nextInputTime = Time.time + currentDelayBetweenInputs;
		gamepadMenuHandler.UseCurrentButton();
	}

	void Update()
	{
		if (creditsPanel.activeSelf || optionsPanel.activeSelf)
			return;

		Vector2 nav = navigateAction.action.ReadValue<Vector2>();

		bool navUp = nav.y > 0.5f;
		bool navDown = nav.y < -0.5f;

		if (navUp || navDown)
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

}
