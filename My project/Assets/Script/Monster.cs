using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    float hp = 100f;
    
    public void Hit(float Damage)
    {
        hp -= Damage;
        Debug.Log("���� ü�� : " + hp);
    }
}
