using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Flashlight : MonoBehaviour
{
    public Light spot_light;
 
    //public PlayerScript things to get forwardvector

    // Start is called before the first frame update
    void Start()
    {
       // spot_light.
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            spot_light.enabled = !spot_light.enabled;

    }
}
