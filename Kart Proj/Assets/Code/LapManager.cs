using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapManager : MonoBehaviour
{
    int maxLaps;
    [SerializeField]
    int curLaps;

    List<float> lapTimes = new List<float>();

    public void SetMaxLaps(int maxLaps)
    {
        this.maxLaps = maxLaps;
    }

    public void Lap(float time)
    {
        if (curLaps > 0)
            lapTimes.Add(time);

        curLaps++;

        if (curLaps == maxLaps+1)
        {
            for (int i = 0;  i < lapTimes.Count; i++)
            {
                TimeSpan t = TimeSpan.FromMilliseconds(lapTimes[i]);
                string answer = string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D3}",
                                        t.Hours,
                                        t.Minutes,
                                        t.Seconds,
                                        t.Milliseconds);
                Debug.Log(answer);
            }
        }
    }
}
