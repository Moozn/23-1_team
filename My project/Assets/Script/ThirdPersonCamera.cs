using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; // The target to follow (the playable character)
    public LayerMask obstacleLayer; // Layer mask for obstacles that should block the camera
    public float minDistance = 2.0f; // Minimum distance from the target
    public float maxDistance = 10.0f; // Maximum distance from the target
    private float sensitivityX; // X-axis rotation sensitivity
    private float sensitivityY; // Y-axis rotation sensitivity
    public float minYAngle = -30.0f; // Minimum Y-axis rotation angle
    public float maxYAngle = 80.0f; // Maximum Y-axis rotation angle
    private float currentDistance;
    private float rotationX = 0.0f;
    private float rotationY = 0.0f;
    private bool camera = true;

    void Start()
    {
        Setsensitivity();
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        Cursor.visible = false; // Hide the cursor
        currentDistance = (minDistance + maxDistance) / 2;
    }

    public void Setsensitivity()
    {
        sensitivityX = GameMgr.instance.sensitivity() * 10;
        sensitivityY = GameMgr.instance.sensitivity() * 4;
    }

    void Update()
    {
        if (!Cursor.visible)
        {
            // Calculate camera rotation based on mouse input
            rotationX += Input.GetAxis("Mouse X") * sensitivityX * Time.deltaTime * 100;
            rotationY -= Input.GetAxis("Mouse Y") * sensitivityY * Time.deltaTime * 100;
            rotationY = Mathf.Clamp(rotationY, minYAngle, maxYAngle);

            // Zoom in/out with the mouse scroll wheel
            currentDistance -= Input.GetAxis("Mouse ScrollWheel");
            currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);

            // Apply rotation to the camera
            transform.rotation = Quaternion.Euler(rotationY, rotationX, 0);
            target.rotation = Quaternion.Euler(rotationY, rotationX, 0);

            // Calculate desired camera position
            Vector3 desiredPosition = target.position - (transform.rotation * Vector3.forward * currentDistance);

            // Check for obstacles
            RaycastHit hit;
            if (Physics.Raycast(target.position, desiredPosition - target.position, out hit, currentDistance, obstacleLayer))
            {
                // If an obstacle is hit, adjust the camera position to the hit point
                transform.position = hit.point;
            }
            else
            {
                // If no obstacle is hit, apply the desired position to the camera
                transform.position = desiredPosition;
            }

            // Look at the target (the playable character)
            transform.LookAt(target);
        }
    }
}