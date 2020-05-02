using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardRow : MonoBehaviour
{
    public TextMeshProUGUI rankText;
    public TextMeshProUGUI userNameText;
    public TextMeshProUGUI scoreText;

    public void SetData(LeaderboardRowData data)
    {
        rankText.text = data.rank.ToString();
        userNameText.text = data.userName;
        scoreText.text = data.score.ToString();
    }

    public void SetRank(int rank)
    {
        rankText.text = rank.ToString();
    }

    public void SetUserName(string userName)
    {
        userNameText.text = userName;
    }

    public void SetScore(int score)
    {
        scoreText.text = score.ToString();
    }
}
