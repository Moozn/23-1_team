using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionWindows : MonoBehaviour
{
    [SerializeField] private SetUI optionUi;
    [SerializeField] private ThirdPersonCamera camera;
    [SerializeField] private Option option;
    private bool isPaused = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
           
        }
      
    }

    void OpenOptionsMenu()
    {
        optionUi.On();
    }



    void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            option.OnExitClick();
            ResumeGame();
        }
    }

    void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        OpenOptionsMenu();
        Time.timeScale = 0f;
    }

    public void ResumeGame(bool title = true)
    {
        camera.Setsensitivity();
        if (title)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        Time.timeScale = 1f;
    }
}
