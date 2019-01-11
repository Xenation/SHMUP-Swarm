using UnityEngine;

namespace Swarm {
	public abstract class TelegraphableAttack : MonoBehaviour {

		public enum State {
			Telegraphing,
			Attacking
		}

		protected State state = State.Telegraphing;

		protected GameObject telegraph;
		protected GameObject attack;
		protected GameObject canon;
		protected GameObject canonTelegraph;
		protected GameObject canonAttack;
		protected SlideInCanon slideInCanon;

		private void Awake() {
			telegraph = transform.Find("Telegraph").gameObject;
			attack = transform.Find("Attack").gameObject;
			canon = transform.Find("Canon").gameObject;
			canonTelegraph = canon.transform.Find("Telegraph").gameObject;
			canonAttack = canon.transform.Find("Attack").gameObject;
			if (attack.activeInHierarchy) {
				attack.SetActive(false);
			}
			if (canonAttack.activeInHierarchy) {
				canonAttack.SetActive(false);
			}
			slideInCanon = canon.GetComponent<SlideInCanon>();
			slideInCanon.SlideIn(.25f);
			OnAwake();
		}

		public virtual void LaunchAttack() {
			telegraph.SetActive(false);
			canonTelegraph.SetActive(false);
			attack.SetActive(true);
			canonAttack.SetActive(true);
			state = State.Attacking;
		}

		protected abstract void OnAwake();

	}
}
