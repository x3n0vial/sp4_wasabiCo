using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneForestSettings
{
    public static bool[] rockStatus = new bool[4];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void ResetForest()
    {
        Rock.numRock = 0;
    }
}
