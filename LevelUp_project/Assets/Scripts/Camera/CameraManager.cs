using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class CameraManager : MonoBehaviour
{
	public CinemachineCamera SideScrollingCamera;
	public CinemachineCamera IsometricCamera;

	[SerializeField] private Vector3 SHOULDER_LENGTH_ISOMETRIC = new Vector3(10, 10, 10);
	[SerializeField] private Vector3 SHOULDER_LENGTH_TOPDOWN = new Vector3(20, 20, 20);

	public void SwitchCameraMode(CameraMode cameraMode)
	{
		var isIsometricCamera = cameraMode == CameraMode.Isometric || cameraMode == CameraMode.Topdown;
		// Camera switch via priority
		SideScrollingCamera.Priority = cameraMode == CameraMode.SideScroller ? 10 : 0;
		IsometricCamera.Priority = (isIsometricCamera) ? 10 : 0;

		if (isIsometricCamera)
		{
			var follow = IsometricCamera.GetComponent<CinemachineThirdPersonFollow>();

			var isIsometric = cameraMode == CameraMode.Isometric;
			var finalShoulderLength = isIsometric ? SHOULDER_LENGTH_ISOMETRIC : SHOULDER_LENGTH_TOPDOWN;
			var shoulderOffsetDirection = (isIsometric) ? new Vector3(-1, 1, -1) : new Vector3(-1, 2, -1);

			StartCoroutine(UpdateShoulder(follow, Vector3.Scale(finalShoulderLength, shoulderOffsetDirection)));

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
