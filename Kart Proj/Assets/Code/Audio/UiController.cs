using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    public Slider musicSlider, sfxSlider;

    private void Start()
    {
        musicSlider.value = AudioManager.Instance.musicSource.volume;
        sfxSlider.value = AudioManager.Instance.sfxSource.volume;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ExitSettings();
    }

    public void ToggleMusic()
    {
        AudioManager.Instance.ToggleMusic();
    }

    public void ToggleSfx()
    {
        AudioManager.Instance.ToggleSfx();
    }

    public void MusicVolume()
    {
        AudioManager.Instance.MusicVolume(musicSlider.value);
    }

    public void SfxVolume()
    {
        AudioManager.Instance.SfxVolume(sfxSlider.value);
    }

    public void ExitSettings()
    {
        SceneManager.LoadScene(1);
    }
}
