using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;  // �÷��̾� ĳ������ Transform ������Ʈ
    public float rotationSpeed = 2.0f;  // ī�޶� ȸ�� �ӵ�
    public float zoomSpeed = 2.0f;  // ī�޶� �� �ӵ�
    public float minZoom = 2.0f;  // �ּ� �� �Ÿ�
    public float maxZoom = 10.0f;  // �ִ� �� �Ÿ�
    
    private Vector3 offset;  // �÷��̾� ĳ���Ϳ� ī�޶� ������ �Ÿ�

    void Start()
    {
        offset = transform.position - player.position;  // �ʱ� �Ÿ� ����
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

        // ī�޶� ȸ��
        Quaternion rotation = Quaternion.Euler(0, horizontalInput, 0);
        offset = rotation * offset;

        // ī�޶� ���� �̵�
        offset = new Vector3(offset.x, Mathf.Clamp(offset.y + verticalInput, minZoom, maxZoom), offset.z);

        // ī�޶� �̵� �� �ֽ� ���� ����
        transform.position = player.position + offset;
        transform.LookAt(player.position);
    }
}
    
