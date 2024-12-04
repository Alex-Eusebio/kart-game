using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Goal : MonoBehaviour
{
    float time;
    public int maxLaps = 3;
    [SerializeField]
    TextMeshProUGUI timerTxt;

    private void Start()
    {

    }

    private void Update()
    {
        time += Time.deltaTime*1000; 
        TimeSpan t = TimeSpan.FromMilliseconds(time);
        timerTxt.text = string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D3}",
                                t.Hours,
                                t.Minutes,
                                t.Seconds,
                                t.Milliseconds);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<LapManager>() != null)
        {
            other.GetComponent<LapManager>().Lap(time);
        }
    }
}
