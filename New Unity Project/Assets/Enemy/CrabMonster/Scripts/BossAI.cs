using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public Transform[] waypoints;
    public int speed;

    private int waypointIndex;
    private float dist;
    private double restTime = 1.5f;

    public UnityEngine.AI.NavMeshAgent agent;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        waypointIndex = 0;
        transform.LookAt(waypoints[waypointIndex].position);
        //agent = this.GetComponent<NavMeshAGent>;
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
       
        Patrol();
    }

    void Patrol()
    {
        // transform.Translate(Vector3.forward * speed * Time.deltaTime);
        agent.SetDestination(waypoints[waypointIndex].position);
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
