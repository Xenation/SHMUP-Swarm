using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static float gameScore = 0.0f;
    private static int leaderBScore = 0;
    private static string textScore = "";
    

    public static void setScore(float time)
    {
        gameScore = time;
        leaderBScore = (int)time;
        float tmpMin = leaderBScore / 60.0f;
        int tmpSec = (int) (leaderBScore - tmpMin);
        textScore += (int)tmpMin + " min " + tmpSec;
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
}
