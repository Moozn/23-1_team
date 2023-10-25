using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform camreaArm;
    float sensitivity = 2.0f;
    float maxVerticalAngle = 160.0f;
    private void Update()
    {
        LookAround();
    }

    private void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = camreaArm.rotation.eulerAngles;
        //   camreaArm.rotation = Quaternion.Euler(camAngle.x - mouseDelta.y, camAngle.y + mouseDelta.x, camAngle.z);
        //camreaArm.rotation = Quaternion.Euler(camAngle.x - mouseDelta.y * sensitivity, camAngle.y + mouseDelta.x * sensitivity, camAngle.z);

        camAngle.x -= mouseDelta.y * sensitivity;
        camAngle.y += mouseDelta.x * sensitivity;

        camAngle.x = Mathf.Clamp(camAngle.x, -maxVerticalAngle, maxVerticalAngle);

        camreaArm.rotation = Quaternion.Euler(camAngle);
    }
}
