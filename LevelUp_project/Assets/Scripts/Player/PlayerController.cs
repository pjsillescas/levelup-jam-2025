using input;
using UnityEngine;

namespace player
{
	public class PlayerController : MonoBehaviour
	{
		public enum CameraMode { SideScroller, Isometric }

		[SerializeField]
		private float moveSpeed = 5f;
		[SerializeField]
		private CameraMode currentMode = CameraMode.SideScroller;

		[SerializeField]
		private float jumpForce = 7f;
		[SerializeField]
		private Transform groundCheck;
		[SerializeField]
		private float groundCheckRadius = 0.3f;
		[SerializeField]
		private LayerMask groundLayer;

		private bool isGrounded;
		private Rigidbody rb;
		private InputActions input;
		private ModeSwitcher modeSwitcher;
		
		void Awake()
		{
			input = new InputActions();
			input.Enable();
		}

		private void Start()
		{
			rb = GetComponent<Rigidbody>();
			modeSwitcher = GetComponent<ModeSwitcher>();
			SwitchToMode(currentMode); // Init constraints
		}

		void Update()
		{
			var inputVector = input.Player.Move.ReadValue<Vector2>();
			var moveVector = new Vector3(inputVector.x, 0f, inputVector.y);

			if (currentMode == CameraMode.SideScroller)
			{
				moveVector.z = 0f; // No depth
				rb.linearVelocity = new Vector3(moveVector.x * moveSpeed, rb.linearVelocity.y, 0f);
			}
			else if (currentMode == CameraMode.Isometric)
			{
				// Rotate input for isometric movement
				Vector3 isoInput = Quaternion.Euler(0, -45, 0) * moveVector.normalized;
				Vector3 move = isoInput * moveSpeed;
				rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);
			}

			isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
			var jump = input.Player.Jump.WasPressedThisFrame();
			if (jump && isGrounded)
			{
				rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // Clear Y before jump
				rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
			}
		}

		public void SwitchToMode(CameraMode mode)
		{
			currentMode = mode;

			if (mode == CameraMode.SideScroller)
			{
				rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
			}
			else if (mode == CameraMode.Isometric)
			{
				rb.constraints = RigidbodyConstraints.FreezeRotation;
			}

			modeSwitcher.ToggleMode(currentMode);
		}

		public void SwitchCameraMode()
		{
			currentMode = currentMode == CameraMode.SideScroller ? CameraMode.Isometric : CameraMode.SideScroller;

			SwitchToMode(currentMode);
		}
	}
}