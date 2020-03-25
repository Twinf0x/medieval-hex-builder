using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string gameSceneName;

    private void Start()
    {
        AudioManager.instance?.StopAll();
        AudioManager.instance?.Play("MenuTheme");
    }

    public void OpenGameScene() {
        AudioManager.instance?.Stop("MenuTheme");
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameSceneName);
    }
}
