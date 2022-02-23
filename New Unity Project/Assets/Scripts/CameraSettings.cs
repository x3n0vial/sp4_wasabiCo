using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

public class CameraSettings : MonoBehaviour
{

    public CinemachineFreeLook freelookCam;
    public CinemachineVirtualCamera standardCam;
    public CinemachineVirtualCamera deathCam;
    
    public GameObject deathCamTarget;

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
        deathCam.Priority = -1;
    }

    // Update is called once per frame
    void Update()
    {


        // FOR TESTING
        if (Input.GetKeyDown(KeyCode.M))
            GameSettings.CAMERA_MOUSEPAN_ENABLED = !GameSettings.CAMERA_MOUSEPAN_ENABLED;

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

    public void ActivateDeathCam(Transform target, Vector3 direction)
    {
        deathCam.LookAt = target;
        deathCam.Follow = target;
        deathCam.Priority = 10;
        CinemachineTransposer transposer = deathCam.GetCinemachineComponent<CinemachineTransposer>();
        Vector3 camDir = -1 * direction;
        float theta = Mathf.Acos(Vector3.Dot(camDir, new Vector3(0, 0, -1)) / camDir.magnitude);
        if (camDir.x > 0)
            theta *= -1;
        float radius = 2.0f;
        transposer.m_FollowOffset = new Vector3(Mathf.Cos(theta) * radius, transposer.m_FollowOffset.y, Mathf.Sin(theta) * radius);
        Debug.Log("Camera Dir = " + camDir);
        Debug.Log("Camera Offset = " + transposer.m_FollowOffset);
    }

    public void DeactivateDeathCam()
    {
        deathCam.Priority = -1;
    }
}
