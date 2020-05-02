using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestConnector : DatabaseConnector
{
    private LoginData currentLoginData;

    public RestConnector()
    {
        currentLoginData = new LoginData();
    }

    public override void PostUser(UserData user, Action callback)
    {
        RestClient.Put($"{databaseURL}users/{user.Id}.json?auth={currentLoginData.idToken}", user)
            .Then(response => 
            {
                if(response.StatusCode == 200)
                {
                    callback();
                }
                else
                {
                    RefreshLogin(currentLoginData.refreshToken, login => 
                    {
                        PostUser(user, callback);
                    });
                }
            });
    }

    public override void GetUser(string userId, Action<UserData> callback)
    {
        RestClient.Get($"{databaseURL}users/{userId}.json?auth={currentLoginData.idToken}")
            .Then(response => 
            {
                if(response.StatusCode == 200)
                {
                    var user = JsonUtility.FromJson<UserData>(response.Text);
                    callback(user);
                }
                else
                {
                    RefreshLogin(currentLoginData.refreshToken, login =>
                    {
                        GetUser(userId, callback);
                    });
                }
            });
    }

    public override void LoginAnonymously(Action<LoginData> callback)
    {
        var body = new AnonymousSignupBody() {returnSecureToken = true};
        RestClient.Post(loginURL, body)
            .Then(response => 
            {
                if(response.StatusCode == 200)
                {
                    var login = JsonUtility.FromJson<LoginData>(response.Text);
                    currentLoginData = login;
                    callback(login);
                }
                else
                {
                    //TODO Handle failed login
                }
            });
    }

    public override void RefreshLogin(string refreshToken, Action<LoginData> callback)
    {
        var body = new RefreshIdTokenBody() {grant_type = "refresh_token", refresh_token = refreshToken};
        RestClient.Post(refreshURL, body)
            .Then(response => 
            {
                if(response.StatusCode == 200)
                {
                    var refreshData = JsonUtility.FromJson<RefreshData>(response.Text);
                    currentLoginData = refreshData.ToLoginData();
                    callback(currentLoginData);
                }
                else
                {
                    //TODO Handle failed refresh
                }
            });
    }

    public override void PostSingleScore(SingleScoreData score, Action callback)
    {
        RestClient.Put($"{databaseURL}scores.json?auth={currentLoginData.idToken}", score)
            .Then(response => 
            {
                if(response.StatusCode == 200)
                {
                    callback();
                }
                else
                {
                    RefreshLogin(currentLoginData.refreshToken, login => 
                    {
                        PostSingleScore(score, callback);
                    });
                }
            });
    }

    public override void GetTopScores(int amount, Action<ScoreCollection> callback)
    {
        RestClient.Get($"{databaseURL}scores.json?orderBy=\"score\"&limitToLast={amount.ToString()}")
            .Then(response => 
            {
                if(response.StatusCode == 200)
                {
                    //rather ugly work around as Unity's built in json utility can't deserialize json arrays
                    var json = "{\"scores\":" + response.Text + "}";
                    var scores = JsonUtility.FromJson<ScoreCollection>(json);
                    callback(scores);
                }
                else
                {
                    RefreshLogin(currentLoginData.refreshToken, login =>
                    {
                        GetTopScores(amount, callback);
                    });
                }
            });
    }
}
