using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class GrowingPlatformTrigger : MonoBehaviour
{
    [SerializeField] private float timeToGrow;
    [SerializeField] private SplineExtrude splineExtrude;
    [SerializeField] private SplineAnimate splineAnimator;
    private bool isGrowing;




    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(GrowVine());
            splineAnimator.Play();
        }
    }


    private IEnumerator GrowVine()
    {
        isGrowing = true;

        float start = 0f;
        float end = 1f; // 100%
        float duration = timeToGrow;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            splineExtrude.Range = new Vector2(start, Mathf.Lerp(start, end, t));
            splineExtrude.Rebuild();
            yield return null;
        }

        splineExtrude.Range = new Vector2(start, end); //Make sure it ends at 100%
        isGrowing = false;
    }
}

