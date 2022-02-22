using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//gets the player for the current scene and sets it
//to use code in scripts:
//1) make variable: Transform target;
//2) call in the start function: target = GameHandler.instance.player.transform;
//if still don't understand, can refer to the Minimap.cs script
//now don't need to keep creating public player objects in the script and keep having to drag and drop the player constantly KSDNFJKSNGSJKN

public class GameHandler : MonoBehaviour
{
    #region Singleton

    public static GameHandler instance;
    void Awake()
    {
        instance = this;
    }
    #endregion

    public GameObject player;
    public CameraSettings cameraSettings;
    public Flashlight flashlight;
    public SceneLightsManager lightManager;
    public Camera mainCamera;
    public Camera UICamera;
    public LevelLoader levelLoader;
}
