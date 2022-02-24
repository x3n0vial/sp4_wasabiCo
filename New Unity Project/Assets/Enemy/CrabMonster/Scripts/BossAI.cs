//Nicole
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    public Transform[] waypoints;
    public int speed;

    private int waypointIndex;
    private float dist, distToPlayer;
    private double restTime = 1.5f, stunTime = 0f, flashTime = 1f;
    private float stunMeter = 0f, stunRate = 0.5f;

    public NavMeshAgent agent;
    public Animator anim;
    public Collider collider;

    public GameObject Player;
    public Flashlight flashlight;

    public float angleOfView, rangeofView;

    LayerMask wallLayer;

    // Start is called before the first frame update
    void Start()
    {
        waypointIndex = 0;
        transform.LookAt(waypoints[waypointIndex].position);
        wallLayer = LayerMask.GetMask("Wall");
        //anim = GetComponent<Animator>();
        //agent = GetComponent<NavMeshAgent>();
        //collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector3.Distance(transform.position, waypoints[waypointIndex].position);
        distToPlayer = Vector3.Distance(transform.position, Player.transform.position);

        if (distToPlayer > 10f)
            Patrol();

        if (dist<1f)
        {
            if (restTime <= 0f)
            {
                IncreaseIndex();
                restTime = 1.5f;
                anim.SetBool("Patrol", true);
                anim.SetBool("Rest", false);
            }
            else
            {
                // rest for a while before moving to next waypoint
                restTime -= Time.deltaTime;
                anim.SetBool("Rest", true);
                anim.SetBool("Patrol", false);
            }
        }

        if (distToPlayer < 10f && distToPlayer > 3f)
        {
            if (canSeePlayer())
            {
                // if enemy can see player
                Follow();
            }
            else
            {
                // continue patrol if cannot see player
                anim.SetBool("Follow", false);
                Patrol();
            }
        }
        else
        {
            anim.SetBool("Follow", false);
        }

        if (distToPlayer < 3f)
        {
            // attack player if within range
            Attack();
        }
        else
        {
            anim.SetBool("Attack", false);
        }



        if (isInFlashlight())
        {
            //Stun();
            if(stunMeter<1f)
            {
                stunMeter += stunRate * Time.deltaTime;
                //anim.SetBool("Stun", false);
                //Debug.Log(stunMeter);
            }
            else if (stunMeter>=0f)
            {
                stunTime = 2f;
                stunMeter = 0f;

            }
        }
        else
        {
            if (stunMeter > 0f)
            {
                stunMeter -= stunRate * Time.deltaTime * 2;
            }
               
        }

        if (stunTime <= 2f && stunTime >= 0f)
        {
            anim.SetBool("Stun", true);
            stunTime -= Time.deltaTime;
            agent.speed = 0;
            Debug.Log(stunTime);
        }
        else if (stunTime <= 0f) 
        {
            anim.SetBool("Stun", false);
        }
    }
    
    bool isInFlashlight()
    {
        //if(stunTime>0f)
        {
            if (flashlight.CheckIfInFlashlight(collider))
            {
                return true;
            }
        }
        return false;
    }

    void Stun()
    {
        agent.speed = 0;
        stunTime -= Time.deltaTime;
        anim.SetBool("Stun", true);
        // anim.SetBool("Patrol", false);
        // Debug.Log("Stunned");
        // anim.SetBool("Attack", false);
    }

    bool canSeePlayer()
    {
        Vector3 dirToPlayer = Player.transform.position - transform.position;
        if (Vector3.Angle(transform.forward, dirToPlayer) <= angleOfView/2)
        {
            if (Physics.Raycast(transform.position, dirToPlayer, out RaycastHit hit, rangeofView))
            {
                if (hit.collider.tag=="Player")
                {
                    return true;                
                }

            }
        }
        return false;
    }

    void Attack()
    {
        anim.SetBool("Attack", true);
    }

    void Follow()
    {
        agent.SetDestination(Player.transform.position);
        anim.SetBool("Follow", true);
        anim.SetBool("Patrol", false);
        anim.SetBool("Attack", false);
        agent.speed = 3.5f;
    }

    void Patrol()
    {
        // transform.Translate(Vector3.forward * speed * Time.deltaTime);
        agent.SetDestination(waypoints[waypointIndex].position);
        agent.speed = 2.0f;
    }

    void Die()
    {
        anim.SetBool("Die", true);
    }

    void IncreaseIndex()
    {
        waypointIndex++;
        if(waypointIndex>=waypoints.Length)
        {
            waypointIndex = 0;
        }
        transform.LookAt(waypoints[waypointIndex].position);
    }
}
