using UnityEngine;
using UnityEngine.Splines;

public class GrowingPlatformTrigger : MonoBehaviour
{
    [Tooltip("Duration in seconds for full grow or shrink")]
    [SerializeField] private float timeToGrowUngrow = 2f;

    [Header("Spline references")]
    [SerializeField] private SplineAnimate splineAnimator;
    [SerializeField] private SplineExtrude splineExtrude;

    [Header("Extrusion range")]
    [SerializeField] private float startValue = 0f;
    [SerializeField] private float endValue = 1f;

    private float currentValue = 0f; // Normalized progress (0 to 1)
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

        // Only update while within valid range
        if ((isGrowing && currentValue < 1f) || (!isGrowing && currentValue > 0f))
        {
            currentValue += direction * speed * Time.deltaTime;
            currentValue = Mathf.Clamp01(currentValue);

            // Apply easing (ease-in/out)
            float easedValue = currentValue * currentValue * (3f - 2f * currentValue); // smoothstep

            // Update SplineExtrude
            float finalRange = Mathf.Lerp(startValue, endValue, easedValue);
            splineExtrude.Range = new Vector2(0, finalRange);
            splineExtrude.Rebuild();

            // Update SplineAnimate manually
            splineAnimator.ElapsedTime = easedValue * animationDuration;
        }
    }
}
