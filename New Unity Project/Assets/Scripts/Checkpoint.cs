using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checkpoint : MonoBehaviour
{
    public Vector3 spawnPos;
    public Canvas game_canvas;
    public ChargeFlashlight ChargeUICanvas;
    public Light pointLight;
    public CheckpointID ID;

    bool is_saved = false;
    float auto_save_radius = 5.0f;
    float charge_radius = 5.0f;

    void Awake()
    {
        CheckpointManager.AddCheckpoint(this);
    }

    void Update()
    {
        // CHeck for Save Condition
        float displacement = (GameHandler.instance.player.transform.position - transform.position).magnitude;
        if (!is_saved && displacement <= auto_save_radius)
        {
            Unlock();
        }

        if (is_saved && displacement <= charge_radius)
        {
            game_canvas.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E) && !ChargeUICanvas.gameObject.activeSelf)
                ChargeUICanvas.StartChargeFlashlight();
        }
        else
        {
            game_canvas.gameObject.SetActive(false);
        }

    }
    
    public void Unlock()
    {
        is_saved = true;
        pointLight.color = Color.white;
        if (ID > CheckpointManager.last_ID)
        {
            CheckpointManager.last_ID = ID;
            GameSettings.currentCheckpoint = this;
        }
    }

   
}
