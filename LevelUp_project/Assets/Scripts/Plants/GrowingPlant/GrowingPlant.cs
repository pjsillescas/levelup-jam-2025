using UnityEngine;
using UnityEngine.Splines;

public class GrowingPlant : MonoBehaviour
{
    public Platform platform;
    public SplineAnimate splineAnimate;

    void Update()
    {
        if (splineAnimate == null) return;

        if (platform.isPlayer)
        {
            splineAnimate.Play();
        }
        else
        {
            StartCoroutine(ResetSplineAfterDelay());
        }
    }

    private System.Collections.IEnumerator ResetSplineAfterDelay()
    {
        if (splineAnimate == null) yield break;

        yield return new WaitForSeconds(2);
        splineAnimate.Restart(false);
    }
}
