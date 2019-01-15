﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Swarm
{
    public class tutorialScript : MonoBehaviour
    {
        public PlayerSwarm swarm;
        public bossLife boss;
        public GameObject pickupSpawner;
        public Text description;
        private float timeSpent;
        public float startTime = 2.0f;
        private int part = 0;
        private bool thumbStickMoved = false;
        private Vector3 originalPos;
        private PickupSpawner spawner;
        private CameraController Cc;
        private DistanceJoint2D playerMovement;
        // Start is called before the first frame update
        void Start()
        {
            description.text = "";
            timeSpent = Time.time;
            originalPos = swarm.cursor.position;
            spawner = pickupSpawner.GetComponent<PickupSpawner>();
            Cc = GetComponent<CameraController>();
            Cc.X = swarm.cursor.position.x;
            Cc.Y = swarm.cursor.position.y;
            boss.gameObject.SetActive(false);
            playerMovement = swarm.cursor.GetComponent<DistanceJoint2D>();
            playerMovement.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {

            if(Time.time - timeSpent > startTime && part == 0)
            {
                description.text = "These are pyus.\nThey are both your munitions and lives.";
                part++;
                timeSpent = Time.time;
            }
            else if (Time.time - timeSpent > startTime && part == 1)
            {
                description.text = "Move with the left thumbstick.";
                

                if (swarm.cursor.position != originalPos)
                {
                    thumbStickMoved = true;
                    part++;
                }
                    
                
            }
            else if (thumbStickMoved && part == 2)
            {
                description.text = "Go on this pikcup to get more pyus.";
                part++;
                spawner.spawnAt(new Vector3(swarm.transform.position.x + 0.1f, swarm.transform.position.y + 0.1f)); //swarm.cursor.position.z));
            }
            else if ( spawner.nbOfPickups == 0 && part == 3)
            {
                description.text = "You can transform into a bigger Pyu by holding Right Trigger. \nIn this form you are stronger.\nHowever you are slower, and you can't shoot.";
                part++;
            }
            else if(Input.GetButtonUp("Fire2") && part == 4)
            {
                description.text = "Here is your enemy.\nPress A to shoot a pyu.";

                boss.gameObject.SetActive(true);
                playerMovement.enabled = true;
                //Add boss and change camera
                Cc.X = 0;
                Cc.Y = 0;

                part++;
            }
            else if(Input.GetButtonUp("Fire1") && part == 5)
            {
                description.text = "You must destroy all 4 parts of it's armor.";
                part++;
            }
            else if ( !boss.hasPartsAlive && part == 6)
            {
                description.text = "Then attack it it's heart.";
                part++;
            }
            else if ( boss ==null && part == 7)
            {
                description.text = "Now let's go for real.\nDon't get all your pyus killed.\nGood Luck !";
            }
        }
    }

}
