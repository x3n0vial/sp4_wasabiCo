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
    int jumpscareTimer = 0;
    int maxJumpscareTimer = 50;

    //timer back to idle
    int idle = 0;
    int maxIdle = 200;

    //timer to jumpscare die, player dies if they step on zombie again
    int jumpscareDieTimer = 0;
    int maxJumpscareDieTimer = 50;

    int steppedOn = 0;

    //death camera
    CameraSettings camera;

    //point of focus
    Transform focusPoint;

    //timer
    float timer = 0;

    //level loader
    LevelLoader levelLoad;

    // Start is called before the first frame update
    void Start()
    {
        target = GameHandler.instance.player.transform;
        anim = GetComponentInParent<Animator>();

        focusPoint = transform.Find("FocusPoint");
        camera = GameHandler.instance.cameraSettings;

        levelLoad = GameHandler.instance.levelLoader;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == target)
        {
            jumpscare = true;
            steppedOn++;
        }
    }

    void Update()
    {
        if (jumpscare)
        {
            jumpscareTimer++;
            if (jumpscareTimer > maxJumpscareTimer)
            {
                anim.SetTrigger("Jumpscare");
                idle++;
                if (idle > maxIdle)
                {
                    anim.ResetTrigger("Jumpscare");
                    anim.SetTrigger("Idle");   
                }
            }
        }

        if (steppedOn > 1)
        {
            jumpscareDieTimer++;
            if (jumpscareDieTimer > maxJumpscareDieTimer)
            {
                anim.ResetTrigger("Idle");
                anim.SetTrigger("JumpscareDie"); 
                
                //insert jumpscare kill player yes
                camera.ActivateDeathCam(focusPoint);
                target.gameObject.SetActive(false);

                timer += Time.deltaTime;
                if (timer >= 3)
                {
                    //play a fadeout transition
                    if (timer >= 3.1)
                    {
                        target.gameObject.SetActive(true);
                        target.position = GameSettings.currentCheckpoint.spawnPos;
                        levelLoad.LoadScene(levelLoad.getSceneName());
                    }
                }
            }
        }
    }
}
