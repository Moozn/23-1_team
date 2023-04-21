using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    float hp = 10000f;
    public void Hit(float Damage)
    {
            hp -= Damage;
            Debug.Log("현재 체력 : " + hp);
    }
}
