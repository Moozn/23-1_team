using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public enum State
{
    Idle, // 기본
    Attack, // 공격
    Hit, // 맞음
    Death // 죽음
}
public enum CalculationFormula
{
    Hp,
    Atk,
    Def,
    Exp,
    Mp_Natural, //자연회복
    Mp_Attack //떄리면회복
}
struct PlayerStat
{
    public float vgr; //생명력
    public float str; //힘
    public float ind; //인내
    public float mnt; // 정신력
    public float NextExp; //필요 경험치
}
public class Player : MonoBehaviour
{
    [SerializeField] SetUI swordCollider;
    private PlayerAnim playeranim;
    private Vector3 moveDirection; //이동방향
    private float moveSpeed; //이동속도
    private Rigidbody rigid;
    private Vector3 rotate;
    private PlayerStat m_playerStat; //캐릭터 스텟
    private float Player_Hp; //체력
    private float Player_Mp; //마나
   // private float Player_Vgr; //생명력
    //private float Player_Mnt;//정신력
    private float Player_Atk; //공격력
    private float Player_Def; //방어력
    //private float Player_Str; //힘
    //private float Player_Ind; // 인내
    private float Player_CurExp; //현재 경험치
    private float Player_NextExp; // 필요경험치
    private int Player_Lv; //레벨
    private float MpRecoverytime; //마나회복 시간 ex)1초마다
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
        MpRecoverytime = 0f;
        Stat_CalculationFormula(1);
        Stat_CalculationFormula(2);
        Stat_CalculationFormula(3);
    }
    public void Add_Stat(int select)
    {
        switch (select)
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
    }
    public void Stat_CalculationFormula(int select) //계산식
    {
        float mp = m_playerStat.mnt * 0.5f;
        switch (select)
        {
            case 1:
                Player_Hp = 80 + (m_playerStat.vgr * 5);
                break;
            case 2:
                Player_Atk = 100 + (m_playerStat .str * 11.5f);
                break;
            case 3:
                Player_Def = 10 + (m_playerStat.ind * 0.3f);
                break;
            case 4:
                m_playerStat.NextExp = 100 + (m_playerStat.NextExp * 0.3f);
                break;
            case 5:
                // 이부분에 Mp 총량을 넘을수 없다 if로 넣을 예정
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
        //   마우스 커서 가는 방향 보기 레이를 쏴서 그걸 맞은 포인트에 - 현재 내 좌표
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());//Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f))
        {
            rotate = new Vector3(hit.point.x, transform.position.y, hit.point.z) - (transform.position);//.normalized;
        } // 내 마우스 방향

        Quaternion newRotate = Quaternion.LookRotation(rotate);
        rigid.rotation = Quaternion.Slerp(rigid.rotation, newRotate, 10 * Time.deltaTime); //현재 방향에서 목표방향으로 시간만큼 돌리기 그래서
        
        //rigid.rotation *= Quaternion.Euler(0.0f, moveDirection.x * 0.5f, 0.0f); //회전
        //  rigid.rotation *= Quaternion.Euler(0.0f, rotate.x, 0.0f);
    }
    private void Movement()
    {
        if (rigid)
        {
            int x = 1,z = 1;
            //애니메이션
            //transform.rotation = Quaternion.LookRotation(moveDirection); //회전
            //   transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime * moveDirection.y); //이동
            Rotate();
            if(moveDirection.x != 0) playeranim.Move(moveDirection.x);
            else playeranim.Move(moveDirection.z);
        //    Debug.Log(transform.forward);
           // Debug.Log(transform.right);
           // if (transform.forward.z < 0) z = -1;
            //if (transform.right.x < 0) x = -1;
            Vector3 velocity = new Vector3(0,0,z) * moveDirection.z * moveSpeed; //transform.forward * moveDirection.z * moveSpeed; //이동
            Vector3 velocity_x = new Vector3(x, 0, 0) * moveDirection.x * moveSpeed;//transform.right * moveDirection.x * moveSpeed;
            rigid.velocity = velocity + velocity_x; //이동
            
        }
    }
    private void FixedUpdate()
    {
        Movement();
    }
    private void Update()
    {
        MpRecoverytime += Time.deltaTime;
        if (MpRecoverytime >= 1f)//1초마다 마나회복
        {
            MpRecoverytime = 0f;
            Stat_CalculationFormula(5);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        pos.y = 0;
        pos = pos + transform.forward * 3 +transform.up * 2;
        Vector3 box = new Vector3(1, 0.5f, 0.5f);
      //  Gizmos.DrawWireCube(pos, box * 2);
        box = new Vector3(0.4f, 2, 0.4f);
        Gizmos.DrawWireCube(pos, box * 2);
    }

    IEnumerator SwordColliderMaintain() // 콜라이더 유지시간
    {
        swordCollider.On();
        yield return new WaitForSeconds(0.1f);
        swordCollider.Off();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Monster")) other.GetComponent<Monster>().Hit(Player_Atk);
    }
    private void Attack()
    {
        Collider[] targets = null;
        Vector3 pos = transform.position;
        pos.y = 0;
        pos = pos + transform.forward * 3 + transform.up * 2;
        Vector3 box = new Vector3(0.2f, 2f, 0.2f);
        targets = Physics.OverlapBox(pos, box,transform.rotation);

        if (null != targets && 0 < targets.Length)
        {
            foreach (var monster in targets)
            {
                monster.GetComponent<Monster>().Hit(Player_Atk);
            }
        }
        Player_CurExp += Player_Atk;
        if(Player_CurExp >= Player_NextExp)
        {
            Player_Lv += 1;
            Player_CurExp = 0;
            Stat_CalculationFormula(4);
        }
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
            StartCoroutine(SwordColliderMaintain());
          //  Attack();
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        bool run = context.ReadValueAsButton();
        if (run && playeranim) moveSpeed = 10f;
        else moveSpeed = 4f;
    }

    public void OnDesh(InputAction.CallbackContext context)
    {
        bool desh = context.ReadValueAsButton();
        if (desh && playeranim)
        {
           
        }
    }

}
