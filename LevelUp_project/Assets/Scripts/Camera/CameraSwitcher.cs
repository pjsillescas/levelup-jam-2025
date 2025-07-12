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

		public void SwitchToIsometric()
		{
			sideCam.Priority = 0;
			isoCam.Priority = 10;
		}

		public void SwitchToSideScroller()
		{
			sideCam.Priority = 10;
			isoCam.Priority = 0;
		}
	}
}