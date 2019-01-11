using System.Collections.Generic;
using UnityEngine;
using Xenon.Processes;

namespace Swarm {
	public class LazerProcess : TimedProcess {

		private float width;
		private float telegraphDuration;
		private float attackDuration;
		private Lazer prefab;
		private Pattern.RuntimeParameters runParams;
		
		private List<Lazer> lazers = new List<Lazer>();
		private TelegraphableAttack.State lazerState = TelegraphableAttack.State.Telegraphing;

		public LazerProcess(Pattern.RuntimeParameters rParams, float width, float telegraphDuration, float attackDuration, Lazer prefab) : base(telegraphDuration + attackDuration) {
			runParams = rParams;
			this.width = width;
			this.telegraphDuration = telegraphDuration;
			this.attackDuration = attackDuration;
			this.prefab = prefab;
		}

		public override void OnBegin() {
			base.OnBegin();
			CreateLazers();
		}

		public override void OnTerminate() {
			base.OnTerminate();
			DestroyLazers();
		}

		public override void TimeUpdated() {
			base.TimeUpdated();
			if (lazerState == TelegraphableAttack.State.Telegraphing && t > telegraphDuration) {
				LaunchAttacks();
				lazerState = TelegraphableAttack.State.Attacking;
			}
		}

		private void CreateLazers() {
			foreach (AttackPoint point in runParams.attackPoints) {
				if (!point.shootingEnabled) continue;
				Lazer lazer = Object.Instantiate(prefab.gameObject, point.transform.position, Quaternion.Euler(0f, 0f, point.rotation), point.transform).GetComponent<Lazer>();
				lazer.SetWidth(width);
				lazers.Add(lazer);
			}
		}

		private void DestroyLazers() {
			foreach (Lazer lazer in lazers) {
				Object.Destroy(lazer.gameObject);
			}
			lazers.Clear();
		}

		private void LaunchAttacks() {
			foreach (Lazer lazer in lazers) {
				lazer.LaunchAttack();
			}
		}

	}
}
