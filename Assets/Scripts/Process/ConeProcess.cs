using System.Collections.Generic;
using UnityEngine;
using Xenon.Processes;

namespace Swarm {
	public class ConeProcess : TimedProcess {

		private enum AttackState {
			NotLaunched,
			Telegraph,
			Attacking
		}

		private float angle;
		private float radius;
		private float telegraphDuration;
		private float attackDuration;
		private Cone prefab;
		private GameObject telegraphEffect;
		private Pattern.RuntimeParameters runParams;

		private List<GameObject> telegraphs = new List<GameObject>();
		private List<Cone> cones = new List<Cone>();
		private AttackState state = AttackState.NotLaunched;

		public ConeProcess(Pattern.RuntimeParameters rParams, float angle, float radius, float telegraphDuration, float attackDuration, Cone prefab, GameObject telegraphEffect) : base(telegraphDuration + attackDuration) {
			runParams = rParams;
			this.angle = angle;
			this.radius = radius;
			this.telegraphDuration = telegraphDuration;
			this.attackDuration = attackDuration;
			this.prefab = prefab;
			this.telegraphEffect = telegraphEffect;
		}

		public override void OnBegin() {
			base.OnBegin();
			state = AttackState.Telegraph;
			CreateTelegraph();
		}

		public override void OnTerminate() {
			base.OnTerminate();
			DestroyCones();
		}

		public override void TimeUpdated() {
			base.TimeUpdated();
			switch (state) {
				case AttackState.NotLaunched:
					break;
				case AttackState.Telegraph:
					if (t > telegraphDuration) {
						DestroyTelegraph();
						CreateCones();
						state = AttackState.Attacking;
					}
					break;
				case AttackState.Attacking:
					break;
			}
		}

		private void CreateCones() {
			foreach (AttackPoint point in runParams.attackPoints) {
				if (!point.shootingEnabled) continue;
				Cone cone = Object.Instantiate(prefab.gameObject, point.transform.position, Quaternion.Euler(0f, 0f, point.rotation), point.transform).GetComponent<Cone>();
				cone.SetAngleRadius(angle, radius);
				cones.Add(cone);
			}
		}

		private void DestroyCones() {
			foreach (Cone cone in cones) {
				Object.Destroy(cone.gameObject);
			}
			cones.Clear();
		}

		private void CreateTelegraph() {
			foreach (AttackPoint point in runParams.attackPoints) {
				if (!point.shootingEnabled) continue;
				telegraphs.Add(Object.Instantiate(telegraphEffect, point.transform.position, Quaternion.Euler(0f, 0f, point.rotation), point.transform));
			}
		}

		private void DestroyTelegraph() {
			foreach (GameObject telegraph in telegraphs) {
				Object.Destroy(telegraph);
			}
			telegraphs.Clear();
		}

	}
}
