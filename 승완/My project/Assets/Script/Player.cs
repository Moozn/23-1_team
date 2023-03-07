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
public class Player : MonoBehaviour
{
    private PlayerAnim playeranim;
    private Vector3 moveDirection;
    private float moveSpeed = 4f;
    private Rigidbody rigid;
    private Vector3 rotate;
    private float Player_Hp; //체력
    private float Player_Mp; //마나
    private float Player_Vgr; //생명력
    private float Player_Mnt;//정신력
    private float Player_Atk; //공격력
    private float Player_Def; //방어력
    private float Player_Str; //힘
    private float Player_Ind; // 인내
    private float Player_CurExp; //현재 경험치
    private float Player_NextExp; // 필요경험치
    private int Player_Lv; //레벨
    private float MpRecoverytime;
    public void Initialize()
    {
        Player_Lv = 1;
        Player_Str = 5;
        Player_Ind = 5;
        Player_Mnt = 5;
        Player_Vgr = 5;
        Player_CurExp = 0;
        Player_NextExp = 100f;
        Player_Mp = 50;
        MpRecoverytime = 0f;
        Stat_CalculationFormula(1);
        Stat_CalculationFormula(2);
        Stat_CalculationFormula(3);
    }
    public void Stat_CalculationFormula(int select) //계산식
    {
        float mp = Player_Mnt * 0.5f;
        switch (select)
        {
            case 1:
                Player_Hp = 80 + (Player_Vgr * 5);
                break;
            case 2:
                Player_Atk = 100 + (Player_Str * 11.5f);
                break;
            case 3:
                Player_Def = 10 + (Player_Ind * 0.3f);
                break;
            case 4:
                Player_NextExp = 100 + (Player_NextExp * 0.3f);
                break;
            case 5:
                // 이부분에 Mp 총량을 넘을수 없다 if로 넣을 예정
                Player_Mp += Player_Mnt * 0.5f;
                break;
            case 6:
                Player_Mp += 10 + Player_Mnt * 0.5f;
                break;
        }
    }

    private void Awake()
    {
        playeranim = GetComponent<PlayerAnim>();
        rigid = GetComponent<Rigidbody>();
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
            //애니메이션
            //transform.rotation = Quaternion.LookRotation(moveDirection); //회전
            //   transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime * moveDirection.y); //이동
            Rotate();
            playeranim.Move(moveDirection.z);
            Vector3 velocity = transform.forward * moveDirection.z * moveSpeed; //이동
            Vector3 velocity_x = transform.right * moveDirection.x * moveSpeed;
            rigid.velocity = velocity + velocity_x; //이동
            
        }
    }
    private void Update()
    {
        Movement();
        MpRecoverytime += Time.deltaTime;
        if (MpRecoverytime >= 1f)
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
        if(input != null)
        {
            moveDirection = new Vector3(input.x, 0f, input.y);
        }

    }
    public void OnLeftMouse(InputAction.CallbackContext context)
    {
        bool mouse = context.ReadValueAsButton();
        if (mouse && playeranim)
        {
            playeranim.Attack();
            Attack();
        }
    }
    


    public void Move(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
    }

    public void Rotate(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
    }
}
