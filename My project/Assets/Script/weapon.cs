using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    private float damage;
    public void Set_Damage(float _damage)
    {
        damage = _damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Monster") && !this.tag.Equals(other.tag)) other.GetComponent<Monster>().Hit(damage);
        else if (other.tag.Equals("Player")) other.GetComponent<Player>().Hit(damage);
    } 
}
