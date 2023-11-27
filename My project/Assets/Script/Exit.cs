using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    [SerializeField] Player player;
    public void OnTitleExit()
    {
        SaveLoadMgr.instance.Save(player.GetStat());
        GameMgr.instance.SceneChange(1);
    }

    public void OnGameExitClick()
    {
        SaveLoadMgr.instance.Save(player.GetStat());
        GameMgr.instance.OnGameExitClick();
    }
}
