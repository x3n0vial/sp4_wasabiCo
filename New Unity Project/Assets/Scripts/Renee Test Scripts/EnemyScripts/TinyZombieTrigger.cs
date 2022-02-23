using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinyZombieTrigger : MonoBehaviour
{
    Transform target;
    bool trigger = false;

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
}
