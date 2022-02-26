using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TinyZombieRun_Controller : MonoBehaviour
{
    //player
    Transform target;

    //player distance from enemy
    float distance;

    //handler of flashlight
    Flashlight flashlight;

    //how far the enemy follows the player
    float lookRadius = 1000f;

    //enemy's agent
    NavMeshAgent agent;

    //enemy's animator
    Animator anim;

    //enemy's collider
    Collider collider;

    //enemy's trigger
    public TinyZombieTrigger trigger;
  
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

    //enemy speed
    public float speed;

    //enemy setspeed
    public SetSpeed setSpeed;

    //enemy type
    public string type;

    //enemy audio
    public AudioClip dissapearSound;
    public AudioClip attackSound;
    public AudioClip zombieSound;
    public AudioClip jumpscareSound;
    public AudioClip jumpscareSound2;
    private AudioSource audioSource;

    //ui overlay
    GameObject UIOverlay;

    void Start() //initialise the variables
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        target = GameHandler.instance.player.transform;
        flashlight = GameHandler.instance.flashlight;
        collider = GetComponent<Collider>();

        agent.speed = speed;

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
        //RaycastHit hit;
        //if (Physics.Linecast(transform.position, target.position, out hit, -1)) //if behind wall, lose player
        //{
        //    if (hit.transform.CompareTag("Player"))
        //    {
                agent.SetDestination(target.position);
                if (distance <= agent.stoppingDistance)
                {
                    FaceTarget();
                    agent.speed = 0;

                    jumpscare = true;
                }
        //    }
        //}
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
        else
        {
            anim.ResetTrigger("Walk");
            anim.ResetTrigger("Attack");
            anim.SetTrigger("Idle");
        }
    }
    void Update()
    {
        distance = Vector3.Distance(target.position, transform.position);

        TinyZombieAnimation();

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }

        if (!jumpscare && stunned() && trigger.getTrigger() && type == "Run")
        {
            audioSource.clip = dissapearSound;
        }
        else if (!jumpscare && trigger.getTrigger() && type == "n")
        {
            audioSource.clip = dissapearSound;
        }
        else if (!jumpscare && stunned())
        {
            audioSource.clip = attackSound;
        }
        else if (!jumpscare)
        {
            audioSource.clip = zombieSound;
        }
        else if (jumpscare)
        {
            audioSource.clip = jumpscareSound;
        }

        if (jumpscare)
        {
            //insert jumpscare kill player yes
            camera.ActivateDeathCam(focusPoint);
            target.gameObject.SetActive(false);
            timer += Time.deltaTime; 
            transform.Find("light").gameObject.SetActive(true);
            UIOverlay.SetActive(false);
            //play a fadeout transition
            if (timer >= 3.1)
            {
                target.gameObject.SetActive(true);
                levelLoad.LoadNextLevel(levelLoad.getSceneName());
                CheckpointManager.ClearCheckpoints();
            }
        }

        if (type == "Run")
        {
            if (stunned() && trigger.getTrigger())
            {
                transform.gameObject.SetActive(false);
            }
        }

        if (type == "n")
        {
            transform.Find("light2").gameObject.SetActive(true);
            //audioSource.clip = jumpscareSound2;
        }

        if (!stunned() && trigger.getTrigger())
        {
            if (setSpeed.getSpeedStatus())
            {
                agent.speed = setSpeed.getSpeed();
            }
            else
            {
                agent.speed = speed;
            }
            
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
