using GameJolt.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    public GameObject stats;

    // Start is called before the first frame update
    void Start()
    {
        int scoreValue = 400;
        string scoreText = "6 min 40";
        int tableID = 395523;
        string guestName = "Mamene";
        string extraData = "";

        //GameJolt.API.Scores.Add(scoreValue, scoreText, guestName, tableID, extraData, (bool success) => {
        //    Debug.Log(string.Format("Score Add {0}.", success ? "Successful" : "Failed"));
        //});

        GameJoltUI.Instance.ShowLeaderboards();
        
        stats.transform.Find("totalPyus").GetComponent<Text>().text = "Total Pyus collected : " + ScoreManager.totalPyus;
        stats.transform.Find("wins").GetComponent<Text>().text = "Number of Wins : " + ScoreManager.nbOfWins;
        stats.transform.Find("shot").GetComponent<Text>().text = "Number of Pyus shot : " + ScoreManager.nbPyuShot;
        stats.transform.Find("killed").GetComponent<Text>().text = "Numer of Pyus killed : " + ScoreManager.nbPyuKilled;
        stats.transform.Find("totalPlayed").GetComponent<Text>().text = "Number of games played : " + ScoreManager.nbOfGamesPlayed;

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
