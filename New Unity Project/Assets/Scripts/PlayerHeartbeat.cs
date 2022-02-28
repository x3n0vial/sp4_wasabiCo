using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeartbeat : MonoBehaviour
{
    bool trigger = false;

    //audio
    public AudioClip heartBeat;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "TinyZombie" ||
            other.transform.tag == "Zombie" ||
            other.transform.tag == "BigZombie" ||
            other.transform.tag == "Skeleton")
        {
            trigger = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "TinyZombie" || 
            other.transform.tag == "Zombie" ||
            other.transform.tag == "BigZombie" ||
            other.transform.tag == "Skeleton")
        {
            trigger = false;
        }
    }

    public void Update()
    {
        if (trigger)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }

            //play heartbeat sensor
            audioSource.clip = heartBeat;
        }
        else
        {
            audioSource.Pause();
        }
    }
}
