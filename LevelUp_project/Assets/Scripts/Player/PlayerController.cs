using input;
using UnityEngine;
using UnityEngine.Splines;

namespace player
{
	public class PlayerController : MonoBehaviour
	{
		[Header("Player Settings")]
		[SerializeField]
		private float moveSpeed = 10f;
		[SerializeField]
		private CameraMode currentMode = CameraMode.SideScroller;
		[SerializeField]
		private float jumpForce = 7f;
		[SerializeField, Range(0f, 1f)]
		private float airControl = 0.43f;
		[SerializeField]
		private bool canJump;


		[Header("GroundCheck")]
		[SerializeField]
		private Transform groundCheck;
		[SerializeField]
		private float groundCheckRadius = 0.2f;
		[SerializeField]
		private LayerMask groundLayer;

		[Header("VineCheck")]
		[SerializeField]
		private Transform vineCheck;
		[SerializeField]
		private float vineCheckRadius = 2f;
		[SerializeField]
		private LayerMask vineLayer;

		[Header("Side Scroller Spline")]
		[SerializeField]
		private SplineContainer sideScrollerSpline;
		
		[Header("Particle Systems")]
        [SerializeField] private ParticleSystem walkParticles;
        [SerializeField] private ParticleSystem jumpParticles;


        private bool isGrounded;
		private Rigidbody rb;
		private InputActions input;
		private CameraManager cameraManager;
		private float splineLength;
		private float distancePercentage = 0f;
		private bool _isOnVine;
		private Quaternion savedRotation;

		void Awake()
		{
			input = new InputActions();
			input.Enable();
			canJump = true;
		}

        private void OnDisable()
        {
			input.Disable();
        }

        private void OnDestroy()
        {
			input.Dispose();
        }

        private void Start()
		{
			rb = GetComponent<Rigidbody>();
			cameraManager = FindFirstObjectByType<CameraManager>();
			SwitchToMode(currentMode);

			if (sideScrollerSpline != null)
			{
				splineLength = sideScrollerSpline.CalculateLength();
			}
			else
			{
				Debug.LogWarning("SideScrollerSpline is not assigned.");
			}

			if (currentMode == CameraMode.SideScroller)
			{
				ApplyMovementSideScroller(-0.1f);
			}

		}

		void Update()
		{
			var inputVector = input.Player.Move.ReadValue<Vector2>();
			ApplyMovement(inputVector);


			bool prevGrounded = isGrounded;
			isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
			if (!prevGrounded && isGrounded)
			{
				if(!jumpParticles.isPlaying) jumpParticles.Play();
				AudioManager.instance.PlaySFX(3);
            }
				
				

                _isOnVine = Physics.CheckSphere(vineCheck.position, vineCheckRadius, vineLayer);

			var jump = input.Player.Jump.WasPressedThisFrame();
			if (jump && isGrounded && canJump)
			{
				ApplyJump();
			}
		}

		private void ApplyMovement(Vector2 inputVector)
		{
			var moveVector = new Vector3(inputVector.x, 0f, inputVector.y);

			if (currentMode == CameraMode.SideScroller)
			{
				ApplyMovementSideScroller(moveVector.x);
			}
			else if (currentMode == CameraMode.Isometric || currentMode == CameraMode.Topdown)
			{
				ApplyMovementIsometric(moveVector);
			}
			if((moveVector.x != 0 || moveVector.z != 0) && isGrounded)
			{
                if (!walkParticles.isPlaying)
				{
                    walkParticles.Play();
					AudioManager.instance.PlaySFX(1);
                }
                    

			}
			else
			{
				walkParticles.Stop();
				AudioManager.instance.StopSFX(1);	
			}

		}

		private float GetMoveSpeed()
		{
			var airControlFactor = (isGrounded) ? 1 : airControl;
			return (moveSpeed) * airControlFactor;
		}

		private void ApplyMovementSideScroller(float inputX)
		{
			if (Mathf.Abs(inputX) < 0.1f) return;

            var speed = GetMoveSpeed() / splineLength;
			var deltaMove = -Mathf.Sign(inputX) * speed * Time.deltaTime;
			distancePercentage = Mathf.Clamp(distancePercentage + deltaMove, 0, 1);

			Vector3 targetPosition = sideScrollerSpline.EvaluatePosition(distancePercentage);
			transform.position = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
			//Vector3 currentPosition = transform.position;
			//var direction = targetPosition - currentPosition;
			//rb.linearVelocity = new Vector3(direction.x * moveSpeed, rb.linearVelocity.y, direction.z * moveSpeed);
		}

		private void ApplyMovementIsometric(Vector3 moveVector)
		{
			// Rotate input for isometric movement
			Vector3 isoInput = Quaternion.Euler(0, 45, 0) * moveVector.normalized;
			var speed = GetMoveSpeed();
			Vector3 move = isoInput * speed;

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
		private void ApplyJump()
		{
			rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
			rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
			jumpParticles.Play();
			AudioManager.instance.PlaySFX(2);
		}

		public void SwitchToMode(CameraMode mode)
		{
			currentMode = mode;

			if (mode == CameraMode.SideScroller)
			{
				rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
			}
			else if (mode == CameraMode.Isometric || mode == CameraMode.Topdown)
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

		public void SaveRotation()
		{
			savedRotation = transform.rotation;
		}

		public void RestoreRotation()
		{
			//transform.rotation = savedRotation;
			transform.rotation = Quaternion.identity;
		}

		public bool IsJumpEnabled()
		{
			return canJump;
		}

		public void EnableJump()
		{
			canJump = true;
		}

		public void DisableJump()
		{
			canJump = false;
		}

		public void SwitchJump()
		{
			canJump = !canJump;
		}

		public CameraMode GetCurrentCameraMode()
		{
			return currentMode;
		}
	}
}

