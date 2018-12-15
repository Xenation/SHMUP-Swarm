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
		public Transform bossTransform;
		public float bossRadius = 3f;

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
		}

		private void FixedUpdate() {
			//Vector2 nextPos = cursor.position + (Vector3) velocity * Time.fixedDeltaTime;
			cursorRB.velocity = velocity;
		}

	}
}
