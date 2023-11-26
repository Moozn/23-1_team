using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionWindows : MonoBehaviour
{
    [SerializeField] private SetUI option;
    [SerializeField] private ThirdPersonCamera camera;
    private bool isPaused = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
            OpenOptionsMenu();
        }
    }

    void OpenOptionsMenu()
    {
        option.On();
    }



    void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            PauseGame();
        }
    }

    void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        camera.Setsensitivity();
        isPaused = !isPaused;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }
}
