using System;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public static event EventHandler<Platform> OnPlayerRide;
	public static event EventHandler<Platform> OnPlayerLeave;

	public bool isPlayer; // Para GrowingPlant.cs
    private bool canChangeState = true; // Controla si se puede cambiar isPlayer para evitar clipping
    public float stateChangeCooldown = 0.5f; // Tiempo de espera entre cambios
    private Vector3 _lastPosition;


    [SerializeField]
    private float smoothing = 0.2f;
    
    private void Start()
    {
        _lastPosition = transform.position;
    }

	private void OnCollisionEnter(Collision collision)
	{
		if (canChangeState && collision.gameObject.CompareTag("Player"))
		{
			isPlayer = true;
			collision.transform.SetParent(transform);

			OnPlayerRide?.Invoke(this, this);
			//StartCoroutine(StateChangeCooldown());
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if (canChangeState && collision.gameObject.CompareTag("Player") && isPlayer) //
		{
			isPlayer = false;
			collision.transform.SetParent(null);
			OnPlayerLeave?.Invoke(this, this);
			StartCoroutine(StateChangeCooldown());
		}
	}

	private System.Collections.IEnumerator StateChangeCooldown()
	{
		canChangeState = false;
		yield return new WaitForSeconds(stateChangeCooldown);
		canChangeState = true;
    }
	private void FixedUpdate()
    {
        _velocity = (transform.position - _lastPosition);
        _lastPosition = transform.position;
    }

    private Vector3 _velocity;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.position += _velocity;
        }
	}
}

