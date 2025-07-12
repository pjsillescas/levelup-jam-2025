using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class CameraManager : MonoBehaviour
{
	public CinemachineCamera SideScrollingCamera;
	public CinemachineCamera IsometricCamera;

	private const float SHOULDER_LENGTH_ISOMETRIC = 10;
	private const float SHOULDER_LENGTH_TOPDOWN = 20;

	public void SwitchCameraMode(CameraMode cameraMode)
	{
		var isIsometricCamera = cameraMode == CameraMode.Isometric || cameraMode == CameraMode.Topdown;
		// Camera switch via priority
		SideScrollingCamera.Priority = cameraMode == CameraMode.SideScroller ? 10 : 0;
		IsometricCamera.Priority = (isIsometricCamera) ? 10 : 0;

		if (isIsometricCamera)
		{
			var follow = IsometricCamera.GetComponent<CinemachineThirdPersonFollow>();

			var finalShoulderLength = 
				(cameraMode == CameraMode.Isometric) ? SHOULDER_LENGTH_ISOMETRIC : SHOULDER_LENGTH_TOPDOWN;

			StartCoroutine(UpdateShoulder(follow, finalShoulderLength * new Vector3(-1, 1, -1)));

		}
	}

	public IEnumerator UpdateShoulder(CinemachineThirdPersonFollow follow, Vector3 finalShoulderOffset)
	{
		int n = 50;
		var initialOffset = follow.ShoulderOffset;
		for (int i = 0; i < n; i++)
		{
			follow.ShoulderOffset = Vector3.Lerp(initialOffset, finalShoulderOffset, (float)i / n);
			yield return new WaitForSeconds(0.01f);
		}

		yield return null;
	}
}
