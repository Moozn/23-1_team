using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;  // �÷��̾� ĳ������ Transform ������Ʈ
    public float rotationSpeed = 2.0f;  // ī�޶� ȸ�� �ӵ�
    public float zoomSpeed = 2.0f;  // ī�޶� �� �ӵ� ����
    public float minAngle = 4.0f;  // �ּ� �� �Ÿ� ����
    public float maxAngle = 10.0f;  // �ִ� �� �Ÿ� ����
    public float minZoomDistance = 10.0f;  // �� ���� �ּ� �� �Ÿ�
    public float maxZoomDistance = 30.0f;  // �� ���� �ִ� �� �Ÿ�
    public float wheelzoomSpeed = 20.0f;  // 
    public Vector3 offset;  // �÷��̾� ĳ���Ϳ� ī�޶� ������ �Ÿ�
    void Start()
    {
        offset = transform.position - player.position;  // �ʱ� �Ÿ� ����
    }


    void LateUpdate()
    {
        float horizontalInput = Input.GetAxis("Mouse X") * rotationSpeed;
        float verticalInput = -Input.GetAxis("Mouse Y") * rotationSpeed;

        // ī�޶� ȸ��
        Quaternion rotation = Quaternion.Euler(0, horizontalInput, 0);
        offset = rotation * offset;

        // ī�޶� ���� �̵�
        offset = new Vector3(offset.x, Mathf.Clamp(offset.y + verticalInput, minAngle, maxAngle), offset.z);


        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float newDistance = offset.magnitude - scrollInput * wheelzoomSpeed;
        
        newDistance = Mathf.Clamp(newDistance, minZoomDistance, maxZoomDistance);
        offset = offset.normalized * newDistance;


        transform.position = player.position + offset;
        transform.LookAt(player.position);
    }
}