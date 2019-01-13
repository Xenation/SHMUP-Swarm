using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Swarm
{
    public class bossLife : MonoBehaviour
    {
        [SerializeField]
        private int pv;
        public int phase2Threshhold;
        public int phase3Threshhold;
        private int currentPhase = 1;
        public bool isPart = true;
        private Animator animator;
        public Camera cam;
        
        public PlayerSwarm player;

        public float openingDuration = 5;
        private float openingTime = 2;
        private bool isAnimationEnd = false;

        private bool inHitStun = false;
        private Material mat;

        public float hitStunDuration = 0.05f;
        private float hitStunFirstFrame = 0;

        private float ScoreTimer = 0;

        public GameObject endCristalPrefab;

        //Ajouter une référence vers chaque part du boss


        private void Awake()
        {
            ScoreTimer = Time.time;
        }
        // Start is called before the first frame update
        void Start()
        {
            animator = gameObject.GetComponent<Animator>();
            SpriteRenderer rend = gameObject.GetComponent<SpriteRenderer>();
            mat = rend.material;
            AkSoundEngine.SetState("BossPhase", "Phase1");


            if (ScoreManager.bossPhase == 2)
            {
                pv = phase2Threshhold;
            }
            else if (ScoreManager.bossPhase == 3)
            {
                pv = phase3Threshhold;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!isPart && isAnimationEnd && Time.time - openingTime > openingDuration)
            {
                animator.SetBool("isOpen", false);
                AkSoundEngine.PostEvent("Play_BossClose", gameObject);
                isPart = true;
                isAnimationEnd = false;

                foreach (GameObject part in GameObject.FindGameObjectsWithTag("part"))
                {
                    part.GetComponent<partController>().resetPart();
                }

                CheckPhase();
            }

            if (currentPhase == 1)
            {
                AkSoundEngine.SetState("BossPhase", "Phase1");
            }
            else if (currentPhase == 2)
            {
                AkSoundEngine.SetState("BossPhase", "Phase2");
            }
            else if (currentPhase == 3)
            {
                AkSoundEngine.SetState("BossPhase", "Phase3");
            }


            if (inHitStun == true)
            {
                hitstun();
            }
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            if (!isPart && isAnimationEnd && col.gameObject.layer == LayerMask.NameToLayer("ProjectileUnit"))
            {
                lowerPV();
                hitstun();
            }
        }

        void lowerPV()
        {
            pv--;
            if (pv <= 0)
            {

                Invoke("End", 1.0f);

                //Destroy(this.gameObject);
            }
            else
            {
                //INSERER SON DEGAT SUR BOSS
                AkSoundEngine.PostEvent("Play_CoeurHit", gameObject);
            }
        }

        public void End()
        {
            ScoreTimer = Time.time - ScoreTimer;
            ScoreManager.endTime = Time.time;
            ScoreManager.bossDead = true;
            //Envoyez le score dans la prochaine scene + leaderboard

            //Spawn a item that when hit, brings you to win screen
            Instantiate(endCristalPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);

            Destroy(this.gameObject);

            //SceneManager.LoadScene("Win");
            AkSoundEngine.SetState("BossPhase", "Outro");

        }

        public void checkParts()
        {
            isPart = false;
            foreach (GameObject part in GameObject.FindGameObjectsWithTag("part"))
            {
                if (!part.GetComponent<partController>().isDestroyed)
                {
                    isPart = true;
                }
            }

            if (!isPart)
            {
                openingTime = Time.time;
                foreach (GameObject part in GameObject.FindGameObjectsWithTag("part"))
                {
                    animator.SetBool("isOpen", true);
                    //part.SetActive(false);
                    AkSoundEngine.PostEvent("Play_BossOpen", gameObject);               
                }

            }
        }

        private void hitstun()
        {
            if (!inHitStun)
            {
                inHitStun = true;
                mat.SetFloat("_ReplaceAmount", 1.0f);

                hitStunFirstFrame = Time.time;
            }
            else if (Time.time > (hitStunFirstFrame + hitStunDuration))
            {
                inHitStun = false;
                mat.SetFloat("_ReplaceAmount", 0.0f);
                //cam.transform.position =new Vector3(0, 0, -10);
            }
            else
            {
                //ScreenShake
                cam.transform.position += new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));


            }
        }

        private void CheckPhase()
        {
            if (pv <= phase3Threshhold)
            {
                //Change sprite and patterns to second phase
                Debug.Log("changed phase to 3");
                currentPhase = 3;
                ScoreManager.bossPhase = 3;
            }
            else if (pv <= phase2Threshhold)
            {
                //Change sprite and patterns to third phase
                Debug.Log("phase 2");
                currentPhase = 2;
                ScoreManager.bossPhase = 2;
            }
            else
            {
                currentPhase = 1;
                ScoreManager.bossPhase = 1;
            }
        }

        private void OnDestroy()
        {
            //Debug.Log("died");
            //SceneManager.LoadScene("Win");
        }

        public void AlertObservers(string message)
        {
            if (message.Equals("AttackAnimationEnded"))
            {
                isAnimationEnd = true;
            }
        }
    }

}
