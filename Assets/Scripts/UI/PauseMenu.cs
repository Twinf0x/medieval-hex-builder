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
        if((bool) AudioManager.instance?.IsMusicOn)
        {
            musicButtonImage.sprite = musicOn;
            musicHeader.text = "Music On";
        }
        else
        {
            musicButtonImage.sprite = musicOff;
            musicHeader.text = "Music Off";
        }
    }

    private void UpdateSfxButton()
    {
        if((bool) AudioManager.instance?.AreSfxOn)
        {
            sfxButtonImage.sprite = sfxOn;
            sfxHeader.text = "SFX On";
        }
        else
        {
            sfxButtonImage.sprite = sfxOff;
            sfxHeader.text = "SFX Off";
        }
    }
}
