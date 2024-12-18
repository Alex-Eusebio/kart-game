using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public float MaxTimeToReachNextCheckpoint = 30f;
    public float FirstTimeToReachNextCheckpoint = 3f;
    public float TimeLeft = 30f; 
    int maxLaps;
    [SerializeField]
    int curLaps;

    public KartAgent kartAgent;
    public Checkpoint nextCheckPointToReach;

    private int CurrentCheckpointIndex;
    private List<Checkpoint> Checkpoints;
    private Checkpoint lastCheckpoint;

    public event Action<Checkpoint> reachedCheckpoint;

    void Start()
    {
        Checkpoints = FindObjectOfType<Checkpoints>().checkPoints;
        ResetCheckpoints();
    }

    public void SetMaxLap(int laps)
    {
        maxLaps = laps;
    }

    public void ResetCheckpoints()
    {
        CurrentCheckpointIndex = 0;
        TimeLeft = FirstTimeToReachNextCheckpoint;

        SetNextCheckpoint();
    }

    private void Update()
    {
        if (!kartAgent.carSystem.isStunned)
            TimeLeft -= Time.deltaTime;

        if (TimeLeft < 0f)
        {
            kartAgent.AddReward(-5f);
            kartAgent.EndEpisode();
            curLaps = 0;
        }
    }

    public void CheckPointReached(Checkpoint checkpoint)
    {
        if (nextCheckPointToReach != checkpoint) return;

        lastCheckpoint = Checkpoints[CurrentCheckpointIndex];
        reachedCheckpoint?.Invoke(checkpoint);
        CurrentCheckpointIndex++;

        if (CurrentCheckpointIndex >= Checkpoints.Count)
        {
            kartAgent.AddReward(1f * Checkpoints.Count);
            curLaps++;

            if (curLaps >= maxLaps)
            {
                Debug.Log($"{gameObject.transform.parent.name} COMPLETED! ({kartAgent.GetCumulativeReward()})");
                curLaps = 0;
                kartAgent.EndEpisode();
            } else
            {
                Debug.Log($"{gameObject.transform.parent.name} Completed the {curLaps} lap! ({kartAgent.GetCumulativeReward()})");
                ResetCheckpoints();
            }
        }
        else
        {
            kartAgent.AddReward(0.4f * Checkpoints.Count);
            SetNextCheckpoint();
        }
    }

    private void SetNextCheckpoint()
    {
        if (Checkpoints.Count > 0)
        {
            if (CurrentCheckpointIndex == 0)
                TimeLeft = FirstTimeToReachNextCheckpoint;
            else if (CurrentCheckpointIndex > 0)
                TimeLeft = MaxTimeToReachNextCheckpoint;

            nextCheckPointToReach = Checkpoints[CurrentCheckpointIndex];

        }
    }
}
