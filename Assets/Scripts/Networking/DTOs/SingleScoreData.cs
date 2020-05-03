using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SingleScoreData
{
    public string Id;
    public string userId;
    public string userName;
    public int score;

    public SingleScoreData(string Id, string userId, string userName, int score)
    {
        this.Id = Id;
        this.userId = userId;
        this.userName = userName;
        this.score = score;
    }
}
