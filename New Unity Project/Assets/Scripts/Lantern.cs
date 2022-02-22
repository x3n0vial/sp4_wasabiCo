using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lantern : MonoBehaviour
{

    public Canvas UICanvas;
    public Flashlight playerFlashlight;

    int LIGHT_LANTERN_POINTS = 10;

    Image progressBar;
    Vector3 bar_ori_pos;
    float bar_pos_offset = 0.23f;
    float bar_full_scale = 0.48f;

    bool isLit = false;

    float light_progress = 0.0f; // percentage completion
    float light_speed = 0.3f; // speed at which perc increases

    float trigger_radius = 5.0f; // radius for UI to show

    // Start is called before the first frame update
    void Start()
    {
        progressBar = UICanvas.gameObject.transform.Find("LightBar").Find("Filler").GetComponent<Image>();

        if (progressBar == null)
            Debug.Log("Lantern::Could not find Progress Bar Image Component from Lantern");

        bar_ori_pos = progressBar.transform.localPosition;
        
    }

    // Update is called once per frame
    void Update()
    {
        // No Need Update if Lantern is already Lit
        if (isLit)
            return;

        bool withinLight = playerFlashlight.CheckIfInFlashlight(GetComponent<MeshCollider>());

        float displacement = (GameHandler.instance.player.transform.position - transform.position).magnitude;
        if (displacement < trigger_radius || withinLight)
            UICanvas.gameObject.SetActive(true);
        else
            UICanvas.gameObject.SetActive(false);

        if (withinLight)
        {
            light_progress += light_speed * Time.deltaTime;

            progressBar.transform.localPosition = new Vector3(bar_ori_pos.x + light_progress * bar_pos_offset,
                bar_ori_pos.y, bar_ori_pos.z);
            progressBar.transform.localScale = new Vector3(light_progress * bar_full_scale,
               progressBar.transform.localScale.y, progressBar.transform.localScale.z);

            if (light_progress >= 1.0f)
            {
                LightLantern();
            }
        }
    }

    private void LightLantern()
    {
        isLit = true;
        transform.Find("Flame").gameObject.SetActive(true);
        UICanvas.gameObject.SetActive(false);
        GameSettings.PLAYER_POINTS += LIGHT_LANTERN_POINTS;
    }
}
