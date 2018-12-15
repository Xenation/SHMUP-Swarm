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

		public bool debug = false;

		[System.NonSerialized] public List<PlayerUnit> units = new List<PlayerUnit>();
		[System.NonSerialized] public Transform cursor;

		private Vector2 velocity;

		private void Awake() {
			for (int i = 0; i < unitsToCreate; i++) {
				Instantiate(unitPrefab, new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f)), Quaternion.identity, transform);
			}
			cursor = transform.Find("Cursor");
			GetComponentsInChildren(units);
		}

		private void Update() {
			velocity.x = Input.GetAxisRaw("Horizontal");
			velocity.y = Input.GetAxisRaw("Vertical");
			velocity.Normalize();
			velocity *= cursorSpeed;
		}

		private void FixedUpdate() {
			cursor.position += (Vector3) velocity * Time.fixedDeltaTime;
		}

	}
}
