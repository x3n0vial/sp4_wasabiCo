using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieGetUp : MonoBehaviour
{
    //player
    Transform target;

    //zombie
    public GameObject zombie;

    //animator
    Animator anim;

    //navMesh agent
    NavMeshAgent agent;

    //boolean to check if jumpscare is triggered
    bool getUp = false;

    //timer to getup
    int getupTimer = 0;
    int maxgetupTimer = 200;

    //shift zombie position downwards slowly
    float zombiePosY = 0;

    //timer to enable physics start chasing
    int chaseTimer = 0;
    int maxChaseTimer = 1000;

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        anim = GetComponentInParent<Animator>();
        agent = GetComponentInParent<NavMeshAgent>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == target)
        {
            getUp = true;
        }
    }

    void Update()
    {
        if (getUp)
        {
            getupTimer++;
            if (getupTimer > maxgetupTimer)
            {
                anim.SetTrigger("Getup");
                chaseTimer++;
                zombiePosY -= 0.000001f * Time.deltaTime;
                if (chaseTimer > maxChaseTimer)
                {
                    this.transform.parent.gameObject.SetActive(false);
                    zombiePosY = 0;
                    zombie.SetActive(true);
                    getUp = false;
                }
            }
        }
        this.transform.parent.transform.position = new Vector3(this.transform.parent.transform.position.x, this.transform.parent.transform.position.y + zombiePosY, this.transform.parent.transform.position.z);
    }
}