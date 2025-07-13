using UnityEngine;

public class ShadowCaster : MonoBehaviour
{
    [SerializeField] private GameObject shadowSphere;
    [SerializeField] private float shadowHeightOffset = 0.01f;
    [SerializeField] private LayerMask shadowLayers;

    private void FixedUpdate()
    {
        Ray downRay = new Ray(transform.position, Vector3.down);

        RaycastHit hit;
        // Ahora el raycast solo considera los layers especificados
        if (Physics.Raycast(downRay, out hit, Mathf.Infinity, shadowLayers))
        {
            Vector3 shadowPos = hit.point + Vector3.up * shadowHeightOffset;
            shadowSphere.transform.position = shadowPos;
        }
    }
}
