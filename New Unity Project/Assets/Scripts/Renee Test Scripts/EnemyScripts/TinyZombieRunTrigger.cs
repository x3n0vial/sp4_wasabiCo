using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinyZombieRunTrigger : MonoBehaviour
{
    Transform target;
    bool trigger = false;
    public GameObject zombie;

    void Start()
    {
        target = GameHandler.instance.player.transform;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == target)
        {
            trigger = true;
        }
    }

    public bool getTrigger()
    {
        return trigger;
    }

    public void Update()
    {
        if (trigger)
        {
            zombie.SetActive(true);
        }
    }
}
