using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapManager : MonoBehaviour
{
    [SerializeField]
    int maxLaps;
    [SerializeField]
    int curLaps;

    List<float> lapTimes = new List<float>();

    private void Start()
    {
        maxLaps = FindAnyObjectByType<Goal>().maxLaps;
    }

    public void Lap(float time)
    {
        if (curLaps > 0)
            lapTimes.Add(time);

        curLaps++;

        if (curLaps == maxLaps+1)
        {
            Debug.Log(transform.parent.name);
            for (int i = 0;  i < lapTimes.Count; i++)
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
        }
    }
}
