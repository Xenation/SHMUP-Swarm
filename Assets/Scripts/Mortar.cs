using UnityEngine;

namespace Swarm {
	public class Mortar : MonoBehaviour {

		public enum State {
			Seeking,
			Locking,
			Attack
		}

		private State state = State.Seeking;
		private GameObject telegraph;
		private GameObject attack;
		private Animator attackAnimator;
		private float radius;
		private float seekSpeed;

		private Boss boss;

		private void Awake() {
			telegraph = transform.Find("Telegraph").gameObject;
			attack = transform.Find("Attack").gameObject;
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

		public void SetState(State nState) {
			switch (state) {
				case State.Seeking:
					if (nState == State.Locking) {
						state = nState;
					}
					break;
				case State.Locking:
					if (nState == State.Attack) {
						PerformAttack();
						state = nState;
					}
					break;
				case State.Attack:
					break;
			}
		}

		private void PerformAttack() {
			telegraph.SetActive(false);
			attack.SetActive(true);
			attack.transform.position = telegraph.transform.position;
			Collider2D[] touched = Physics2D.OverlapCircleAll(attack.transform.position, radius, LayerMask.GetMask("PlayerUnits"));
			foreach(Collider2D col in touched) {
				PlayerUnit unit = col.GetComponent<PlayerUnit>();
				if (unit == null) continue;
				unit.Die();
			}
		}

		private void Update() {
			switch (state) {
				case State.Seeking:
					Vector2 vel = (boss.swarm.cursor.position - telegraph.transform.position).normalized * seekSpeed * Time.deltaTime;
					telegraph.transform.position += new Vector3(vel.x, vel.y);
					break;
				case State.Locking:
					break;
				case State.Attack:
					if (attackAnimator.GetCurrentAnimatorStateInfo(0).speed == 0.01f) { // TODO ugly af
						Destroy(gameObject);
					}
					break;
			}
		}

	}
}
