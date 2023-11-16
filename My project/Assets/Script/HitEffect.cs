using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    public GameObject hitEffectPrefab;
    private Vector3 hitPoint;
    // ��Ʈ �߻��� ȣ���� �޼���
    public void ShowHitEffect()
    {
        // ��Ʈ ����Ʈ�� �ν��Ͻ�ȭ
        Debug.Log(hitPoint);
        GameObject hitEffect = Instantiate(hitEffectPrefab, hitPoint, Quaternion.identity);

        // ��Ʈ ����Ʈ ����� ������ ����
   //     Destroy(hitEffect, hitEffect.GetComponent<ParticleSystem>().main.duration);
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitPoint = hit.point;
    }
}
