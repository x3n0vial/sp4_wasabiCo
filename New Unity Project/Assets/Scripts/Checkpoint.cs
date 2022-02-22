using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checkpoint : MonoBehaviour
{
    public Vector3 spawnPos;
    public Canvas game_canvas;
    public Light pointLight;
   // public GameObject chargeUI;

    bool is_saved = false;
    float auto_save_radius = 5.0f;
    float charge_radius = 5.0f;

    void Awake()
    {
        
    }

    void Update()
    {
        // CHeck for Save Condition
        float displacement = (GameHandler.instance.player.transform.position - transform.position).magnitude;
        if (!is_saved && displacement <= auto_save_radius)
        {
            GameSettings.currentCheckpoint = this;
            is_saved = true;
            pointLight.color = Color.white;
        }

        if (is_saved && displacement <= charge_radius)
        {
            game_canvas.gameObject.SetActive(true);
        }
        else
        {
            game_canvas.gameObject.SetActive(false);
        }



    }

   
}
