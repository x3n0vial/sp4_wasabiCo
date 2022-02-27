using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySliderValue : MonoBehaviour
{
    public Text displayText;

    private Slider slider;
   
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void UpdateDisplayText()
    {
        displayText.text = slider.value.ToString("0.0") + "x";

    }
}
