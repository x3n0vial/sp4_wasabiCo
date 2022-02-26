using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieJumpscare : MonoBehaviour
{
    //player
    Transform target;

    //animator
    Animator anim;

    //boolean to check if jumpscare is triggered
    bool jumpscare = false;

    //timer to jumpscare
    float jumpscareTimer = 0;
    float maxJumpscareTimer = 0.3f;

    //timer back to idle
    float idle = 0;
    float maxIdle = 2.5f;

    //timer to jumpscare die, player dies if they step on zombie again
    float jumpscareDieTimer = 0;
    float maxJumpscareDieTimer = 0.3f;

    bool steppedOn = false;

    //death camera
    CameraSettings camera;

    //point of focus
    Transform focusPoint;

    //timer
    float timer = 0;

    //level loader
    LevelLoader levelLoad;

    //enemy audio
    public AudioClip attackSound;
    public AudioClip jumpscareSound;
    private AudioSource audioSource;

    float audioTimer = 0;

    //ui overlay
    GameObject UIOverlay;

    // Start is called before the first frame update
    void Start()
    {
        target = GameHandler.instance.player.transform;
        anim = GetComponentInParent<Animator>();

        focusPoint = transform.Find("FocusPoint");
        camera = GameHandler.instance.cameraSettings;
        levelLoad = GameHandler.instance.levelLoader;
        audioSource = gameObject.GetComponent<AudioSource>();
        UIOverlay = GameHandler.instance.UILayout;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == target)
        {
            jumpscare = true;
            if (idle > maxIdle)
            {
                steppedOn = true;
            }
        }
    }

    void Update()
    {   
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }


        if (steppedOn)
        {
            jumpscareDieTimer+=Time.deltaTime;
            if (jumpscareDieTimer > maxJumpscareDieTimer)
            {
                anim.ResetTrigger("Idle");
                anim.SetTrigger("JumpscareDie");

                //insert jumpscare kill player yes
                camera.ActivateDeathCam(focusPoint);
                target.gameObject.SetActive(false);
                transform.Find("light").gameObject.SetActive(true);

                timer += Time.deltaTime;
                //play a fadeout transition
                if (timer >= 3.1)
                {
                    levelLoad.LoadNextLevel(levelLoad.getSceneName());
                    CheckpointManager.ClearCheckpoints();
                }
            }
        }

        if (steppedOn && jumpscareDieTimer > maxJumpscareDieTimer)
        {
            audioSource.clip = jumpscareSound;
        }
        else if (jumpscare)
        {
            audioSource.clip = attackSound;
        }


        if (jumpscare)
        { 
            jumpscareTimer += Time.deltaTime;
            if (jumpscareTimer > maxJumpscareTimer)
            {
                anim.SetTrigger("Jumpscare");
                idle += Time.deltaTime;
                UIOverlay.SetActive(false);
                if (idle > maxIdle)
                {
                    anim.ResetTrigger("Jumpscare");
                    anim.SetTrigger("Idle");
                }
            }
        }
    }
}
