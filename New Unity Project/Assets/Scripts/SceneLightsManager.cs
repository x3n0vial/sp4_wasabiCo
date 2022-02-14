using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLightsManager : MonoBehaviour
{


    private int num_stages = 4;
    private int currStage = 0;
   
   
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DarkenSceneLight()
    {
        currStage++;

        if (currStage > num_stages)
        {
            Debug.Log("Cannot Darken Scene Futher!");
            return;
        }
        float color_amt = 200.0f / num_stages;
        color_amt *= (num_stages - currStage);
        color_amt /= 255.0f;
        RenderSettings.ambientLight = new Color(color_amt, color_amt, color_amt, color_amt);
       // RenderSettings.ambientIntensity -= intensity_amt;
        Debug.Log("AmbientLight "+ RenderSettings.ambientLight);
      //  Debug.Log("Intensity"+ RenderSettings.ambientIntensity);
        
    }
}
