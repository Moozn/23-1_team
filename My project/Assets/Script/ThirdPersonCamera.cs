using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;  // 플레이어 캐릭터의 Transform 컴포넌트
    public float rotationSpeed = 2.0f;  // 카메라 회전 속도
    public float zoomSpeed = 2.0f;  // 카메라 줌 속도
    public float minZoom = 2.0f;  // 최소 줌 거리
    public float maxZoom = 10.0f;  // 최대 줌 거리
    
    private Vector3 offset;  // 플레이어 캐릭터와 카메라 사이의 거리

    void Start()
    {
        offset = transform.position - player.position;  // 초기 거리 설정
    }
    public void OnRightMouse(InputAction.CallbackContext context)
    {
        bool mouse = context.ReadValueAsButton();
        if (mouse)
        {
           
            //  float mouseY = Input.GetAxis("Mouse Y") * 3;
            //  Camera.main.transform.Rotate(Vector3.left, mouseY);
            //  //    playerstate = State.Attack;

        }
    }

    void LateUpdate()
    {
        float horizontalInput = Input.GetAxis("Mouse X") * rotationSpeed;
        float verticalInput = -Input.GetAxis("Mouse Y") * rotationSpeed;

        // 카메라 회전
        Quaternion rotation = Quaternion.Euler(0, horizontalInput, 0);
        offset = rotation * offset;

        // 카메라 상하 이동
        offset = new Vector3(offset.x, Mathf.Clamp(offset.y + verticalInput, minZoom, maxZoom), offset.z);

        // 카메라 이동 및 주시 방향 설정
        transform.position = player.position + offset;
        transform.LookAt(player.position);
    }
}
    
