using GameJolt.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
