using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public enum SkillType
{
    recovery, //회복스킬
    short_dis, // 근거리스킬
    long_dis, // 원거리 스킬
}
struct Skilldata
{
    public float damage;
    public float cooltime;
    public float effect_playtime;
    public SkillType type;
}
public class Skill : MonoBehaviour
{
    [SerializeField] private Image Skill_icon;
  //  [SerializeField] private Skill_Value skill_Data;
    [SerializeField] private ParticleSystem skill_Effect;
    [SerializeField] private ParticleTrigger pt;
    [SerializeField] private AudioSource skillAudio;
    [SerializeField] private SkillData data;
    private Skilldata skilldata;
    private Player player;
    private KeyCode skill_key;
    private bool skill;
    private float effectTime;
    private float offset;
    private float coolTime;
    private float curTime;
    private float skillDamage;

    private void Awake()
    {
        curTime = 0f;
        coolTime = 3f;
        skillDamage = 0f;
        offset = 1 / coolTime;
        effectTime = 0.5f;
        skill = true;
        skilldata.cooltime = data.Cooltime;
        skilldata.effect_playtime = data.Effect_playtime;
        skilldata.type = data.Type;
        skilldata.damage = 0f;
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    private void Set_damage(float _damage)
    {
        skilldata.damage = _damage;
    }
    private void FixedUpdate()
    {
       
        if (!skill)
        {
            curTime += Time.deltaTime;
            if (curTime > skilldata.cooltime)
            {
                skill = true;
                curTime = 0f; 
            }
        }
    }

    private void Long_skill() //원거리 스킬 마우스 포인터에 나타남
    {
        if (pt) pt.SetDamage(100f);
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());//Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            skill_Effect.transform.position = hit.point;
        }
    }
    private void recovery_skill() //회복스킬 플레이어 회복시킴
    {
        player.recovery(skilldata.damage);
    }
    private void skill_type()
    {
        switch(skilldata.type)
        {
            case SkillType.recovery:
                recovery_skill();
                break;
            case SkillType.short_dis:
                break;
            case SkillType.long_dis:
                Long_skill();
                break;
        }
    }
    private IEnumerator EffectPlay() //이펙트 시간에 맞춰서 파티클 꺼주기
    {
        skill_Effect.Play();
        yield return new WaitForSeconds(skilldata.effect_playtime);
        skill_Effect.Stop();
    }

    private IEnumerator CoolTime(float cool)
    {
        while (cool > curTime)
        {
            curTime += Time.deltaTime;
          //  Skill_icon.fillAmount = offset * curTime; //1 / 쿨타임 = offset 이니 이걸 흐린 시간 만큼 곱하면 흐린 시간 =  쿨타임이 되면 fillAmount에 1이 들어간다.
            yield return new WaitForFixedUpdate();
            skill = false;
        }

        curTime = 0f;
        //Skill_icon.fillAmount = 0f;
        //skill = true;
    }
    public void Press_Skill()
    {
        if (curTime <= 0 && player.Mp_Consume(data.Cost)) //스킬 코스트 스킬마다 지정해서 그거 소모
        {
            skill_type();
            StartCoroutine(EffectPlay());
        }
        skill = false;
    }

}
