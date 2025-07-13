using player;
using UnityEngine;

public class SubmersibleVolume : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if(other.TryGetComponent(out PlayerController playerController))
		{
			Debug.Log("sumergido");
			playerController.SetIsSubmerged(true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.TryGetComponent(out PlayerController playerController))
		{
			playerController.SetIsSubmerged(false);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.TryGetComponent(out PlayerController playerController))
		{
			Debug.Log("entramos?");
			playerController.SetIsSubmerged(true);
		}
	}

}
