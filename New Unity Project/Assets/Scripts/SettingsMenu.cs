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
        switch (DropDown_flashlightKey.options[DropDown_flashlightKey.value].text)
        {
            case "F":
                GameSettings.FLASHLIGHT_KEY = KeyCode.F;
                break;
            case "TAB":
                GameSettings.FLASHLIGHT_KEY = KeyCode.Tab;
                break;
            default:
                break;
        }
    }

    public void UpdatePickUpKeybind()
    {
        switch (DropDown_pickupItemKey.options[DropDown_pickupItemKey.value].text)
        {
            case "E":
                GameSettings.PICKUP_ITEM_KEY = KeyCode.E;
                break;
            case "R":
                GameSettings.PICKUP_ITEM_KEY = KeyCode.R;
                break;
            case "Q":
                GameSettings.PICKUP_ITEM_KEY = KeyCode.Q;
                break;
            case "Z":
                GameSettings.PICKUP_ITEM_KEY = KeyCode.Z;
                break;
            case "X":
                GameSettings.PICKUP_ITEM_KEY = KeyCode.X;
                break;
            case "C":
                GameSettings.PICKUP_ITEM_KEY = KeyCode.C;
                break;
            default:
                break;
        }
    }

    public void UpdateThrowKeybind()
    {
        switch (DropDown_throwItemKey.options[DropDown_throwItemKey.value].text)
        {
            case "Left Mouse Button":
                GameSettings.THROW_ITEM_KEY = KeyCode.Mouse0;
                break;
            case "Right Mouse Button":
                GameSettings.THROW_ITEM_KEY = KeyCode.Mouse1;
                break;
            default:
                break;
        }
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
