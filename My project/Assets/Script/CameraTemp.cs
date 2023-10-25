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
        // LateUpdate이기 때문에 플레이어 위치는 이동했지만 아직 카메라는 이동하지 않음
        // 이동한 플레이어 위치를 바라봄
        transform.LookAt(_player.transform);
        // 다음 프레임에 플레이어가 받아갈 디렉션 저장
        cameraDirection = _player.transform.position - transform.position;
        cameraDirection.Normalize();
        // 플레이어를 바라보고있는 그 방향대로 뒤로 물러남
        transform.position = _player.transform.position + fdelta * cameraDirection;
        // y값의 위치는 플레이어의 일정 시점 위로 고정 시킴
        Vector3 v = transform.position;
        v.y = _player.transform.position.y + 5.0f;
        transform.position = v;
    }
}