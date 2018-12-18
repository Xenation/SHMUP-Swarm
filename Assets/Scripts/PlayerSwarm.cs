using System.Collections.Generic;
using UnityEngine;

namespace Swarm {
	public class PlayerSwarm : MonoBehaviour {

		public float cursorSpeed = 5f;
		public float cursorRadius = 1f;
		public GameObject unitPrefab;
		public int unitsToCreate = 50;
		public float unitSpeed = 4f;
		public float unitRadius = .1f;
		public float suicideSpeed = 10f;
		public Transform bossTransform;
        private int nbOfUnits;

		public bool debug = false;

		[System.NonSerialized] public List<PlayerUnit> units = new List<PlayerUnit>();
		[System.NonSerialized] public Transform cursor;

		private Vector2 velocity;
		private Rigidbody2D cursorRB;

		private void Awake() {
			for (int i = 0; i < unitsToCreate; i++) {
				Instantiate(unitPrefab, new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f)), Quaternion.identity, transform);
			}
			cursor = transform.Find("Cursor");
			cursorRB = cursor.GetComponent<Rigidbody2D>();
			GetComponentsInChildren(units);
		}

		private void Update() {
			velocity.x = Input.GetAxisRaw("Horizontal");
			velocity.y = Input.GetAxisRaw("Vertical");
			velocity.Normalize();
			velocity *= cursorSpeed;

			if (Input.GetButtonDown("Fire1") && units.Count > 0) {
                //INSERER LE SON D'UN TIR ALLIE
                AkSoundEngine.PostEvent("Play_Shots", gameObject);
				PlayerUnit unit = units[0];
				units.RemoveAt(0);
				unit.Suicide();
			}

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
