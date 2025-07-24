using player;
using System;
using Unity.VisualScripting;
using UnityEngine;
using static player.PlayerController;

public class PlayerEffects : MonoBehaviour
{
	[Header("Particle Systems")]
	[SerializeField] private ParticleSystem walkParticles;
	[SerializeField] private ParticleSystem jumpParticles;

	private bool isGrounded;

	void Start()
	{
		PlayerController.OnMove += OnMove;
		PlayerController.OnJump += OnJump;
		PlayerController.OnGrounded += OnGrounded;
		
		isGrounded = true;
	}

	private void OnDestroy()
	{
		PlayerController.OnMove -= OnMove;
		PlayerController.OnJump -= OnJump;
		PlayerController.OnGrounded -= OnGrounded;
	}

	private void OnMove(object sender, Vector2 inputVector)
	{
		if ((inputVector.x != 0 || inputVector.y != 0) && isGrounded)
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

	private void OnJump(object sender, EventArgs args)
	{
		jumpParticles.Play();
		AudioManager.instance.PlaySFX(2);
		isGrounded = false;
	}

	private void OnGrounded(object sender, GroundType groundType)
	{
		isGrounded = true;

		if (!jumpParticles.isPlaying) jumpParticles.Play();

		if (groundType == GroundType.Waterlily)
		{
			AudioManager.instance.PlaySFX(5);
		}
		else
		{
			AudioManager.instance.PlaySFX(3);
		}
	}
}
