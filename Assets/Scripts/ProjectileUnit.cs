using UnityEngine;

namespace Swarm {
	public class ProjectileUnit : MonoBehaviour {

		private PlayerSwarm swarm;

		private Rigidbody2D rb;
		private Vector2 velocity;

		public void Init(PlayerSwarm sw) {
			gameObject.layer = LayerMask.NameToLayer("ProjectileUnit");
			rb = GetComponent<Rigidbody2D>();
			swarm = sw;
			velocity = swarm.bossTransform.position - transform.position;
			velocity.Normalize();
			velocity *= swarm.suicideSpeed;
		}

		private void OnCollisionEnter2D(Collision2D collision) {
			Boss boss = collision.gameObject.GetComponent<Boss>();
			partController part = collision.gameObject.GetComponent<partController>();
			if (boss || part) {
                //Add vibration
				Destroy(gameObject);
                //SON DE MORT A L'IMPACT EN MODE KAMIKAZE
                AkSoundEngine.PostEvent("Play_Death", gameObject);
			}
		}

		private void FixedUpdate() {
			rb.velocity = velocity;
			rb.rotation = Vector2.SignedAngle(Vector2.up, velocity.normalized);
		}

	}
}
