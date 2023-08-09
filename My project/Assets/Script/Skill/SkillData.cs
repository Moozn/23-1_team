using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Object/SkillData", order = int.MaxValue)]
public class SkillData : ScriptableObject
{
    [SerializeField] float cooltime;
    public float Cooltime { get { return cooltime; } }
    [SerializeField] float effect_playtime;
    public float Effect_playtime { get { return effect_playtime; } }
    [SerializeField] SkillType type;
    public SkillType Type { get { return type; } }
    public float Cost { get { return cost; } }
    [SerializeField] float cost;
}
