using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSettings
{
    // Keybkinds
    public static KeyCode FLASHLIGHT_KEY = KeyCode.F;
    public static KeyCode PICKUP_ITEM_KEY = KeyCode.E;
    public static KeyCode THROW_ITEM_KEY = KeyCode.Mouse0;

    // Camera
    public static float MOUSE_SENSITIVITY = 5.0f;
    public static bool CAMERA_MOUSEPAN_ENABLED = false;

    // Checkpoints
    public static Checkpoint currentCheckpoint = null;

    // Game Variables
    public static int PLAYER_POINTS = 0;
}
