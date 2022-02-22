using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{

    public Vector3 axis;
    public GameObject pivot;

    GameObject door;
   
    float theta = 0.0f;
    float FALL_SPEED = 50.0f;
    float FALL_ACCEL = 40.0f;
    bool isTriggered = false;

    void Start()
    {
        door = transform.parent.gameObject;
    }

    void Update()
    {
        if (isTriggered)
        {
            FALL_SPEED += FALL_ACCEL * Time.deltaTime;
            float angle = FALL_SPEED * Time.deltaTime;
         
            door.transform.RotateAround(pivot.transform.position, axis, angle);
            theta += angle;
            if (theta >= 90.0f)
            {
                isTriggered = false;
                door.transform.RotateAround(pivot.transform.position, axis, 90.0f - theta); 
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartFall();
        }
    }

    void StartFall()
    {
        isTriggered = true;
    }
}