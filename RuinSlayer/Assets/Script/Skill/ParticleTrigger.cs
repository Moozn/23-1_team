using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class ParticleTrigger : MonoBehaviour
{
    private float damage;
    public void SetDamage(float _damage)
    {
        damage = _damage;
    }
    private void OnParticleTrigger()
    {
        Debug.Log("¸ÂÀ½");
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Monster")
        {
            Debug.Log($"Effect Collision : {other.name}");
            other.GetComponent<Monster>().Hit(damage);      
        }
    }
}
