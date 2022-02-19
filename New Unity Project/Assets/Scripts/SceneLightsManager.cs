using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLightsManager : MonoBehaviour
{
    public Light dirLight;

    private int num_stages = 3;
    private int currStage = 0;

    private float dir_light_intensity;
   
   
    // Start is called before the first frame update
    void Start()
    {
        dir_light_intensity = dirLight.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public int GetCurrentStage()
    {
        return currStage;
    }

    public void DarkenSceneLight()
    {
        currStage++;

        if (currStage > num_stages)
        {
            Debug.Log("Cannot Darken Scene Futher!");
            return;
        }
        float color_amt = 150.0f / num_stages;
        color_amt *= (num_stages - currStage);
        color_amt /= 255.0f;
        RenderSettings.ambientLight = new Color(color_amt, color_amt, color_amt, color_amt);

        dirLight.intensity -= dir_light_intensity / num_stages;
        
    }

    public void InitToStage(int num_stage)
    {
        if (num_stage > num_stages)
            currStage = num_stages;
        else
            currStage = num_stage;
    }
}
