using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    // Start is called before the first frame update
    public Elevator elevator;
    public float TargetPositionY;


    //FOR TESTING
    public void StartElevator()
    {
        elevator.StartElevator(TargetPositionY);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickable"))
        {
            elevator.StartElevator(TargetPositionY);
        }
    }

}
