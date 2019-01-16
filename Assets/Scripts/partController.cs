﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swarm
{
    public class partController : MonoBehaviour
    {

        [SerializeField]
        private int pv = 1;
        public int basepv = 1;
        public bool isDestroyed = false;
        public Camera cam;

        private bool inHitStun = false;
        private bool inDestroyShake = false;
        private float hitStunFirstFrame = 0;
        private float destroyFirstFrame = 0;
        public float hitStunDuration = 0.05f;
        public float destroyDuration = 0.2f;
        private Material mat;
        public Animator explosion_fx;

		public float regenTime = .5f;
		[Tooltip("Full Heath Sprites For Each Phase")]
		public Sprite[] healedSprites;
		[Tooltip("Destroyed Sprites For Each Phase")]
		public Sprite[] destroyedSprites;

		private Boss boss;
		private SpriteRenderer sprRenderer;
		private float regenStartTime = 0f;
		private bool isRegenerating = false;
        public List<Animator> sparklesList;
        private float lastSparkleTime ;
        public float sparkleTimerDuration = 0.7f;

        private void Awake()
        {
            SpriteRenderer rend = gameObject.GetComponent<SpriteRenderer>();
            mat = rend.material;
            boss = GetComponentInParent<Boss>();
            sprRenderer = GetComponent<SpriteRenderer>();
        }

        // Start is called before the first frame update
        void Start()
        {
            pv = basepv;
        }

        // Update is called once per frame
        void Update()
        {
            if (inHitStun)
                hitstun();
            if (inDestroyShake)
                destroyShake();
			if (isRegenerating) {
				float progress = (Time.time - regenStartTime) / regenTime;
				if (progress >= 1f) {
					isRegenerating = false;
					sprRenderer.sprite = healedSprites[boss.phaseIndex];
					mat.SetFloat("_TransitionHeight", 0f);
				} else {
					mat.SetFloat("_TransitionHeight", progress);
				}
			}
            if (isDestroyed && Time.time - lastSparkleTime > sparkleTimerDuration)
            {
                lastSparkleTime = Time.time;
                int randSparkle = Random.Range(0, sparklesList.Count-1);
                sparklesList[randSparkle].SetTrigger("sparkleTrigger");
            }
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.layer == 11 && !isDestroyed)
            {
                lowerPV();
            }
        }

        void lowerPV()
        {
            pv--;
            if (pv <= 0)
            {
				Damaged();
            }
            else
            {
                hitstun();
                lastSparkleTime = Time.time;
                //INSERER SON DEGATS
                AkSoundEngine.PostEvent("Play_NormalHit", gameObject);
            }
        }

        public void ResetSprite()
        {
            sprRenderer.sprite = healedSprites[boss.phaseIndex];
        }

		private void Damaged() {
			
			//mat.SetFloat("_ReplaceAmount", 0.5f);
			
			destroyShake();
			explosion_fx.SetTrigger("explosion");

			//INSERER SON DESTRUCTION D'UNE PARTIE
			AkSoundEngine.PostEvent("Play_HardHit", gameObject);
            this.GetComponent<Collider2D>().isTrigger = true;
			sprRenderer.sprite = destroyedSprites[boss.phaseIndex];
            isDestroyed = true;
            transform.parent.GetComponent<bossLife>().checkParts();
            foreach (Animator spark in sparklesList)
            {
                spark.gameObject.SetActive(true);
            }
        }

		public void Heal() {
			isDestroyed = false;
			pv = basepv;
			mat.SetFloat("_ReplaceAmount", 0.0f);
            this.GetComponent<Collider2D>().isTrigger = false;
            //sprRenderer.sprite = healedSprites[boss.phaseIndex];
            mat.SetTexture("_SecondTex", healedSprites[boss.phaseIndex].texture);
			regenStartTime = Time.time;
			isRegenerating = true;
            foreach (Animator spark in sparklesList)
            {
                spark.gameObject.SetActive(false);
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
            }
            else
            {
                //ScreenShake
                cam.transform.position += new Vector3(Random.Range(-0.02f, 0.02f), Random.Range(-0.02f, 0.02f));


            }
        }

        private void destroyShake()
        {
            if (!inDestroyShake)
            {
                inDestroyShake = true;
                destroyFirstFrame = Time.time;
                inHitStun = false;
            }
            else if (Time.time > (destroyFirstFrame + destroyDuration))
            {
                inDestroyShake = false;
            }
            else
            {
                cam.transform.position += new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f));
            }
                
        }

        public void animationEnd(bool endAnimation)
        {

        }

		public void SetDisolveProgress(float progress) {
			mat.SetFloat("_DisolveAmount", progress);
		}

    }

}
