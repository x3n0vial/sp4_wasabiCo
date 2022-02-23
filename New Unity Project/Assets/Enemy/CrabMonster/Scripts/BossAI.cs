using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public Transform[] waypoints;
    public int speed;

    private int waypointIndex;
    private float dist, distToPlayer;
    private double restTime = 1.5f, attackTime = 0f;

    public UnityEngine.AI.NavMeshAgent agent;
    public Animator anim;
    public GameObject Player;

    public float angleOfView, rangeofView;

    LayerMask wallLayer;

    // Start is called before the first frame update
    void Start()
    {
        waypointIndex = 0;
        transform.LookAt(waypoints[waypointIndex].position);
        wallLayer = LayerMask.GetMask("Wall");
    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector3.Distance(transform.position, waypoints[waypointIndex].position);
        if(dist<1f)
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
                restTime -= Time.deltaTime;
                anim.SetBool("Rest", true);
                anim.SetBool("Patrol", false);
            }
        }

        //Vector3 dir = transform.rotation * Vector3.forward;
        //float theta = Mathf.Acos(Vector3.Dot(Player.transform.position - transform.position, dir) / ((Player.transform.position - transform.position).magnitude * dir.magnitude));
        //RaycastHit hit;
        distToPlayer = Vector3.Distance(transform.position, Player.transform.position);
        if (distToPlayer < 10f && distToPlayer > 5f)
        {
            if (canSeePlayer())
            {
                Follow();
            }
            else
            {
                anim.SetBool("Follow", false);
                Patrol();
            }
        }
        else
        {
            anim.SetBool("Follow", false);
        }

        if (distToPlayer<5f)

        {
                Attack();
        }
        else
        {
            anim.SetBool("Attack", false);
        }

        if (distToPlayer > 10f)
            Patrol();
    }
    
    bool canSeePlayer()
    {
        Vector3 dirToPlayer = Player.transform.position - transform.position;
        if (Vector3.Angle(transform.forward, dirToPlayer) <= angleOfView/2)
        {
            if (Physics.Raycast(transform.position, dirToPlayer, out RaycastHit hit, rangeofView))
            {
                if (hit.collider.tag=="Player")
                    return true;
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
    }

    void Patrol()
    {
        // transform.Translate(Vector3.forward * speed * Time.deltaTime);
        agent.SetDestination(waypoints[waypointIndex].position);
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
