using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Pause Menu Animation")]
    public GameObject pauseMenuParent;
    public Transform pauseMenuPanel;
    public float panelDropHeight;
    public float panelDropDuration;

    [Header("Confirmation Dialog")]
    public GameObject confirmationDialogParent;
    public TextMeshProUGUI confirmationTitle;
    public Button confirmationButton;
    public SceneManager sceneManager;

    [Header("Music Settings")]
    public Button toggleMusicButton;
    public Image musicButtonImage;
    public TextMeshProUGUI musicHeader;
    public Sprite musicOn;
    public Sprite musicOff;

    [Header("SFX Settings")]
    public Button toggleSfxButton;
    public Image sfxButtonImage;
    public TextMeshProUGUI sfxHeader;
    public Sprite sfxOn;
    public Sprite sfxOff;

    [Header("Quick Restart")]
    public Button quickRestartButton;
    public string quickRestartTitle;

    [Header("Back to Menu")]
    public string backToMenuTitle;

    private void Awake()
    {
        toggleMusicButton.onClick.AddListener(() => ToggleMusic());
        toggleSfxButton.onClick.AddListener(() => ToggleSfx());

        UpdateMusicButton();
        UpdateSfxButton();
    }

    private void Update()
    {
        if(Treasury.instance.gameOver)
        {
            return;
        }
        
        if(Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.P))
        {
            if(pauseMenuParent.activeInHierarchy)
            {
                HidePauseMenu();
            }
            else
            {
                ShowPauseMenu();
            }
        }
    }

    public void ShowPauseMenu()
    {
        pauseMenuParent.SetActive(true);
        StartCoroutine(SimpleAnimations.instance.Translate(pauseMenuPanel, panelDropDuration, new Vector3(0f, -1 * panelDropHeight, 0f)));
    }

    public void HidePauseMenu()
    {
        StartCoroutine(SimpleAnimations.instance.Translate(pauseMenuPanel, panelDropDuration, new Vector3(0f, panelDropHeight, 0f), 1f, () => pauseMenuParent.SetActive(false)));
    }

    public void ShowConfirmationDialog()
    {
        confirmationDialogParent.SetActive(true);
    }

    public void HideConfirmationDialog()
    {
        confirmationDialogParent.SetActive(false);
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
        if(AudioManager.instance != null && AudioManager.instance.IsMusicOn)
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
        if(AudioManager.instance != null && AudioManager.instance.AreSfxOn)
        {
            sfxButtonImage.sprite = sfxOn;
        }
        else
        {
            sfxButtonImage.sprite = sfxOff;
        }
    }

    public void BackToMenu()
    {
        confirmationButton.onClick.RemoveAllListeners();
        confirmationButton.onClick.AddListener(() => GoBackToMenu());
        confirmationTitle.text = backToMenuTitle;
        ShowConfirmationDialog();
    }

    public void QuickRestart()
    {
        confirmationButton.onClick.RemoveAllListeners();
        confirmationButton.onClick.AddListener(() => Restart());
        confirmationTitle.text = quickRestartTitle;
        ShowConfirmationDialog();
    }

    private void Restart()
    {
        sceneManager.LoadScene("SampleScene");
    }

    private void GoBackToMenu()
    {
        sceneManager.LoadScene("MainMenu");
    }
}
