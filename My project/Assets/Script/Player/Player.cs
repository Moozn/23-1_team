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
    [SerializeField] private State playerstate;
    [SerializeField] private weapon sword; // ���ݶ��̴�
    [SerializeField] private SliderScript hp_slider;
    [SerializeField] private SliderScript mp_slider;
    [SerializeField] private TextUi text;
    [SerializeField] private SetUI infoUi;
    [SerializeField] private Monster monster;
    private PlayerAnim playeranim;
    private Vector3 moveDirection; //�̵�����
    private float moveSpeed; //�̵��ӵ�
    private Rigidbody rigid;
    private Vector3 rotate;
    private PlayerStat m_playerStat; //ĳ���� ����
    private float Player_Hp; //ü��
    private float Player_Mp; //����
                             // private float Player_Vgr; //�����
                             //private float Player_Mnt;//���ŷ�
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
    private bool isDead => (0 >= Player_Hp); // ���� ���� Ȯ��
    public void Initialize()
    {
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
        Stat_CalculationFormula(1);
        Stat_CalculationFormula(2);
        Stat_CalculationFormula(3);
    }
    public void Add_Exp(float Exp)
    {
        Player_CurExp += Exp; //�÷��̾ ������ ���Ϳ��� exp����
    }
    public void Add_Stat(int select)
    {
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
                m_playerStat.NextExp = 100 + (m_playerStat.NextExp * 0.3f);
                break;
            case 5:
                // �̺κп� Mp �ѷ��� ������ ���� if�� ���� ����
                Player_Mp += m_playerStat.mnt * 0.5f;
                break;
            case 6:
                Player_Mp += 10 + m_playerStat.mnt * 0.5f;
                break;
        }
    }

    private void Awake()
    {
        playeranim = GetComponent<PlayerAnim>();
        rigid = GetComponent<Rigidbody>();
        Initialize();
    }
    private void Rotate()
    {
        //   ���콺 Ŀ�� ���� ���� ���� ���̸� ���� �װ� ���� ����Ʈ�� - ���� �� ��ǥ
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());//Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f))
        {
            rotate = new Vector3(hit.point.x, transform.position.y, hit.point.z) - (transform.position);//.normalized;
        } // �� ���콺 ����

        Quaternion newRotate = Quaternion.LookRotation(rotate);
        rigid.rotation = Quaternion.Slerp(rigid.rotation, newRotate, 10 * Time.deltaTime); //���� ���⿡�� ��ǥ�������� �ð���ŭ ������ �׷���

        //rigid.rotation *= Quaternion.Euler(0.0f, moveDirection.x * 0.5f, 0.0f); //ȸ��
        //  rigid.rotation *= Quaternion.Euler(0.0f, rotate.x, 0.0f);
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
        if (Player_Hp + hill <= max_Hp)  Player_Hp += hill; //�� ������ ���� ���� �ִ�ü�º��� �������� �ϰ� ������ �׳� �ִ�ü���� �������
        else Player_Hp = max_Hp;
    }
    public void Hit(float Damage) // ȸ���Ҷ� ���� ����
    {
        if (playeranim && !playerstate.Equals(State.Dash) && !isDead)
        {
                Player_Hp -= Damage;
                playeranim.Hit();
                playerstate = State.Hit;
                Debug.Log("����" + Player_Hp);
        }
    }
    private void Movement()
    {
        if (rigid)
        {
            int x = 1, z = 1;
            //�ִϸ��̼�
            //transform.rotation = Quaternion.LookRotation(moveDirection); //ȸ��
            //   transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime * moveDirection.y); //�̵�
            Rotate();
            if (moveDirection.x != 0) playeranim.Move(moveDirection.x);
            else playeranim.Move(moveDirection.z);
            //    Debug.Log(transform.forward);
            // Debug.Log(transform.right);
            // if (transform.forward.z < 0) z = -1;
            //if (transform.right.x < 0) x = -1;
            Vector3 velocity = new Vector3(0, 0, z) * moveDirection.z * moveSpeed; //transform.forward * moveDirection.z * moveSpeed; //�̵�
            Vector3 velocity_x = new Vector3(x, 0, 0) * moveDirection.x * moveSpeed;//transform.right * moveDirection.x * moveSpeed;
            rigid.velocity = velocity + velocity_x; //�̵�
        }
    }
    private IEnumerator Die() //������Ű�� �� �ؾ��ҵ�
    {
        // if(playerstate.Equals(State.Death)) playeranim.Die();
        playerstate = State.Death;
        playeranim.Die(true);
        yield return new WaitForSeconds(1f);
        playeranim.Die(true);
        //   gameObject.SetActive(false);
        MaxHPMP();
        monster.Deactivation();
        infoUi.On();
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
        if(!playerstate.Equals(State.Death))Movement();
    }
    private void Update()
    {
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
    public void OnAttack()
    {
        swordEffect.On();
        swordCollider.On(); //�ִϸ��̼� �̺�Ʈ�� �־���
    }
    public void OffAttack()
    {
        swordEffect.Off();
        swordCollider.Off();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        if(input != null) moveDirection = new Vector3(input.x, 0f, input.y);
    }
    public void OnLeftMouse(InputAction.CallbackContext context)
    {
        bool mouse = context.ReadValueAsButton();
        if (mouse && playeranim)
        {
            playeranim.Attack();
        //    playerstate = State.Attack;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        bool run = context.ReadValueAsButton();
        if (playeranim)
        {
            playeranim.Run(run);
            if(run) moveSpeed = 10f;
            else moveSpeed = 4f;
        }
    }

    public void OnDesh(InputAction.CallbackContext context)
    {
        bool desh = context.ReadValueAsButton();
        if(playeranim)
        {
            if (desh)
            {
                playeranim.Dash();
              //  playerstate = State.Desh;
            }
        }
       
    }



}
