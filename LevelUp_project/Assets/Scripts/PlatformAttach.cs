using UnityEngine;

public class PlatformAttach : MonoBehaviour
{
    [SerializeField] GameObject platform;

    void FixedUpdate()
    {
        transform.position = platform.transform.position;
        transform.rotation = platform.transform.rotation;
    }
}
