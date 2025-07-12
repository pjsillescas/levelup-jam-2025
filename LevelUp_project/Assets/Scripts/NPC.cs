using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("Rotation Settings")]
    public Vector3 rotationAxis = Vector3.up;
    public float rotationRange = 30f;
    public float rotationSpeed = 2f;

    private float currentAngle = 0f;
    private float targetAngle = 0f;
    private bool rotatingForward = true;

    void Start()
    {
        // Elegir dirección aleatoria
        rotatingForward = Random.value > 0.5f;
        // Elegir ángulo inicial aleatorio dentro del rango correspondiente
        currentAngle = rotatingForward
            ? Random.Range(0f, rotationRange)
            : Random.Range(-rotationRange, 0f);
        SetNewTargetAngle();
    }

    void Update()
    {
        RotatePingPongRandom();
    }

    private void RotatePingPongRandom()
    {
        float rotationStep = rotationSpeed * Time.deltaTime;
        if (rotatingForward)
        {
            currentAngle = Mathf.MoveTowards(currentAngle, targetAngle, rotationStep);
            if (Mathf.Approximately(currentAngle, targetAngle))
            {
                rotatingForward = false;
                SetNewTargetAngle();
            }
        }
        else
        {
            currentAngle = Mathf.MoveTowards(currentAngle, targetAngle, rotationStep);
            if (Mathf.Approximately(currentAngle, targetAngle))
            {
                rotatingForward = true;
                SetNewTargetAngle();
            }
        }
        transform.localRotation = Quaternion.Euler(rotationAxis * currentAngle);
    }

    private void SetNewTargetAngle()
    {
        if (rotatingForward)
            targetAngle = Random.Range(0f, rotationRange);
        else
            targetAngle = Random.Range(-rotationRange, 0f);
    }
}
