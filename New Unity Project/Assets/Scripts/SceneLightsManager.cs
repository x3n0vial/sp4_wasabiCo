using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLightsManager : MonoBehaviour
{
    public Light dirLight;
    public Light player_dirLight;

    private int num_stages = 3;
    private int currStage = 0;

    private float dir_light_intensity;
    private float player_dir_light_intensity;
    private float env_light_intensity;
    
   
   
    // Start is called before the first frame update
    void Start()
    {
        dir_light_intensity = dirLight.intensity;
        player_dir_light_intensity = player_dirLight.intensity;
        env_light_intensity = RenderSettings.reflectionIntensity;
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
        player_dirLight.intensity -= ((player_dir_light_intensity - 0.1f) / num_stages); // 0.1f min
        RenderSettings.reflectionIntensity -= ((env_light_intensity - 0.1f) / num_stages); // 0.1f min

    }

    public void InitToStage(int num_stage)
    {
        if (num_stage > num_stages)
            currStage = num_stages;
        else
            currStage = num_stage;
    }
}
