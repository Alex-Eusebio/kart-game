using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class LapManager : MonoBehaviour
{
    public int characterId;
    public int maxLaps;
    public int curLaps;
    
    List<float> lapTimes = new List<float>();

    public Checkpoint nextCheckPointToReach;

    public int currentCheckpointIndex;
    public List<Checkpoint> checkpoints;
    public Checkpoint lastCheckpoint;

    private void Start()
    {
        maxLaps = FindAnyObjectByType<Goal>().maxLaps;
        checkpoints = FindObjectOfType<Goal>().checkpointList;
        ResetCheckpoints();
    }

    public void Lap(float time)
    {
        if (nextCheckPointToReach == null || curLaps == 0)
        {
            if (curLaps > 0)
                lapTimes.Add(time);

            curLaps++;
            ResetCheckpoints();

            if (curLaps == maxLaps + 1)
            {
                if (!FindObjectOfType<Goal>().someoneComplete)
                {
                    AudioManager.Instance.StopMusic();
                    AudioManager.Instance.PlayMusic("raceComplete");
                    FindObjectOfType<Goal>().someoneComplete = true;
                }

                FindObjectOfType<Goal>().SendPlayer(characterId, lapTimes);
                Destroy(transform.parent.gameObject);
            }
            else
            {
                if (FindAnyObjectByType<GateScript>())
                    FindAnyObjectByType<GateScript>().HandleGateLogic(curLaps);

                if (curLaps == maxLaps)
                {
                    transform.parent.GetComponentInChildren<DebugCanvas>().LaspLap();

                    if (!FindObjectOfType<Goal>().someoneLastLap)
                    {
                        AudioManager.Instance.PlaySfx("lastLap");
                        FindObjectOfType<Goal>().someoneLastLap = true;
                    }

                } 
            }
        }
    }

    public void ResetCheckpoints()
    {
        currentCheckpointIndex = 0;
        SetNextCheckpoint();
    }

    private void SetNextCheckpoint()
    {
        if (checkpoints.Count > 0)
        {
            nextCheckPointToReach = checkpoints[currentCheckpointIndex];
        }
    }

    public void CheckPointReached(Checkpoint checkpoint)
    {
        if (nextCheckPointToReach != checkpoint) return;

        if (currentCheckpointIndex < checkpoints.Count-1)
        {
            lastCheckpoint = checkpoints[currentCheckpointIndex];
            currentCheckpointIndex++;

            if (currentCheckpointIndex < checkpoints.Count)
                SetNextCheckpoint();
        } else
        {
            lastCheckpoint = checkpoints[currentCheckpointIndex];
            nextCheckPointToReach = null;
        }
    }
}
