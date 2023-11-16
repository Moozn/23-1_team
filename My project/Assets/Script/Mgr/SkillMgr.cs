using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillMgr : MonoBehaviour
{
    public static SkillMgr instance = null;
    [SerializeField] private Skill[] skills = new Skill[3];
    [SerializeField] private AudioSource QskillAudio;
    [SerializeField] private AudioSource WskillAudio;
    [SerializeField] private AudioSource EskillAudio;
    private void Awake()
    {
        if (this != Instance) Destroy(gameObject);
    }
    public static SkillMgr Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<SkillMgr>();
                if (!instance) instance = new GameObject("SkillManager").AddComponent<SkillMgr>();
                instance.Initialize();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;

        }
    }
    void Initialize()
    {
    }
    public void Skill(InputAction.CallbackContext context)
    {
        int var = context.ReadValue<int>();
        OnSkill(var);
    }
    public void OnQSkill(InputAction.CallbackContext context)
    {
        bool Q = context.ReadValueAsButton();
        if (Q)
        {
            OnSkill(0);
            AudioMgr.Instance.PlayAudio(QskillAudio);
        }
    }
    public void OnWSkill(InputAction.CallbackContext context)
    {
        bool W = context.ReadValueAsButton();
        if (W)
        {
            OnSkill(1);
            AudioMgr.Instance.PlayAudio(WskillAudio);
        }
    }
    public void OnESkill(InputAction.CallbackContext context)
    {
        bool E = context.ReadValueAsButton();
        if (E)
        {
            OnSkill(2);
            AudioMgr.Instance.PlayAudio(EskillAudio);
        }

    }
    private void OnSkill(int skill)
    {
        skills[skill].Press_Skill();
    }
}
