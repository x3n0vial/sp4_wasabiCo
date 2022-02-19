using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    // Start is called before the first frame update
    public Elevator elevator;


    //FOR TESTING
    public void StartElevator(float TargetPosY)
    {
        elevator.StartElevator(TargetPosY);
    }

    private void OnTriggerEnter(Collider other)
    {
        // if item hit,
       // elevator.StartElevator();
    }

}
