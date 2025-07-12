using UnityEngine;

public class Platform : MonoBehaviour
{

    private Quaternion originalRotation;



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            originalRotation = collision.transform.rotation;
            collision.transform.SetParent(transform);

            collision.transform.rotation = originalRotation;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.rotation = originalRotation;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.parent = null;
            other.transform.rotation = originalRotation;
        }
    }

}

