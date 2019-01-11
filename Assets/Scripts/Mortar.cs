using UnityEngine;

namespace Swarm {
	public class Mortar : TelegraphableAttack {
		
		private Animator attackAnimator;
		private float radius;
		private float seekSpeed;
		private bool isLocked = false;

		private Boss boss;

		protected override void OnAwake() {
			attackAnimator = attack.GetComponentInChildren<Animator>();
			if (attack.activeInHierarchy) {
				attack.SetActive(false);
			}
			boss = GetComponentInParent<Boss>();
		}

		public void Init(float radius, float seekSpeed) {
			this.radius = radius;
			this.seekSpeed = seekSpeed;
			telegraph.transform.localScale = Vector3.one * radius;
			attack.transform.localScale = Vector3.one * radius;
		}

		public void Lock() {
			isLocked = true;
		}

		public override void LaunchAttack() {
			base.LaunchAttack();
			attack.transform.position = telegraph.transform.position;
			Collider2D[] touched = Physics2D.OverlapCircleAll(attack.transform.position, radius, LayerMask.GetMask("PlayerUnits"));
			foreach (Collider2D col in touched) {
				PlayerUnit unit = col.GetComponent<PlayerUnit>();
				if (unit == null) continue;
				unit.Die();
			}
		}

		private void Update() {
			if (state == State.Telegraphing && !isLocked) {
				Vector2 vel = (boss.swarm.cursor.position - telegraph.transform.position).normalized * seekSpeed * Time.deltaTime;
				telegraph.transform.position += new Vector3(vel.x, vel.y);
			} else if (state == State.Attacking && attackAnimator.GetCurrentAnimatorStateInfo(0).speed == 0.01f) { // TODO ugly af
				Destroy(gameObject);
			}
		}
		
	}
}
