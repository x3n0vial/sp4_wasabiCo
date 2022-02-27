using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Dropdown DropDown_flashlightKey;
    public Dropdown DropDown_pickupItemKey;
    public Dropdown DropDown_throwItemKey;

    public Slider Slider_mouseSensitivity;
    public Slider Slider_masterVolume;

    public GameObject GO_mouseSensitivity;

    void Start()
    {
       
    }

    public void UpdateFlashlightKeybind()
    {
        //GameSettings.FLASHLIGHT_KEY
    }

    public void UpdatePickUpKeybind()
    {
       
    }

    public void UpdateThrowKeybind()
    {

    }
    

    public void UpdateMouseSensitivity()
    {
        GameSettings.MOUSE_SENSITIVITY = Slider_mouseSensitivity.value;
    }

    public void UpdateVolume()
    {
        float newVolume = Slider_masterVolume.value;
        PlayerPrefs.SetFloat("volume", newVolume);
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
    }
}
