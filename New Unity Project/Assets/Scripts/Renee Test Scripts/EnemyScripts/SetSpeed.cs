using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSpeed : MonoBehaviour
{
    public float speed;
    bool speedSet = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Zombie")
        {
            speedSet = true;
        }
    }
    // Update is called once per frame
    public bool getSpeedStatus()
    {
        return speedSet;
    }

    public float getSpeed()
    {
        return speed;
    }
}
