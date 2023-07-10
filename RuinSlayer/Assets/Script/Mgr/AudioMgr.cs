using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMgr : MonoBehaviour
{
    public static AudioMgr instance = null;
    private float volume;
    private float bgm;
    private AudioSource backgroundAudio;
    private void Awake()
    {
        if (this != Instance) Destroy(gameObject);
        backgroundAudio = GetComponent<AudioSource>();
    }
    public static AudioMgr Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<AudioMgr>();
                if (!instance) instance = new GameObject("AudioManager").AddComponent<AudioMgr>();
                instance.Initialize();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;

        }
    }
    void Initialize()
    {
        volume = 1;
    }
    public void VolumeUpdate(float _volume, float _bgm) //º¼·ý Á¶Àý
    {
        this.volume = _volume;
        this.bgm = _bgm;
        backgroundAudio.volume = bgm;
    }
    public void BGMUpdate(float _volume)
    {
        this.bgm = _volume;
    }
    public void PlayAudio(AudioSource audio)
    {
        audio.volume = volume;
        audio.Play();
    }
}
