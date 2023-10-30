using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; // The target to follow (the playable character)
    public float minDistance = 2.0f; // Minimum distance from the target
    public float maxDistance = 10.0f; // Maximum distance from the target
    public float sensitivityX = 5.0f; // X-axis rotation sensitivity
    public float sensitivityY = 2.0f; // Y-axis rotation sensitivity
    public float minYAngle = -30.0f; // Minimum Y-axis rotation angle
    public float maxYAngle = 80.0f; // Maximum Y-axis rotation angle

    private float currentDistance;
    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        Cursor.visible = false; // Hide the cursor
        currentDistance = (minDistance + maxDistance) / 2;
    }

    void Update()
    {
        // Calculate camera rotation based on mouse input
        rotationX += Input.GetAxis("Mouse X") * sensitivityX;
        rotationY -= Input.GetAxis("Mouse Y") * sensitivityY;
        rotationY = Mathf.Clamp(rotationY, minYAngle, maxYAngle);

        // Zoom in/out with the mouse scroll wheel
        currentDistance -= Input.GetAxis("Mouse ScrollWheel");
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);

        // Apply rotation to the camera
        transform.rotation = Quaternion.Euler(rotationY, rotationX, 0);

        // Calculate desired camera position
        Vector3 desiredPosition = target.position - (transform.rotation * Vector3.forward * currentDistance);

        // Apply the position to the camera
        transform.position = desiredPosition;

        // Look at the target (the playable character)
        transform.LookAt(target);
    }
}