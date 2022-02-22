using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public Transform[] waypoints;
    public int speed;

    private int waypointIndex;
    private float dist;

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
            IncreaseIndex();
        }
       
        Patrol();
    }

    void Patrol()
    {
        // transform.Translate(Vector3.forward * speed * Time.deltaTime);
        agent.SetDestination(waypoints[waypointIndex].position);
        anim.SetTrigger("Walk_Cycle_1");
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
