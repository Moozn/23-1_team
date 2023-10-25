using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTemp : MonoBehaviour
{
    public GameObject _player = null;
    public float fdelta = -5.0f;
    public Vector3 cameraDirection = Vector3.zero;
    void Start()
    {

    }

    void Update()
    {

    }

    private void LateUpdate()
    {
        // LateUpdate�̱� ������ �÷��̾� ��ġ�� �̵������� ���� ī�޶�� �̵����� ����
        // �̵��� �÷��̾� ��ġ�� �ٶ�
        transform.LookAt(_player.transform);
        // ���� �����ӿ� �÷��̾ �޾ư� �𷺼� ����
        cameraDirection = _player.transform.position - transform.position;
        cameraDirection.Normalize();
        // �÷��̾ �ٶ󺸰��ִ� �� ������ �ڷ� ������
        transform.position = _player.transform.position + fdelta * cameraDirection;
        // y���� ��ġ�� �÷��̾��� ���� ���� ���� ���� ��Ŵ
        Vector3 v = transform.position;
        v.y = _player.transform.position.y + 5.0f;
        transform.position = v;
    }
}