using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tournament : MonoBehaviour
{
    public GameObject pyuPrefab;

    public Text winText;

    [HideInInspector] public List<BRPyu> contenders;

    public int nbOfContendersPerTeam = 5;

    [System.Serializable]
    public class Team
    {
        public string team;
        public Color color;
    }

    public Team[] Teams;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Team team in Teams)
        {
            for(int i = 0; i < nbOfContendersPerTeam; i++)
            {
                Vector3 pos = new Vector3(transform.position.x + Random.Range(-15, 15), transform.position.y + (Random.Range(-15, 15)), 0);
                GameObject pyu = Instantiate(pyuPrefab, pos, Quaternion.identity, transform); //ADD random position
                pyu.GetComponent<BRPyu>().team = team.team;
                pyu.GetComponent<BRPyu>().tournament = this;
                pyu.GetComponent<BRPyu>().color = team.color;
                //pyu.GetComponent<SpriteRenderer>().color = team.color;

            }
        }
        //Adds contenders to list
        GetComponentsInChildren(contenders);
    }

    // Update is called once per frame
    void Update()
    {
        if(nbOfTeamsLeft() == 1)
        {
            Debug.Log("winners");
            //Afficher équipe gagnant.
            winText.color = contenders[0].color;
            winText.text = contenders[0].team + " wins !";
        }
        else if(nbOfTeamsLeft() == 0)
        {
            winText.text = "Draw !";
            //Afficher aucun gagnant.
        }
    }

    public int nbOfTeamsLeft()
    {
        List<string> teamNames = new List<string>();
        teamNames.Add(contenders[0].team);

        foreach (BRPyu pyu in contenders)
        {
            bool exists = false;
            foreach(string name in teamNames)
            {
                if(name == pyu.team)
                {
                    exists = true;
                }
            }
            if (!exists)
            {
                teamNames.Add(pyu.team);
            }
        }

        return teamNames.Count;
    }
}
