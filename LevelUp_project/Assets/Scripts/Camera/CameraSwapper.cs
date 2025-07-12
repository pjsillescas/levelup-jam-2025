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
			var cameraMode = playerController.GetCurrentCameraMode();
			var newCameraMode = (cameraMode == CameraMode.Isometric) ? CameraMode.Topdown : CameraMode.Isometric;
			playerController.SwitchToMode(newCameraMode);
		}
	}
}
