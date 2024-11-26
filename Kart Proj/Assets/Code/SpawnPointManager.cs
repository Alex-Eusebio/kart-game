using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    public Transform[] spawnPoints;
    [SerializeField]
    int maxLaps;

    private void Start()
    {
        SetMaxLaps();
    }

    public Vector3 SelectRandomSpawnpoint()
    {
        int rnd = Random.Range(0, spawnPoints.Length);
        return spawnPoints[rnd].position;
    }

    private void SetMaxLaps()
    {
        foreach (Transform t in spawnPoints)
        {
            t.GetComponentInChildren<CheckpointManager>().SetMaxLap(maxLaps);
        }
    }
}
