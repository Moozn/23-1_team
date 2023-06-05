using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class GameMgr : MonoBehaviour // �ٸ� Ŭ�������� ��ӹ��� �� ����
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
