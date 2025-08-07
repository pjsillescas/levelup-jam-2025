using UnityEngine;
using UnityEngine.Splines;

public class GrowingPlatformTrigger : MonoBehaviour
{
    [Tooltip("Duration in seconds for full grow or shrink")]
    [SerializeField] private float timeToGrowUngrow = 4f;

    [Header("Spline references")]
    [SerializeField] private SplineAnimate splineAnimator;
    [SerializeField] private SplineExtrude splineExtrude;

    [Header("Extrusion range")]
    [SerializeField] private float startValue = 0f;
    [SerializeField] private float endValue = 1f;

    [Header("Custom growth curves")]
    [SerializeField] private AnimationCurve extrusionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private float currentValue = 0f;
    private bool isGrowing = false;

    private float animationDuration => splineAnimator.Duration;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isGrowing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isGrowing = false;
        }
    }

    private void Update()
    {
        float direction = isGrowing ? 1f : -1f;
        float speed = 1f / timeToGrowUngrow;

        if ((isGrowing && currentValue < 1f) || (!isGrowing && currentValue > 0f))
        {
            currentValue += direction * speed * Time.deltaTime;
            currentValue = Mathf.Clamp01(currentValue);

            // Eased values
            float extrusionValue = extrusionCurve.Evaluate(currentValue);
            float animationValue = animationCurve.Evaluate(currentValue);

            // Update SplineExtrude (tallo)
            float finalRange = Mathf.Lerp(startValue, endValue, extrusionValue);
            splineExtrude.Range = new Vector2(0, finalRange);
            splineExtrude.Rebuild();

            // Update SplineAnimate (hoja)
            splineAnimator.ElapsedTime = animationValue * animationDuration;
        }
    }
}
