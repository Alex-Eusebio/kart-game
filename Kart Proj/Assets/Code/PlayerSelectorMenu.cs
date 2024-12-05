using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelectorMenu : MonoBehaviour
{
    private void Start()
    {
        PlayerPrefs.DeleteKey("PlayerCount");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(1);
        }
    }

    public void SelectPlayer(int i)
    {
        PlayerPrefs.SetInt("PlayerCount", i);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
