//Nicole
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    public Transform[] waypoints;
    public int speed;

    private bool start = true, isPatrolling = false, isStunned = false;
    private double getUpTime = 4.6f;

    private int waypointIndex;
    private float distToCurrWaypoint, distToPlayer;
    private double restTime = 1.6f, stunTime = 0f;
    private float stunMeter = 0f, stunRate = 2f;

    public NavMeshAgent agent;
    public Animator anim;
    public Collider collider;

    public GameObject Player;
    public Flashlight flashlight;

    public float angleOfView, rangeofView;

    AudioSource SFX;
    public AudioClip SFX_Growl;
    public AudioClip SFX_Kill;

    GameObject UIOverlay;
    Transform focusPoint;
    bool isAttack = false;
    double deathTimer = 1f;

    LayerMask wallLayer;

    // Start is called before the first frame update
    void Start()
    {
        waypointIndex = 0;
        wallLayer = LayerMask.GetMask("SolidObject");
        focusPoint = transform.Find("FocusPoint");
        UIOverlay = GameHandler.instance.UILayout;
        SFX = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!SFX.isPlaying)
        {
            SFX.Play();
        }

        if (isAttack)
        {
            SFX.clip = SFX_Growl;
        }
        else
        {
            SFX.clip = SFX_Kill;
        }

        if (start)
        {
            if (getUpTime >= 0f)
            {
                agent.speed = 0f;
                getUpTime -= Time.deltaTime;

            }
            else if (getUpTime < 0f)
            {
                start = false;
                //Patrol();
            }
        }
        else
        {
            distToCurrWaypoint = Vector3.Distance(transform.position, waypoints[waypointIndex].position);
            distToPlayer = Vector3.Distance(transform.position, Player.transform.position);

            if (distToPlayer > 20f)
                Patrol();
            if (isPatrolling)
            {
                if (distToCurrWaypoint < 1f)
                {
                    if (restTime > 0f)
                    {
                        // rest for a while befoe continue
                        restTime -= Time.deltaTime;
                        Rest();
                    }
                    else
                    {
                        // continue patrol after patrol
                        IncreaseIndex();
                        restTime = 1.6f;
                        Patrol();
                    }
                }
            }

            if (distToPlayer <= 20f && distToPlayer >= 3f)
            {
                if (canSeePlayer())
                {
                    // if enemy can see player and is within range
                    Follow();
                }
                else
                {
                    Patrol();
                }
            }

            if (distToPlayer < 3f)
            {
                // attack player if within range
                Attack();
                isAttack = true;
                GameHandler.instance.cameraSettings.ActivateDeathCam(focusPoint);
                UIOverlay.SetActive(false);
            }
            else
            {
                anim.SetBool("Attack", false);
            }

            if(isAttack)
            {
                if (deathTimer > 0f)
                {
                    deathTimer -= Time.deltaTime;
                    isAttack = true;
                }
                else if (deathTimer <= 0f)
                {
                    deathTimer = 1f;
                    isAttack = false;
                    GameHandler.instance.levelLoader.LoadNextLevel(GameHandler.instance.levelLoader.getSceneName());
                    CheckpointManager.ClearCheckpoints();
                }
            }

            if (isInFlashlight())
            {
                if (stunMeter < 5f)
                {
                    if (!isStunned)
                    {
                        stunMeter += stunRate * Time.deltaTime;
                        anim.SetBool("Stun", false);
                    }
                }
                else if (stunMeter >= 5f)
                {
                    stunTime = 2f;
                    isStunned = true;
                    stunMeter = 0f;
                }
            }
            else
            {
                if (stunMeter > 0f)
                {
                    stunMeter -= stunRate * Time.deltaTime * 1.5f;
                }

            }

            if (isStunned)
            {
                if (stunTime > 0f)
                {
                    stunTime -= Time.deltaTime;
                    Stun();
                }
                else if (stunTime <= 0f) // reach end of stun duration
                {
                    isStunned = false;
                    anim.SetBool("Stun", false);
                }
            }
        }
    }
    
    void Rest()
    {
        anim.SetBool("Idle", true);
        anim.SetBool("Patrol", false);
        anim.SetBool("Follow", false);
        anim.SetBool("Attack", false);
        agent.speed = 0f;
        isPatrolling = true;
    }

    bool isInFlashlight()
    {
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
        anim.SetBool("Stun", true);
         Debug.Log("Stunned");
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
        anim.SetBool("Follow", false);
        anim.SetBool("Stun", false);
    }

    void Follow()
    {
        anim.SetBool("Follow", true);
        //anim.SetBool("Idle", false);
        agent.speed = 2.5f;
        agent.SetDestination(Player.transform.position);
        isPatrolling = false;
    }

    void Patrol()
    {
        anim.SetBool("Patrol", true);
        anim.SetBool("Idle", false);
        anim.SetBool("Follow", false);
        agent.speed = 2f;
        agent.SetDestination(waypoints[waypointIndex].transform.position);
        isPatrolling = true;
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
    }
}
