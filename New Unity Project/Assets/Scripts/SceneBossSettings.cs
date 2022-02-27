using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBossSettings : MonoBehaviour
{
    void Start()
    {
        // Scene Related Inits Before Start of Each Level
        GameHandler.instance.lightManager.InitToFinalStage();
    }

}
