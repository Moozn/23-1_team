using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    [SerializeField] SetUI menu;

    private void Start()
    {
        if (menu != null) menu.On();
    }

    public void OnMenuOn()
    {
        menu.On();
    }

    public void OnMenuOff()
    {
        menu.Off();
    }
    public void InGame()
    {
        GameMgr.Instance.SceneChange(0);
    }
    public void OnGameExitClick()
    {
        GameMgr.instance.OnGameExitClick();
    }
}
