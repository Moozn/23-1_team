using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    private float damage = 20f;
    public void Set_Damage(float _damage)
    {
        damage = _damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Monster") && !this.tag.Equals(other.tag)) other.GetComponentInParent<Monster>().Hit(damage);
        else if (other.tag.Equals("Player") && this.GetComponentInParent<Monster>().isAttack())
        {
            other.GetComponent<Player>().Hit(damage);
        }
    } 
}
