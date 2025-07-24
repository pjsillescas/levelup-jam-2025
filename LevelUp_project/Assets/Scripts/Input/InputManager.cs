using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace input
{
	public class InputManager : MonoBehaviour
	{
		public static event EventHandler<Vector2> OnInputMove;
		public static event EventHandler OnInputJumpPressed;
		public static event EventHandler OnInputJumpReleased;
		public static event EventHandler OnInputPausePressed;
		public static event EventHandler OnInputDialogueSpeedPressed;

		[SerializeField]
		private InputActionReference moveAction;
		[SerializeField]
		private InputActionReference jumpAction;
		[SerializeField]
		private InputActionReference pauseAction;
		[SerializeField]
		private InputActionReference dialogueSpeedAction;

		private bool isEnabled = true;

		private void Awake()
		{
			isEnabled = true;
		}

		private void OnDestroy()
		{
			isEnabled = false;
		}

		private void OnEnable()
		{
			isEnabled = true;
		}

		private void OnDisable()
		{
			isEnabled = false;
		}

		void Update()
		{
			if (isEnabled)
			{
				var inputVector = moveAction.action.ReadValue<Vector2>();
				OnInputMove?.Invoke(this, inputVector);

				if (jumpAction.action.WasPressedThisFrame())
				{
					OnInputJumpPressed?.Invoke(this, EventArgs.Empty);
				}

				if (jumpAction.action.WasReleasedThisFrame())
				{
					OnInputJumpReleased?.Invoke(this, EventArgs.Empty);
				}
			}

			if (pauseAction.action.WasPressedThisFrame())
			{
				OnInputPausePressed?.Invoke(this, EventArgs.Empty);
			}
			
			if (dialogueSpeedAction.action.WasPressedThisFrame())
			{
				OnInputDialogueSpeedPressed?.Invoke(this, EventArgs.Empty);
			}

		}
	}
}
