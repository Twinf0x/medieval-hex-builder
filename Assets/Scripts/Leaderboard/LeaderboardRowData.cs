using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LeaderboardRowData
{
    public int rank;
    public string userName;
    public int score;

    public LeaderboardRowData(int rank, string userName, int score)
    {
        this.rank = rank;
        this.userName = userName;
        this.score = score;
    }
}
