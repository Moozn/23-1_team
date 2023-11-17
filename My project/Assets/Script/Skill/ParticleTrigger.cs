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

    private void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("¸ÂÀ½");
            other.GetComponent<Player>().Hit(damage);      
        }
    }
}
