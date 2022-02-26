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


    public float MAX_FADE_TIMING = 1.0F;
    public float MIN_FADE_TIMING = 0.0F;
    public float MAX_ON_TIMING = 2.0F;
    public float MAX_OFF_TIMING = 2.0F;
    public float MIN_ON_TIMING = 0.0F;
    public float MIN_OFF_TIMING = 0.0F;

    Light light;
    float DEFAULT_INTENSITY;

    LIGHT_STATE state = LIGHT_STATE.ON;

    float change_speed = 0.0f;
    float state_timer = 0.0f;
    float light_intensity = 0.0f;



    void Start()
    {
        light = GetComponent<Light>();
        DEFAULT_INTENSITY = light.intensity;
    }

    void Update()
    {

        if (state_timer > 0)
            state_timer -= Time.deltaTime;


        switch (state)
        {
            case LIGHT_STATE.ON:
                if (state_timer <= 0)
                {
                    state = LIGHT_STATE.TURNING_OFF;
                    state_timer = Random.Range(MIN_FADE_TIMING, MAX_FADE_TIMING);
                    light_intensity = Random.Range(0.0f, DEFAULT_INTENSITY * 0.0F);
                    change_speed = (light.intensity - light_intensity) / state_timer;
                }
                break;
            case LIGHT_STATE.TURNING_OFF:
                light.intensity += change_speed * Time.deltaTime;
                if (state_timer <= 0)
                {
                    state = LIGHT_STATE.OFF;
                    light.intensity = light_intensity;
                    state_timer = Random.Range(MIN_OFF_TIMING, MAX_OFF_TIMING);
                }
                break;
            case LIGHT_STATE.OFF:
                if (state_timer <= 0)
                {
                    state = LIGHT_STATE.TURNING_ON;
                    state_timer = Random.Range(MIN_FADE_TIMING, MAX_FADE_TIMING);
                    light_intensity = Random.Range(DEFAULT_INTENSITY * 0.1F, DEFAULT_INTENSITY);
                    change_speed = (light.intensity - light_intensity) / state_timer;
                }
                break;
            case LIGHT_STATE.TURNING_ON:
                light.intensity += change_speed * Time.deltaTime;
                if (state_timer <= 0)
                {
                    state = LIGHT_STATE.ON;
                    light.intensity = light_intensity;
                    state_timer = Random.Range(MIN_ON_TIMING, MAX_ON_TIMING);
                }
                break;
        }
    }
}
