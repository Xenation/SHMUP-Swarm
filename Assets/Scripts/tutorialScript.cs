using System.Collections;
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
        public Image l;
        public Image a;
        public Image rt;
        public Image pickup;
        public Lazer lazer;

        private float timeSpent;
        public float startTime = 2.0f;
        private int part = 0;
        private bool thumbStickMoved = false;
        private Vector3 originalPos;
        private PickupSpawner spawner;
        private CameraController Cc;
        private DistanceJoint2D playerMovement;
        private int prevUnitCount;

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
            a.enabled = false;
            l.enabled = false;
            rt.enabled = false;
            pickup.enabled = false;
            AkSoundEngine.SetState("BossPhase", "Phase1");
        }

        // Update is called once per frame
        void Update()
        {

            if(Time.time - timeSpent > startTime && part == 0)
            {
                description.text = "These are your units.\nThey are both your munitions and lives!";
                part++;
                timeSpent = Time.time;
            }
            else if (Time.time - timeSpent > startTime && part == 1)
            {
                description.text = "Move with the         left thumbstick or DIRECTINAL KEYS";
                l.enabled = true;

                if (swarm.cursor.position != originalPos)
                {
                    thumbStickMoved = true;
                    part++;
                }
                timeSpent = Time.time;
                    
                
            }
            else if (thumbStickMoved && (Time.time - timeSpent) > startTime  && part == 2)
            {
                l.enabled = false;
                description.text = "Go on          to have more units";
                pickup.enabled = true;
                part++;
                spawner.spawnAt(new Vector3(swarm.transform.position.x + 0.1f, swarm.transform.position.y + 0.1f)); //Change position of the pickup according to the player.
                timeSpent = Time.time;
                prevUnitCount = swarm.units.Count;
            }
            else if (swarm.units.Count > prevUnitCount && part == 3)
            {
                pickup.enabled = false;
                description.text = "You can transform into a bigger unit by holding         or RIGHT CLICK \n In this form you are stronger.\nHowever you are slower, and you can't shoot.";
                part++;
                rt.enabled = true;
            }

            //ADD PART TO SHOW THAT SHRINK HELPS AGAINST LAZERS
            else if (Input.GetButtonUp("Fire2") && part == 4)
            {
                rt.enabled = false;
                description.text = "Lazer on the right side, go through it in shrink not to die";
                part++;

                //Activate lazer
                //Instantiate();
            }
            else if(swarm.cursor.localPosition.x > 11 && part == 5) //Check position of player (if he is to the right of the lazer)
            {
                //Deactivate lazer

                description.text = "Here is your enemy.\nPress       or LEFT CLICK to shoot an unit.\nYou automaticly shoot towards the boss";
                a.enabled = true;

                boss.gameObject.SetActive(true);
                playerMovement.enabled = true;

                Cc.X = 0;
                Cc.Y = 0;

                part++;
            }
            else if(Input.GetButtonUp("Fire1") && part == 6)
            {
                a.enabled = false;
                description.text = "You must destroy all 4 parts of its armor.";
                part++;
            }
            else if ( !boss.hasPartsAlive && part == 7)
            {
                description.text = "Then attack its heart.";
                part++;
            }
            else if ( boss ==null && part == 8)
            {
                description.text = "Let's go for real now!\nDon't get all your units killed!\n";
            }
        }
    }

}
