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
      //  MonsterSpawner.Instance.GetList();
        monster.Activation();
        player.restart();
        infoUi.Off();
    }

    public void InTitle()
    {
        GameMgr.Instance.SceneChange(1);
    }

}
