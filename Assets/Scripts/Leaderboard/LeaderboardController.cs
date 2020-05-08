using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LeaderboardController : MonoBehaviour
{
    public GameObject rowPrefab;
    public Transform leaderboardContainer;
    public TextMeshProUGUI loadingHint;
    public int rowHeight;
    public int rowAmount = 10;

    private List<LeaderboardRow> rows;
    private RestConnector connector;

    private void Start()
    {
        connector = new RestConnector();
        rows = new List<LeaderboardRow>();
    }

    public void ResetLeaderboard()
    {
        ClearLeaderboard();
        LoadLeaderboard();
    }

    public void ClearLeaderboard()
    {
        foreach (LeaderboardRow row in rows)
        {
            Destroy(row.gameObject);
        }
    }

    public void LoadLeaderboard()
    {
        loadingHint.gameObject.SetActive(true);
        connector.GetTopScores(rowAmount, scores => DisplayLeaderboard(scores));
    }

    public void DisplayLeaderboard(ScoreCollection scoreCollection)
    {
        var scoreDataList = new List<SingleScoreData>(scoreCollection.scores).Where(scoreData => scoreData != null);
        scoreDataList = scoreDataList.OrderByDescending(scoreData => scoreData.score);

        foreach (SingleScoreData scoreData in scoreDataList)
        {
            var rank = rows.Count + 1;
            AddRow(new LeaderboardRowData(rank, scoreData.userName, scoreData.score));
        }

        loadingHint.gameObject.SetActive(false);
    }

    public void AddRow(LeaderboardRowData rowData)
    {
        var rowObject = Instantiate(rowPrefab, leaderboardContainer.position, Quaternion.identity, leaderboardContainer);
        var yPosition = -1 * (rowData.rank - 1) * rowHeight;
        rowObject.transform.Translate(new Vector3(0f, yPosition, 0f));

        var row = rowObject.GetComponent<LeaderboardRow>();
        row.SetData(rowData);

        rows.Add(row);
    }
}
