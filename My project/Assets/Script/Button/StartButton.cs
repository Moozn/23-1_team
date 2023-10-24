using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Monster monster;
    [SerializeField] private SetUI infoUi;
    public void OnClick()
    {
        monster.Activation();
        player.restart();
        infoUi.Off();
    }
}
