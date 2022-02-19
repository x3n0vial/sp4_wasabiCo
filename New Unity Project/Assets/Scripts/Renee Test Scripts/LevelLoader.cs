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

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
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
}
