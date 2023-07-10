using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class GameMgr : MonoBehaviour // 다른 클래스에서 상속받을 수 없음
{
    static GameMgr instance = null;
    public static GameMgr Instance
    {
        get
        {
            if(!instance)
            {
                instance = FindObjectOfType<GameMgr>();
                if (!instance) instance = new GameObject("GmaeManagr").AddComponent<GameMgr>();

                instance.Initialize();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    PlayerInput input;
    [SerializeField] Player player;

    private void Awake()
    {
        if (this != Instance) Destroy(gameObject);
    }

    void Initialize() 
    {
    }
    void StartGame() { }
}
