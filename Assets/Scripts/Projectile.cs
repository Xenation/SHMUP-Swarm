using UnityEngine;

namespace Swarm {
	[RequireComponent(typeof(Collider2D))]
	public class Projectile : MonoBehaviour {

		public float speed;
		public int damage;

		private Vector2 velocity;

		public void Launch() {
			velocity = transform.right * speed;
		}

		private void OnCollisionEnter2D(Collision2D collision) {
			PlayerUnit unit = collision.gameObject.GetComponent<PlayerUnit>();
			if (unit != null) {
				// TODO Apply damage to unit
				Destroy(gameObject);
			}
		}

		private void FixedUpdate() {
			transform.position += (Vector3) velocity * Time.fixedDeltaTime;
		}

	}
}
