using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum MonstrState
{
    Idle, // �⺻
    Run,
    Attack, // ����
    Breth,
    Hit, // ����
    Dash, // �뽬
    Death // ����
}
public class Monster : MonoBehaviour
{ //�Ҹ� ����Ʈ �ֱ�  
    private float Mob_HpMax;
    private float Mob_Hp;
    private float Mob_Atk;
    private float Mob_Exp;
    private float stemina; //���׹̳�
    private Animator anim;
    [SerializeField] private MonstrState state;
    [SerializeField] private Player player;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private weapon r_hand; // �⺻����
    [SerializeField] private weapon l_hand; // �⺻����
    [SerializeField] private SetUI r_handCoillder; // �⺻����
    [SerializeField] private SetUI l_handCoillder; // �⺻����
    [SerializeField] private SliderScript hpsilder;
    private NavMeshAgent agent;
    private float dis_Pattern1, dis_Pattern2, dis_Pattern3; //���ϰŸ�
    private float player_dis;
    [SerializeField] private GameObject bullet;
    private bool delay;
    private int min, max;
    private bool dash_; //��.. �����̸�  �� �������� �ϴ� �̰� �ݶ��̴��� �Ӹ����� ������ �־ 2���� ��⋚���� �׳� �ѹ� ������ false���ְ� �ٽ� true�� �ѹ��� ��� �Ҳ���
    private float stiffen; //����
    private void Init()
    {
        Mob_HpMax = 10000f;
        Mob_Hp = Mob_HpMax;
        Mob_Exp = 0;
        state = MonstrState.Idle;
        anim = GetComponent<Animator>();
        //  rigid = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        delay = true;
        min = 0;
        max = 0;
        dis_Pattern1 = 30f;
        dis_Pattern2 = 10f;
        dis_Pattern3 = 5f;
        stemina = 100f;
        r_hand.Set_Damage(10);
        l_hand.Set_Damage(10);
        dash_ = true;
        stiffen = 100f;
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
    public void Fire() //��ä�� �߻�
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
    private void OnTriggerEnter(Collider other)
    {
        if(state.Equals(MonstrState.Dash) && dash_)
        {
            dash_ = false;
            player.Hit(30f);
        }
    }
    public void Hit(float Damage)
    {
        Mob_Hp -= Damage;
        stiffen -= 5f;
        Debug.Log("���� ü�� : " + Mob_Hp);
    }
    private void Move()
    {
        agent.isStopped = false;
        anim.SetFloat("Run", 1);
        agent.SetDestination(player.transform.position);
    } //�ѱ�

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
        if (!state.Equals(MonstrState.Dash))
        {
            Vector3 rotate = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z) - transform.position;
            Quaternion newRotate = Quaternion.LookRotation(rotate);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotate, 2 * Time.deltaTime);
        }
    }
    private void Exploration() //Ž��
    {
        float dis = distance();

        if (dis <= dis_Pattern1 && delay)
        {
            min = 0;
            max = 1;
            if (dis <= dis_Pattern2) //������ think�� ��� �����ְ� �ൿ�� ���� ���� ��������
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
            else state = MonstrState.Run;
        }
    }
    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);

        int ranpattern = Random.Range(min, max);
        Debug.Log(ranpattern);
        yield return new WaitForSeconds(0.5f);
        if(stiffen <= 0)
        {
            StartCoroutine(Stern());
            stiffen = 100f;
        }
        else if (stemina >= 30)
        {
            stemina -= 20;

            StartCoroutine(Rush());
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
        else StartCoroutine(Stemina_Recovery());
    }
    private float distance()
    {
        return Vector3.Distance(transform.position, player.transform.position);
    }
    private void Update()
    {
        hpsilder.Slider_Update(Mob_Hp / Mob_HpMax);
        if (stemina >= 30) Exploration();
        Lookat();
    }
    private void FixedUpdate()
    {
        if (state.Equals(MonstrState.Dash))
        {
            rigid.AddForce(transform.forward * 500);
        }
        if (state.Equals(MonstrState.Run)) Move();
        else StartCoroutine(MoveStop());
    }
    public void Attack2()
    {
        anim.SetTrigger("Attack2");
    }
    IEnumerator Stern()
    {
        state = MonstrState.Hit;
      //  anim.SetTrigger("Attack");
        yield return new WaitForSeconds(3f);
        state = MonstrState.Idle;
        StartCoroutine(Think());
    }
    IEnumerator Attack()
    {
        state = MonstrState.Attack;
        anim.SetTrigger("Attack");
        r_handCoillder.On();
        l_handCoillder.On();
        yield return new WaitForSeconds(0.5f);
        r_handCoillder.Off();
        l_handCoillder.Off();
        StartCoroutine(Think());
    }
    IEnumerator Rush()
    {

        anim.SetBool("Dash", true);
        agent.isStopped = false;
        state = MonstrState.Dash;
        dash_ = true;
        yield return new WaitForSeconds(1f);
        state = MonstrState.Idle;
        anim.SetBool("Dash", false);
        agent.isStopped = true;
        StartCoroutine(Think());
    }
    IEnumerator Stemina_Recovery()
    {
        yield return new WaitForSeconds(2f);
        stemina += 100;
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
        r_handCoillder.On();
        yield return new WaitForSeconds(0.5f);
        state = MonstrState.Idle;
        r_handCoillder.Off();
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
        gameObject.SetActive(false); //��Ȱ��ȭ
    }
}
