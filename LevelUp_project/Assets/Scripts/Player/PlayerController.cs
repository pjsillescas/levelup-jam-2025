using input;
using UnityEngine;

namespace player
{
	public class PlayerController : MonoBehaviour
	{


        private Rigidbody rb;
        private InputActions input;
        private CameraManager cameraManager;

		[Header("Player Settings")]
        [SerializeField]
        private float moveSpeed = 5f;
        [SerializeField]
        private CameraMode currentMode = CameraMode.SideScroller;
        [SerializeField]
		private float jumpForce = 7f;

		[Header("GroundCheck")]
		[SerializeField]
		private Transform groundCheck;
		[SerializeField]
		private float groundCheckRadius = 0.3f;
		[SerializeField]
		private LayerMask groundLayer;
		private bool isGrounded;


        //Climb through vines
        [Header("VineCheck")]
		[SerializeField]
		private Transform vineCheck;
		[SerializeField]
		private float vineCheckRadius;
		[SerializeField]
		private LayerMask vineLayer;

		private bool _isOnVine;



		void Awake()
		{
			input = new InputActions();
			input.Enable();
		}

		private void Start()
		{
			rb = GetComponent<Rigidbody>();
			cameraManager = FindFirstObjectByType<CameraManager>();
			SwitchToMode(currentMode);
		}

		void Update()
		{
			var inputVector = input.Player.Move.ReadValue<Vector2>();
			ApplyMovement(inputVector);

			isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
            _isOnVine = Physics.CheckSphere(vineCheck.position, vineCheckRadius, vineLayer);

			var jump = input.Player.Jump.WasPressedThisFrame();
			if (jump && isGrounded)
			{
				ApplyJump();
			}
		}

		private void ApplyMovement(Vector2 inputVector)
		{
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

				if (!_isOnVine)
				{
					rb.useGravity = true;
                    rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);
                }
				else
				{
					rb.useGravity = false;
					rb.linearVelocity = new Vector3(0f, -move.x, 0f);
				}

                    
			}
		}

		private void ApplyJump()
		{
			rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
			rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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

			cameraManager.SwitchCameraMode(currentMode);
		}

		public void SwitchCameraMode()
		{
			currentMode = currentMode == CameraMode.SideScroller ? CameraMode.Isometric : CameraMode.SideScroller;

			SwitchToMode(currentMode);
		}
	}
}