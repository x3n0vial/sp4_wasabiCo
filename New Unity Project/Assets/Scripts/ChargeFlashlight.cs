using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeFlashlight : MonoBehaviour
{
    public Flashlight player_flashlight;

    Image progressBar;

    float battery = 0.0f; // max is 100.0f
    float charge_rate = 10.0f;

    float full_batt_scale = 6.2f; // taken from unity, scale when bar is filled
    float full_batt_translate = 213.5f;
    Vector3 original_pos;

    // Start is called before the first frame update
    void Start()
    {
        progressBar = transform.Find("ProgressBar").GetComponent<Image>();
        original_pos = progressBar.transform.position;
    }

    // Update is called once per frame
    void Update()
    {


        // increase progress when space is held
        if (Input.GetKey(KeyCode.F))
        {
            battery += charge_rate * Time.deltaTime;
            Debug.Log("F Key Pressed, charging...Battery Level: " + battery);
        }

        float perc = battery / 100.0f;
     
      
        progressBar.transform.position = new Vector3(original_pos.x +  (0.5f * perc * full_batt_translate), original_pos.y, original_pos.z);
        Vector3 oriScale = progressBar.transform.localScale;
        progressBar.transform.localScale = new Vector3(perc * full_batt_scale, oriScale.y, oriScale.z);

        // color progress bar accordingly
        progressBar.color = new Color(1 - perc, perc, 0);

        if (battery >= 100.0f)
        {
            gameObject.SetActive(false);
            player_flashlight.RefillBattery();
            player_flashlight.gameObject.SetActive(true);
        }

    }

    public void StartChargeFlashlight()
    {
        gameObject.SetActive(true);
        player_flashlight.gameObject.SetActive(false);
    }

}
