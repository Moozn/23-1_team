using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectMgr : MonoBehaviour
{
    public static EffectMgr instance = null;
    private void Awake()
    {
        if (this != Instance) Destroy(gameObject);
    }
    public static EffectMgr Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<EffectMgr>();
                if (!instance) instance = new GameObject("EffectManager").AddComponent<EffectMgr>();
                instance.Initialize();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;

        }
    }
    void Initialize()
    {
    }
    public IEnumerator PlayEffect(ParticleSystem particle,float time)
    {
        particle.Play();
        yield return new WaitForSeconds(time);
        particle.Stop();
    }
}
