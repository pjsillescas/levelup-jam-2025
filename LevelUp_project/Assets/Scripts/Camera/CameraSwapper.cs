using player;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwapper : MonoBehaviour
{
    [SerializeField]
    private CameraMode cameraMode;

	private CameraMode originalCameraMode;

	private void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent(out PlayerController playerController))
		{
			originalCameraMode = playerController.GetCurrentCameraMode();
			var newCameraMode = (originalCameraMode == CameraMode.Isometric) ? CameraMode.Topdown : CameraMode.Isometric;
			playerController.SwitchToMode(newCameraMode);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.TryGetComponent(out PlayerController playerController))
		{
			playerController.SwitchToMode(originalCameraMode);
		}
	}
}
