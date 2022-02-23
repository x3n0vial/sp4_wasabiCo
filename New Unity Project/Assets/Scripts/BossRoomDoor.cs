using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomDoor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameHandler.instance.levelLoader.LoadNextLevel("SceneBossRoom");
        }
    }
}
