using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CheckpointManager
{
    static List<Checkpoint> checkpoints = new List<Checkpoint>();
    public static CheckpointID last_ID = CheckpointID.UNSET;

    public static void AddCheckpoint(Checkpoint cp)
    {
        checkpoints.Add(cp);
        if (cp.ID == last_ID)
            GameSettings.currentCheckpoint = cp;
    }

    public static void InitCheckpoints()
    {
        Debug.Log("Initting Checkpoints...");
        Debug.Log("checkpoint count: " + checkpoints.Count);
        foreach (Checkpoint cp in checkpoints)
        {
            if (cp.ID <= last_ID)
            {
                cp.Unlock();
                if (GameSettings.currentCheckpoint == null ||
                    ( GameSettings.currentCheckpoint != null
                    && cp.ID > GameSettings.currentCheckpoint.ID))
                {
                    GameSettings.currentCheckpoint = cp;
                }
            }
        }
    }

    public static void ClearCheckpoints()
    {
        checkpoints.Clear();
    }
}

public enum CheckpointID
{
    UNSET,
    FOREST_ENTRY,

    SCENE_NIGHTMARE_START,

    NIGHTMARE_ENTRY,
    NIGHTMARE_MORGUE,
    NIGHTMARE_LAB,

    SCENE_BOSS_START,

    BOSS_ENTRY,

    TEST1,
    TEST2,

    NUM_ID_TOTAL
}
