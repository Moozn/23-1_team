using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameMgr : MonoBehaviour
{
    public static GameMgr instance = null;
    private float mouse_sensitivity; // 마우스 민감도
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
    void Initialize() {
        mouse_sensitivity = 0.5f;
    }// EnemyTest(); } // 테스트 후 삭제 예정!!
    public void Set_sensitivity(float sensitivity)
    {
        mouse_sensitivity = sensitivity;
    }

    public void OnGameExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
    public float sensitivity() { return mouse_sensitivity; }
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
