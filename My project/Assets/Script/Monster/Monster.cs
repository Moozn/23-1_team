using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum MonstrState
{
    Idle, // 기본
    Run,
    Attack, // 공격
    Breth,
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
    private NavMeshAgent agent;
    private float dis_Pattern1, dis_Pattern2, dis_Pattern3; //패턴거리
    private float player_dis;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject bulletpos;
    [SerializeField] private GameObject bulletpos2;
    private bool delay;
    private int min,max;
    private void Init()
    {
        Mob_HpMax = 10000f;
        Mob_Hp = Mob_HpMax;
        Mob_Exp = 0;
        state = MonstrState.Idle;
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        delay = true;
        min = 0;
        max = 0;
        dis_Pattern1 = 30f;
        dis_Pattern2 = 10f;
        dis_Pattern3 = 5f;
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
          GameObject _bullet = Instantiate(bullet, transform.position, Quaternion.identity);
          _bullet.transform.forward = Quaternion.Euler(0f, currentAngle, 0f) * transform.forward;
        }
    }
    public void Hit(float Damage)
    {
        Mob_Hp -= Damage;
        Debug.Log("현재 체력 : " + Mob_HpMax);
    }
    private void Move()
    {
        agent.isStopped = false;
        anim.SetFloat("Run", 1);
        agent.SetDestination(player.transform.position);
    } //쫓기

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
    private void Exploration() //탐색
    {
        float dis = distance();

     if(dis <= dis_Pattern1 && delay)
        {
            min = 0;
            max = 1;
            if (dis <= dis_Pattern2) //패턴은 think로 계속 돌려주고 행동은 값을 통해 제어해줌
            {
                if (dis <= dis_Pattern3)
                {
                    min = 3;
                    max = 5;
                }
                else
                {
                    min = 1;
                    max = 3;
                }
            }
            else  state = MonstrState.Run;
        }
    }
    IEnumerator Think()
    {
            yield return new WaitForSeconds(0.4f);

            int ranpattern = Random.Range(min, max);
            Debug.Log(ranpattern);
            switch (ranpattern)
            {
                case 0:
                     StartCoroutine(Delay());
                    break;
                case 1:
                    StartCoroutine(Rush());
                    break;
                case 2:
                    StartCoroutine(Breth());
                    break;
                case 3:
                    StartCoroutine(Attack());
                    break;
                case 4:
                    StartCoroutine(JumpAttack());
                    break;
            }
    }
    private float distance()
    {
        return Vector3.Distance(transform.position, player.transform.position);
    }
    private void Update()
    {
        Exploration();
    }
    private void FixedUpdate()
    {
        Lookat();
      //  Fire();
        if (state.Equals(MonstrState.Dash)) rigid.AddForce(transform.forward * 5, ForceMode.Impulse);
        if (state.Equals(MonstrState.Run)) Move();
        else StartCoroutine(MoveStop());
    }
    public void Attack2()
    {
        anim.SetTrigger("Attack2");
    }
    IEnumerator Attack()
    {
        state = MonstrState.Attack;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Think());
    }
    IEnumerator Rush()
    {
       anim.SetBool("Dash", true);
        state = MonstrState.Dash;
        yield return new WaitForSeconds(0.5f);
      //  state = MonstrState.Idle;
        anim.SetBool("Dash", false);
        StartCoroutine(Think());
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Think());
    }
    IEnumerator MoveStop()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetFloat("Run", 0);
        agent.isStopped = true;
    }
    IEnumerator JumpAttack()
    {
        anim.SetTrigger("JumpAttack");
        state = MonstrState.Attack;
        yield return new WaitForSeconds(0.5f);
        state = MonstrState.Idle;
        StartCoroutine(Think());
    }
    IEnumerator Breth()
    {
        anim.SetTrigger("Breth");
        state = MonstrState.Breth;
        yield return new WaitForSeconds(1f);
        state = MonstrState.Idle;
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
