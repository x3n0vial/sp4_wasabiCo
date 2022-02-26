using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    enum LIGHT_STATE
    {
        ON,
        TURNING_OFF,
        OFF,
        TURNING_ON,

        NUM_TOTAL
    }


    Light light;

    LIGHT_STATE state = LIGHT_STATE.ON;

    float change_speed = 0.0f;
    float state_timer = 0.0f;
    float light_intensity = 0.0f;


    void Start()
    {
        light = GetComponent<Light>();
    }

    void Update()
    {
        switch (state)
        {
            case LIGHT_STATE.ON:
                break;
            case LIGHT_STATE.TURNING_OFF:
                break;
            case LIGHT_STATE.OFF:
                break;
            case LIGHT_STATE.TURNING_ON:
                break;
        }
    }
}
