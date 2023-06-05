using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonstrState
{
    Idle, // 기본
    Attack, // 공격
    Hit, // 맞음
    Dash, // 대쉬
    Death // 죽음
}
public class Monster : MonoBehaviour
{ //소리 이펙트 넣기
    private float Mob_HpMax;
    private float Mob_Hp;
    private float Mob_Atk;
    private float Mob_Exp;
    private Animator anim;
    [SerializeField] private MonstrState state;
    [SerializeField] private Player player;
    private Rigidbody rigid;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject bulletpos;
    [SerializeField] private GameObject bulletpos2;
    private void Init()
    {
        Mob_HpMax = 10000f;
        Mob_Hp = Mob_HpMax;
        Mob_Exp = 0;
        state = MonstrState.Idle;
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }
    private void Awake()
    {
        Init();
        StartCoroutine(Think());
    }
    private void OnEnable()
    {
        Init();
    }
    private void OnDisable()
    {
        SetExp();
    }
    public void Fire() //부채꼴 발사
    {
        float angle = 45;
        int bulletnum = 10;
        float angleIncrement = angle / (bulletnum - 1);  

        for (int i = 0; i < bulletnum; i++)
        {
          float currentAngle = -angle / 2f + i * angleIncrement;
          GameObject _bullet = Instantiate(bullet, transform.forward, Quaternion.identity);
          _bullet.transform.forward = Quaternion.Euler(0f, currentAngle, 0f) * transform.forward;
        }
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
    private void Lookat()
    {
        Vector3 rotate = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z) - (transform.position);
        Quaternion newRotate = Quaternion.LookRotation(rotate);
        rigid.rotation = Quaternion.Slerp(rigid.rotation, newRotate, 10 * Time.fixedDeltaTime);
    }
    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);

        int ranpattern = Random.Range(0, 5);
        state = MonstrState.Idle;
        switch (ranpattern)
        {
            case 0:
            case 1:
               StartCoroutine(Rush());
                break;
            case 2:
            case 3:
               StartCoroutine(JumpAttack());
                break;
            case 4:
                StartCoroutine(Breth());
                break;
        }
    }
    private void FixedUpdate()
    {
        Lookat();
      //  Fire();
        if (state.Equals(MonstrState.Dash)) rigid.AddForce(transform.forward * 5, ForceMode.Impulse);
    }
    IEnumerator Rush()
    {
        anim.SetBool("Dash", true);
        state = MonstrState.Dash;
        
        yield return new WaitForSeconds(1.5f);

        anim.SetBool("Dash", false);
        state = MonstrState.Idle;

        StartCoroutine(Think());
    }

    IEnumerator JumpAttack()
    {
        anim.SetTrigger("JumpAttack");
        yield return new WaitForSeconds(1f);

        StartCoroutine(Think());
    }
    IEnumerator Breth()
    {
        anim.SetTrigger("Breth");
        yield return new WaitForSeconds(1f);

        StartCoroutine(Think());
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
