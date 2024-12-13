using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    float time = -5000;
    bool triggeredMusic = false;
    public int maxLaps = 3;

    public List<Checkpoint> checkpointList;

    [SerializeField]
    private string musicName;
    private bool isPause = false;

    [SerializeField]
    private GameObject pauseMenu;

    private void Start()
    {
        pauseMenu.SetActive(false);

        AudioManager.Instance.PlaySfx("raceStart");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (Input.GetKeyDown(KeyCode.Return) && isPause)
        {
            ExitTrack();
        }
    }

    public void ExitTrack()
    {
        SceneManager.LoadScene(3);
        AudioManager.Instance.StopAllSounds();
    }

    public void TogglePause()
    {
        isPause = !isPause;

        CarSystem[] cars = FindObjectsOfType<CarSystem>();

        foreach (CarSystem c in cars)
        {
            if (c is not AICarSystem)
                c.transform.parent.GetComponentInChildren<Camera>().enabled = !isPause;
        }
        pauseMenu.SetActive(isPause);
    }

    public string GetTimer()
    {
        if (!isPause)
            time += Time.deltaTime * 1000;

        if (time >= 0 && !triggeredMusic)
        {
            triggeredMusic = true;
            AudioManager.Instance.PlayMusic(musicName);
        }

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
