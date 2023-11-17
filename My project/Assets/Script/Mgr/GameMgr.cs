using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameMgr : MonoBehaviour
{
    public static GameMgr instance = null;

    public static GameMgr Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<GameMgr>();
                if (!instance) instance = new GameObject("GameManager").AddComponent<GameMgr>();
                instance.Initialize();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }
    private void Awake() { if (this != Instance) Destroy(gameObject); }
    void Initialize() { }// EnemyTest(); } // �׽�Ʈ �� ���� ����!!
    public void SceneChange(int SceneNum)
    {
        switch (SceneNum)
        {
            case 0:
                SceneManager.LoadScene("GameScene");
                break;
            case 1:
                SceneManager.LoadScene("TitleScene");
                break;
        }

    }
}
