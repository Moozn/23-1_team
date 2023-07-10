using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Skilㅣ : MonoBehaviour
{
    [SerializeField] private Image Skill_icon;
  //  [SerializeField] private Skill_Value skill_Data;
    [SerializeField] private ParticleSystem skill_Effect;
    [SerializeField] private AudioSource skillAudio;
    private Player player;
    private KeyCode skill_key;
    private bool skill;
    private float effectTime;
    private float offset;
    private float coolTime;
    private float curTime;
    private float skillDamage;

    float effectDist = 1;

    private void Awake()
    {
        curTime = 3f;
        skillDamage = 0f;
        offset = 1 / coolTime;
        skill = true;
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    private void SkillAttack(LayerMask target)
    {
        Collider[] targets = null;

        Vector3 pos = transform.position;
        pos.y = 0;  //스킬 나오면 맞춰서 진행
    //  pos = pos + transform.forward * skill_Data.centerPosition.z + transform.right * skill_Data.centerPosition.x;
    //  //skillAudio.Play();
    //  AudioMgr.instance.PlayAudio(skillAudio);
    //  switch (skill_Data.effectType)
    //  {
    //      case EffectType.Hp:
    //          targets = Physics.OverlapBox(pos, skill_Data.boxSize, transform.rotation, target);
    //          skillDamage = player.maxhp * 0.5f;
    //          break;
    //      case EffectType.Damage:
    //          targets = Physics.OverlapBox(pos, skill_Data.boxSize, transform.rotation, target);
    //          skillDamage = player.damage * 1.5f;
    //          break;
    //      case EffectType.DefensivePower:
    //          targets = Physics.OverlapSphere(pos, skill_Data.radius, target);
    //          skillDamage = player.defensivePower * 4.5f;
    //          break;
    //  }
    //
        if (null != targets && 0 < targets.Length)
        {
            foreach (var monster in targets)
            {
                monster.GetComponent<Monster>().Hit(skillDamage);
            }
        }
    }

    public void SkilCheck(LayerMask target)
    {
        if (!skill && Input.GetKeyDown(skill_key)) //키 누르면 발동되는 식으로
        {
            skill = true;
            StartCoroutine(EffectPlay());
            StartCoroutine(CoolTime(coolTime));
            SkillAttack(target);
        }
    }
    public void OnSkill()
    {
        skill = false;
        Skill_icon.fillAmount = 0f;
    }
    private IEnumerator EffectPlay() //이펙트 시간에 맞춰서 파티클 꺼주기
    {
        skill_Effect.Play();
        yield return new WaitForSeconds(effectTime);
        skill_Effect.Stop();
    }

    private IEnumerator CoolTime(float cool)
    {
        while (cool > curTime)
        {
            curTime += Time.deltaTime;
            Skill_icon.fillAmount = offset * curTime; //1 / 쿨타임 = offset 이니 이걸 흐린 시간 만큼 곱하면 흐린 시간 =  쿨타임이 되면 fillAmount에 1이 들어간다.
            yield return new WaitForFixedUpdate();
        }

        curTime = 0f;
        Skill_icon.fillAmount = 0f;
        skill = false;
    }
}
