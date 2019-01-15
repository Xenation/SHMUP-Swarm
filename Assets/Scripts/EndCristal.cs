using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Swarm
{
    public class EndCristal : MonoBehaviour
    {
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
                SceneManager.LoadScene("Win");
            }
            
        }
    }
}

