using UnityEngine;
using UnityEngine.Splines;

public class PlatformMovement : MonoBehaviour
{
    [Header("Platform Movement Settings")]
    [SerializeField] private float speed = 1f;
    [Tooltip("Tiempo de espera en segundos antes de cambiar de dirección")]
    [SerializeField] private float waitTime = 0f;
    [Tooltip("Si está activado, la plataforma frena en acercarse al destino y acelera al alejarse")]
    [SerializeField] private bool smoothedMovement;
    [Tooltip("Proporción donde empieza la zona de frenado suave al acercarse a los extremos entrar valor entre 0y 1")]
    [SerializeField] private float slowdownZone = 0.3f;
    [Tooltip("Activar si el circuito es cerrado")]
    [SerializeField] private bool closedLoop;
    [Tooltip("Si está activado, la plataforma se mueve al entrar en contacto con el jugador y vuelve al inicio al salir")]
    [SerializeField] private bool movesOnPlayerContact;
    [Tooltip("Velocidad de rotación suave de la plataforma")]
    [SerializeField] private float rotationSpeed = 2f;


    [Header("Spline Reference")]
    [Tooltip("Crear spline con la herramienta spline de Unity y arrastrar aquí")]
    [SerializeField] private SplineContainer spline;
    [Tooltip("Componente que gestiona el movimiento del jugador sobre la plataforma evitando que se deforme por la escala de la plataforma")]
    [SerializeField] private PlatformAttach platformAttach;

    //Variables internas para gestionar el movimiento de la plataforma
    private float distancePercentage = 0f;
    private float splineLength;
    private bool reverseDirection = false;
    private bool isPlayerOnPlatform = false;
    private float realSpeed;
    private Transform attachTransform;
    private float waitTimer = 0f;


    void Start()
    {
        attachTransform = platformAttach.transform;
        splineLength = spline.CalculateLength();
        attachTransform.position = spline.EvaluatePosition(0f);
    }

    void Update()
    {
        if (movesOnPlayerContact && !isPlayerOnPlatform && distancePercentage < 0.01f || waitTimer > Time.time)
            return; // No mover la plataforma si no hay jugador en contacto

        //Modificar la velocidad según si el movimiento es suave
        if (smoothedMovement)
        {
            // Calcular la distancia al punto más cercano (inicio o final)
            float distanceToStart = distancePercentage;
            float distanceToEnd = 1f - distancePercentage;
            float minDistance = Mathf.Min(distanceToStart, distanceToEnd);

            // Crear una curva de frenado suave cerca de los extremos
            if (minDistance < slowdownZone)
            {
                float speedMultiplier = minDistance / slowdownZone;
                realSpeed = Mathf.Max(0.2f, speed * Mathf.Clamp01(speedMultiplier));
            }
            else
            {
                realSpeed = speed;
            }
        }
        else
        {
            realSpeed = speed;
        }



        if (reverseDirection)
        {
            distancePercentage -= realSpeed * Time.deltaTime / splineLength;
            Vector3 currentPosition = spline.EvaluatePosition(distancePercentage);
            attachTransform.position = currentPosition;
            if (distancePercentage <= 0.01f)
            {
                waitTimer = Time.time + waitTime; // Iniciar temporizador de espera
                distancePercentage = 0f;
                reverseDirection = false; // Detener la reversa al llegar al inicio
            }

        }
        else
        {
            distancePercentage += realSpeed * Time.deltaTime / splineLength;
            Vector3 currentPosition = spline.EvaluatePosition(distancePercentage);
            attachTransform.position = currentPosition;
            if (distancePercentage >= 1f)
            {
                waitTimer = Time.time + waitTime; // Iniciar temporizador de espera
                if (closedLoop)
                {
                    distancePercentage = 0f; // Reiniciar al inicio si es un circuito cerrado
                }
                else
                {
                    distancePercentage = 1f; // Detener al final si no es un circuito cerrado
                    reverseDirection = true; // Cambiar dirección para volver al inicio
                }
            }
        }

        //Modificar rotación de la plataforma
        Vector3 nextPosition;
        if (reverseDirection)
            nextPosition = spline.EvaluatePosition(Mathf.Max(0f, distancePercentage - 0.05f));
        else
            nextPosition = spline.EvaluatePosition(Mathf.Min(1f, distancePercentage + 0.05f));

        Vector3 direction = nextPosition - transform.position;
        if (direction.magnitude < 0.01f)
            return; // Evitar rotación si la dirección es muy pequeña

        // Calcular la rotación objetivo
        Quaternion targetRotation = Quaternion.LookRotation(direction, transform.up);

        // Aplicar rotación suave usando Slerp
        attachTransform.rotation = Quaternion.Slerp(attachTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(attachTransform); // Hacer que el jugador sea hijo de la plataforma
            isPlayerOnPlatform = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnPlatform = false;
            other.transform.SetParent(null); // Quitar al jugador de la plataforma
            if (movesOnPlayerContact)
                reverseDirection = true; //Asegurar que la plataforma vuelve al inicio al salir el jugador
        }
    }
}
