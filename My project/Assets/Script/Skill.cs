using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Skil�� : MonoBehaviour
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
        pos.y = 0;  //��ų ������ ���缭 ����
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
        if (!skill && Input.GetKeyDown(skill_key)) //Ű ������ �ߵ��Ǵ� ������
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
    private IEnumerator EffectPlay() //����Ʈ �ð��� ���缭 ��ƼŬ ���ֱ�
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
            Skill_icon.fillAmount = offset * curTime; //1 / ��Ÿ�� = offset �̴� �̰� �帰 �ð� ��ŭ ���ϸ� �帰 �ð� =  ��Ÿ���� �Ǹ� fillAmount�� 1�� ����.
            yield return new WaitForFixedUpdate();
        }

        curTime = 0f;
        Skill_icon.fillAmount = 0f;
        skill = false;
    }
}
