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

    // from outside view
    public GameObject enterDoorL, enterDoorR;
    public GameObject exitDoorL, exitDoorR;

    LIFT_STATES state;


    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case LIFT_STATES.OPENING_ENTRY:
                // OPEN DOOR ANIM
                break;
            case LIFT_STATES.OPEN_WAITING:
                // WAIT FOR PLAYER TO ENTER
                break;
            case LIFT_STATES.CLOSING_ENTRY:
                // CLOSE DOOR ANIM
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
            state = LIFT_STATES.OPENING_ENTRY;
    }
}
