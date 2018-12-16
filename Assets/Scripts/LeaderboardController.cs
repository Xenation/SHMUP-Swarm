using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using TMPro;

public class LeaderboardController : MonoBehaviour
{
	private string textFromWWW;
	private string url = "https://raw.githubusercontent.com/Xenation/SHMUP-Swarm/master/Assets/Scripts/Leaderboard.txt?token=AXXqOuDnb07FcI_HV5OWQnWtZMnOu0G5ks5cH2NowA%3D%3D"; // <-- enter your url here
	private string[] nameScore;
	private GameObject[] playerScores;
	void Start()
	{
		playerScores = GameObject.FindGameObjectsWithTag("playerScore");
		StartCoroutine(GetTextFromWWW());
	}


	IEnumerator GetTextFromWWW()
	{
			WWW www = new WWW(url);

			yield return www;

			if (www.error != null)
			{
				Debug.Log("Ooops, something went wrong...");
			}
			else
			{
				textFromWWW = www.text;
			}
			string[] splitScores = textFromWWW.Split(';');
		int num = 0;
		foreach (string scores in splitScores)
		{
			if (scores != null)
			{
				nameScore = scores.Split(',');
				if (nameScore.Length == 2){
					playerScores[num].GetComponent<TextMeshProUGUI>().text = nameScore[0] + " " + nameScore[1];
				
				num++;
				}
			}
		}
	}
}

