using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class MovingVehicle : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private float arcAngle = 90f;
    [SerializeField] int rayCount = 9;
    [SerializeField] float rayLength = 10f;
    [SerializeField] private Transform rayOrigin;


    [Header("Spline Settings")]
    [SerializeField] SplineAnimate splineAnimator;
    [SerializeField] float resumeDelay;



    [Header("Wheel animation")]
    [SerializeField] private List<GameObject> wheelPrefabs;
    [SerializeField] private float wheelRotationSpeed;
    private bool isPlayerInFront;
    private bool isPaused = false;
    private float resumeTimer = 0f;

    private void FixedUpdate()
    {
        bool isPlayerInFront = CastRayArc();
        RotateWheels();

        if (isPlayerInFront)
        {
            if (!isPaused)
            {
                splineAnimator.Pause();
                isPaused = true;

            }
            resumeTimer = 0f;
        }
        else
        {
            //Timer
            if (isPaused)
            {
                resumeTimer += Time.deltaTime;
                if(resumeTimer > resumeDelay)
                {
                    isPaused = false;
                    splineAnimator.Play();
                }
            }
        }
    }


    private bool CastRayArc()
    {
        float halfArc = arcAngle / 2;
        isPlayerInFront = false;

        for (int i = 0; i < rayCount; i++)
        {
            float t = (float)i / (rayCount - 1);
            float angle = Mathf.Lerp(-halfArc, halfArc, t);

            Quaternion rayRotation = Quaternion.Euler(0, angle, 0);
            Vector3 direction = rayRotation * rayOrigin.transform.forward;

            //Raycast
            Ray ray = new Ray(rayOrigin.transform.position, direction);
            if (Physics.Raycast(ray, out RaycastHit hit, rayLength))
            {
                
                if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Vehicle"))
                {
                    
                    isPlayerInFront = true;
                }
            }
            Debug.DrawRay(rayOrigin.transform.position, direction * rayLength, Color.green);
        }
        return isPlayerInFront;
    }


    private void RotateWheels()
    {
        if(!isPlayerInFront)
        {
            foreach (GameObject pairOfWheels in wheelPrefabs)
            {
                pairOfWheels.transform.Rotate(Vector3.right * wheelRotationSpeed * Time.deltaTime);

            }


        }
    }
}
