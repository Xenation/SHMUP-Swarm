using UnityEngine;

namespace Swarm {
	public class Mortar : TelegraphableAttack {

		private Animator attackAnimator;
		private float radius;
		private float seekSpeed;
		private bool isLocked = false;
		private GameObject visualProjectile;
		private float visualProjectileSpeed = 1;

		private Boss boss;
		private float distanceFromBoss = 1f;

		protected override void OnAwake() {
			attackAnimator = attack.GetComponentInChildren<Animator>();
			if (attack.activeInHierarchy) {
				attack.SetActive(false);
			}
			boss = GetComponentInParent<Boss>();
			distanceFromBoss = Vector3.Distance(boss.transform.position, canon.transform.position);
			visualProjectile = canon.transform.Find("VisualProjectile").gameObject;
			if (visualProjectile.activeInHierarchy) {
				visualProjectile.SetActive(false);
			}
		}

		public void Init(float radius, float seekSpeed) {
			this.radius = radius;
			this.seekSpeed = seekSpeed;
			telegraph.transform.localScale = Vector3.one * radius;
			attack.transform.localScale = Vector3.one * radius;
		}

		public void Lock(float lockTime) {
			isLocked = true;
			visualProjectile.SetActive(true);
			visualProjectileSpeed = (telegraph.transform.position - visualProjectile.transform.position).magnitude / lockTime;
		}

		public override void LaunchAttack() {
			base.LaunchAttack();
			visualProjectile.SetActive(false);
			attack.transform.position = telegraph.transform.position;
            AkSoundEngine.PostEvent("Stop_Mortier", gameObject);
            Collider2D[] touched = Physics2D.OverlapCircleAll(attack.transform.position, radius, LayerMask.GetMask("PlayerUnits"));
			foreach (Collider2D col in touched) {
				PlayerUnit unit = col.GetComponent<PlayerUnit>();
				if (unit == null) continue;
				unit.Die();
			}
		}

		private void Update() {
			if (state == State.Telegraphing) {
				Vector2 vel;
				if (!isLocked) { // Seeking
                    AkSoundEngine.PostEvent("Play_Mortier", gameObject);
					vel = (boss.swarm.cursor.position - telegraph.transform.position).normalized * seekSpeed * Time.deltaTime;
					telegraph.transform.position += new Vector3(vel.x, vel.y);
					// Align with boss
					Vector2 toCrosshair = ((Vector2) telegraph.transform.position - (Vector2) boss.transform.position).normalized;
					canon.transform.position = boss.transform.position + (Vector3) toCrosshair * distanceFromBoss;
					canon.transform.rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.right, toCrosshair));
				} else { // Locked
					Vector2 toTarget = (telegraph.transform.position - visualProjectile.transform.position);
					vel = toTarget.normalized * visualProjectileSpeed * Time.deltaTime;
					visualProjectile.transform.position += new Vector3(vel.x, vel.y);
				}
			} else if (state == State.Attacking && attackAnimator.GetCurrentAnimatorStateInfo(0).speed == 0.01f) { // TODO ugly af
				Destroy(gameObject);
			}
        }

    }
}
