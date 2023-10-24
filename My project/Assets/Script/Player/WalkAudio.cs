using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAudio : MonoBehaviour
{
    private bool _ground;
    public bool ground { get { return _ground; } }
    [SerializeField] private AudioSource walkAudio;
    private void Start()
    {
        _ground = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            //walkAudio.Play();
            AudioMgr.instance.PlayAudio(walkAudio);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground") _ground = false;
    }
}
