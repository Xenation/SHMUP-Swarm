using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Swarm
{
    public class EndCristal : MonoBehaviour
    {
        public bool tutorial;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            PlayerUnit pyu = collision.gameObject.GetComponent<PlayerUnit>();
            if (pyu)
            {
                if (tutorial)
                {
					SceneSwitcher.SwitchScene("Menu");
                }
                else
                {
					SceneSwitcher.SwitchScene("Win", 2f, 1f);
                }
            }
            
        }
    }
}

