using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{

    private static float startTime;                     // Start of boss fight
    private static float pastTime = 0;                      //For checkpoints
    [HideInInspector] public static float endTime;      //Time at which the scene changed
    [HideInInspector] public static int bossPhase = 1;      //The boss's phase when scene changed
    [HideInInspector] public static bool bossDead = false;      //If the boss is dead

    private static float gameScore = 0.0f;
    private static int leaderBScore = 0;
    public static string textScore = "";

    private void Start()
    {
        Object.DontDestroyOnLoad(this.gameObject);
    }


    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "PlayScene")
        {
            startTime = Time.time;

            if (bossDead)
            {
                pastTime = 0;
                bossDead = false;
                bossPhase = 1;
            }
        }


        if(scene.name == "Lose")
        {
            pastTime += (endTime - startTime);
        }

        if(scene.name == "Win")
        {
            setScore(pastTime + (endTime - startTime));
        }

        if(scene.name == "Menu")
        {
            resetScore();
            bossDead = false;
            bossPhase = 1;
            pastTime = 0;
        }
    }

    public static void setScore(float time)
    {
        resetScore();
        gameScore = time;
        leaderBScore = (int)time;
        int tmpMin =(int) (leaderBScore / 60.0f);
        int tmpSec = (int) (leaderBScore - (tmpMin * 60));
        textScore += tmpMin + " min " + tmpSec;
    }

    public static int getScore()
    {
        return leaderBScore;
    }

    public static void resetScore()
    {
        gameScore = 0;
        leaderBScore = 0;
        textScore = "";
    }

    public static void sendScore(string name)
    {
        int scoreValue = leaderBScore;
        string scoreText = textScore;
        int tableID = 395523;
        string guestName = name;
        string extraData = "";

        GameJolt.API.Scores.Add(scoreValue, scoreText, guestName, tableID, extraData, (bool success) => {
            Debug.Log(string.Format("Score Add {0}.", success ? "Successful" : "Failed"));
        });
    }
}
