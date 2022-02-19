using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    enum LIFT_STATES
    {
        DEFAULT,
        OPENING_ENTRY,
        OPEN_WAITING,
        CLOSING_ENTRY,
        MOVING,
        OPENING_EXIT,
        CLOSING_EXIT,


        LSTATES_TOTAL
    }

    //UPDATES RN WORK ASSUMING ALL LIFTS ARE FACING X AXIS, LEFT TO RIGHT

    // L/R based on from outside view
    public GameObject enterDoorL, enterDoorR;
    public GameObject exitDoorL, exitDoorR;

    // Lift State
    LIFT_STATES state;

    // Vars
    private float door_speed = 1.0f; // Speed of door open/close
    private float temp_dis_moved = 0.0f; // Temp Var

    // DOOR ORIGINAL POSITIONS
    private Vector3 enterDoorL_oriPos, enterDoorR_oriPos;
    private Vector3 exitDoorL_oriPos, exitDoorR_oriPos;

    private void Awake()
    {
        enterDoorL_oriPos = enterDoorL.transform.position;
        enterDoorR_oriPos = enterDoorR.transform.position;
        exitDoorL_oriPos = exitDoorL.transform.position;
        exitDoorR_oriPos = exitDoorR.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float deltaZ = 0.0f;
        switch (state)
        {
            case LIFT_STATES.OPENING_ENTRY:
                // OPEN DOOR ANIM
                deltaZ = door_speed * Time.deltaTime;
                enterDoorL.transform.position = new Vector3(enterDoorL.transform.position.x, enterDoorL.transform.position.y, enterDoorL.transform.position.z + deltaZ);
                enterDoorR.transform.position = new Vector3(enterDoorR.transform.position.x, enterDoorR.transform.position.y, enterDoorR.transform.position.z - deltaZ);
                temp_dis_moved += deltaZ;
                if (temp_dis_moved >= 5.0f)
                    state = LIFT_STATES.OPEN_WAITING;
                break;
            case LIFT_STATES.OPEN_WAITING:
                // WAIT FOR PLAYER TO ENTER
                break;
            case LIFT_STATES.CLOSING_ENTRY:
                // CLOSE DOOR ANIM
                deltaZ = door_speed * Time.deltaTime;
                enterDoorL.transform.position = new Vector3(enterDoorL.transform.position.x, enterDoorL.transform.position.y, enterDoorL.transform.position.z - deltaZ);
                enterDoorR.transform.position = new Vector3(enterDoorR.transform.position.x, enterDoorR.transform.position.y, enterDoorR.transform.position.z + deltaZ);
                if (enterDoorL.transform.position.z < enterDoorL_oriPos.z)
                {
                    state = LIFT_STATES.MOVING;
                }

                break;
            case LIFT_STATES.MOVING:
                // MOVE ELEVATOR
                break;
            case LIFT_STATES.OPENING_EXIT:
                // OPEN DOOR ANIM
                break;
            case LIFT_STATES.CLOSING_EXIT:
                // CLOSE DOOR ANIM
            break;
            default:
                break;
        }
    }

    public void StartElevator()
    {
        if (state == LIFT_STATES.DEFAULT)
        {
            state = LIFT_STATES.OPENING_ENTRY;
            temp_dis_moved = 0.0f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") 
            && state == LIFT_STATES.OPEN_WAITING)
        {
            state = LIFT_STATES.CLOSING_ENTRY;
        }
    }
}
