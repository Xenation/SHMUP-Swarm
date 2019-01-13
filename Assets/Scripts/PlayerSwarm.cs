using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Swarm {
	public class PlayerSwarm : MonoBehaviour {

        public GameObject unitPrefab;
		public int unitsToCreate = 50;

        public float cursorShrinkSpeed = 2.5f;
        public float cursorShrinkRadius = 0.5f;
        public float unitShrinkSpeed = 8f;
        public float unitShrinkRadius = 0.5f;

        public float cursorNormalSpeed = 5f;
        public float cursorNormalRadius = 1f;
        public float unitNormalSpeed = 4f;
        public float unitNormalRadius = .1f;

		public float suicideSpeed = 10f;
        public float freezeTime = 1.0f;
		public Transform bossTransform;
        private int nbOfUnits;
  
		public bool debug = false;
        private bool inShrink = false;
        private int ShrinkUnits;

        [HideInInspector]
        public float cursorSpeed = 5f;
        public float cursorRadius = 1f;
        public float unitSpeed = 4f;
        public float unitRadius = .1f;

        [System.NonSerialized] public List<PlayerUnit> units = new List<PlayerUnit>();
		[System.NonSerialized] public Transform cursor;

		private Vector2 velocity;
		private Rigidbody2D cursorRB;

        private float rtpcValue = 5.0f;
        private float distanceToBoss;

        private bool inPause = false;
        private float defaultTimeScale;

        private SpriteRenderer cursorSprite;

        private PlayerUnit shrinkUnit;
        
		private void Awake() {
			cursor = transform.Find("Cursor");
			cursorRB = cursor.GetComponent<Rigidbody2D>();

			for (int i = 0; i < unitsToCreate; i++) {
				float perim = Random.Range(0f, Mathf.PI);
				float dist = Random.Range(0f, cursorRadius);
				Instantiate(unitPrefab, cursor.position + new Vector3(Mathf.Cos(perim) * dist, Mathf.Sin(perim) * dist), Quaternion.identity, transform);
			}

            defaultTimeScale = Time.fixedDeltaTime;
            cursorSprite = cursor.GetComponent<SpriteRenderer>();
			GetComponentsInChildren(units);

            cursorSpeed = cursorNormalSpeed;
            cursorRadius = cursorNormalRadius;
            unitSpeed = unitNormalSpeed;
            unitRadius = unitNormalRadius;

            shrinkUnit = new PlayerUnit();

            //Instantiate(unitPrefab, cursor.position, Quaternion.identity, transform);
            //shrinkUnit.gameObject.transform.localScale = new Vector3(2.0f, 2.0f, 1.0f);
            //shrinkUnit.gameObject.SetActive(false);
        }

		private void Update() {
			velocity.x = Input.GetAxisRaw("Horizontal");
			velocity.y = Input.GetAxisRaw("Vertical");
			//velocity.Normalize();

			if (Input.GetButtonDown("Fire1") && units.Count > 0 && cursorSpeed != cursorShrinkSpeed) {
                
				PlayerUnit unit = units[0];
				units.RemoveAt(0);
				unit.Suicide();
			}

            if (Input.GetButtonDown("Fire2"))
            {
                cursorSpeed = cursorShrinkSpeed;
                cursorRadius = cursorShrinkRadius;
                unitSpeed = unitShrinkSpeed;
                //unitRadius = unitShrinkRadius;

                //Second version of shrink
                inShrink = true;
                //units.Clear();
                cursorSprite.color = Color.blue;

            }

            if (inShrink)
            {
                //Kill all pyus and change cursor to a bigger pyu.
                Vector3 minDist = new Vector3(cursor.transform.position.x - cursorShrinkRadius, cursor.transform.position.y - cursorShrinkRadius, 0);
                Vector3 maxDist = new Vector3(cursor.transform.position.x + cursorShrinkRadius, cursor.transform.position.y + cursorShrinkRadius, 0);

                

                foreach (PlayerUnit unit in units)
                {
                    if (unit.transform.position.x <= maxDist.x &&
                        unit.transform.position.x >= minDist.x &&
                        unit.transform.position.y <= maxDist.y &&
                        unit.transform.position.y >= minDist.y)
                    {
                        Destroy(unit.gameObject);
                        ShrinkUnits++;
                    }

                }
            }

            if (Input.GetButtonUp("Fire2"))
            {
                cursorSpeed = cursorNormalSpeed;
                cursorRadius = cursorNormalRadius;
                unitSpeed = unitNormalSpeed;
                unitRadius = unitNormalRadius;

                //Respawn pyus and change sprite back to cursor
                for(int i = 0; i < ShrinkUnits; i++)
                {
                    float perim = Random.Range(0f, Mathf.PI);
                    float dist = Random.Range(0f, cursorRadius);
                    Instantiate(unitPrefab, cursor.position + new Vector3(Mathf.Cos(perim) * dist, Mathf.Sin(perim) * dist), Quaternion.identity, transform);
                }
                GetComponentsInChildren(units);
                inShrink = false;
                ShrinkUnits = 0;
                cursorSprite.color = Color.gray;
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                if (inPause)
                {
                    Time.timeScale = 1;
                    Time.fixedDeltaTime = defaultTimeScale;
                    inPause = !inPause;
                }
                else
                {
                    Time.timeScale = 0.001f;
                    Time.fixedDeltaTime = defaultTimeScale * 0.001f;
                    inPause = !inPause;
                }
            }

            velocity *= cursorSpeed;

            //Le nombre d'unitées est obtenable avec units.Count
            AkSoundEngine.SetRTPCValue("PyuNumber", units.Count);
            rtpcValue = Vector3.Distance(bossTransform.position, cursor.transform.position);
            AkSoundEngine.SetRTPCValue("ElectroFilter", rtpcValue);

            if(units.Count == 0 && !inShrink)
            {
                SceneManager.LoadScene("Lose");
            }
            
        }

		private void FixedUpdate() {
			cursorRB.velocity = velocity;
        }

        //This function must sent vibrations to the controller according to the distance between the cursor and attacks / death
        //Adding a second collider to cursor might be helpfull for optimization
        private void ProximityToDanger()
        {
            //Check if gameObjects containing OnKillCollision are close
            //Add vibration of length fixedDuration
        }


        public void AddUnit(GameObject unit)
        {
            PlayerUnit testUnit = unit.GetComponent<PlayerUnit>();
            if(testUnit)
                units.Add(testUnit);
        }

        public void RemoveUnit(PlayerUnit unit)
        {
            units.Remove(unit);
        }

        public int getNbOfUnits()
        {
            return units.Count;
        }
	}
}
