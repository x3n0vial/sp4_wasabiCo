using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class TinyZombie_Controller : MonoBehaviour
{
    float lookRadius;
    Transform target;
    NavMeshAgent agent;
    Animator anim;
    int lightsCount = 0;
    public SceneLightsManager lights;
    Renderer renderer;
    Material[] mats;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        target = PlayerManager.instance.player.transform;
        agent.speed = 0.1f;
        renderer = GetComponentInChildren<Renderer>();
        mats = renderer.materials;
    }

    void ChangeSpeed() //speed of the enemy
    {
        agent.speed = 0.6f * lightsCount;
    }

    void ChangeRadius() //radius of the radius the enemy is aware of
    {
        lookRadius = 3f * lightsCount;
    }
    void FaceTarget() //changes direction to face the player 
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
    void RenderMaterials()
    {
        Color color;
        if (lightsCount == 0)
        {
            color = new Color32(64, 64, 64, 255);
        }
        else
        {
            color = new Color32(255, 255, 255, 255);
        }
        for (int i = 0; i < mats.Length; i++)
        {
            mats[i].SetColor("_Color", color);
        }
    }

    private void OnDrawGizmosSelected() //draws the sphere of the radius of where the enemy can see
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
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
        RenderMaterials();
        if (lights.GetCurrentStage() == 0)
        {
            anim.SetBool("Idle", false);
            
        }
        else
        {
            float distance = Vector3.Distance(target.position, transform.position);

            if (distance <= lookRadius)
            {
                RaycastHit hit;
                if (Physics.Linecast(transform.position, target.position, out hit, -1)) //if behind wall, lose player
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        agent.SetDestination(target.position);
                        anim.SetBool("Idle", false);
                        anim.SetBool("Walk", true);
                        anim.SetBool("Attack", false);
                        if (lights.GetCurrentStage() > 2) //only start to attack player when more active
                        {
                            if (distance <= agent.stoppingDistance)
                            {
                                FaceTarget();
                                anim.SetBool("Attack", true);
                                anim.SetBool("Walk", false);
                                anim.SetBool("Idle", false);
                                agent.speed = 0;
                                //play the jumpscare

                            }
                        }
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

}
