using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    public string build;
    public PauseMenu pausemenu;
    string SceneName;
    
    void Update()
    {   
        //if (Input.GetKeyDown("0"))
        //{
        //    build = 0;
        //    LoadNextLevel();
        //}
    }
    
    public void LoadNextLevel(string sceneName)
    {
        build = sceneName;
        StartCoroutine(LoadLevel());
    }
    public string getSceneName()
    {
        SceneName = SceneManager.GetActiveScene().name;
        return SceneName;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadLastSavedLevel()
    {
        if (CheckpointManager.last_ID < CheckpointID.SCENE_NIGHTMARE_START)
            build = "ForestScene";
        else if (CheckpointManager.last_ID < CheckpointID.SCENE_BOSS_START)
            build = "SceneNightmare";
        else
            build = "SceneBossRoom";

        SceneManager.LoadScene(build);
        CheckpointManager.ClearCheckpoints();
    }

    IEnumerator LoadLevel()
    {
        //play anim
        transition.SetTrigger("Start");
        //wait
        yield return new WaitForSeconds(transitionTime);
        //load scene
        SceneManager.LoadScene(build);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
