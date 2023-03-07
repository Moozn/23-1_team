using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public enum State
{
    Idle, // �⺻
    Attack, // ����
    Hit, // ����
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
public class Player : MonoBehaviour
{
    private PlayerAnim playeranim;
    private Vector3 moveDirection;
    private float moveSpeed = 4f;
    private Rigidbody rigid;
    private Vector3 rotate;
    private float Player_Hp; //ü��
    private float Player_Mp; //����
    private float Player_Vgr; //�����
    private float Player_Mnt;//���ŷ�
    private float Player_Atk; //���ݷ�
    private float Player_Def; //����
    private float Player_Str; //��
    private float Player_Ind; // �γ�
    private float Player_CurExp; //���� ����ġ
    private float Player_NextExp; // �ʿ����ġ
    private int Player_Lv; //����
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
    public void Stat_CalculationFormula(int select) //����
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
                // �̺κп� Mp �ѷ��� ������ ���� if�� ���� ����
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
    private void Movement()
    {
        if (rigid)
        {
            //�ִϸ��̼�
            //transform.rotation = Quaternion.LookRotation(moveDirection); //ȸ��
            //   transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime * moveDirection.y); //�̵�
            Rotate();
            playeranim.Move(moveDirection.z);
            Vector3 velocity = transform.forward * moveDirection.z * moveSpeed; //�̵�
            Vector3 velocity_x = transform.right * moveDirection.x * moveSpeed;
            rigid.velocity = velocity + velocity_x; //�̵�
            
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
