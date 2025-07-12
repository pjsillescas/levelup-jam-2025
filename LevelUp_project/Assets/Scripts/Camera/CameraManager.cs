using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
	public CinemachineCamera SideScrollingCamera;
	public CinemachineCamera IsometricCamera;
	public CinemachineCamera TopdownCamera;

	public void SwitchCameraMode(CameraMode cameraMode)
	{
		// Camera switch via priority
		SideScrollingCamera.Priority = cameraMode == CameraMode.SideScroller ? 10 : 0;
		IsometricCamera.Priority = cameraMode == CameraMode.Isometric ? 10 : 0;
		TopdownCamera.Priority = cameraMode == CameraMode.Topdown ? 10 : 0;
	}
}
