using Unity.Cinemachine;
using UnityEngine;

namespace camera
{
	public class CameraSwitcher : MonoBehaviour
	{
		[SerializeField]
		private CinemachineCamera sideCam;
		[SerializeField]
		private CinemachineCamera isoCam;
		[SerializeField]
		private CinemachineCamera topdownCam;

		public void SwitchToIsometric()
		{
			sideCam.Priority = 0;
			isoCam.Priority = 10;
			topdownCam.Priority = 0;
		}

		public void SwitchToSideScroller()
		{
			sideCam.Priority = 10;
			isoCam.Priority = 0;
			topdownCam.Priority = 0;
		}
		public void SwitchToTopdown()
		{
			topdownCam.Priority = 10;
			sideCam.Priority = 0;
			isoCam.Priority = 0;
		}
	}
}