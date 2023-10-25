using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTemp : MonoBehaviour
{
    Camera _camera;
    // 입력 값, 키보드 입력을 받아 캐릭터를 회전 시킬 각도를 결정한다
    // 그냥 if 문으로 각도만 받아도 되는데 if 중복 없이 벡터로 표현하는 걸 개인적으로 선호해서
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
        // 앞으로 가기 때문에 회전 각 0도
        if (Input.GetKey(KeyCode.W))
        {
            // 해당 각에 해당하는 벡터를 더해준다 대입이 아니라
            // 쓸데없이 코사인 사인 쓴 이유는 각도로 표현하기 위해서
            _direction += new Vector2(Mathf.Cos(0 * Mathf.Deg2Rad), Mathf.Sin(0 * Mathf.Deg2Rad));
        }
        // 왼쪽으로 가기 때문에 회전 각 -90도
        if (Input.GetKey(KeyCode.A))
        {
            _direction += new Vector2(Mathf.Cos(-90 * Mathf.Deg2Rad), Mathf.Sin(-90 * Mathf.Deg2Rad));
        }
        // 뒤로 가기 때문에 회전 각 180도
        if (Input.GetKey(KeyCode.S))
        {
            _direction += new Vector2(Mathf.Cos(180 * Mathf.Deg2Rad), Mathf.Sin(180 * Mathf.Deg2Rad));
        }
        // 오른쪽으로 가기 때문에 회전 각 90도 그 일반적인 유클리드 좌표계와 다르게 반시계 방향으로 돌기때문에 반대로 해줌 
        if (Input.GetKey(KeyCode.D))
        {
            _direction += new Vector2(Mathf.Cos(90 * Mathf.Deg2Rad), Mathf.Sin(90 * Mathf.Deg2Rad));
        }

        // 입력값을 받은 후 방향벡터 정규화 이러면 각도가 자연스럽게 나온다
        _direction.Normalize();
        // 입력값 결과가 ad, ws같은 서로 반대방향의 합이면 0이니까 이동할 필요없음
        if (_direction == Vector2.zero)
            return;

        // 0도 벡터와(사실상 Vector2.right) _direction간의 각도 받아옴 if문 중복사용하면 그냥 45도 90도 180도 135도.. 바로 받아오면 됨
        float angle = Vector2.SignedAngle(new Vector2(Mathf.Cos(0 * Mathf.Deg2Rad), Mathf.Sin(0 * Mathf.Deg2Rad)), _direction);
        // 카메라가 플레이어를 바라보는 벡터 얻어냄
        Vector3 moveDirection = _camera.gameObject.GetComponent<CameraTemp>().cameraDirection;
        // y 방향 벡터는 제거해줌
        moveDirection.y = 0.0f;
        // 벡터 노말라이즈 해주고 카메라가 0, 0, 1 벡터에 대해 몇도 돌아가 있는지 체크 근데 이럴필요없이 그냥 카메라 로테이션 y받아와도 될듯
        moveDirection.Normalize();
        float cameraAngle = Vector3.SignedAngle(Vector3.forward, moveDirection, Vector3.up);
        // 플레이어의 로테이션을 카메라가 돌아가있는 각도에 입력값 각도만큼 더해서 돌려줌
        transform.rotation = Quaternion.Euler(Vector3.up * (cameraAngle + angle));
        // 보는 방향으로 걸어감
        transform.Translate(Vector3.forward * Time.deltaTime * 10.0f);
    }
}
