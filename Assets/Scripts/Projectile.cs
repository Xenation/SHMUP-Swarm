using UnityEngine;

namespace Swarm {
	[RequireComponent(typeof(Collider2D))]
	public class Projectile : MonoBehaviour {

		public float speed;
		public float maxDistanceFromOrigin = 20f;
		[System.NonSerialized] public Projectile prefab;

		private Vector2 velocity;
		private float maxDistanceSqr;

		public void Awake() {
			maxDistanceSqr = maxDistanceFromOrigin * maxDistanceFromOrigin;
		}

		public void Launch() {
            velocity = transform.right * speed;
            AkSoundEngine.PostEvent("Play_Bullets", gameObject);
		}

		private void OnCollisionEnter2D(Collision2D collision) {
			if (!gameObject.activeInHierarchy) return;
			PlayerUnit unit = collision.gameObject.GetComponent<PlayerUnit>();
            PlayerShrink shrink = collision.gameObject.GetComponent<PlayerShrink>();
			if (unit || shrink) {
				ProjectileManager.I.ProjectileDeath(this);
			}
		}

		private void Update() {
			if (Vector2.Dot(transform.position, transform.position) > maxDistanceSqr) {
				ProjectileManager.I.ProjectileDeath(this);
			}
		}

		private void FixedUpdate() {
			transform.position += (Vector3) velocity * Time.fixedDeltaTime;
		}

	}
}
