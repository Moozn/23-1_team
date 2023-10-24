using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public enum State
{
    Idle, // 기본
    Attack, // 공격
    Hit, // 맞음
    Dash, // 대쉬 회피기임
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
public struct PlayerStat
{
    public float vgr; //생명력
    public float str; //힘
    public float ind; //인내
    public float mnt; // 정신력
    public float NextExp; //필요 경험치
}
public class Player : MonoBehaviour
{ //소리 이펙트 넣기
    [SerializeField] SetUI swordCollider;
    [SerializeField] SetUI swordEffect;
    [SerializeField] ParticleSystem[] ps = new ParticleSystem[3];
    [SerializeField] private State playerstate;
    [SerializeField] private weapon sword; // 검콜라이더
    [SerializeField] private SliderScript hp_slider;
    [SerializeField] private SliderScript mp_slider;
    [SerializeField] private TextUi text;
    [SerializeField] private SetUI infoUi;
    [SerializeField] private Monster monster;
    [SerializeField] private PlayerAnim playeranim;
    [SerializeField] private AudioSource hitAudio;
    [SerializeField] private AudioSource dieAudio;
    [SerializeField] private AudioSource swordAudio;
    [SerializeField] private Transform camera;
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
    private float max_Hp;
    private float max_mp;
    private float MpRecoverytime; //마나회복 시간 ex)1초마다
    private bool isDead => (0 >= Player_Hp); // 죽음 상태 확인
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
        Player_CurExp += Exp; //플레이어가 죽을때 몬스터에서 exp들고옴
    }
    public void Add_Stat(int select)
    {
        if (Player_CurExp - m_playerStat.NextExp >= 0) //현재 경험치가 필요보다 클경우만 스텟을 올림
        {
            Player_Lv++;
            Player_CurExp -= m_playerStat.NextExp;
            switch (select) //경험치로 사게 만들기
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
    public void Stat_CalculationFormula(int select) //계산식
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
                // 이부분에 Mp 총량을 넘을수 없다 if로 넣을 예정
                if (Player_Mp + m_playerStat.mnt * 0.5f <= max_mp)
                    Player_Mp += m_playerStat.mnt * 0.5f;
                else Player_Mp = max_mp;
                break;
            case 6:
                m_playerStat.NextExp += (m_playerStat.NextExp * 0.3f);
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
        int rotationAngle = 0; //w
        if (moveDirection.z > 0 && moveDirection.x > 0) rotationAngle = 45;
        else if (moveDirection.z > 0 && moveDirection.x < 0) rotationAngle = 315;
        else if (moveDirection.z < 0 && moveDirection.x > 0) rotationAngle = 135; 
        else if (moveDirection.z < 0 && moveDirection.x < 0) rotationAngle = 225;
        else if (moveDirection.z < 0) rotationAngle = 180; //d
        else if (moveDirection.x < 0) rotationAngle = 270; //a //0보다 작을때
        else if (moveDirection.x > 0) rotationAngle = 90; //d 
        float currentRotationAngle = Mathf.LerpAngle(transform.eulerAngles.y, rotationAngle + camera.eulerAngles.y, 10 * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, currentRotationAngle, 0);
        // //   마우스 커서 가는 방향 보기 레이를 쏴서 그걸 맞은 포인트에 - 현재 내 좌표
        // Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());//Input.mousePosition);
        // RaycastHit hit;
        //
        // if (Physics.Raycast(ray, out hit, 1000f))
        // {
        //     rotate = new Vector3(hit.point.x, transform.position.y, hit.point.z) - (transform.position);//.normalized;
        // } // 내 마우스 방향
        //
        // Quaternion newRotate = Quaternion.LookRotation(rotate);
        // rigid.rotation = Quaternion.Slerp(rigid.rotation, newRotate, 10 * Time.deltaTime); //현재 방향에서 목표방향으로 시간만큼 돌리기 그래서

        //rigid.rotation *= Quaternion.Euler(0.0f, moveDirection.x * 0.5f, 0.0f); //회전
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
        if (Player_Hp + hill <= max_Hp)  Player_Hp += hill; //힐 했을때 더한 값이 최대체력보다 낮을떄만 하고 높으면 그냥 최대체력을 줘버리기
        else Player_Hp = max_Hp;
    }
    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(0.5f);
        playerstate = State.Idle;
    }
    public void Hit(float Damage) // 회피할때 빼곤 맞음
    {
        if (playeranim && !playerstate.Equals(State.Dash) && !isDead && !playerstate.Equals(State.Hit))
        {
            Player_Hp -= Damage;
            Debug.Log("플레이어 맞음" + Player_Hp);
            if (!isDead)
            {
                
                playeranim.Hit();
                AudioMgr.instance.PlayAudio(hitAudio);
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
            //애니메이션
            //transform.rotation = Quaternion.LookRotation(moveDirection); //회전
            //   transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime * moveDirection.y); //이동

            // Quaternion newRotate = Quaternion.LookRotation(rotate + new Vector3(0, 0, 0));
            // rigid.rotation = Quaternion.Slerp(rigid.rotation, newRotate, 10 * Time.deltaTime); //현재 방향에서 목표방향으로 시간만큼 돌리기 그래서
            Rotate();
            if (moveDirection.x != 0) playeranim.Move(moveDirection.x);
            else playeranim.Move(moveDirection.z);
            if (x != 0 || z != 0)
            {
                Vector3 velocity = new Vector3(x, 0, z) + playeranim.transform.forward * moveSpeed;
                //    Vector3 velocity = new Vector3(x, 0,z) * moveSpeed; //이동                                     
                rigid.velocity = velocity; //+ velocity_x; //이동
            }
        }
    }
    public void restart()
    {
        playerstate = State.Idle;
    }
    private IEnumerator Die() //정지시키고 뭐 해야할듯
    {
        // if(playerstate.Equals(State.Death)) playeranim.Die();
        playerstate = State.Death;
        playeranim.Die(true);
        
        yield return new WaitForSeconds(1f);

        //   gameObject.SetActive(false);
        MaxHPMP();
        playeranim.Die(false);
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
        if (MpRecoverytime >= 1f)//1초마다 마나회복
        {
            MpRecoverytime = 0f;
            Stat_CalculationFormula(5);
        }
        if (isDead)
        {
            StartCoroutine(Die());
        }
    }
    IEnumerator SwordColliderMaintain() // 콜라이더 유지시간
    {
        swordCollider.On();
        yield return new WaitForSeconds(0.1f);
        swordCollider.Off();
    }
    private void OnTriggerEnter(Collider other)
    {
        //if (other.tag.Equals("Monster")) other.GetComponent<Monster>().Hit(Player_Atk);
    } //이건 검에 따로 달 예정 콜라이더
    public void OnAttack(int Combo)
    {//임시 캐릭터 용 코드 나중에 지워도됨
        swordEffect.On();
        swordCollider.On(); //애니메이션 이벤트에 넣었음
        StartCoroutine(AttackEffect(Combo));
        AudioMgr.instance.PlayAudio(swordAudio);
    }
    public void OffAttack(int Combo)
    {
        swordEffect.Off();
        swordCollider.Off();
        ps[Combo].Stop();
    }
    private IEnumerator AttackEffect(int Combo)
    {
        if (Combo < 2) yield return new WaitForSeconds(0.15f);
        else yield return new WaitForSeconds(0.25f);
        ps[Combo].Play();
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        if(input != null) moveDirection = new Vector3(input.x, 0f, input.y);
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
            playeranim.Run(run);
            if(run) moveSpeed = 10f;
            else moveSpeed = 4f;
        }
    }

    public void OnDesh(InputAction.CallbackContext context)
    {
        bool desh = context.ReadValueAsButton();
        if(playeranim && !playerstate.Equals(State.Death))
        {
            if (desh)
            {
                playeranim.Dash();
              //  playerstate = State.Desh;
            }
        }
       
    }



}
