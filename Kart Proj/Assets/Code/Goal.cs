using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Goal : MonoBehaviour
{
    float time = -5000;
    public int maxLaps = 3;

    public List<Checkpoint> checkpointList;

    [SerializeField]
    private string musicName;

    private void Start()
    {
        AudioManager.Instance.PlayMusic(musicName);
    }

    public string GetTimer()
    {
        time += Time.deltaTime * 1000;
        CountDown();
        TimeSpan t = TimeSpan.FromMilliseconds(time);
        return string.Format("{0:D2}:{1:D2}:{2:D2}",
                                t.Minutes,
                                t.Seconds,
                                t.Milliseconds);
    }

    private void CountDown()
    {
        if (time > 0)
        {
            foreach (PlayerInputHandler p in FindObjectsOfType<PlayerInputHandler>())
            {
                p.playerHasControll = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<LapManager>() != null)
        {
            other.GetComponent<LapManager>().Lap(time);
        }
    }
}
