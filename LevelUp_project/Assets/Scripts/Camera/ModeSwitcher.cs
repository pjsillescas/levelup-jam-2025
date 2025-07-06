using player;
using Unity.Cinemachine;
using UnityEngine;

public class ModeSwitcher : MonoBehaviour
{
	//public PlayerController playerController;
	public CinemachineCamera sideCam;
	public CinemachineCamera isoCam;

	private PlayerController.CameraMode currentMode = PlayerController.CameraMode.SideScroller;

	public void ToggleMode(PlayerController.CameraMode currentMode)
	{
		this.currentMode = currentMode;

		//playerController.SwitchToMode(currentMode);

		// Camera switch via priority
		sideCam.Priority = currentMode == PlayerController.CameraMode.SideScroller ? 10 : 0;
		isoCam.Priority = currentMode == PlayerController.CameraMode.Isometric ? 10 : 0;
	}
}
