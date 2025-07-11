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

		[SerializeField]
		private float inertialDecay = 0.2f;

		private bool isGrounded;
		private Rigidbody rb;
		private InputActions input;
		private CameraManager cameraManager;
		private float splineLength;
		private float distancePercentage = 0f;
		private bool _isOnVine;
		private Quaternion savedRotation;
		
		private Vector3 inertialForce;

		void Awake()
		{
			input = new InputActions();
			input.Enable();

			inertialForce = Vector3.zero;
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

			savedRotation = transform.rotation;

			Platform.OnPlatformEnter += OnPlatformEnter;
			Platform.OnPlatformLeave += OnPlatformLeave;
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
				//rb.linearVelocity = new Vector3(moveVector.x * moveSpeed, rb.linearVelocity.y, 0f);
				ApplyMovementSideScroller(moveVector.x);
			}
			else if (currentMode == CameraMode.Isometric)
			{
				/*
				// Rotate input for isometric movement
				Vector3 isoInput = Quaternion.Euler(0, -45, 0) * moveVector.normalized;
				Vector3 move = isoInput * moveSpeed;
				rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);
				*/
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
			if (Mathf.Abs(inputX) < 0.1f) { return; }

			var speed = GetMoveSpeed() / splineLength;
			var deltaMove = -Mathf.Sign(inputX) * speed * Time.deltaTime ;
			distancePercentage = Mathf.Clamp(distancePercentage + deltaMove, 0, 1);

			Vector3 targetPosition = sideScrollerSpline.EvaluatePosition(distancePercentage);
			transform.position = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
			//Vector3 currentPosition = transform.position;
			//var direction = targetPosition - currentPosition;
			//rb.linearVelocity = new Vector3(direction.x * moveSpeed, rb.linearVelocity.y, direction.z * moveSpeed);
		}

		private Vector3 move;
		private void ApplyMovementIsometric(Vector3 moveVector)
		{
			// Rotate input for isometric movement
			Vector3 isoInput = Quaternion.Euler(0, -45, 0) * moveVector.normalized;
			isoInput = -isoInput; // Invertir controles
			var speed = GetMoveSpeed();

			//ApplyIsometricDisplacement(isoInput * speed);

			move = isoInput * speed;

			/*
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
			*/
		}

		private Vector3 velocity;

		private void AddVelocity(Vector3 deltaVelocity)
		{
			float factor = 0.8f;
			velocity = Vector3.Lerp(velocity, deltaVelocity, factor); // velocity * (1 - factor) + factor * deltaVelocity;
			//velocity = deltaVelocity;
		}
		private void ApplyIsometricDisplacement(Vector3 move)
		{
			if (!_isOnVine)
			{
				rb.useGravity = true;
				AddVelocity(move + inertialForce /*+ new Vector3(0, rb.linearVelocity.y, 0)*/);
				rb.linearVelocity = velocity;


				//Debug.Log($"({velocity.x:C3},{velocity.y:C3},{velocity.z:C3})");

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
			//var inertialForce = Vector3.zero;
			/*
			if (performInertialForce)
			{
				performInertialForce = false;
				inertialForce = this.inertialForce;
			}
			*/
			//var force = (Vector3.up * jumpForce) + inertialForce;

			//Debug.Log($"jump force ({force.x},{force.y},{force.z})");

			//rb.AddForce(force, ForceMode.Impulse);

			inertialForce += new Vector3(0, jumpForce, 0);

			Debug.Log($"salto ({inertialForce.x} _ {inertialForce.x} _ {inertialForce.x})");
		}

		private void DecayInertialForces()
		{
			float decayRate = 0.9f; // tune this
			if (inertialForce.sqrMagnitude > 0.001f)
			{
				inertialForce = inertialForce * decayRate;// Vector3.Lerp(inertialForce, Vector3.zero, Time.deltaTime * decayRate);
				Debug.Log($"inertialforce ('{inertialForce.x}','{inertialForce.y}','{inertialForce.z}')");
			}
		}

		private void LateUpdate()
		{
			if (currentMode != CameraMode.Isometric)
			{
				return;
			}
			ApplyIsometricDisplacement(move);
			DecayInertialForces();
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

		public void RestoreRotation()
		{
			//transform.rotation = savedRotation;
			transform.rotation = savedRotation;
		}

		private void OnPlatformEnter(object sender, Platform platformEntered)
		{
			;
		}

		private void OnPlatformLeave(object sender, Platform platformLeft)
		{
			var forward = transform.forward;
			RestoreRotation();
			if (currentMode == CameraMode.Isometric)
			{
				float inertialSpeed = 3000f;
				//var lastDisplacement = platformLeft.GetLastDisplacement();
				var lastDisplacement = forward;
				if (lastDisplacement.sqrMagnitude > 0.1f)
				{
					inertialForce = lastDisplacement.normalized * inertialSpeed;
					//performInertialForce = true;
					//rb.AddForce(inertialForce, ForceMode.Impulse);
					AddVelocity(inertialForce);
				}
			}
		}


	}
}