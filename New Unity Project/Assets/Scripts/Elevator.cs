using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    enum LIFT_STATES
    {
        DEFAULT,
        OPENING_ENTRY,
        ENTER_WAITING,
        CLOSING_ENTRY,
        MOVING,
        OPENING_EXIT,
        EXIT_WAITING,
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
    private bool isPlayerinLift = false;
    private float target_pos_y = 0.0f; // Target Floor(Y Pos) to move to
    private float door_speed = 1.5f; // Speed of door open/close
    private float elevator_speed = 3.0f; // Speed of elevator movement
    private float temp_dis_moved = 0.0f; // Temp Var
    private float DOOR_OPEN_OFFSET = 5.0f;

    // DOOR ORIGINAL POSITIONS
    private Vector3 enterDoorL_oriPos, enterDoorR_oriPos;
    private Vector3 exitDoorL_oriPos, exitDoorR_oriPos;

    private void Awake()
    {
        enterDoorL_oriPos = enterDoorL.transform.position;
        enterDoorR_oriPos = enterDoorR.transform.position;
        exitDoorL_oriPos = exitDoorL.transform.position;
        exitDoorR_oriPos = exitDoorR.transform.position;

        DOOR_OPEN_OFFSET *= transform.localScale.z;
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
                if (temp_dis_moved >= DOOR_OPEN_OFFSET)
                    state = LIFT_STATES.ENTER_WAITING;
                break;
            case LIFT_STATES.ENTER_WAITING:
                // WAIT FOR PLAYER TO ENTER
                if (isPlayerinLift)
                    state = LIFT_STATES.CLOSING_ENTRY;
                break;
            case LIFT_STATES.CLOSING_ENTRY:
                // CLOSE DOOR ANIM
                deltaZ = door_speed * Time.deltaTime;
                enterDoorL.transform.position = new Vector3(enterDoorL.transform.position.x, enterDoorL.transform.position.y, enterDoorL.transform.position.z - deltaZ);
                enterDoorR.transform.position = new Vector3(enterDoorR.transform.position.x, enterDoorR.transform.position.y, enterDoorR.transform.position.z + deltaZ);
                if (enterDoorL.transform.position.z < enterDoorL_oriPos.z)
                {
                    state = LIFT_STATES.MOVING;
                    enterDoorL.transform.position = enterDoorL_oriPos;
                    enterDoorR.transform.position = enterDoorR_oriPos;
                }
                break;
            case LIFT_STATES.MOVING:
                // MOVE ELEVATOR
                float deltaY = elevator_speed * Time.deltaTime;
                if (target_pos_y < transform.position.y)
                    deltaY *= -1;
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + deltaY, transform.localPosition.z);
                Debug.Log("Moving Lift");
                if ((deltaY < 0 && transform.position.y < target_pos_y)
                    || (deltaY > 0 && transform.position.y > target_pos_y))
                {
                    transform.position = new Vector3(transform.position.x, target_pos_y, transform.position.z);
                    state = LIFT_STATES.OPENING_EXIT;
                    Debug.Log("Elevator has reached target height, Opening Doors now" + transform.position);
                    temp_dis_moved = 0;
                }
                break;
            case LIFT_STATES.OPENING_EXIT:
                // OPEN DOOR ANIM
                deltaZ = door_speed * Time.deltaTime;
                exitDoorL.transform.position = new Vector3(exitDoorL.transform.position.x, exitDoorL.transform.position.y, exitDoorL.transform.position.z - deltaZ);
                exitDoorR.transform.position = new Vector3(exitDoorR.transform.position.x, exitDoorR.transform.position.y, exitDoorR.transform.position.z + deltaZ);
                temp_dis_moved += deltaZ;
                if (temp_dis_moved >= DOOR_OPEN_OFFSET)
                    state = LIFT_STATES.EXIT_WAITING;
                break;
            case LIFT_STATES.EXIT_WAITING:
                // WAIT FOR PLAYER TO LEAVE
                if (!isPlayerinLift)
                    state = LIFT_STATES.CLOSING_EXIT;
                break;
            case LIFT_STATES.CLOSING_EXIT:
                // CLOSE DOOR ANIM
                deltaZ = door_speed * Time.deltaTime;
                exitDoorL.transform.position = new Vector3(exitDoorL.transform.position.x, exitDoorL.transform.position.y, exitDoorL.transform.position.z + deltaZ);
                exitDoorR.transform.position = new Vector3(exitDoorR.transform.position.x, exitDoorR.transform.position.y, exitDoorR.transform.position.z - deltaZ);
                if (exitDoorL.transform.position.z > exitDoorL_oriPos.z)
                {
                    state = LIFT_STATES.DEFAULT;
                    exitDoorL.transform.position = new Vector3(exitDoorL_oriPos.x, target_pos_y, exitDoorL_oriPos.z);
                    exitDoorR.transform.position = new Vector3(exitDoorR_oriPos.x, target_pos_y, exitDoorR_oriPos.z);
                 
                }
                break;
            default:
                break;
        }
    }

    public void StartElevator(float TargetPosY)
    {
        if (state == LIFT_STATES.DEFAULT)
        {
            state = LIFT_STATES.OPENING_ENTRY;
            temp_dis_moved = 0.0f;
            target_pos_y = TargetPosY;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerinLift = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            isPlayerinLift = false;
        }
    }
}
