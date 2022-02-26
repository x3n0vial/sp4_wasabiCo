using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Zombie_Controller : MonoBehaviour
{
    //player
    Transform target;

    //player distance from enemy
    float distance;

    ////handler of flashlight
    //Flashlight flashlight;

    //how far the enemy follows the player
    float lookRadius = 100;

    //enemy's agent
    NavMeshAgent agent;

    //enemy's animator
    Animator anim;

    //enemy's collider
    Collider collider;

    //handler of trigger
    public ZombieGetUp trigger;

    //death camera
    CameraSettings camera;

    //point of focus
    Transform focusPoint;

    //timer
    float timer = 0;

    //level loader
    LevelLoader levelLoad;

    //jumpscare boolean
    bool jumpscare = false;

    //enemy direction
    Vector3 direction;

    //enemy audio
    public AudioClip attackSound;
    public AudioClip jumpscareSound;
    private AudioSource audioSource;

    //ui overlay
    GameObject UIOverlay;

    void Start() //initialise the variables
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        target = GameHandler.instance.player.transform;
        //flashlight = GameHandler.instance.player.GetComponentInChildren<Flashlight>();
        agent.speed = 2.32f;
        collider = GetComponent<Collider>();
        focusPoint = transform.Find("FocusPoint");
        camera = GameHandler.instance.cameraSettings;
        levelLoad = GameHandler.instance.levelLoader;
        audioSource = gameObject.GetComponent<AudioSource>();
        UIOverlay = GameHandler.instance.UILayout;
    }
    void FaceTarget() //changes direction to face the player 
    {
        direction = (target.position - transform.position).normalized;
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
    void followPlayer() //follow the player
    {
        agent.SetDestination(target.position);
        if (distance <= agent.stoppingDistance)
        {
            FaceTarget();
            agent.speed = 0;

            jumpscare = true;
        }
    }
    
    void ZombieAnimation() //Animation controller for tinyZombie
    {
        if (trigger.GetTriggerStatus())
        {
            if (distance <= agent.stoppingDistance)
            {
                anim.ResetTrigger("Idle");
                anim.ResetTrigger("Run");
                anim.SetTrigger("Attack");
            }
            else if (PlayerWithinViewDistance())
            {
                anim.ResetTrigger("Idle");
                anim.ResetTrigger("Attack");
                anim.SetTrigger("Run");
            }
        }
        else
        {
            anim.ResetTrigger("Run");
            anim.ResetTrigger("Attack");
            anim.SetTrigger("Idle");
        }
    }
        void Update()
    {
        distance = Vector3.Distance(target.position, transform.position);

        ZombieAnimation();

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }

        if (jumpscare)
        {
            audioSource.clip = jumpscareSound;
        }
        else
        {
            audioSource.clip = attackSound;
        }

        agent.speed = 2.32f;

        if (jumpscare)
        {
            //insert jumpscare kill player yes
            camera.ActivateDeathCam(focusPoint);
            target.gameObject.SetActive(false);
            transform.Find("light").gameObject.SetActive(true);

            timer += Time.deltaTime;
            UIOverlay.SetActive(false);
            //play a fadeout transition
            if (timer >= 3.1)
            {
                levelLoad.LoadNextLevel(levelLoad.getSceneName());
                CheckpointManager.ClearCheckpoints();
            }
        }

        if (trigger.GetTriggerStatus())
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
