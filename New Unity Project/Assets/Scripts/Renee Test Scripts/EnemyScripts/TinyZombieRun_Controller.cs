using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TinyZombieRun_Controller : MonoBehaviour
{
    //player
    Transform target;

    //player distance from enemy
    float distance;

    //handler of flashlight
    Flashlight flashlight;

    //how far the enemy follows the player
    float lookRadius = 10f;

    //enemy's agent
    NavMeshAgent agent;

    //enemy's animator
    Animator anim;

    //enemy's collider
    Collider collider;

    //enemy's trigger
    public TinyZombieTrigger trigger;
  
    //death camera
    CameraSettings camera;

    //point of focus
    Transform focusPoint;

    void Start() //initialise the variables
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        target = GameHandler.instance.player.transform;
        flashlight = GameHandler.instance.flashlight;
        agent.speed = 0.1f;
        collider = GetComponent<Collider>();
        agent.speed = 6f;

        focusPoint = transform.Find("FocusPoint");
        camera = GameHandler.instance.cameraSettings;
    }
    void FaceTarget() //changes direction to face the player 
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
    private void OnDrawGizmosSelected() //draws the sphere of the radius of where the enemy can see
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
    bool PlayerWithinViewDistance() //if player is within view distance of the enemy, return true
    {
        return (distance <= lookRadius);
    }
    void followPlayer() //follow the player
    {
        RaycastHit hit;
        if (Physics.Linecast(transform.position, target.position, out hit, -1)) //if behind wall, lose player
        {
            if (hit.transform.CompareTag("Player"))
            {
                agent.SetDestination(target.position);
                if (distance <= agent.stoppingDistance)
                {
                    FaceTarget();
                    agent.speed = 0;

                    //insert jumpscare kill player yes
                    camera.ActivateDeathCam(focusPoint);
                    target.gameObject.SetActive(false);
                }
            }
        }
    }
    bool stunned() //stunned by player's flashlight
    {
        return (flashlight.CheckIfInFlashlight(collider));
    }
    void TinyZombieAnimation() //Animation controller for tinyZombie
    {
        if (distance <= agent.stoppingDistance)
        {
            anim.ResetTrigger("Walk");
            anim.ResetTrigger("Idle");
            anim.SetTrigger("Attack");
        }
        else if (PlayerWithinViewDistance())
        {
            anim.ResetTrigger("Attack");
            anim.ResetTrigger("Idle");
            anim.SetTrigger("Walk");
        }
        else
        {
            anim.ResetTrigger("Walk");
            anim.ResetTrigger("Attack");
            anim.SetTrigger("Idle");
        }
    }
    void Update()
    {
        distance = Vector3.Distance(target.position, transform.position);

        TinyZombieAnimation();
        if (stunned() && trigger.getTrigger())
        {
            //play spooky sound effect
            transform.gameObject.SetActive(false);
        }

        if (!stunned() && trigger.getTrigger())
        {
            agent.speed = 6f;
            if (PlayerWithinViewDistance()) //turn this into an else if statement
            {
                followPlayer();
            }
            else
            {
                agent.speed = 0;
            }
        }
        else
        {
            agent.speed = 0;
        }
    }
}
