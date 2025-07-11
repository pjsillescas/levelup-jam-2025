using System;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public static event EventHandler<Platform> OnPlatformEnter;
    public static event EventHandler<Platform> OnPlatformLeave;

    private Vector3 _lastPosition;
	private Vector3 _lastDisplacement;

	[SerializeField]
    private float smoothing = 0.2f;
    private void Start()
    {
        _lastPosition = transform.position;
    }

    private void FixedUpdate()
    {
		_lastDisplacement = (transform.position - _lastPosition);
        _lastPosition = transform.position;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
			collision.transform.position += _lastDisplacement;
        }
    }

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			OnPlatformEnter?.Invoke(this, this);
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			OnPlatformLeave?.Invoke(this, this);
		}

	}
}

