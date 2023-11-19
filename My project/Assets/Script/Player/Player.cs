using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public enum State
{
    Idle, // �⺻
    Attack, // ����
    Hit, // ����
    Dash, // �뽬 ȸ�Ǳ���
    Death // ����
}
public enum CalculationFormula
{
    Hp,
    Atk,
    Def,
    Exp,
    Mp_Natural, //�ڿ�ȸ��
    Mp_Attack //������ȸ��
}
public struct PlayerStat
{
    public float vgr; //�����
    public float str; //��
    public float ind; //�γ�
    public float mnt; // ���ŷ�
    public float NextExp; //�ʿ� ����ġ
}
public class Player : MonoBehaviour
{ //�Ҹ� ����Ʈ �ֱ�
    [SerializeField] SetUI swordCollider;
    [SerializeField] SetUI swordEffect;
    [SerializeField] ParticleSystem[] ps = new ParticleSystem[3];
    [SerializeField] private State playerstate;
    [SerializeField] private weapon sword; // ���ݶ��̴�
    [SerializeField] private SliderScript hp_slider;
    [SerializeField] private SliderScript mp_slider;
    [SerializeField] private TextUi text;
    [SerializeField] private SetUI infoUi;
    [SerializeField] private Monster monster;
    [SerializeField] private PlayerAnim playeranim;
    [SerializeField] private AudioSource hitAudio; //�´¼Ҹ�
    [SerializeField] private AudioSource dieAudio;  //�״¼Ҹ�
    [SerializeField] private AudioSource swordAudio;//�˼Ҹ�
    [SerializeField] private AudioSource shieldAudio; //���мҸ�
    [SerializeField] private AudioSource rollingAudio; // ������
    [SerializeField] private ThirdPersonCamera camera;
    [SerializeField] private SetUI endUI;
    [SerializeField] private SetUI expUI;
    private Vector3 moveDirection; //�̵�����
    private float moveSpeed; //�̵��ӵ�
    private Rigidbody rigid;
    private Vector3 rotate;
    private PlayerStat m_playerStat; //ĳ���� ����
    private float Player_Hp; //ü��
    private float Player_Mp; //����
    private bool backrolling;
                             // private float Player_Vgr; //�����
                             //private float Player_Mnt;//���ŷ�
                             // private int rotationAngle = 0;
    private float Player_Atk; //���ݷ�
    private float Player_Def; //����
    //private float Player_Str; //��
    //private float Player_Ind; // �γ�
    private float Player_CurExp; //���� ����ġ
    private float Player_NextExp; // �ʿ����ġ
    private int Player_Lv; //����
    private float max_Hp;
    private float max_mp;
    private float MpRecoverytime; //����ȸ�� �ð� ex)1�ʸ���
    private Vector2 input;
    private bool isDead => (0 >= Player_Hp); // ���� ���� Ȯ��
    private float playtime;
    private bool timecheck;
    private float totalEXP;
    public void Initialize()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        moveSpeed = 4f;
        Player_Lv = 1;
        m_playerStat.vgr = 5;
        m_playerStat.str = 5;
        m_playerStat.ind = 5;
        m_playerStat.mnt = 5;
        m_playerStat.NextExp = 100f;
        // Player_Str = 5;
        // Player_Ind = 5;
        // Player_Mnt = 5;
        // Player_Vgr = 5;
        Player_CurExp = 0;
        //Player_NextExp = 100f;
        Player_Mp = 50;
        max_mp = Player_Mp;
        MpRecoverytime = 0f;
        backrolling = false;
        Stat_CalculationFormula(1);
        Stat_CalculationFormula(2);
        Stat_CalculationFormula(3);
        playtime = 0f;
        timecheck = true;
        totalEXP = 0f;
    }
    public void Add_Exp(float Exp)
    {
        Player_CurExp += Exp; //�÷��̾ ������ ���Ϳ��� exp����
        totalEXP += Exp;
    }
    public void Add_Stat(int select)
    {
        if (Player_CurExp - m_playerStat.NextExp >= 0) //���� ����ġ�� �ʿ亸�� Ŭ��츸 ������ �ø�
        {
            Player_Lv++;
            Player_CurExp -= m_playerStat.NextExp;
            switch (select) //����ġ�� ��� �����
            {
                case 1:
                    m_playerStat.vgr++;
                    break;
                case 2:
                    m_playerStat.str++;
                    break;
                case 3:
                    m_playerStat.ind++;
                    break;
                case 4:
                    m_playerStat.mnt++;
                    break;
            }
            Stat_CalculationFormula(select);
            Stat_CalculationFormula(6);
        }
    }
    public void Stat_CalculationFormula(int select) //����
    {
        float mp = m_playerStat.mnt * 0.5f;
        switch (select)
        {
            case 1:
                Player_Hp = 80 + (m_playerStat.vgr * 5);
                max_Hp = Player_Hp;
                break;
            case 2:
                Player_Atk = 100 + (m_playerStat.str * 11.5f);
                sword.Set_Damage(Player_Atk);
                break;
            case 3:
                Player_Def = 10 + (m_playerStat.ind * 0.3f);
                break;
            case 4:
                Player_Mp += 10 + m_playerStat.mnt * 0.5f;
                break;
            case 5:
                // �̺κп� Mp �ѷ��� ������ ���� if�� ���� ����
                if (Player_Mp + m_playerStat.mnt * 0.5f <= max_mp)
                    Player_Mp += m_playerStat.mnt * 0.5f;
                else Player_Mp = max_mp;
                break;
            case 6:
                m_playerStat.NextExp += (m_playerStat.NextExp * 0.3f);
                break;
        }
    }
    private void Start()
    {
        Initialize();
    }
    private void Awake()
    {
        playeranim = GetComponent<PlayerAnim>();
        rigid = GetComponent<Rigidbody>();
        Initialize();
        playtime = 0f;
    }
    private void Rotate()
    {
        float rotationAngle;

        if (moveDirection.magnitude > 0)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            rotationAngle = targetAngle + camera.transform.eulerAngles.y;
        }
        else
        {
            rotationAngle = transform.eulerAngles.y;
        }
        Quaternion targetRotation = Quaternion.Euler(0, rotationAngle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 30 * Time.deltaTime);
    }
    public bool Mp_Consume(float skill_mp)
    {
        if (Player_Mp - skill_mp < 0) return false;
        else
        {
            Player_Mp -= skill_mp;
            return true;
        }

    }
    public void recovery(float hill)
    {
        if (Player_Hp + hill <= max_Hp) Player_Hp += hill; //�� ������ ���� ���� �ִ�ü�º��� �������� �ϰ� ������ �׳� �ִ�ü���� �������
        else Player_Hp = max_Hp;
    }
    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(0.5f);
        playerstate = State.Idle;
    }
    public void Hit(float Damage) // ȸ���Ҷ� ���� ����
    {
        if (playeranim && !playerstate.Equals(State.Dash) && !isDead && !playerstate.Equals(State.Hit))
        {
            Player_Hp -= Damage;
            Debug.Log("�÷��̾� ����" + Player_Hp);
            if (!isDead)
            {

                playeranim.Hit();
                AudioMgr.instance.PlayAudio(hitAudio); //���Ⱑ �Ҹ�
                playerstate = State.Hit;
                StartCoroutine(Timer());
            }
            else AudioMgr.instance.PlayAudio(dieAudio);
        }
    }
    private void Movement()
    {
        if (rigid)
        {
            float x = moveDirection.x, z = moveDirection.z;
            if (moveDirection.x != 0) playeranim.Move(moveDirection.x);
            else playeranim.Move(moveDirection.z);
            Vector3 velocity = new Vector3(x, 0, z)  +playeranim.transform.forward * moveSpeed;
            if (backrolling)
            {
                velocity =  -playeranim.transform.forward * 8;
                rigid.velocity = velocity;
            }
            else if ((playeranim.Equals(State.Attack)))
            {
                rigid.velocity = velocity;
            }
            else if (x != 0 || z != 0)
            {
                rigid.velocity = velocity;

            }
            else
            {
               
                rigid.velocity = new Vector3(0, 0, 0);

            }
            Rotate();
        }
    }
    public void OffEXP()
    {
        expUI.Off();
    }
    public void restart()
    {
        playerstate = State.Idle;
        playeranim.Die(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        timecheck = true;
        camera.setcamera(true);
    }
    public void Oninfo()
    {
        infoUi.On();
    }

    public void OffIinfo()
    {
        infoUi.Off();
    }
    private IEnumerator Die() //������Ű�� �� �ؾ��ҵ�
    {
        //if(playerstate.Equals(State.Death)) playeranim.Die();
        playerstate = State.Death;
        playeranim.Die(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        camera.setcamera(false);
        text.Exp(Player_CurExp, totalEXP);
        yield return new WaitForSeconds(1.5f);
        timecheck = false;
        //   gameObject.SetActive(false);
        MaxHPMP();
       
        monster.Deactivation();
        // infoUi.On();
        expUI.On();
    }
    private void MaxHPMP()
    {
        Player_Hp = max_Hp;
        Player_Mp = max_mp;
    }
    public void StateChange(State state)
    {
        playerstate = state;
    }
    private void FixedUpdate()
    {
        if (!playerstate.Equals(State.Death) && !playerstate.Equals(State.Attack)) Movement();
    }
    private void Update()
    {
        if (timecheck) playtime += Time.deltaTime;
        hp_slider.Slider_Update(Player_Hp / max_Hp);
        mp_slider.Slider_Update(Player_Mp / max_mp);
        text.statText(Player_Lv, Player_CurExp, m_playerStat);
        MpRecoverytime += Time.deltaTime;
        if (MpRecoverytime >= 1f)//1�ʸ��� ����ȸ��
        {
            MpRecoverytime = 0f;
            Stat_CalculationFormula(5);
        }
        if (isDead)
        {
            StartCoroutine(Die());
        }
    }
    public void End()
    {
        endUI.On();
        camera.setcamera(false);
        Cursor.lockState = CursorLockMode.None;
        timecheck = false;
        text.EndText(Player_Lv, playtime);
    }
    IEnumerator SwordColliderMaintain() // �ݶ��̴� �����ð�
    {
        swordCollider.On();
        yield return new WaitForSeconds(0.1f);
        swordCollider.Off();
    }
    private void OnTriggerEnter(Collider other)
    {
        //if (other.tag.Equals("Monster")) other.GetComponent<Monster>().Hit(Player_Atk);
    } //�̰� �˿� ���� �� ���� �ݶ��̴�
    public void OnAttack(int Combo)
    {//�ӽ� ĳ���� �� �ڵ� ���߿� ��������
        swordCollider.On(); //�ִϸ��̼� �̺�Ʈ�� �־���
        playerstate = State.Attack;
        AudioMgr.instance.PlayAudio(swordAudio);
    }
    public void OffAttack(int Combo)
    {
        swordCollider.Off();
        playerstate = State.Idle;
    }
    public void OnEffect(int Combo)
    {
    //    swordEffect.On();
   //  if(Combo != 2)   ps[Combo].Play();
    }
    public void OffEffect(int Combo)
    {
     //   ps[Combo].Stop();
      //  swordEffect.Off();
    }
    private IEnumerator AttackEffect(int Combo)
    {
        if (Combo < 2) yield return new WaitForSeconds(0.15f);
        else yield return new WaitForSeconds(0.25f);
        ps[Combo].Play();
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
        if (input != null) moveDirection = new Vector3(input.x, 0f, input.y);
    }
    public void OnLeftMouse(InputAction.CallbackContext context)
    {
        bool mouse = context.ReadValueAsButton();
        if (mouse && playeranim && !playerstate.Equals(State.Death))
        {
            playeranim.Attack();
            //    playerstate = State.Attack;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        bool run = context.ReadValueAsButton();
        if (playeranim && !playerstate.Equals(State.Death))
        {
            if (!((input.x == 0 && input.y == 0) && moveSpeed == 4))
            {
                playeranim.Run(run);
                if (run) moveSpeed = 10f;
                else moveSpeed = 4f;
            }
        }
    }
    public void OnRolling()
    {
        backrolling = true;
    }

    public void OffRolling()
    {
        backrolling = false;
    }
    public void OnDesh(InputAction.CallbackContext context)
    {
        bool desh = context.ReadValueAsButton();
        if (playeranim && !playerstate.Equals(State.Death))
        {
            if (desh)
            {
                if (!(input.x == 0 && input.y == 0))
                {
                    playeranim.Dash();
                    AudioMgr.Instance.PlayAudio(rollingAudio); //������
                                                               //  playerstate = State.Desh;
                }
                else
                {
                    playeranim. BackRoll();
                }
            }
        }

    }



}
