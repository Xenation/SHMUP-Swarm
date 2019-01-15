using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Swarm
{
    public class tutorialScript : MonoBehaviour
    {
        public PlayerSwarm swarm;
        public GameObject pickupSpawner;
        public Text description;
        private float timeSpent;
        public float startTime = 2.0f;
        private int part = 0;
        private bool thumbStickMoved = false;
        private Vector3 originalPos;
        private PickupSpawner spawner;
        // Start is called before the first frame update
        void Start()
        {
            description.text = "";
            timeSpent = Time.time;
            originalPos = swarm.cursor.position;
            spawner = pickupSpawner.GetComponent<PickupSpawner>();
        }

        // Update is called once per frame
        void Update()
        {
            if(Time.time - timeSpent > startTime && part == 0)
            {
                description.text = "units";
                part++;
                timeSpent = Time.time;
            }
            else if (Time.time - timeSpent > startTime && part == 1)
            {
                description.text = "Move with left thumbstick";
                

                if (swarm.cursor.position != originalPos)
                {
                    thumbStickMoved = true;
                    part++;
                }
                    
                
            }
            else if (thumbStickMoved && part == 2)
            {
                description.text = "Go on this pikcup to get more pyu's";
                part++;
                spawner.spawnAt(new Vector3(swarm.cursor.position.x + 1, swarm.cursor.position.y, swarm.cursor.position.z));
            }
            else if ( spawner.nbOfPickups == 0 && part == 3)
            {
                description.text = "shrink";
                part++;
            }
            else if(Input.GetButtonUp("Fire2") && part == 4)
            {
                description.text = "Wow ur a genius !";
            }
        }
    }

}
