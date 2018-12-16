using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Swarm
{
    public class AmmoDisplay : MonoBehaviour
    {

        public float distance = 0.2f;
        public Text text;
        public PlayerSwarm ps;

        private void Start()
        {
            text.text = "";
        }

        public void FixedUpdate()
        {
            text.text = ""+ps.getNbOfUnits();

            Vector3 vectDir = ps.cursor.transform.position;
            vectDir.Normalize();

            Vector3 textPos = ps.cursor.transform.position + vectDir * distance;

            text.transform.position = textPos;
        }
    }
}