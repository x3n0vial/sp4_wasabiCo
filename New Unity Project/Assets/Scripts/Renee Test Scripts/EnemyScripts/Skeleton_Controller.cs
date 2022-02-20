using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Skeleton_Controller : MonoBehaviour
{
    //player
    Transform target;

    //player distance from enemy
    float distance;

    //handler of lights
    public SceneLightsManager lights;
    int lightsCount = 0;

    //handler of flashlight
    Flashlight flashlight;

    //Enemy Waypoints
    public Transform[] waypoints;
    int m_CurrentWaypointIndex;

    //What the enemy sees infront of them
    public Enemy_POV enemyPOV;

    //how far the enemy follows the player
    float lookRadius;

    //when the enemy starts running at the player
    float chaseRadius;
    //the speed the enemy follows the player
    float chaseSpeed;

    //enemy idle time before continuing
    int rest;
    int restMax = 800;

    //enemy's agent
    NavMeshAgent agent;

    //enemy's animator
    Animator anim;

    //enemy's collider
    Collider collider;

    void Start() //initialise the variables
    {
        chaseSpeed = 1;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        target = PlayerManager.instance.player.transform;
        flashlight = PlayerManager.instance.player.GetComponentInChildren<Flashlight>();
        collider = GetComponent<Collider>();
        agent.speed = 0.1f;
        agent.SetDestination(waypoints[0].position);
    }

    void ChangeSpeed() //speed of the enemy. Increases based on what stage the level is at and the chase speed
    {
        agent.speed = 0.6f * chaseSpeed * lightsCount;
    }
    void ChangeRadius() //radius of the radius the enemy is aware of
    {
        lookRadius = 3f * lightsCount;
        chaseRadius = 1.2f * lightsCount;
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
    bool PlayerWithinChaseRadius() //if player is within the running radius of the enemy, return true
    {
        return (distance <= chaseRadius);
    }
    void followPlayer() //follow the player 
    {
        RaycastHit hit;
        if (Physics.Linecast(transform.position, target.position, out hit, -1)) //if behind wall, lose player
        {
            if (hit.transform.CompareTag("Player"))
            {
                agent.SetDestination(target.position);
                if (PlayerWithinChaseRadius())
                {
                    chaseSpeed = 1.5f;
                    if (distance <= agent.stoppingDistance)
                    {
                        FaceTarget();
                        agent.speed = 0;
                        //insert jumpscare kill player yes

                    }
                }
                else
                {
                    chaseSpeed = 1;
                }
            }
            else
            {
                enemyPOV.SetPlayerInView(false);
                followWaypoint();
            }
        }
    }
    void followWaypoint() //follow the waypoints
    {
        if (agent.remainingDistance < agent.stoppingDistance)
        {
            rest++;
            if (rest >= restMax)
            {
                m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
                agent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                rest = 0;
            }
        }
    }
    bool stunned() //stunned by player's flashlight
    {
        return (flashlight.CheckIfInFlashlight(collider));
    }
    void SkeletonAnimation() //Animation controller for the skeleton
    {
        if (distance <= agent.stoppingDistance || stunned())
        { //plays attack
            anim.ResetTrigger("Run");
            anim.ResetTrigger("Walk");
            anim.ResetTrigger("Idle");
            anim.SetTrigger("Attack");
        }
        else if (PlayerWithinChaseRadius() && enemyPOV.GetPlayerInView())
        { //plays run
            anim.ResetTrigger("Attack");
            anim.ResetTrigger("Walk");
            anim.ResetTrigger("Idle");
            anim.SetTrigger("Run");
        }
        else if ((agent.remainingDistance < agent.stoppingDistance))
        { //plays idle
            anim.ResetTrigger("Attack");
            anim.ResetTrigger("Walk");
            anim.ResetTrigger("Run");
            anim.SetTrigger("Idle");
        }
        else
        { //plays walk
            anim.ResetTrigger("Attack");
            anim.ResetTrigger("Run");
            anim.ResetTrigger("Idle");
            anim.SetTrigger("Walk");
        }
    }
    void Update()
    {
        lightsCount = lights.GetCurrentStage();
        if (lightsCount > 4)
        {
            lightsCount = 4;
        }
        ChangeSpeed();
        ChangeRadius();

        distance = Vector3.Distance(target.position, transform.position);
        Debug.Log("test"+stunned());
        SkeletonAnimation();

        if (!stunned())
        {
            if (PlayerWithinViewDistance() && enemyPOV.GetPlayerInView()) //turn this into a else if statement
            {
                followPlayer();
            }
            else
            {
                followWaypoint();
            }
        }
        else
        {
            agent.speed = 0;
        }
    }
}
