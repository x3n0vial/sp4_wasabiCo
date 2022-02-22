using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeFlashlight : MonoBehaviour
{
    Image progressBar;

    float battery = 0.0f; // max is 100.0f
    float charge_rate = 10.0f;

    float full_batt_scale = 10.7f; // taken from unity, scale when bar is filled
    float full_batt_translate = 535.0f;
    Vector3 original_pos;

    void Start()
    {
        progressBar = transform.Find("Panel").Find("ProgressBar").GetComponent<Image>();
        original_pos = progressBar.transform.localPosition;
    }

    void Update()
    {


        // increase progress when space is held
        if (Input.GetKey(KeyCode.F))
        {
            battery += charge_rate * Time.deltaTime;
        }

        // translate and scale progress bar accordingly
        float perc = battery / 100.0f;

        progressBar.transform.localPosition = new Vector3(original_pos.x + (perc * full_batt_translate), original_pos.y, original_pos.z);
        Vector3 oriScale = progressBar.transform.localScale;
        progressBar.transform.localScale = new Vector3(perc * full_batt_scale, oriScale.y, oriScale.z);


        // color progress bar accordingly
        progressBar.color = new Color(1 - perc, perc, 0);

        if (battery >= 100.0f)
        {
            gameObject.SetActive(false);
            GameHandler.instance.flashlight.RefillBattery();
            GameHandler.instance.flashlight.gameObject.SetActive(true);
        }

    }

    public void StartChargeFlashlight()
    {
        gameObject.SetActive(true);
        GameHandler.instance.flashlight.gameObject.SetActive(false);
    }

}
