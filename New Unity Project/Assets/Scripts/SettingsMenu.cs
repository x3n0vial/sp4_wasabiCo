using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Dropdown DropDown_flashlightKey;
    public Dropdown DropDown_pickupItemKey;
    public Dropdown DropDown_throwItemKey;
    public Dropdown DropDown_movementKey;

    public Dropdown Dropdown_cameraMode;

    public GameObject GO_mouseSensitivity;

    void Start()
    {
        if (GameSettings.CAMERA_MOUSEPAN_ENABLED)
            Dropdown_cameraMode.value = 0;
        else
            Dropdown_cameraMode.value = 1;
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
    public void UpdateMovementKeybind()
    {

    }


    public void UpdateCameraMode()
    {

        string option = Dropdown_cameraMode.options[Dropdown_cameraMode.value].text;
        if (option == "Freelook")
        {
            GameSettings.CAMERA_MOUSEPAN_ENABLED = true;
            GO_mouseSensitivity.SetActive(true);
        }
        else if (option == "Auto")
        {
            GameSettings.CAMERA_MOUSEPAN_ENABLED = false;
            GO_mouseSensitivity.SetActive(false);
        }

       
    }

   
}
