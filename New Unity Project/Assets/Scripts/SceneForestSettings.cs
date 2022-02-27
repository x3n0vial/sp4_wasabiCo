using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneForestSettings
{
    public static bool[] rockStatus = new bool[4];
  

    public static void ResetForest()
    {
        Rock.numRock = 0;
    }
}
