using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmManager : MonoBehaviour
{
    public AudioClip bgm1;
    public AudioClip bgm2;
    private AudioSource audioSource;

    public float fadeTime = 1.0f;

    public int count = 1;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = bgm1;
        audioSource.volume = 0.5f;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchBGM1()
    {
        FadeOut();
        audioSource.clip = bgm1;
        audioSource.Play();
    }
    public void SwitchBGM2()
    {
        FadeOut();
        audioSource.clip = bgm2;
        audioSource.Play();
    }

    public void ForestBGM()
    {
        if (count == 1)
        {
            FadeOut();
            count++;
            return;
        }
        else if (count == 2)
        {
            audioSource.clip = bgm2;
            audioSource.Play();
            count++;
            return;
        }
        else
            return;
    }

    public void FadeOut()
    {
        if (fadeTime == 0)
        {
            audioSource.volume = 0;
            return;
        }
        StartCoroutine(_FadeOut());
    }

    IEnumerator _FadeOut()
    {
        float t = fadeTime;
        while (t > 0)
        {
            yield return null;
            t -= Time.deltaTime;
            audioSource.volume = t / fadeTime;
        }
        audioSource.Stop();
        audioSource.volume = 0.5f;
        yield break;
    }
}
