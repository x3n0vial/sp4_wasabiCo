using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBossSettings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameHandler.instance.lightManager.InitToStage(3);
    }

}