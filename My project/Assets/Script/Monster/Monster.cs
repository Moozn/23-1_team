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
    private float stemina; //스테미너
    private Animator anim;
    [SerializeField] private MonstrState state;
    [SerializeField] private Player player;
    [SerializeField] private Rigidbody rigid;
  // [SerializeField] private weapon r_hand; // 기본공격
  // [SerializeField] private weapon l_hand; // 기본공격
  // [SerializeField] private SetUI r_handCoillder; // 기본공격
  // [SerializeField] private SetUI l_handCoillder; // 기본공격
    [SerializeField] private SliderScript hpsilder;
    private float cooltime;
    private bool fly;
    private bool falling;
    private bool attack2;
    [SerializeField] private int pattern_var; //패턴 변수
    private NavMeshAgent agent;
    private float dis_Pattern1, dis_Pattern2, dis_Pattern3; //패턴거리
    private float player_dis;
    [SerializeField] private GameObject bullet;
    private bool delay;
    private int min, max;
    private bool dash_; //흠.. 변수이르  ㅁ 뭘로하지 일단 이건 콜라이더가 머리에도 몸에도 있어서 2번씩 닿기떄문에 그냥 한번 닿으면 false해주고 다시 true로 한번만 닿게 할꺼임
    private float stiffen; //경직
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private ParticleSystem breath;
    [SerializeField] private ParticleSystem breath_2;
    [SerializeField] private SetUI magicalCamp; //마법진 마법 묶음
    [SerializeField] private SetUI magicalCircle; //마법진
    [SerializeField] private SetUI set_hitEffect; //마법진
    [SerializeField] private ParticleSystem magical_Camp; //마법진
    [SerializeField] private ParticleSystem magical; 
    private float maicalTime;
    [SerializeField] private AudioSource breath_Audio1; //브레스1
    [SerializeField] private AudioSource breath_Audio2; //브레스2
    [SerializeField] private AudioSource swingAudio1; //휘두르기1
    [SerializeField] private AudioSource swingAudio2; //휘두르기2
    [SerializeField] private AudioSource hitAudio; //피격
    [SerializeField] private AudioSource dieAudio; //죽음
    [SerializeField] private AudioSource magicalcreatAudio; // 마법진 깔릴때
    [SerializeField] private AudioSource roarAudio; // 포효
    [SerializeField] private AudioSource magicalAudio; // 마법소리 

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
        dis_Pattern1 = 30f;
        dis_Pattern2 = 10f;
        dis_Pattern3 = 5f;
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
    private void OnTriggerEnter(Collider other) //돌진
    {
    }
    public void Hit(float Damage)
    {
        if (!state.Equals(MonstrState.Hit))
        {
            state = MonstrState.Hit;
            set_hitEffect.On();
            hitEffect.Play();
            StartCoroutine(Timer());
            Mob_Hp -= Damage;
            stiffen -= 5f;
        }
        
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
        gameObject.SetActive(false); //비활성화
    }
    public bool isAttack()
    {
        return state.Equals(MonstrState.Attack);
    }
   private void Update()
   {
       hpsilder.Slider_Update(Mob_Hp / Mob_HpMax);
        if (stemina >= 30) ; //Exploration();
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
            switch(pattern_var%4)
            {
                case 0: //팔 휘두르기 2
                    attack2 = true;
                  //  StartCoroutine(Attack());
                    StartCoroutine(MagicalCamp());
                    break;
                case 1: //팔 휘두르기 1
                    attack2 = false;
                    //   StartCoroutine(Attack());
                    StartCoroutine(MagicalCamp());
                    break;
                case 2: //브레스 길게
                        //  StartCoroutine(Breth_Long());
                    StartCoroutine(MagicalCamp());
                    break;
                case 3:
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
        state = MonstrState.Attack;
        anim.SetTrigger("Attack");
        AudioMgr.Instance.PlayAudio(swingAudio1); //스윙1
        yield return new WaitForSeconds(2f);
        if (attack2)
        {
            Attack2();
            yield return new WaitForSeconds(2f);
        }
        cooltime = 3f;
        yield return new WaitForSeconds(1.5f);
        state = MonstrState.Idle;
        StartCoroutine(Pattern());
    }
    public void Attack2()
    {
        state = MonstrState.Attack;
        anim.SetTrigger("Attack2");
        AudioMgr.Instance.PlayAudio(swingAudio2);//스윙2

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
    //  AudioMgr.Instance.PlayAudio(magicalcreatAudio);
        yield return new WaitForSeconds(1f);
    //  AudioMgr.Instance.PlayAudio(roarAudio); //포효를 어디에 넣지 하다가 일단 생성 떨어지기 중간에 넣었음
        yield return new WaitForSeconds(1.5f);
        b_magical = false;
        magical_Camp.Stop();
        magicalCircle.Off();
        StartCoroutine(Magical());
    }
    private IEnumerator Magical()
    {
        magical.Play();
      //  AudioMgr.Instance.PlayAudio(magicalAudio);
        yield return new WaitForSeconds(0.5f);
        magical.Stop();
        magicalCamp.Off();
        StartCoroutine(Pattern());
    }
    IEnumerator Breth_Long()
    {
        AudioMgr.Instance.PlayAudio(breath_Audio1);
        anim.SetTrigger("Breth_Long");
        breath.Play();
        yield return new WaitForSeconds(3.6f);
        //AudioMgr.Instance.PlayAudioStop (breath_Audio1);
        breath.Stop();
        cooltime = 3f;
        StartCoroutine(Pattern());
    }
    IEnumerator Breth_Short()
    {
        anim.SetTrigger("Breth_Short");
        AudioMgr.Instance.PlayAudio(breath_Audio2);
        breath_2.Play(); //브레스 짧은거
        yield return new WaitForSeconds(2f);
        breath_2.Stop();
        // AudioMgr.Instance.PlayAudioStop(breath_Audio1);
        cooltime = 2f;
        StartCoroutine(Pattern());
    }
}
