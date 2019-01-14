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
		public int health { get { return pv; } }
        public int phase2Threshhold;
        public int phase3Threshhold;
        private int currentPhase = 1;
		private List<partController> parts = new List<partController>(4);
        public bool hasPartsAlive = true;
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

		public delegate void EventNotify();
		public event EventNotify OnStunStarted;
		public event EventNotify OnStunEnded;

        //Ajouter une référence vers chaque part du boss


        private void Awake()
        {
			GetComponentsInChildren(parts);
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
            if (!hasPartsAlive && isAnimationEnd && Time.time - openingTime > openingDuration)
            {
                animator.SetBool("isOpen", false);
                AkSoundEngine.PostEvent("Play_BossClose", gameObject);
                hasPartsAlive = true;
                isAnimationEnd = false;

                foreach (GameObject part in GameObject.FindGameObjectsWithTag("part"))
                {
                    part.GetComponent<partController>().resetPart();
                }
				
				OnStunEnded?.Invoke();
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
            if (!hasPartsAlive && isAnimationEnd && col.gameObject.layer == LayerMask.NameToLayer("ProjectileUnit"))
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

			GetComponent<Boss>().Die();

            //Spawn a item that when hit, brings you to win screen
            Instantiate(endCristalPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity, transform);

            //SceneManager.LoadScene("Win");
            AkSoundEngine.SetState("BossPhase", "Outro");
            AkSoundEngine.PostEvent("Stop_SFX", gameObject);

        }

        public void checkParts()
        {
			hasPartsAlive = false;
			foreach (partController part in parts) {
				if (!part.isDestroyed) {
					hasPartsAlive = true;
				}
			}

            if (!hasPartsAlive)
            {
                openingTime = Time.time;
				animator.SetBool("isOpen", true);
				AkSoundEngine.PostEvent("Play_BossOpen", gameObject);
				OnStunStarted?.Invoke();
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

		public void SetPhase(int phase) {
			currentPhase = phase;
			ScoreManager.bossPhase = phase;
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
