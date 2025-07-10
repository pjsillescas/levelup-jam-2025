using player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TravelVolume : MonoBehaviour
{
    [SerializeField]
    private string sceneToTravelTo;

	private void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent(out PlayerController playerController))
		{
			Debug.Log($"saltando a '{sceneToTravelTo}'");
			SceneManager.LoadScene(sceneToTravelTo);
		}
	}
}
