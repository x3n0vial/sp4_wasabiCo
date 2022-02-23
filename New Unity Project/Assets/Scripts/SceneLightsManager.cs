using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLightsManager : MonoBehaviour
{
    public Light dirLight;
    public Light player_dirLight;


    public float MinDirLightIntensity = 0.1f;
    public float MinPlayerDirLightIntensity = 0.1f;
    public float MinEnvLightIntensity = 0.1f;
    public float MinAmbientIntensity = 0.0f;

    private int num_stages = 3;
    private int currStage = 0;

    private float ambient_intensity;
    private float dir_light_intensity;
    private float player_dir_light_intensity;
    private float env_light_intensity;
    
   
   
    // Start is called before the first frame update
    void Start()
    {
        dir_light_intensity = dirLight.intensity;
        player_dir_light_intensity = player_dirLight.intensity;
        env_light_intensity = RenderSettings.reflectionIntensity;
        ambient_intensity = RenderSettings.ambientLight.r;
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
        float color_amt = (ambient_intensity - MinAmbientIntensity) / num_stages;
        color_amt *= (num_stages - currStage);
        color_amt /= 255.0f;
        RenderSettings.ambientLight = new Color(color_amt, color_amt, color_amt, color_amt);

        dirLight.intensity -= (dir_light_intensity - MinDirLightIntensity) / num_stages;
        player_dirLight.intensity -= ((player_dir_light_intensity - MinPlayerDirLightIntensity) / num_stages); 
        RenderSettings.reflectionIntensity -= ((env_light_intensity - MinEnvLightIntensity) / num_stages); 

    }

    public void InitToStage(int num_stage)
    {
        if (num_stage > num_stages)
            currStage = num_stages;
        else
            currStage = num_stage;

        float color_amt = (ambient_intensity - MinAmbientIntensity) / num_stages;
        color_amt *= (num_stages - currStage);
        color_amt /= 255.0f;
        RenderSettings.ambientLight = new Color(color_amt, color_amt, color_amt, color_amt);


        dirLight.intensity = (dir_light_intensity - MinDirLightIntensity) * ((num_stages - currStage)/num_stages);
        player_dirLight.intensity = (player_dir_light_intensity - MinPlayerDirLightIntensity) * ((num_stages - currStage) / num_stages);
        RenderSettings.reflectionIntensity = (env_light_intensity - MinEnvLightIntensity) * ((num_stages - currStage) / num_stages);
    }
}
