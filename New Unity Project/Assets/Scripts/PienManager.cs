using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PienManager : MonoBehaviour
{
    public int noOfPien;
    public Text text_noOfPien;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();    
    }

    void Update()
    {
       text_noOfPien.text = noOfPien.ToString();

        if (getNoOfPien() <= 0)
            GameHandler.instance.levelLoader.LoadNextLevel("SceneCredits");
    }
    // Start is called before the first frame update
    public int getNoOfPien()
    {
        return noOfPien;
    }

    public void setNoOfPien(int n)
    {
        noOfPien = n;
    }

    public void subtractNoOfPien()
    {
        noOfPien--;
    }

    public void playAudio()
    {
        if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
