using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checkpoint : MonoBehaviour
{
    public Vector3 spawnPos;
    public Canvas game_canvas;
    public Light pointLight;

    bool is_saved = false;
    float auto_save_radius = 5.0f;

    void Awake()
    {
        
    }

    void Update()
    {
        // CHeck for Save Condition
        float displacement = (PlayerManager.instance.player.transform.position - transform.position).magnitude;
        if (!is_saved && displacement <= auto_save_radius)
        {
            GameSettings.currentCheckpoint = this;
            is_saved = true;
            pointLight.color = Color.white;
        }


        // Rotate canvas to face player
        // Get Facing Camera Direction
        Vector3 targetDir = -1 * Camera.main.transform.forward;
        targetDir.y = 0;
        targetDir.Normalize();
   
        // Get angle to rotate
        float theta = Mathf.Acos(Vector3.Dot(targetDir, new Vector3(0, 0, -1) / (targetDir.magnitude)));
        if (targetDir.x > 0.0f)
            theta *= -1;
    
        game_canvas.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Mathf.Rad2Deg * theta, transform.rotation.eulerAngles.z);

    }
}
