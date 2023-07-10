using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatButton : MonoBehaviour
{
    [SerializeField] Player player;
    public void OnClick(int index)
    {
        player.Add_Stat(index);
    }
}
