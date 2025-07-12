using player;
using UnityEngine;

public class JumpInhibitorVolume : MonoBehaviour
{
	public enum JumpInhibit { Inhibit, Activate, Swap }
	[SerializeField]
	private JumpInhibit inhibitJump;

	private bool originalJumpState;

	private void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent(out PlayerController playerController))
		{
			originalJumpState = playerController.IsJumpEnabled();
			Debug.Log(inhibitJump);
			switch (inhibitJump)
			{
				case JumpInhibit.Swap:
					Debug.Log("switch jump");
					playerController.SwitchJump();
					break;
				case JumpInhibit.Inhibit:
					Debug.Log("inhibit jump");
					playerController.DisableJump();
					break;
				case JumpInhibit.Activate:
				default:
					Debug.Log("activate jump");
					playerController.EnableJump();
					break;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.TryGetComponent(out PlayerController playerController))
		{
			if (originalJumpState)
			{
				playerController.EnableJump();
			}
			else
			{
				playerController.DisableJump();
			}
		}
	}
}
