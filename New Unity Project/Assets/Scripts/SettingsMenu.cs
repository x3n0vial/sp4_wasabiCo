using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Dropdown flashlightKeyDropDown;
    public Dropdown interactKeyDropDown;
    public Dropdown movementKeyDropDown;
   
    public void UpdateFlashlightKeybind()
    {
        //GameSettings.FLASHLIGHT_KEY
    }

    public void UpdateCameraMode(bool enableMouse)
    {
        GameSettings.CAMERA_MOUSEPAN_ENABLED = enableMouse;
    }
}
