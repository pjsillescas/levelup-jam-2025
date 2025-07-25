using input;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

namespace player
{
	public class PlayerController : MonoBehaviour
	{
		public enum GroundType { Ground, Waterlily }

		public static event EventHandler OnJump;
		public static event EventHandler<GroundType> OnGrounded;
		public static event EventHandler<Vector2> OnMove;

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
		private float fallControl = 2.5f;
		[SerializeField]
		private float coyoteTime = 0.2f;

		[SerializeField]
		private bool canJump;

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

		private bool isGrounded = true;
		private Rigidbody rb;
		private CameraManager cameraManager;
		private float splineLength;
		private float distancePercentage = 0f;
		private bool _isOnVine;
		private bool isSubmerged;
		private float coyoteTimer;



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

			InputManager.OnInputMove += OnInputMove;
			InputManager.OnInputJumpPressed += OnInputJump;
		}

		private void OnDestroy()
		{
			InputManager.OnInputMove -= OnInputMove;
			InputManager.OnInputJumpPressed -= OnInputJump;
		}

		private void OnInputJump(object sender, EventArgs args)
		{
			if (canJump)
			{
				if (isGrounded || coyoteTimer > 0)
				{
					OnJump?.Invoke(this, EventArgs.Empty);
					ApplyJump();

					coyoteTimer = 0f;
				}
			}
		}

		private bool FellInWaterlily()
		{
			const string WATERLILY_TAG = "waterlily";
			var colliders = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, groundLayer);
			return colliders.ToList().Select(col => col.CompareTag(WATERLILY_TAG)).Aggregate((carry, val) => carry || val);
		}

		private void OnInputMove(object sender, Vector2 inputVector)
		{
			ApplyMovement(inputVector);
		}

		void Update()
		{
			bool prevGrounded = isGrounded;
			isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
			if (!prevGrounded && isGrounded)
			{
				var groundType = (FellInWaterlily()) ? GroundType.Waterlily : GroundType.Ground;
				OnGrounded?.Invoke(this, groundType);
			}
			ProcessCoyoteTime();

			_isOnVine = Physics.CheckSphere(vineCheck.position, vineCheckRadius, vineLayer);
		}

		private void ProcessCoyoteTime()
		{
			coyoteTimer = (isGrounded) ? coyoteTime : coyoteTimer - Time.deltaTime;
		}

		private void ApplyMovement(Vector2 inputVector)
		{
			OnMove?.Invoke(this, inputVector);

			var moveVector = new Vector3(inputVector.x, 0f, inputVector.y);

			if (currentMode == CameraMode.SideScroller)
			{
				ApplyMovementSideScroller(moveVector.x);
			}
			else if (currentMode == CameraMode.Isometric || currentMode == CameraMode.Topdown)
			{
				ApplyMovementIsometric(moveVector);
			}
		}

		private float GetMoveSpeed()
		{
			var airControlFactor = (isGrounded) ? 1 : airControl;
			return (moveSpeed) * airControlFactor;
		}

		private void ApplyMovementSideScroller(float inputX)
		{
			if (Mathf.Abs(inputX) < 0.1f || sideScrollerSpline == null)
			{
				return;
			}

			var speed = GetMoveSpeed() / splineLength;
			var deltaMove = -Mathf.Sign(inputX) * speed * Time.deltaTime;
			distancePercentage = Mathf.Clamp(distancePercentage + deltaMove, 0, 1);

			Vector3 targetPosition = sideScrollerSpline.EvaluatePosition(distancePercentage);
			var newPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
			transform.position = newPosition;
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

		void FixedUpdate()
		{
			// Fall control
			if (rb.linearVelocity.y < 0)
			{
				rb.linearVelocity += (fallControl * Physics.gravity.y * Time.fixedDeltaTime) * Vector3.up;
			}
		}
	}
}
