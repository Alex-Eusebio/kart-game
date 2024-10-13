using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool isOnTurn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CheckpointManager>() != null)
        {
            other.GetComponent<CheckpointManager>().CheckPointReached(this);
        }
    }
}
