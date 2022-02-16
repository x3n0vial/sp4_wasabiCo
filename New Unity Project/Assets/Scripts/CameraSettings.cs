using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

public class CameraSettings : MonoBehaviour
{

    public CinemachineFreeLook freelookCam;
    public CinemachineVirtualCamera standardCam;
    // Start is called before the first frame update
    void Start()
    {
        freelookCam.m_XAxis.m_MaxSpeed = GameSettings.MOUSE_SENSITIVITY;
        freelookCam.m_YAxis.m_MaxSpeed = 0.02f + GameSettings.MOUSE_SENSITIVITY * 0.01f;

        if (GameSettings.CAMERA_MOUSEPAN_ENABLED)
        {
            freelookCam.Priority = 1;
            standardCam.Priority = 0;
        }
        else
        {
            freelookCam.Priority = 0;
            standardCam.Priority = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {

        freelookCam.m_XAxis.m_MaxSpeed = GameSettings.MOUSE_SENSITIVITY;
        freelookCam.m_YAxis.m_MaxSpeed = 0.02f + GameSettings.MOUSE_SENSITIVITY * 0.01f;

        if (GameSettings.CAMERA_MOUSEPAN_ENABLED)
        {
            freelookCam.Priority = 1;
            standardCam.Priority = 0;
        }
        else
        {
            freelookCam.Priority = 0;
            standardCam.Priority = 1;
        }
    }
}