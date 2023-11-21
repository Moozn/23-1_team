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
    public LayerMask collisionLayer; // Set the layers that the camera should collide with
    public float collisionRadius = 0.3f; // Adjust the collision radius
    public float collisionOffset = 0.2f; // Adjust the offset to prevent clipping
    private float currentDistance;
    private float rotationX = 0.0f;
    private float rotationY = 0.0f;
    private bool camera;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        Cursor.visible = false; // Hide the cursor
        currentDistance = (minDistance + maxDistance) / 2;
        camera = true;
    }

    public void setcamera(bool b)
    {
        camera = b;
    }


  

    void HandleCameraCollision()
    {
        RaycastHit hit;
        Vector3 desiredPosition = transform.position;

        // Cast a ray from the camera to its desired position
        if (Physics.SphereCast(transform.position, collisionRadius, transform.forward, out hit, Mathf.Infinity, collisionLayer))
        {
            // Adjust the camera position based on the hit point and collision offset
            desiredPosition = hit.point - transform.forward * collisionOffset;
        }

        // Smoothly move the camera to the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 10f);
    }

    void LateUpdate()
    {
        //HandleCameraCollision();
    }
    void Update()
    {
        if (camera)
        {
            // Calculate camera rotation based on mouse input
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;
            rotationY -= Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minYAngle, maxYAngle);

            // Zoom in/out with the mouse scroll wheel
            currentDistance -= Input.GetAxis("Mouse ScrollWheel");
            currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);

            // Apply rotation to the camera
            //transform.rotation = Quaternion.Euler(rotationY, rotationX, 0);
            target.rotation = Quaternion.Euler(rotationY, rotationX, 0);
            // Calculate desired camera position
            //   Vector3 desiredPosition = target.position - (transform.rotation * Vector3.forward * currentDistance);

            // Apply the position to the camera
            //transform.position = desiredPosition;
           
            // Look at the target (the playable character)
            //transform.LookAt(target);
        }
    }
}