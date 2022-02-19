using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Skeleton_Controller : MonoBehaviour
{
    float lookRadius;
    float chaseRadius;
    float chaseSpeed;

    int rest;
    int restMax = 800;


    Transform target;
    NavMeshAgent agent;
    Animator anim;
    int lightsCount = 0;
    public SceneLightsManager lights;

    float distance;

    public Transform[] waypoints;
    int m_CurrentWaypointIndex;

    // Start is called before the first frame update
    void Start()
    {
        chaseSpeed = 1;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        target = PlayerManager.instance.player.transform;
        agent.speed = 0.1f;
        agent.SetDestination(waypoints[0].position);
    }

    void ChangeSpeed() //speed of the enemy
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

        SkeletonAnimation();

        if (PlayerWithinViewDistance())
        {
            followPlayer();            
        }
        else {
            followWaypoint();
        }
    }

    bool PlayerWithinViewDistance()
    {
        return (distance <= lookRadius);
    }
    bool PlayerWithinChaseRadius()
    {
        return (distance <= chaseRadius);
    }
    void followPlayer()
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
                        //play the jumpscare

                    }
                }
                else
                {
                    chaseSpeed = 1;
                }
            }
            else
            {
                followWaypoint();
            }
        }
    }
    void followWaypoint()
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
    void SkeletonAnimation()
    {
        if (distance <= agent.stoppingDistance)
        { //plays attack
            anim.ResetTrigger("Attack");
            anim.ResetTrigger("Walk");
            anim.ResetTrigger("Idle");
            anim.SetTrigger("Attack");
        }
        else if (PlayerWithinChaseRadius())
        { //plays run
            anim.ResetTrigger("Attack");
            anim.ResetTrigger("Walk");
            anim.ResetTrigger("Idle");
            anim.SetTrigger("Run");
        }
        else if (agent.remainingDistance < agent.stoppingDistance)
        { //plays idle
            anim.ResetTrigger("Attack");
            anim.ResetTrigger("Walk");
            anim.ResetTrigger("Idle");
            anim.SetTrigger("Idle");
        }
        else
        { //plays walk
            anim.ResetTrigger("Attack");
            anim.ResetTrigger("Walk");
            anim.ResetTrigger("Idle");
            anim.SetTrigger("Walk");
        }
    }

}
