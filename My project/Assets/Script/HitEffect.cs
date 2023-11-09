using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    public GameObject hitEffectPrefab;

    // ��Ʈ �߻��� ȣ���� �޼���
    public void ShowHitEffect(Vector3 hitPoint)
    {
        // ��Ʈ ����Ʈ�� �ν��Ͻ�ȭ
        GameObject hitEffect = Instantiate(hitEffectPrefab, hitPoint, Quaternion.identity);

        // ��Ʈ ����Ʈ ����� ������ ����
        Destroy(hitEffect, hitEffect.GetComponent<ParticleSystem>().main.duration);
    }
}
