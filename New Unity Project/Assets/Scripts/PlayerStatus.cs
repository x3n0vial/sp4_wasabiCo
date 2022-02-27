using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus 
{

    static bool is_chased = false;
    static GameObject chasing_enemy = null;

    public static void ActivateChaseStatus(GameObject enemy)
    {
        if (!is_chased)
        {
            is_chased = true;
            chasing_enemy = enemy;
            GameHandler.instance.BGMManager.SwitchBGM2();
            Debug.Log("Player being chased");
        }
    }


    public static void DeactivateChaseStatus(GameObject enemy)
    {
        if (chasing_enemy == enemy)
        {
            is_chased = false;
            chasing_enemy = null;
            GameHandler.instance.BGMManager.FadeOut();
            GameHandler.instance.BGMManager.SwitchBGM1();
            Debug.Log("Player no longer being chased");
        }
    }

}
