using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DatabaseConnector
{
    [SerializeField] internal const string projectId = "hexagon-hamlet";
    [SerializeField] internal const string apiKey = "AIzaSyCv4NlKTWObBjVkJmVerDPVe3KG-vYwOzY";
    internal static readonly string databaseURL = $"https://{projectId}.firebaseio.com/";
    internal static readonly string loginURL = $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={apiKey}";
    internal static readonly string refreshURL = $"https://securetoken.googleapis.com/v1/token?key={apiKey}";

    public abstract void PostUser(UserData user, Action onSuccess = null, Action onFailure = null);
    public abstract void GetUser(string userId, Action<UserData> onSuccess = null, Action onFailure = null);
    public abstract void LoginAnonymously(Action<LoginData> onSuccess = null, Action onFailure = null);
    public abstract void RefreshLogin(string refreshToken, Action<LoginData> onSuccess = null, Action onFailure = null);

    public abstract void PostSingleScore(SingleScoreData score, Action onSuccess = null, Action onFailure = null);
    public abstract void GetTopScores(int amount, Action<ScoreCollection> onSuccess = null, Action onFailure = null);
}
