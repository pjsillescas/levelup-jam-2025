using UnityEngine;

public class Platform : MonoBehaviour
{
    private Vector3 _lastPosition;


    [SerializeField]
    private float smoothing = 0.2f;
    private void Start()
    {
        _lastPosition = transform.position;
    }

    private void FixedUpdate()
    {
        _velocity = (transform.position - _lastPosition);
        _lastPosition = transform.position;
    }

    private Vector3 _velocity;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.position += _velocity;
        }
    }
}

