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

    [SerializeField]
    private int musicIndex = -1; // Índice de la música a reproducir después de la cinemática

    private CyntiaAnimation cyntiaAnimation;
    private Animator cyntiaAnimator;

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

        if (playerController != null)
        {
            cyntiaAnimation = playerController.GetComponentInChildren<CyntiaAnimation>();
            if (cyntiaAnimation != null)
            {
                cyntiaAnimator = cyntiaAnimation.GetComponent<Animator>();
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

		if (cyntiaAnimation != null)
		{
			cyntiaAnimation.enabled = false; // Desactivar CyntiaAnimation
		}

		if (cyntiaAnimator != null)
		{
			cyntiaAnimator.enabled = false; // Desactivar Animator
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

		if (cyntiaAnimation != null)
		{
			cyntiaAnimation.enabled = true; // Reactivar CyntiaAnimation
		}

		if (cyntiaAnimator != null)
		{
			cyntiaAnimator.enabled = true; // Reactivar Animator
		}

		if (!string.IsNullOrEmpty(sceneToTravelTo)) // Si no está vacío el nombre de la escena
		{
			SceneManager.LoadScene(sceneToTravelTo);
		}

		if (musicIndex >= 0 && AudioManager.instance != null) // Reproducir música si el índice es válido
		{
			AudioManager.instance.PlayMusic(musicIndex);
		}
	}
}

