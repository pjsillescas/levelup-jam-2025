using UnityEngine;

public class MainMenu : MonoBehaviour
{
	void Start()
	{
		AudioManager.instance.PlayMusic(1);
	}

	private void OnDestroy()
	{
		AudioManager.instance.StopMusic();
	}
}
