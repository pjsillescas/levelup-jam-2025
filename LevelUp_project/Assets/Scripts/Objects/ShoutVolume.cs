using UnityEngine;

public class ShoutVolume : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			AudioManager.instance.PlaySFX(4);
		}
	}
}
