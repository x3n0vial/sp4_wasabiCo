using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TinyZombie_Controller : MonoBehaviour
{
    float lookRadius;
    Transform target;
    NavMeshAgent agent;
    Animator anim;
    int lightsCount = 0;
    public SceneLightsManager lights;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        target = PlayerManager.instance.player.transform;
        agent.speed = 0.1f;
    }

    void ChangeSpeed()
    {
        agent.speed = 0.2f * lightsCount;
    }

    void ChangeRadius()
    {
        lookRadius = 2f * lightsCount;
    }

    // Update is called once per frame
    void Update()
    {
        lightsCount = lights.GetCurrentStage();
        if (lightsCount > 4)
        {
            lightsCount = 4;
        }
        ChangeSpeed();
        ChangeRadius();
        if (lights.GetCurrentStage() == 0)
        {
            anim.SetBool("Idle", true);
        }
        else
        {
            float distance = Vector3.Distance(target.position, transform.position);

            if (distance <= lookRadius)
            {
                agent.SetDestination(target.position);
                anim.SetBool("Idle", false);
                anim.SetBool("Walk", true);
                anim.SetBool("Attack", false);
                //anim.SetTrigger("Walk");
                if (lights.GetCurrentStage() > 2) //only start to attack player when more active
                {
                    if (distance <= agent.stoppingDistance)
                    {
                        FaceTarget();
                        anim.SetBool("Attack", true);
                        anim.SetBool("Walk", false);
                        anim.SetBool("Idle", false);
                        //play the jumpscare

                    }
                }
            }
            else
            {
                //anim.SetTrigger("Idle");
                anim.SetBool("Walk", false);
                anim.SetBool("Idle", true);
                anim.SetBool("Attack", false);
                agent.speed = 0;
            }
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
