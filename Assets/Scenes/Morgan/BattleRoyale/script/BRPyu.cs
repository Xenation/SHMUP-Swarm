using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BRPyu : MonoBehaviour
{
    public string team;
    public Tournament tournament;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(BRPyu pyu in tournament.contenders)
        {
            if (team != pyu.team)
            {

            }
        }
    }
}
