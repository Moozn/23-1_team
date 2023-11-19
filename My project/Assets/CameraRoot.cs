using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRoot : MonoBehaviour
{
  [SerializeField] Transform cameraFollwTarget;
    private float xRotation = 0f;
    private float yRotation = 0f;

    private void LateUpdate()
    {
        CameraRotation();
    }

    void CameraRotation()
    {
        xRotation += Input.GetAxis("Mouse X");
        yRotation += Input.GetAxis("Mouse Y");
        xRotation = Mathf.Clamp(xRotation, -30, 70);
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);
        cameraFollwTarget.rotation = rotation;
    }
}
