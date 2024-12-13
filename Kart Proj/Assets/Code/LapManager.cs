using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class LapManager : MonoBehaviour
{
    public int maxLaps;
    public int curLaps;

    List<float> lapTimes = new List<float>();

    public Checkpoint nextCheckPointToReach;

    private int currentCheckpointIndex;
    [SerializeField]
    private List<Checkpoint> checkpoints;
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

                Debug.Log("-----------------------");
                Debug.Log(transform.parent.name);
                for (int i = 0; i < lapTimes.Count; i++)
                {
                    TimeSpan t = TimeSpan.FromMilliseconds(lapTimes[i]);
                    string answer = string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D3}",
                                            t.Hours,
                                            t.Minutes,
                                            t.Seconds,
                                            t.Milliseconds);
                    Debug.Log($"Lap {i} - {answer}");
                }
                Debug.Log("-----------------------");
                Destroy(transform.parent.gameObject);
            } else
            {
                if (FindAnyObjectByType<GateScript>())
                    FindAnyObjectByType<GateScript>().HandleGateLogic(curLaps);

                if (curLaps == maxLaps)
                {
                    transform.parent.GetComponentInChildren<DebugCanvas>().LaspLap();
                    AudioManager.Instance.PlaySfx("lastLap");
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
