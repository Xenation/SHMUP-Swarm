using System.Collections.Generic;
using UnityEngine;

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
		public Transform bossTransform;
        private int nbOfUnits;

		public bool debug = false;

        [HideInInspector]
        public float cursorSpeed = 5f;
        public float cursorRadius = 1f;
        public float unitSpeed = 4f;
        public float unitRadius = .1f;

        [System.NonSerialized] public List<PlayerUnit> units = new List<PlayerUnit>();
		[System.NonSerialized] public Transform cursor;

		private Vector2 velocity;
		private Rigidbody2D cursorRB;

		private void Awake() {
			cursor = transform.Find("Cursor");
			cursorRB = cursor.GetComponent<Rigidbody2D>();

			for (int i = 0; i < unitsToCreate; i++) {
				float perim = Random.Range(0f, Mathf.PI);
				float dist = Random.Range(0f, cursorRadius);
				Instantiate(unitPrefab, cursor.position + new Vector3(Mathf.Cos(perim) * dist, Mathf.Sin(perim) * dist), Quaternion.identity, transform);
			}

			GetComponentsInChildren(units);

            cursorSpeed = cursorNormalSpeed;
            cursorRadius = cursorNormalRadius;
            unitSpeed = unitNormalSpeed;
            unitRadius = unitNormalRadius;
        }

		private void Update() {
			velocity.x = Input.GetAxisRaw("Horizontal");
			velocity.y = Input.GetAxisRaw("Vertical");
			//velocity.Normalize();

			if (Input.GetButtonDown("Fire1") && units.Count > 0) {
                //INSERER LE SON D'UN TIR ALLIE
                AkSoundEngine.PostEvent("Play_Shots", gameObject);
				PlayerUnit unit = units[0];
				units.RemoveAt(0);
				unit.Suicide();
			}

            if (Input.GetButtonDown("Fire2"))
            {
                cursorSpeed = cursorShrinkSpeed;
                cursorRadius = cursorShrinkRadius;
                unitSpeed = unitShrinkSpeed;
                unitRadius = unitShrinkRadius;
            }

            if (Input.GetButtonUp("Fire2"))
            {
                cursorSpeed = cursorNormalSpeed;
                cursorRadius = cursorNormalRadius;
                unitSpeed = unitNormalSpeed;
                unitRadius = unitNormalRadius;
            }

            velocity *= cursorSpeed;

            //Le nombre d'unitées est obtenable avec units.Count

        }

		private void FixedUpdate() {
			cursorRB.velocity = velocity;
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
