using UnityEngine;
using UnityEngine.Splines;

public class GrowPlantPlatformTrigger : MonoBehaviour
{
    private SplineAnimate _splineAnimator;
    [Tooltip("Reference to the vine manager attached to the vine plant gameobject")]
    [SerializeField] private GrowVineManager _vineManager;

    private void Start()
    {
        if (_vineManager == null) Debug.LogWarning("Falta por asignar la referencia al vineManager de la planta en la plataforma!");
        _splineAnimator = GetComponent<SplineAnimate>();
        if (_splineAnimator == null) Debug.LogWarning("El componente spline animate tiene que ser hermano de el script GorwPlantPlatformTrigger de la plataforma!!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _splineAnimator.Play();
            _vineManager.ToggleVines();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // TODO Implementar Bajada de la plataforma No se puede invertir hay que hacer alguna triquiñuela ( PREGUNTAR A DOMENEC)
           
        }
    }
}
