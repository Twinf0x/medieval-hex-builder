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

    internal void Post<T>(string url, T requestBody, Action onSuccess = null, Action onFailure = null)
    {
        RestClient.Post(url, requestBody)
            .Then(response =>
            {
                if(response.StatusCode == 200)
                {
                    onSuccess?.Invoke();
                }
                else if(response.StatusCode == 401 || response.StatusCode == 403)
                {
                    RefreshLogin(currentLoginData.refreshToken, login => Post<T>(url, requestBody, onSuccess, onFailure));
                }
                else
                {
                    onFailure?.Invoke();
                }
            });
    }

    internal void Post<T, U>(string url, T requestBody, Action<U> onSuccess = null, Action onFailure = null)
    {
        RestClient.Post(url, requestBody)
            .Then(response =>
            {
                if(response.StatusCode == 200)
                {
                    var deserializedResponse = JsonUtility.FromJson<U>(response.Text);
                    onSuccess?.Invoke(deserializedResponse);
                }
                else if(response.StatusCode == 401 || response.StatusCode == 403)
                {
                    RefreshLogin(currentLoginData.refreshToken, login => Post<T, U>(url, requestBody, onSuccess, onFailure));
                }
                else
                {
                    onFailure?.Invoke();
                }
            });
    }

    internal void Put<T>(string url, T requestBody, Action onSuccess = null, Action onFailure = null)
    {
        Debug.Log("Put");
        RestClient.Put(url, requestBody)
            .Then(response =>
            {
                Debug.Log("Successful call");
                onSuccess?.Invoke();
            })
            .Catch(error => {
                var requestError = error as RequestException;
                
                if(requestError.StatusCode == 401 || requestError.StatusCode == 403)
                {
                    Debug.Log("Need to refresh login");
                    RefreshLogin(currentLoginData.refreshToken, login => Put<T>(url, requestBody, onSuccess, onFailure));
                }
                else
                {
                    Debug.Log("Failed call");
                    onFailure?.Invoke();
                }
            });
    }

    internal void Put<T, U>(string url, T requestBody, Action<U> onSuccess = null, Action onFailure = null)
    {
        RestClient.Put(url, requestBody)
            .Then(response =>
            {
                if(response.StatusCode == 200)
                {
                    var deserializedResponse = JsonUtility.FromJson<U>(response.Text);
                    onSuccess?.Invoke(deserializedResponse);
                }
                else if(response.StatusCode == 401 || response.StatusCode == 403)
                {
                    RefreshLogin(currentLoginData.refreshToken, login => Put<T, U>(url, requestBody, onSuccess, onFailure));
                }
                else
                {
                    onFailure?.Invoke();
                }
            });
    }

    internal void Get(string url, Action onSuccess = null, Action onFailure = null)
    {
        RestClient.Get(url)
            .Then(response =>
            {
                if(response.StatusCode == 200)
                {
                    onSuccess?.Invoke();
                }
                else if(response.StatusCode == 401 || response.StatusCode == 403)
                {
                    RefreshLogin(currentLoginData.refreshToken, login => Get(url, onSuccess, onFailure));
                }
                else
                {
                    onFailure?.Invoke();
                }
            });
    }

    internal void Get<T>(string url, Action<T> onSuccess = null, Action onFailure = null)
    {
        RestClient.Get(url)
            .Then(response =>
            {
                if(response.StatusCode == 200)
                {
                    T deserializedResponse = JsonUtility.FromJson<T>(response.Text);
                    onSuccess?.Invoke(deserializedResponse);
                }
                else if(response.StatusCode == 401 || response.StatusCode == 403)
                {
                    RefreshLogin(currentLoginData.refreshToken, login => Get<T>(url, onSuccess, onFailure));
                }
                else
                {
                    onFailure?.Invoke();
                }
            });
    }

    internal void GetString(string url, Action<string> onSuccess = null, Action onFailure = null)
    {
        RestClient.Get(url)
            .Then(response =>
            {
                if(response.StatusCode == 200)
                {
                    onSuccess?.Invoke(response.Text);
                }
                else if(response.StatusCode == 401 || response.StatusCode == 403)
                {
                    RefreshLogin(currentLoginData.refreshToken, login => GetString(url, onSuccess, onFailure));
                }
                else
                {
                    onFailure?.Invoke();
                }
            });
    }

    public override void PostUser(UserData user, Action onSuccess = null, Action onFailure = null)
    {
        Debug.Log("PostUser");
        var url = $"{databaseURL}users/{user.Id}.json?auth={currentLoginData.idToken}";
        Put<UserData>(url, user, onSuccess, onFailure);
    }

    public override void GetUser(string userId, Action<UserData> onSuccess = null, Action onFailure = null)
    {
        var url = $"{databaseURL}users/{userId}.json?auth={currentLoginData.idToken}";
        Get<UserData>(url, onSuccess, onFailure);
    }

    public override void LoginAnonymously(Action<LoginData> onSuccess = null, Action onFailure = null)
    {
        var body = new AnonymousSignupBody() {returnSecureToken = true};
        Post<AnonymousSignupBody, LoginData>(loginURL, body, (loginData) => {this.currentLoginData = loginData; onSuccess?.Invoke(loginData); }, onFailure);
    }

    public override void RefreshLogin(string refreshToken, Action<LoginData> onSuccess = null, Action onFailure = null)
    {
        var body = new RefreshIdTokenBody() {grant_type = "refresh_token", refresh_token = refreshToken};
        Post<RefreshIdTokenBody, LoginData>(refreshURL, body, (loginData) => {this.currentLoginData = loginData; onSuccess?.Invoke(loginData); }, onFailure);
    }

    public override void PostSingleScore(SingleScoreData score, Action onSuccess = null, Action onFailure = null)
    {
        Debug.Log("PostSingleScore called");
        var url = $"{databaseURL}scores.json?auth={currentLoginData.idToken}";
        Post<SingleScoreData>(url, score, onSuccess, onFailure);
    }

    public override void GetTopScores(int amount, Action<ScoreCollection> onSuccess = null, Action onFailure = null)
    {
        var url = $"{databaseURL}scores.json?orderBy=\"score\"&limitToLast={amount.ToString()}";
        GetString(
            url, 
            (response) => {
                var json = "{\"scores\":" + formatScoreResponse(response) + "}";
                var scores = JsonUtility.FromJson<ScoreCollection>(json);
                onSuccess?.Invoke(scores);
            }, 
            onFailure);
    }

    private string formatScoreResponse(string responseText)
    {
        responseText = responseText.Remove(0, 1);

        var scoreObjects = responseText.Split('}');
        string result = "";
        foreach (string scoreObject in scoreObjects)
        {
            if(string.IsNullOrEmpty(scoreObject))
            {
                continue;
            }

            if(result.Length > 0)
            {
                result += ',';
            }

            var objectValue  = scoreObject.Substring(scoreObject.IndexOf('{'));

            result +=  objectValue + "}";
        }

        return $"[{result}]";
    }
}
