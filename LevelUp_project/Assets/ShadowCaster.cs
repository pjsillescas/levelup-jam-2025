using UnityEngine;

public class ShadowCaster : MonoBehaviour
{
    [SerializeField] private GameObject shadowSphere;
    [SerializeField] private float shadowHeightOffset = 0.01f; // 

    private void FixedUpdate()
    {
        Ray downRay = new Ray(transform.position, Vector3.down);

        RaycastHit hit;
        //Math Infinty can be changed if there is not going to be any ground below.
        if (Physics.Raycast(downRay, out hit, Mathf.Infinity))
        {
            Vector3 shadowPos = hit.point + Vector3.up * shadowHeightOffset;
            shadowSphere.transform.position = shadowPos;
        }
    }

}
