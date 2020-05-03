using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubmitScoreDialog : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public Button submitButton;

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
        
        SetupNewUser(username, SubmitCurrentScore);
        submitButton.interactable = false;
        usernameInput.interactable = false;
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
        connector.PostSingleScore(data, () => Debug.Log("Submitted score"));
    }
}
