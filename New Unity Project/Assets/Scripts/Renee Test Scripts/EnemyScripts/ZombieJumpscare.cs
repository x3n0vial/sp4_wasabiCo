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
    int maxIdle = 250;

    //timer to jumpscare die, player dies if they step on zombie again
    int jumpscareDieTimer = 0;
    int maxJumpscareDieTimer = 50;

    int steppedOn = 0;

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        anim = GetComponentInParent<Animator>();
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

            }
        }
    }
}