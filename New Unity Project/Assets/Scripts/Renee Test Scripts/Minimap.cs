using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    Transform target;
    void Start()
    {
        target = PlayerManager.instance.player.transform;
    }

    void LateUpdate()
    {
        Vector3 newPos = target.position;
        newPos.y = transform.position.y;
        transform.position = newPos;
    }
}
