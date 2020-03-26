using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Scene Management")]
    public string gameSceneName;

    [Header("Music Settings")]
    public Button toggleMusicButton;
    public Image musicButtonImage;
    public Sprite musicOn;
    public Sprite musicOff;

    [Header("SFX Settings")]
    public Button toggleSfxButton;
    public Image sfxButtonImage;
    public Sprite sfxOn;
    public Sprite sfxOff;

    private void Start()
    {
        toggleMusicButton.onClick.AddListener(() => ToggleMusic());
        toggleSfxButton.onClick.AddListener(() => ToggleSfx());

        UpdateMusicButton();
        UpdateSfxButton();

        AudioManager.instance?.StopAll();
        AudioManager.instance?.Play("MenuTheme");
    }

    public void OpenGameScene() {
        AudioManager.instance?.Stop("MenuTheme");
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameSceneName);
    }

    public void ToggleMusic()
    {
        AudioManager.instance?.ToggleMusic();
        UpdateMusicButton();
    }

    public void ToggleSfx()
    {
        AudioManager.instance?.ToggleSfx();
        UpdateSfxButton();
    }

    private void UpdateMusicButton()
    {
        if((bool) AudioManager.instance?.IsMusicOn)
        {
            musicButtonImage.sprite = musicOn;
        }
        else
        {
            musicButtonImage.sprite = musicOff;
        }
    }

    private void UpdateSfxButton()
    {
        if((bool) AudioManager.instance?.AreSfxOn)
        {
            sfxButtonImage.sprite = sfxOn;
        }
        else
        {
            sfxButtonImage.sprite = sfxOff;
        }
    }
}
