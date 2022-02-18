using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashlightUI : MonoBehaviour
{
   
    CanvasGroup displayIcon;
    Image batteryBar;

    float default_batt_scale_y;
    float default_batt_pos_y;

    void Start()
    {
        displayIcon = GetComponent<CanvasGroup>();
        displayIcon.alpha = 0.3f;

        GameObject batteryGO = transform.Find("BatteryBar").Find("Battery").gameObject;
        batteryBar = batteryGO.GetComponent<Image>();
        batteryBar.color = Color.green;

        default_batt_scale_y = batteryBar.transform.localScale.y;
        default_batt_pos_y = batteryBar.transform.localPosition.y;
    }

    public void UpdateBatteryBar(float battery_amt)
    {
        float perc = battery_amt / 100.0f;

        if (perc > 0.5f)
            batteryBar.color = Color.green;
        else if (perc > 0.2f)
            batteryBar.color = Color.yellow;
        else
        {
            batteryBar.color = Color.red;
            displayIcon.alpha = 0.3f;
        }

        Vector3 currScale = batteryBar.transform.localScale;
        batteryBar.transform.localScale = new Vector3(currScale.x, (perc * default_batt_scale_y), currScale.z);
        Vector3 currPos = batteryBar.transform.localPosition;
        float offset = 20.0f;
        batteryBar.transform.localPosition = new Vector3(currPos.x, default_batt_pos_y -  ((1.0f - perc) * offset), currPos.z); 
    }
    public void UpdateDisplay(bool flashlightOn)
    {
        // BAD CODE BUT LAZY SO WORKS FOR NOW
       if (flashlightOn)
            displayIcon.alpha = 0.3f;
       else
            displayIcon.alpha = 0.9f;
    }

}
