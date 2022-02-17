using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TinyZombie_Controller : MonoBehaviour
{
    public float lookRadius = 10f;

    Transform target;
    NavMeshAgent agent;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        target = PlayerManager.instance.player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);
            anim.SetBool("Idle", false);
            anim.SetBool("Walk", true);
            anim.SetBool("Attack", false);
            //anim.SetTrigger("Walk");
            if (distance <= agent.stoppingDistance)
            {
                FaceTarget();
                anim.SetBool("Attack", true);
                anim.SetBool("Walk", false);
                anim.SetBool("Idle", false);
            }
        }
        else
        {
            //anim.SetTrigger("Idle");
            anim.SetBool("Walk", false);
            anim.SetBool("Idle", true);
            anim.SetBool("Attack", false);
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
