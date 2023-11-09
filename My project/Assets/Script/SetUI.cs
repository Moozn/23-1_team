using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUI : MonoBehaviour
{
    private void Awake()
    {
        Off();
    }
    public void On()
    {
        gameObject.SetActive(true);
    }
    public void Off()
    {
        gameObject.SetActive(false);
    }

    public void SetPosition(Vector3 target)
    {
        gameObject.transform.position = target;
    }
}
