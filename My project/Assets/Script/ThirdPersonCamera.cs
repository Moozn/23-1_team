using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;  // 플레이어 캐릭터의 Transform 컴포넌트
    public float rotationSpeed = 2.0f;  // 카메라 회전 속도
    public float zoomSpeed = 2.0f;  // 카메라 줌 속도 각도
    public float minAngle = 4.0f;  // 최소 줌 거리 각도
    public float maxAngle = 10.0f;  // 최대 줌 거리 각도
    public float minZoomDistance = 10.0f;  // 휠 조정 최소 줌 거리
    public float maxZoomDistance = 30.0f;  // 휠 조정 최대 줌 거리
    public float wheelzoomSpeed = 20.0f;  // 
    public Vector3 offset;  // 플레이어 캐릭터와 카메라 사이의 거리
    void Start()
    {
        offset = transform.position - player.position;  // 초기 거리 설정
    }


    void LateUpdate()
    {
        float horizontalInput = Input.GetAxis("Mouse X") * rotationSpeed;
        float verticalInput = -Input.GetAxis("Mouse Y") * rotationSpeed;

        // 카메라 회전
        Quaternion rotation = Quaternion.Euler(0, horizontalInput, 0);
        offset = rotation * offset;

        // 카메라 상하 이동
        offset = new Vector3(offset.x, Mathf.Clamp(offset.y + verticalInput, minAngle, maxAngle), offset.z);


        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float newDistance = offset.magnitude - scrollInput * wheelzoomSpeed;
        
        newDistance = Mathf.Clamp(newDistance, minZoomDistance, maxZoomDistance);
        offset = offset.normalized * newDistance;


        transform.position = player.position + offset;
        transform.LookAt(player.position);
    }
}