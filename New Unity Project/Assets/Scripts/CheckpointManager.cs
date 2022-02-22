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

    NIGHTMARE_ENTRY,
    NIGHTMARE_MORGUE,
    NIGHTMARE_LAB,

    BOSS_ENTRY,

    NUM_ID_TOTAL
}
