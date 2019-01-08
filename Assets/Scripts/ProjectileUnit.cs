using UnityEngine;

namespace Swarm {
	public class ProjectileUnit : MonoBehaviour {

		private PlayerSwarm swarm;

		private Rigidbody2D rb;
		private Vector2 velocity;
        private bool inAttack = false;

		public void Init(PlayerSwarm sw) {
			gameObject.layer = LayerMask.NameToLayer("ProjectileUnit");
			rb = GetComponent<Rigidbody2D>();
			swarm = sw;

            velocity = swarm.bossTransform.position - transform.position;
            velocity.Normalize();

            rb.velocity = Vector2.zero;
            rb.rotation = Vector2.SignedAngle(Vector2.up, velocity.normalized);
            Invoke("FirstAttack", swarm.freezeTime);
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

		private void FixedUpdate()
        {
            Attack();
        }

        private void FirstAttack()
        {
            
            velocity *= swarm.suicideSpeed;
            inAttack = true;
        }

        private void Attack()
        {
            
            if(inAttack)
            {
                rb.velocity = velocity;
                rb.rotation = Vector2.SignedAngle(Vector2.up, velocity.normalized);
            }
        }

	}
}
