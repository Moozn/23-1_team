using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTemp : MonoBehaviour
{
    Camera _camera;
    // �Է� ��, Ű���� �Է��� �޾� ĳ���͸� ȸ�� ��ų ������ �����Ѵ�
    // �׳� if ������ ������ �޾Ƶ� �Ǵµ� if �ߺ� ���� ���ͷ� ǥ���ϴ� �� ���������� ��ȣ�ؼ�
    Vector2 _direction = Vector2.zero;
    void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        _direction = Vector2.zero;
        // ������ ���� ������ ȸ�� �� 0��
        if (Input.GetKey(KeyCode.W))
        {
            // �ش� ���� �ش��ϴ� ���͸� �����ش� ������ �ƴ϶�
            // �������� �ڻ��� ���� �� ������ ������ ǥ���ϱ� ���ؼ�
            _direction += new Vector2(Mathf.Cos(0 * Mathf.Deg2Rad), Mathf.Sin(0 * Mathf.Deg2Rad));
        }
        // �������� ���� ������ ȸ�� �� -90��
        if (Input.GetKey(KeyCode.A))
        {
            _direction += new Vector2(Mathf.Cos(-90 * Mathf.Deg2Rad), Mathf.Sin(-90 * Mathf.Deg2Rad));
        }
        // �ڷ� ���� ������ ȸ�� �� 180��
        if (Input.GetKey(KeyCode.S))
        {
            _direction += new Vector2(Mathf.Cos(180 * Mathf.Deg2Rad), Mathf.Sin(180 * Mathf.Deg2Rad));
        }
        // ���������� ���� ������ ȸ�� �� 90�� �� �Ϲ����� ��Ŭ���� ��ǥ��� �ٸ��� �ݽð� �������� ���⶧���� �ݴ�� ���� 
        if (Input.GetKey(KeyCode.D))
        {
            _direction += new Vector2(Mathf.Cos(90 * Mathf.Deg2Rad), Mathf.Sin(90 * Mathf.Deg2Rad));
        }

        // �Է°��� ���� �� ���⺤�� ����ȭ �̷��� ������ �ڿ������� ���´�
        _direction.Normalize();
        // �Է°� ����� ad, ws���� ���� �ݴ������ ���̸� 0�̴ϱ� �̵��� �ʿ����
        if (_direction == Vector2.zero)
            return;

        // 0�� ���Ϳ�(��ǻ� Vector2.right) _direction���� ���� �޾ƿ� if�� �ߺ�����ϸ� �׳� 45�� 90�� 180�� 135��.. �ٷ� �޾ƿ��� ��
        float angle = Vector2.SignedAngle(new Vector2(Mathf.Cos(0 * Mathf.Deg2Rad), Mathf.Sin(0 * Mathf.Deg2Rad)), _direction);
        // ī�޶� �÷��̾ �ٶ󺸴� ���� ��
        Vector3 moveDirection = _camera.gameObject.GetComponent<CameraTemp>().cameraDirection;
        // y ���� ���ʹ� ��������
        moveDirection.y = 0.0f;
        // ���� �븻������ ���ְ� ī�޶� 0, 0, 1 ���Ϳ� ���� � ���ư� �ִ��� üũ �ٵ� �̷��ʿ���� �׳� ī�޶� �����̼� y�޾ƿ͵� �ɵ�
        moveDirection.Normalize();
        float cameraAngle = Vector3.SignedAngle(Vector3.forward, moveDirection, Vector3.up);
        // �÷��̾��� �����̼��� ī�޶� ���ư��ִ� ������ �Է°� ������ŭ ���ؼ� ������
        transform.rotation = Quaternion.Euler(Vector3.up * (cameraAngle + angle));
        // ���� �������� �ɾ
        transform.Translate(Vector3.forward * Time.deltaTime * 10.0f);
    }
}
