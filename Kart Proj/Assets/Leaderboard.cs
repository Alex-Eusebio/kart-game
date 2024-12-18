using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    [SerializeField]
    Sprite[] icons;
    [SerializeField]
    PlayerLB[] leaderboard;

    public void SetTime(int pos, PlayerComplete player)
    {
        leaderboard[pos].SetLb(player.times[player.times.Length - 1], icons[player.id]);
    }
}

[Serializable]
public struct PlayerLB
{
    public Image icon;
    public TextMeshProUGUI time;

    public void SetLb(string time, Sprite sprite)
    {
        this.time.text = time;
        icon.sprite = sprite;

        icon.transform.parent.gameObject.SetActive(true);
    }
}