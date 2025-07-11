using player;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CinematicTrigger : MonoBehaviour
{
    [SerializeField]
    private string sceneToTravelTo;

    [SerializeField]
    private Animator cinematicAnimator; // Animator para la cinemática

	private void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent(out PlayerController playerController))
		{
			Debug.Log($"Iniciando cinemática antes de saltar a '{sceneToTravelTo}'");
			StartCoroutine(PlayCinematicAndLoadScene());
		}
	}

	private IEnumerator PlayCinematicAndLoadScene()
	{
		if (cinematicAnimator != null)
		{
			cinematicAnimator.SetTrigger("Play"); // Iniciar la animación
			yield return new WaitForSeconds(cinematicAnimator.GetCurrentAnimatorStateInfo(0).length); // Esperar duración de la animación
		}
		SceneManager.LoadScene(sceneToTravelTo);
	}
}

