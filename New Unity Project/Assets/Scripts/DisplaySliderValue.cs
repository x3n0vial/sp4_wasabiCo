using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySliderValue : MonoBehaviour
{
    public Text displayText;

    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateDisplayText()
    {
        displayText.text = slider.value.ToString("0.0") + "x";

    }
}
