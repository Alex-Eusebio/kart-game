using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TMPro;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class Goal : MonoBehaviour
{
    float time = -2200;
    bool triggeredMusic = false;
    public int maxLaps = 3;
    float maxPlayers = 1;

    public List<Checkpoint> checkpointList;

    [SerializeField]
    private string musicName;
    private bool isPause = false;
    public bool someoneComplete = false;
    public bool someoneLastLap = false;

    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject leaderBoardMenu;
    [Header("Finish Race")]
    public List<PlayerComplete> players;
    public GameObject[] characters;  // Array de personagens na cena
    public Transform[] spawnPoints; // Pontos de spawn

    private void Start()
    {
        pauseMenu.SetActive(false);

        for (int i = 0; i < 4; i++)
        {
            if (PlayerPrefs.HasKey("SelectedCharacter" + i))
            {
                maxPlayers = i+1;
            }
        }

        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlaySfx("raceStart");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (players.Count < maxPlayers)
                TogglePause();
            else
            {
                if (!isPause)
                    HandleLeaderBoard();
                else
                    ExitTrack();
            }
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
        AudioManager.Instance.PlayMusic("mainMenu");
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

    private void FixedUpdate()
    {
        if (!isPause || !(players.Count >= maxPlayers))
            time += Time.deltaTime * 1000;
    }

    public string GetTimer()
    {
        if (time >= 0 && !triggeredMusic)
        {
            triggeredMusic = true;
            AudioManager.Instance.PlayMusic(musicName);
        }

        CountDown();
        if (!(players.Count >= maxPlayers))
            return TransformTime(time);
        else return "";
    }

    public int GetPosition(LapManager lapManag)
    {
        LapManager[] manag = FindObjectsOfType<LapManager>();

        int asking = -1;
        List<PlayerDist> car = new List<PlayerDist>();
        foreach (LapManager l in manag)
        {
            if (lapManag == l)
                asking = l.characterId;
            PlayerDist temp;
            if (l.nextCheckPointToReach)
            {
                temp = new PlayerDist(l.characterId,
                l.curLaps,
                l.currentCheckpointIndex,
                Vector3.Distance(l.transform.position, l.nextCheckPointToReach.transform.position));
            } else
            {
                temp = new PlayerDist(l.characterId,
                l.curLaps,
                l.currentCheckpointIndex+1,
                0);
            }

            car.Add(temp);
        }

        car = car
        .OrderByDescending(p => p.lap) // Descending by lap
        .ThenByDescending(p => p.curCheckpoint) // Descending by curCheckpoint
        .ThenBy(p => p.curDistance) // Ascending by curDistance
        .ToList();

        return car.FindIndex(p => p.id == asking);
    }

    private string TransformTime(float time)
    {
        TimeSpan t = TimeSpan.FromMilliseconds(time);
        return string.Format("{0:D2}:{1:D2}:{2:D2}",
                                t.Minutes,
                                t.Seconds,
                                t.Milliseconds);
    }

    public void SendPlayer(int id, List<float> times)
    {
        string[] timesS = new string[times.Count];

        for (int i = 0;  i < times.Count; i++)
        {
            timesS[i] = TransformTime(times[i]);
        }

        PlayerComplete p = new PlayerComplete(id, timesS);
        players.Add(p);

        if (players.Count >= maxPlayers) {
            HandleFinish();
        }
    }

    private void HandleFinish()
    {
        int i = 0;
        foreach (PlayerComplete p in players)
        {
            characters[p.id].SetActive(true);
            characters[p.id].transform.position = spawnPoints[i].position;
            characters[p.id].transform.rotation = spawnPoints[i].rotation;

            i++;
        }
    }

    private void HandleLeaderBoard()
    {
        isPause = true;
        int i = 0;
        foreach (PlayerComplete p in players)
        {
            leaderBoardMenu.SetActive(true);
            leaderBoardMenu.GetComponent<Leaderboard>().SetTime(i, p);

            i++;
        }
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

[Serializable]
public struct PlayerComplete
{
    public int id;
    public string[] times;

    public PlayerComplete(int id, string[] times)
    {
        this.id = id;
        this.times = times;
    }
}

struct PlayerDist
{
    public int id;
    public int lap;
    public int curCheckpoint;
    public float curDistance;

    public PlayerDist(int id, int lap, int curCheckpoint, float curDistance)
    {
        this.id = id;
        this.lap = lap;
        this.curCheckpoint = curCheckpoint;
        this.curDistance = curDistance;
    }
}