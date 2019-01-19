using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{

    private static float startTime;                         // Start of boss fight
    private static float pastTime = 0;                      //For checkpoints
    [HideInInspector] public static float endTime;          //Time at which the scene changed
    [HideInInspector] public static int bossPhase = 1;      //The boss's phase when scene changed
    [HideInInspector] public static bool bossDead = false;  //If the boss is dead
    [HideInInspector] public static int pyusAtLastPhase = 20;

    private static float gameScore = 0.0f;
    private static int leaderBScore = 0;
    public static string textScore = "";

    [HideInInspector] public static int nbPyuShot           = 0;  //done
    [HideInInspector] public static int nbPyuKilled         = 0;  //done
    [HideInInspector] public static int totalPyus           = 0;  //done
    [HideInInspector] public static int nbOfWins            = 0;  //done
    [HideInInspector] public static int nbOfGamesPlayed     = 0;  //done
    [HideInInspector] public static int nbOfLoses           = 0;  //done
    //[HideInInspector] public static int nbOfDeaths;
    [HideInInspector] public static float averageNbOfPyus   = 0; //MAYBE


    private void Start()
    {
        Object.DontDestroyOnLoad(this.gameObject);
        //read local file for player satts
        if (!PlayerPrefs.HasKey("nbPuyShot"))
        {
            //Set all keys
            PlayerPrefs.SetInt("nbPyuShot", nbPyuShot);
            PlayerPrefs.SetInt("nbPyuKilled", nbPyuKilled);
            PlayerPrefs.SetInt("totalPyus", totalPyus);
            PlayerPrefs.SetInt("nbOfWins", nbOfWins);
            PlayerPrefs.SetInt("nbOfGamesPlayed", nbOfLoses);
            PlayerPrefs.SetInt("nbOfLoses", nbOfGamesPlayed);
            PlayerPrefs.SetFloat("averageNbOfPyus", averageNbOfPyus);
        }
        else
        {
            //Get all keys
            nbPyuShot       = PlayerPrefs.GetInt("nbPyuShot");
            nbPyuKilled     = PlayerPrefs.GetInt("nbPyuKilled");
            totalPyus       = PlayerPrefs.GetInt("totalPyus");
            nbOfWins        = PlayerPrefs.GetInt("nbOfWins");
            nbOfGamesPlayed = PlayerPrefs.GetInt("nbOfGamesPlayed");
            nbOfLoses       = PlayerPrefs.GetInt("nbOfLoses");
            averageNbOfPyus = PlayerPrefs.GetFloat("averageNbOfPyus");
        }
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
                pyusAtLastPhase = 20;
            }
        }


        if(scene.name == "Lose")
        {
            nbOfLoses++;
            pastTime += (endTime - startTime);
            //nbOfLoses++;
        }

        if(scene.name == "Win")
        {
            nbOfWins++;
            setScore(pastTime + (endTime - startTime));
        }

        if(scene.name == "Menu")
        {
            resetScore();
            bossDead = false;
            bossPhase = 1;
            pastTime = 0;
            pyusAtLastPhase = 20;
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

    private void OnDestroy()
    {
        PlayerPrefs.SetInt("nbPyuShot", nbPyuShot);
        PlayerPrefs.SetInt("nbPyuKilled", nbPyuKilled);
        PlayerPrefs.SetInt("totalPyus", totalPyus);
        PlayerPrefs.SetInt("nbOfWins", nbOfWins);
        PlayerPrefs.SetInt("nbOfGamesPlayed", nbOfGamesPlayed);
        PlayerPrefs.SetFloat("averageNbOfPyus", averageNbOfPyus);
    }
}
