using player;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwapper : MonoBehaviour
{
    [SerializeField]
    private CameraMode cameraMode;

	private void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent(out PlayerController playerController))
		{
			playerController.SwitchToMode(cameraMode);
		}
	}
}
