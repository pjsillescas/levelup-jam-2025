using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class GrowingPlatformTrigger : MonoBehaviour
{
    [Tooltip("IMPORTANT, THIS VALUE NEEDS TO BE THE SAME AS THE DURATION OF THE SPLINE ANIMATION")]
    [SerializeField] private float timeToGrowUngrow;

    [Header("Spline references")]
    [SerializeField] private SplineAnimate splineAnimator;
    [SerializeField] private SplineExtrude splineExtrude;


    private bool isGrowing;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(GrowUnGrowVine(0f,1f));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(WaitForAnimation());
        }
    }

    private IEnumerator GrowUnGrowVine(float start, float end)
    {
        splineAnimator.Play();
        float elapsed = 0f;
        isGrowing = true;

        while (elapsed < timeToGrowUngrow)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / timeToGrowUngrow);

            //Ease In Ease Out
            float easedT = t* t* (3f - 2f * t);

            splineExtrude.Range = new Vector2(0, Mathf.Lerp(start, end, easedT));
            splineExtrude.Rebuild();
            yield return null;
        }

        isGrowing = false;
        splineAnimator.Pause();
    }

    private IEnumerator WaitForAnimation()
    {
        while (isGrowing)
        {
            yield return null;
        }

        StartCoroutine(GrowUnGrowVine(1f, 0f));
    }


}

