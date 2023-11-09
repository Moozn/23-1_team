using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    public GameObject hitEffectPrefab;

    // 히트 발생시 호출할 메서드
    public void ShowHitEffect(Vector3 hitPoint)
    {
        // 히트 이펙트를 인스턴스화
        GameObject hitEffect = Instantiate(hitEffectPrefab, hitPoint, Quaternion.identity);

        // 히트 이펙트 재생이 끝나면 삭제
        Destroy(hitEffect, hitEffect.GetComponent<ParticleSystem>().main.duration);
    }
}
