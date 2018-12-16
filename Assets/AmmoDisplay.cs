using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Swarm
{
    public class AmmoDisplay : MonoBehaviour
    {

        public Text text;
        public PlayerSwarm ps;

        private void Start()
        {
            text.text = "";
        }

        public void FixedUpdate()
        {
            text.text = ""+ps.getNbOfUnits();

            text.transform.position = ps.cursor.transform.position;
        }
    }
}