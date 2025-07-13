using UnityEngine;

public class Platform : MonoBehaviour
{

    private Quaternion originalRotation;



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            originalRotation = other.transform.rotation;
            if (transform.parent != null)
            {
                other.transform.SetParent(transform.parent);
            }
            else
            {
                other.transform.SetParent(transform);

            }

            other.transform.rotation = originalRotation;
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

