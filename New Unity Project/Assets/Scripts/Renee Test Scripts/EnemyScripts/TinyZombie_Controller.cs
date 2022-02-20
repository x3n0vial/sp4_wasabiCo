using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class TinyZombie_Controller : MonoBehaviour
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

    //how far the enemy follows the player
    float lookRadius;

    //enemy's agent
    NavMeshAgent agent;

    //enemy's animator
    Animator anim;

    //enemy's renderer
    Renderer renderer;

    //enemy's collider
    Collider collider;

    //array of materials for the enemy's materials
    Material[] mats;

    void Start() //initialise the variables
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        target = PlayerManager.instance.player.transform;
        flashlight = PlayerManager.instance.player.GetComponentInChildren<Flashlight>();
        agent.speed = 0.1f;
        renderer = GetComponentInChildren<Renderer>();
        collider = GetComponent<Collider>();
        mats = renderer.materials;
    }
    void ChangeSpeed() //speed of the enemy. Increases based on what stage the level is at
    {
        agent.speed = 0.6f * lightsCount;
    }
    void ChangeRadius() //radius of the radius the enemy is aware of. Increases based on what stage the level is at
    {
        lookRadius = 3f * lightsCount;
    }
    void FaceTarget() //changes direction to face the player 
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
    void RenderMaterials() //change the material color of the enemy
    {
        Color color;
        if (lightsCount == 0)
        {
            color = new Color32(64, 64, 64, 255); //grey
        }
        else
        {
            color = new Color32(255, 255, 255, 255); //white
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
    bool PlayerWithinViewDistance() //if player is within view distance of the enemy, return true
    {
        return (distance <= lookRadius);
    }
    void followPlayer() //follow the player
    {
        RaycastHit hit;
        if (Physics.Linecast(transform.position, target.position, out hit, -1)) //if behind wall, lose player
        {
            if (hit.transform.CompareTag("Player"))
            {
                agent.SetDestination(target.position);
                if (distance <= agent.stoppingDistance)
                {
                    FaceTarget();
                    agent.speed = 0;
                    //insert jumpscare kill player yes

                }
            }
        }
    }
    bool stunned() //stunned by player's flashlight
    {
        return (flashlight.CheckIfInFlashlight(collider));
    }
    void TinyZombieAnimation() //Animation controller for tinyZombie
    {
        if (distance <= agent.stoppingDistance || stunned())
        {
            anim.ResetTrigger("Walk");
            anim.ResetTrigger("Idle");
            anim.SetTrigger("Attack");
        }
        else if (PlayerWithinViewDistance())
        {
            anim.ResetTrigger("Attack");
            anim.ResetTrigger("Idle");
            anim.SetTrigger("Walk");
        }
        else if (lightsCount > 0)
        {
            anim.ResetTrigger("Walk");
            anim.ResetTrigger("Attack");
            anim.SetTrigger("Idle");
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
        RenderMaterials();

        distance = Vector3.Distance(target.position, transform.position);

        TinyZombieAnimation();
        if (!stunned())
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
