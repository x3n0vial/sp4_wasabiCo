using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie_Controller : MonoBehaviour
{
    //player
    Transform target;

    //player distance from enemy
    float distance;

    ////handler of flashlight
    //Flashlight flashlight;

    //how far the enemy follows the player
    float lookRadius = 100;

    //enemy's agent
    NavMeshAgent agent;

    //enemy's animator
    Animator anim;

    //enemy's collider
    Collider collider;

    //handler of trigger
    public ZombieGetUp trigger;

    void Start() //initialise the variables
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        target = GameHandler.instance.player.transform;
        //flashlight = PlayerManager.instance.player.GetComponentInChildren<Flashlight>();
        agent.speed = 2.32f;
        collider = GetComponent<Collider>();
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
        agent.SetDestination(target.position);
        if (distance <= agent.stoppingDistance)
        {
            FaceTarget();
            agent.speed = 0;
            //insert jumpscare kill player yes

        }
        //RaycastHit hit;
        //if (Physics.Linecast(transform.position, target.position, out hit, -1)) //if behind wall, lose player
        //{
        //    if (hit.transform.CompareTag("Player"))
        //    {
        //        agent.SetDestination(target.position);
        //        if (distance <= agent.stoppingDistance)
        //        {
        //            FaceTarget();
        //            agent.speed = 0;
        //            //insert jumpscare kill player yes
        //
        //        }
        //    }
        //}
    }
    //bool stunned() //stunned by player's flashlight
    //{
    //    return (flashlight.CheckIfInFlashlight(collider));
    //}
    void ZombieAnimation() //Animation controller for tinyZombie
    {
        if (trigger.GetTriggerStatus())
        {
            if (distance <= agent.stoppingDistance)
            {
                anim.ResetTrigger("Idle");
                anim.ResetTrigger("Run");
                anim.SetTrigger("Attack");
            }
            else if (PlayerWithinViewDistance())
            {
                anim.ResetTrigger("Idle");
                anim.ResetTrigger("Attack");
                anim.SetTrigger("Run");
            }
        }
        else
        {
            anim.ResetTrigger("Run");
            anim.ResetTrigger("Attack");
            anim.SetTrigger("Idle");
        }
    }
        void Update()
    {
        distance = Vector3.Distance(target.position, transform.position);

        ZombieAnimation();
        agent.speed = 2.32f;
        if (trigger.GetTriggerStatus())
        {
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
