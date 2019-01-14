using System.Collections.Generic;
using UnityEngine;
using Xenon.Processes;

namespace Swarm {
	public class ConeProcess : TimedProcess, AbortableProcess {

		private float angle;
		private float radius;
		private float telegraphDuration;
		private float attackDuration;
		private Cone prefab;
		private Pattern.RuntimeParameters runParams;
		
		private List<Cone> cones = new List<Cone>();
		private TelegraphableAttack.State conesState = TelegraphableAttack.State.Telegraphing;

		public ConeProcess(Pattern.RuntimeParameters rParams, float angle, float radius, float telegraphDuration, float attackDuration, Cone prefab, GameObject telegraphEffect) : base(telegraphDuration + attackDuration) {
			runParams = rParams;
			this.angle = angle;
			this.radius = radius;
			this.telegraphDuration = telegraphDuration;
			this.attackDuration = attackDuration;
			this.prefab = prefab;
		}

		public override void OnBegin() {
			base.OnBegin();
			CreateCones();
		}

		public override void OnTerminate() {
			base.OnTerminate();
			DestroyCones();
		}

		public override void TimeUpdated() {
			base.TimeUpdated();
			if (conesState == TelegraphableAttack.State.Telegraphing && t > telegraphDuration) {
				LaunchAttacks();
				conesState = TelegraphableAttack.State.Attacking;
			}
		}

		private void CreateCones() {
			foreach (AttackPoint point in runParams.attackPoints) {
				if (!point.shootingEnabled) continue;
				Cone cone = Object.Instantiate(prefab.gameObject, point.transform.position, Quaternion.Euler(0f, 0f, point.rotation), point.transform).GetComponent<Cone>();
				cones.Add(cone);
			}
		}

		private void DestroyCones() {
			foreach (Cone cone in cones) {
				Object.Destroy(cone.gameObject);
			}
			cones.Clear();
		}

		private void LaunchAttacks() {
			foreach (Cone cone in cones) {
				cone.LaunchAttack();
				cone.SetAngleRadius(angle, radius);
			}
		}

		public void Abort() {
			DestroyCones();
		}

	}
}
