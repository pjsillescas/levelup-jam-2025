using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
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


		[Header("InputActions")]
		[SerializeField]
		private InputActionReference moveAction;
		[SerializeField]
		private InputActionReference jumpAction;

		[Header("GroundCheck")]
		[SerializeField]
		private Transform groundCheck;
		[SerializeField]
		private float groundCheckRadius = 0.2f;
		[SerializeField]
		private LayerMask groundLayer;
		[SerializeField]
		private LayerMask waterLayer;

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


		private bool isGrounded = true;
		private Rigidbody rb;
		private CameraManager cameraManager;
		private float splineLength;
		private float distancePercentage = 0f;
		private bool _isOnVine;
		private bool isSubmerged;



		private void OnEnable()
		{
			jumpAction.action.started += JumpButtonPressed;
		}

		private void OnDisable()
		{
			jumpAction.action.started -= JumpButtonPressed;
		}

		public void SetIsSubmerged(bool isSubmerged)
		{
			this.isSubmerged = isSubmerged;
		}

		public bool IsSubmerged() => isSubmerged;

		private void Start()
		{
			canJump = true;
			isSubmerged = false;
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


		private void JumpButtonPressed(InputAction.CallbackContext context)
		{

			if (isGrounded && canJump)
			{
				ApplyJump();

			}

		}

		public bool FellInWater()
		{
			return isSubmerged;
		}

		private bool FellInWaterlily()
		{
			const string WATERLILY_TAG = "waterlily";
			var colliders = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, groundLayer);
			return colliders.ToList().Select(col => col.CompareTag(WATERLILY_TAG)).Aggregate((carry, val) => carry || val);
		}

		void Update()
		{
			var inputVector = moveAction.action.ReadValue<Vector2>();
			ApplyMovement(inputVector);


			bool prevGrounded = isGrounded;
			isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
			if (!prevGrounded && isGrounded)
			{
				if (!jumpParticles.isPlaying) jumpParticles.Play();

				//var colliders = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, groundLayer);
				//var isWaterLily = colliders.ToList().Select(col => col.CompareTag("waterlily")).Aggregate((carry, val) => carry || val);
				if (FellInWaterlily())
				{
					AudioManager.instance.PlaySFX(5);
				}
				else
				{
					AudioManager.instance.PlaySFX(3);
				}
			}



			_isOnVine = Physics.CheckSphere(vineCheck.position, vineCheckRadius, vineLayer);


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
			if ((moveVector.x != 0 || moveVector.z != 0) && isGrounded)
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

