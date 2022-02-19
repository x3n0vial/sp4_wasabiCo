using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SprintUI : MonoBehaviour
{
    Image staminabar;
    float bar_xscale;
    float bar_xpos;
    // Start is called before the first frame update
    void Start()
    {
        staminabar = transform.Find("bluebar").gameObject.GetComponent<Image>();
        bar_xscale = staminabar.transform.localScale.x;
        bar_xpos = staminabar.transform.localPosition.x;
    }

    public void UpdateStaminaBar(float stamina)
    {
        float percent = stamina / 100.0f;

        Vector3 currScale = staminabar.transform.localScale;
        staminabar.transform.localScale = new Vector3((percent * bar_xscale), currScale.y, currScale.z);
        Vector3 currPos = staminabar.transform.localPosition;
        float offset = 50.0f;
        staminabar.transform.localPosition = new Vector3(bar_xpos - ((1.0f - percent) * offset), currPos.y, currPos.z);
    }
}
