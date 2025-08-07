using UnityEngine;

public class ShadowCaster : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject shadowSphere;
    [SerializeField] private Transform rayOrigin;

    [Header("Raycast Settings")]
    [SerializeField] private float shadowHeightOffset = 0.01f;
    [SerializeField] private float headOffset = 0.5f; 
    [SerializeField] private LayerMask shadowLayers;


    [Header("Shadow Size Settings")]
    [SerializeField] private float minScale = 0.5f;
    [SerializeField] private float maxScale = 0.8f;
    [SerializeField] private float maxDistance = 10f; // Distance at which the shadow is at min size

    private void FixedUpdate()
    {
        Vector3 startPoint = rayOrigin.transform.position + Vector3.up * headOffset;

        Ray downRay = new Ray(startPoint, Vector3.down);

        if (Physics.Raycast(downRay, out RaycastHit hit, Mathf.Infinity, shadowLayers))
        {
            Vector3 shadowPos = hit.point + Vector3.up * shadowHeightOffset;
            shadowSphere.transform.position = shadowPos;

            // Calculate distance from ray origin to hit point
            float distance = Vector3.Distance(startPoint, hit.point);

            // Normalize distance (clamped between 0 and 1)
            float t = Mathf.Clamp01(distance / maxDistance);

            // Lerp scale based on distance (closer = larger)
            float scale = Mathf.Lerp(maxScale, minScale, t);

            // Apply scale uniformly
            shadowSphere.transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
