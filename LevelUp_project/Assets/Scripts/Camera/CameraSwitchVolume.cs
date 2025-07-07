using player;
using UnityEngine;

public class CameraSwitchVolume : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.TryGetComponent(out PlayerController playerController))
		{
			playerController.SwitchCameraMode();
		}
	}
}
