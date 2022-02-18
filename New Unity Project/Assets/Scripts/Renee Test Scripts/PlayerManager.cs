using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//gets the player for the current scene and sets it
//to use code in scripts:
//1) make variable: Transform target;
//2) call in the start function: target = PlayerManager.instance.player.transform;
//if still don't understand, can refer to the Minimap.cs script
//now don't need to keep creating public player objects in the script and keep having to drag and drop the player constantly KSDNFJKSNGSJKN

public class PlayerManager : MonoBehaviour
{
    #region Singleton

    public static PlayerManager instance;
    void Awake()
    {
        instance = this;   
    }
    #endregion

    public GameObject player;
}
