using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GamePaused = false;
    public GameObject pauseMenuUI;
    public LevelLoader levelLoader;
    public string menuScene;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        GameSettings.CAMERA_MOUSEPAN_ENABLED = true;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
        Cursor.visible = false;
    }

    void Pause()
    {
        GameSettings.CAMERA_MOUSEPAN_ENABLED = false;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
        Cursor.visible = true;
    }

    public void LoadLevel()
    {
        levelLoader.LoadNextLevel(menuScene);
        Resume();
    }
}
