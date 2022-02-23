using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceCanvas : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        // Rotate canvas to face player
        // Get Facing Camera Direction
       // Vector3 targetDir = -1 * GameHandler.instance.UICamera.transform.forward;
        Vector3 targetDir = Camera.main.transform.position - transform.position;
        targetDir.y = 0;
        targetDir.Normalize();

        // Get angle to rotate
        float theta = Mathf.Acos(Vector3.Dot(targetDir, new Vector3(0, 0, -1) / (targetDir.magnitude)));
        if (targetDir.x > 0.0f)
            theta *= -1;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Mathf.Rad2Deg * theta, transform.rotation.eulerAngles.z);
    }
}
