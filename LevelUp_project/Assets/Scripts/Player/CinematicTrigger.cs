using player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using System.Collections;

public class CinematicTrigger : MonoBehaviour
{
    [SerializeField]
    private string sceneToTravelTo;

    [SerializeField]
    private PlayableDirector cinematicDirector;

    [SerializeField]
    private bool playCinematicOnAwake;

    [SerializeField]
    private PlayerController playerController;

    private void Awake()
    {
		if (playerController == null)
		{
			GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
			if (playerObject != null)
			{
				playerController = playerObject.GetComponent<PlayerController>();
			}
		}

        if (playCinematicOnAwake && cinematicDirector != null)
		{
			StartCoroutine(PlayCinematicAndLoadScene());
		}
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Debug.Log($"Iniciando cinemática antes de saltar a '{sceneToTravelTo}'");
			StartCoroutine(PlayCinematicAndLoadScene());
		}
	}

	private IEnumerator PlayCinematicAndLoadScene()
	{
		if (playerController != null)
		{
			playerController.enabled = false; // Desactivar PlayerController
		}

		if (cinematicDirector != null)
		{
			cinematicDirector.Play();
			yield return new WaitForSeconds((float)cinematicDirector.duration);
		}

		if (playerController != null)
		{
			playerController.enabled = true; // Reactivar PlayerController
		}

		if (!string.IsNullOrEmpty(sceneToTravelTo)) // Si no está vacío el nombre de la escena
		{
			SceneManager.LoadScene(sceneToTravelTo);
		}
	}
}

