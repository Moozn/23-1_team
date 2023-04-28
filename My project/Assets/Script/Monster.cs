using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
     private float Mob_HpMax;
    private float Mob_Hp;
    private float Mob_Atk;
    private float Mob_Exp;
    [SerializeField] private Player player;

    private void Init()
    {
        Mob_HpMax = 10000f;
        Mob_Hp = Mob_HpMax;
        Mob_Exp = 0;
    }
    private void Awake()
    {
        Init();
    }
    private void OnEnable()
    {
        Init();
    }
    private void OnDisable()
    {
        SetExp();
    }
    public void Hit(float Damage)
    {
            Mob_Hp -= Damage;
            Debug.Log("현재 체력 : " + Mob_HpMax);
    }
    private void Die()
    {
        //if (capsuleCollider) capsuleCollider.enabled = false;
        MonsterSpawner.Instance.InsertList(this);
    }
    private void SetExp()
    {
        Mob_Exp = (Mob_HpMax - Mob_Hp) * 2;
        player.Add_Exp(Mob_Exp);
    }

    public void Activation()
    {
        gameObject.SetActive(true);
    }
    public void Deactivation()
    {
        gameObject.SetActive(false); //비활성화
    }
}
