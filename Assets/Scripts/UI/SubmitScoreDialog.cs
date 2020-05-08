using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubmitScoreDialog : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TextMeshProUGUI usernameText;
    public Color inputActiveColor;
    public Color inputDisabledColor;
    public Button submitButton;
    public GameObject submitButtonText;
    public Image uploadStatusImage;

    public Sprite uploadOngoing;
    public Sprite uploadSuccessful;
    public Sprite uploadFailed;

    private DatabaseConnector connector;
    private LoginData loginData;
    private UserData userData;

    private bool ValidateUsername(string username)
    {
        if(string.IsNullOrEmpty(username))
        {
            return false;
        }

        return true;
    }

    private void Start()
    {
        Initialize();
        connector = new RestConnector();
    }

    private void Initialize()
    {
        usernameInput.onValueChanged.AddListener(ValidateForm);
        submitButton.onClick.AddListener(ClickedSubmit);
    }

    public void ValidateForm(string username)
    {
        if(!ValidateUsername(username))
        {
            submitButton.interactable = false;
            return;
        }

        submitButton.interactable = true;
    }

    public void ClickedSubmit()
    {
        ClickedSubmit(usernameInput.text);
    }

    public void ClickedSubmit(string username)
    {
        if(!ValidateUsername(usernameInput.text))
        {
            //TODO indicate invalid username
            return;
        }

        uploadStatusImage.sprite = uploadOngoing;
        uploadStatusImage.gameObject.SetActive(true);
        submitButtonText.SetActive(false);
        
        SetupNewUser(username, SubmitCurrentScore);
        submitButton.interactable = false;
        usernameInput.interactable = false;
        usernameText.color = inputDisabledColor;
    }

    public void SetupNewUser(string username, Action callback)
    {
        connector.LoginAnonymously(loginData => 
        {
            this.loginData = loginData;
            this.userData = new UserData(loginData.localId, username);
            PlayerPrefs.SetString("RefreshToken", loginData.refreshToken);
            connector.PostUser(userData, callback);
        });
    }

    private void SubmitCurrentScore()
    {
        var Id = Guid.NewGuid().ToString();
        var score = new SingleScoreData(Id, this.userData.Id, this.userData.name, Treasury.instance.currentMoney);
        SubmitScore(score);
    }

    private void SubmitScore(SingleScoreData data)
    {
        connector.PostSingleScore(data, OnScoreSuccessfullySubmitted, OnScoreSubmissionFailed);
    }

    private void OnScoreSuccessfullySubmitted()
    {
        uploadStatusImage.sprite = uploadSuccessful;
    }

    private void OnScoreSubmissionFailed()
    {
        uploadStatusImage.sprite = uploadFailed;
        
        StartCoroutine(ResetAfterSeconds(2f));
    }

    private IEnumerator ResetAfterSeconds(float timer)
    {
        yield return new WaitForSeconds(timer);

        uploadStatusImage.gameObject.SetActive(false);
        submitButtonText.SetActive(true);

        usernameInput.text = string.Empty;
        usernameInput.interactable = true;
        usernameText.color = inputActiveColor;

    }
}
