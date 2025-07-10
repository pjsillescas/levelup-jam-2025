using input;
using UnityEngine;

public class CyntiaAnimation : MonoBehaviour
{
	private enum WalkDirection { left, right, forward, backward }

	private const float DIRECTION_THRESHOLD = 0.1f;

	private Animator animator;
	private InputActions input;
	private WalkDirection currentDirection;

	private void Awake()
	{
		input = new InputActions();
		input.Enable();

		currentDirection = WalkDirection.left;
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		animator = GetComponent<Animator>();
	}

	private void SetDirection(WalkDirection direction)
	{
		currentDirection = direction;
		animator.SetBool("WalkingRight", direction == WalkDirection.right);
		animator.SetBool("WalkingBackward", direction == WalkDirection.backward);
		animator.SetBool("WalkingForward", direction == WalkDirection.forward);
		animator.SetBool("WalkingLeft", direction == WalkDirection.left);
	}

	// Update is called once per frame
	void Update()
	{
		var inputVector = input.Player.Move.ReadValue<Vector2>();

		animator.SetBool("IsWalking", inputVector.sqrMagnitude > 0);
		if(Mathf.Abs(inputVector.x) > 0.1f && Mathf.Abs(inputVector.y) > 0.1f)
		{
			inputVector.y = 0f;
		}

		if (inputVector.x > DIRECTION_THRESHOLD && currentDirection != WalkDirection.right)
		{
			SetDirection(WalkDirection.right);
		}
		else if (inputVector.y > DIRECTION_THRESHOLD && currentDirection != WalkDirection.forward)
		{
			SetDirection(WalkDirection.forward);
		}
		else if (inputVector.y < - DIRECTION_THRESHOLD && currentDirection != WalkDirection.backward)
		{
			SetDirection(WalkDirection.backward);
		}
		else if (inputVector.x < - DIRECTION_THRESHOLD && currentDirection != WalkDirection.left)
		{
			SetDirection(WalkDirection.left);
		}
	}
}
