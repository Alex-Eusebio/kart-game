using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileVisualizer : MonoBehaviour
{
    public LineRenderer trajectoryLine; // Assign this in the Inspector
    public int resolution = 30; // Number of points in the trajectory line
    public float throwForce = 20f; // Initial force applied to the projectile
    public Transform spawnPoint; // Point where the projectile is spawned

    private void Update()
    {
        if (Input.GetButton("Aim")) // Hold to visualize trajectory
        {
            Vector3 throwDirection = GetThrowDirection();
            ShowTrajectory(spawnPoint.position, throwDirection);
        }
        else
        {
            ClearTrajectory(); // Hide trajectory when not aiming
        }
    }

    private Vector3 GetThrowDirection()
    {
        // Use camera's forward direction for aiming, or the car's forward as needed
        Vector3 throwDirection = Camera.main.transform.forward;
        throwDirection.y = Mathf.Max(throwDirection.y, 0); // Prevent downward throws
        return throwDirection.normalized;
    }

    public void ShowTrajectory(Vector3 startPosition, Vector3 direction)
    {
        trajectoryLine.positionCount = resolution;
        Vector3 velocity = direction * throwForce;
        float timeStep = 0.1f; // Smaller values give smoother curves

        for (int i = 0; i < resolution; i++)
        {
            float t = i * timeStep;
            // Use physics to calculate position over time
            Vector3 position = startPosition + velocity * t + 0.5f * Physics.gravity * t * t;
            trajectoryLine.SetPosition(i, position);
        }
    }

    public void ClearTrajectory()
    {
        trajectoryLine.positionCount = 0;
    }
}
