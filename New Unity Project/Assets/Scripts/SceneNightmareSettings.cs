using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneNightmareSettings : MonoBehaviour
{
    public SceneLightsManager lightsManager;

    // Start is called before the first frame update
    void Start()
    {
        lightsManager.InitToStage(3);
        GameSettings.CAMERA_MOUSEPAN_ENABLED = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
