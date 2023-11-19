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
  // [SerializeField] private weapon r_hand; // �⺻����
  // [SerializeField] private weapon l_hand; // �⺻����
  // [SerializeField] private SetUI r_handCoillder; // �⺻����
  // [SerializeField] private SetUI l_handCoillder; // �⺻����
    [SerializeField] private SliderScript hpsilder;
    private float cooltime;
    private bool fly;
    private bool falling;
    private bool attack2;
    [SerializeField] private int pattern_var; //���� ����
    private NavMeshAgent agent;
    private float player_dis;
    [SerializeField] private GameObject bullet;
    private bool delay;
    private int min, max;
    private bool dash_; //��.. �����̸�  �� �������� �ϴ� �̰� �ݶ��̴��� �Ӹ����� ������ �־ 2���� ��⋚���� �׳� �ѹ� ������ false���ְ� �ٽ� true�� �ѹ��� ��� �Ҳ���
    private float stiffen; //����
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private ParticleSystem breath;
    [SerializeField] private ParticleSystem breath_2;
    [SerializeField] private SetUI magicalCamp; //������ ���� ����
    [SerializeField] private SetUI magicalCircle; //������
    [SerializeField] private SetUI set_hitEffect; //������
    [SerializeField] private ParticleSystem magical_Camp; //������
    [SerializeField] private ParticleSystem magical; 
    private float maicalTime;
    [SerializeField] private AudioSource breath_Audio1; //�극��1
    [SerializeField] private AudioSource breath_Audio2; //�극��2
    [SerializeField] private AudioSource swingAudio1; //�ֵθ���1
    [SerializeField] private AudioSource swingAudio2; //�ֵθ���2
    [SerializeField] private AudioSource hitAudio; //�ǰ�
    [SerializeField] private AudioSource dieAudio; //����
    [SerializeField] private AudioSource magicalcreatAudio; // ������ �򸱶�
    [SerializeField] private AudioSource roarAudio; // ��ȿ
    [SerializeField] private AudioSource magicalAudio; // �����Ҹ� 
    [SerializeField] private SetUI longbreath;
    [SerializeField] private SetUI Shortbreath;
    [SerializeField] private ParticleTrigger ptbreath_long;
    [SerializeField] private ParticleTrigger ptbreath_Short;
    [SerializeField] private ParticleTrigger ptmagical;
    private float swing1; //25
    private float swing2; //25
    private float breath_long; //10
    private float breath_short;//25
    private float f_magical;//5
    private bool b_magical;
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
        attack2 = false;
        min = 0;
        max = 0;
        ptbreath_long.SetDamage(10); //�� �극��
        ptbreath_Short.SetDamage(10); //���� �극��
        ptmagical.SetDamage(1000); //��ų ������
        stemina = 100f;
        cooltime = 3f;
        pattern_var = 1;
        // r_hand.Set_Damage(10);
        // l_hand.Set_Damage(10);
        magicalCamp.Off();
        dash_ = true;
        stiffen = 100f;
        fly = false;
        falling = false;
        swing1 = 20f;
        swing2 = 40f;
        breath_long = 60f;
        breath_short = 80f;
        f_magical = 100f;
        maicalTime = 1.5f;
        b_magical = false;
    }
    private void Awake()
    {
        Init();
        //   StartCoroutine(Think());
        StartCoroutine(Pattern());
    }
    private void OnEnable()
    {
        Init();
        //  StartCoroutine(Think());
        StartCoroutine(Pattern());
    }
    private void OnDisable()
    {
        SetExp();
    }
    private void OnTriggerEnter(Collider other) //����
    {
    }
    public void Hit(float Damage)
    {
        if (!state.Equals(MonstrState.Hit))
        {
            Mob_Hp -= Damage;
            AudioMgr.Instance.PlayAudio(hitAudio);
            if (Mob_Hp > 0)
            {
                state = MonstrState.Hit;
                set_hitEffect.On();
                hitEffect.Play();
                StartCoroutine(Timer());
                Mob_Hp -= Damage;
                stiffen -= 5f;
            }
            else StartCoroutine(Die());
        }
        
    }
    private void Move()
    {
        agent.isStopped = false;
        anim.SetFloat("Run", 1);
        agent.SetDestination(player.transform.position);
    } //�ѱ�

    private IEnumerator Die()
    {
        Mob_Hp = 0f;
        state = MonstrState.Death;
        anim.SetBool("Die", true);
        yield return new WaitForSeconds(2f);
        Cursor.visible = true;
        player.End();
        Deactivation();
        //MonsterSpawner.Instance.InsertList(this);
    }
    public void OnAttack()
    {
        state = MonstrState.Attack;
    }
    public void OffAttack()
    {
        state = MonstrState.Idle;
    }

   // private void Die()
   // {
   //     //if (capsuleCollider) capsuleCollider.enabled = false;
   //     MonsterSpawner.Instance.InsertList(this);
   // }
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
       //     if (rotate.x == 0 && rotate.y == 0 && rotate.z == 0) rotate = transform.forward;
            Quaternion newRotate = Quaternion.LookRotation(rotate.normalized);
        //    Debug.Log(rotate.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotate, 2 * Time.deltaTime);
        }
    }
    public void Activation()
    {
        gameObject.SetActive(true);
    }
    public void Deactivation()
    {
        gameObject.SetActive(false); //��Ȱ��ȭ
    }
    public bool isAttack()
    {
        return state.Equals(MonstrState.Attack);
    }
   private void Update()
   {
       hpsilder.Slider_Update(Mob_Hp / Mob_HpMax);
       // if (stemina >= 30) ; //Exploration();
        if (fly)
        {
            // transform.Translate(Vector3.up * 10000f * Time.deltaTime);
            // Vector3 flyDirection = transform.forward * 10;
            // rigid.velocity = new Vector3(flyDirection.x, rigid.velocity.y, flyDirection.z);
            rigid.AddForce(new Vector3(0f,10f,0f));

            // rigid.velocity = Vector3.zero;
            //rigid.velocity = new Vector3(0F, 10F, 0f);
        }
        if (falling) rigid.AddForce(Vector3.up * -10, ForceMode.VelocityChange);

    }
   private void FixedUpdate()
   {
       if (state.Equals(MonstrState.Dash))
       {
           rigid.AddForce(transform.forward * 500);
       }
       if (state.Equals(MonstrState.Run)) Move();
       Lookat();
        //  else StartCoroutine(MoveStop());
      if(b_magical) magicalCamp.SetPosition(new Vector3(player.transform.position.x, 0.2f, player.transform.position.z));//player.transform.position);


    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(0.1f);
        state = MonstrState.Idle;
        hitEffect.Stop();
        set_hitEffect.Off();
    }
    private IEnumerator Pattern()
    {
        
        yield return new WaitForSeconds(cooltime);
      
        if (distance() >= 10) StartCoroutine(Movement());
        else
        {
            pattern_var++;
            Debug.Log(pattern_var % 4);
            float random = Random.Range(1f, 101f);

            if (swing1 >= random) pattern_var = 0;
            else if (swing2 >= random) pattern_var = 1;
            else if (breath_short >= random) pattern_var = 2;
            else if (breath_long >= random) pattern_var = 3;
            else if (f_magical >= random) pattern_var = 4;
    
            switch (4)//pattern_var)
            {
                case 0: //�� �ֵθ��� 2
                  //  StartCoroutine(Attack());
                    StartCoroutine(Attack());
                    break;
                case 1: //�� �ֵθ��� 1
                    //   StartCoroutine(Attack());
                    attack2 = true;
                    StartCoroutine(Attack());
                    break;
                case 2: //�극�� ���
                        //  StartCoroutine(Breth_Long());
                    StartCoroutine(Breth_Long());
                    break;
                case 3:
                    //  StartCoroutine(Breth_Short());
                    StartCoroutine(Breth_Short());
                    break;
                case 4:
                    //  StartCoroutine(Breth_Short());
                    StartCoroutine(MagicalCamp());
                    break;
            }
        }
        

        // StartCoroutine(JumpAttack());
    }

    private IEnumerator Movement()
    {
        Move();
        yield return new WaitForSeconds(1f);
        cooltime = 1f;
        StartCoroutine(Pattern());
    }

    private IEnumerator JumpAttack()
    {
        fly = true;
        anim.SetBool("Fly", fly);
        yield return new WaitForSeconds(3f);
        fly = false;
        anim.SetBool("Fly", fly);
        anim.SetTrigger("FlyToFalling");
        yield return new WaitForSeconds(1.4f);
        anim.SetBool("Falling", falling);
        falling = true;
        yield return new WaitForSeconds(3f);
        falling = false;
        anim.SetBool("Falling", falling);
        cooltime = 2f;
        StartCoroutine(Pattern());
    }

    IEnumerator Attack()
    {
        anim.SetTrigger("Attack");
        AudioMgr.Instance.PlayAudio(swingAudio1); //����1
        yield return new WaitForSeconds(1.2f);
        if (attack2)
        {
            Attack2();
            yield return new WaitForSeconds(1.2f);
        }
        cooltime = 3f;
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(Pattern());
    }
    public void Attack2()
    {
        anim.SetTrigger("Attack2");
        AudioMgr.Instance.PlayAudio(swingAudio2);//����2
        attack2 = false;

    }
    private float distance()
    {
        return Vector3.Distance(transform.position, player.transform.position);
    }
    private IEnumerator MagicalCamp()
    {
        b_magical = true;
        magicalCamp.On();
        magicalCircle.On();
        magical_Camp.Play();
        AudioMgr.Instance.PlayAudio(magicalcreatAudio);
        yield return new WaitForSeconds(1f);
        AudioMgr.Instance.PlayAudio(roarAudio); //��ȿ�� ��� ���� �ϴٰ� �ϴ� ���� �������� �߰��� �־���
        yield return new WaitForSeconds(1.5f);
        b_magical = false;
        magical_Camp.Stop();
        yield return new WaitForSeconds(0.5f);
        magicalCircle.Off();
        StartCoroutine(Magical());
    }
    private IEnumerator Magical()
    {
        magical.Play();
        AudioMgr.Instance.PlayAudio(magicalAudio);
        yield return new WaitForSeconds(1f);
        magical.Stop();
        magicalCamp.Off();
        StartCoroutine(Pattern());
    }
    public void OnBreth()
    {
        longbreath.On();
        breath.Play();
    }
    public void OffBreth()
    {
        breath.Stop();
        longbreath.Off();
    }

    public void OnBreth_Short()
    {
        Shortbreath.On();
        breath_2.Play();
    }
    public void OffBreth_Short()
    {
        breath_2.Stop();
        Shortbreath.Off();
    }
    IEnumerator Breth_Long()
    {
        AudioMgr.Instance.PlayAudio(breath_Audio1);
        anim.SetTrigger("Breth_Long");
        yield return new WaitForSeconds(3.6f);
        AudioMgr.Instance.PlayAudioStop (breath_Audio1);  
        cooltime = 3f;
        StartCoroutine(Pattern());
    }
    IEnumerator Breth_Short()
    {
        anim.SetTrigger("Breth_Short");
        AudioMgr.Instance.PlayAudio(breath_Audio2);
        breath_2.Play(); //�극�� ª����
        yield return new WaitForSeconds(1f);
        breath_2.Stop();
         AudioMgr.Instance.PlayAudioStop(breath_Audio1);
        cooltime = 2f;
        StartCoroutine(Pattern());
    }
}
