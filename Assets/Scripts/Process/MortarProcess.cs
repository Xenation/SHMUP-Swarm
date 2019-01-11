using System.Collections.Generic;
using UnityEngine;
using Xenon.Processes;

namespace Swarm {
	public class MortarProcess : TimedProcess {

		private float aimTime;
		private float radius;
		private float seekSpeed;
		private float lockTime;
		private GameObject prefab;

		private Mortar.State mortarState;
		private Pattern.RuntimeParameters runParams;
		private List<Mortar> mortars = new List<Mortar>();

		public MortarProcess(Pattern.RuntimeParameters rParams, float aimTime, float radius, float seekSpeed, float lockTime, GameObject prefab) : base(aimTime + lockTime) {
			runParams = rParams;
			this.aimTime = aimTime;
			this.radius = radius;
			this.seekSpeed = seekSpeed;
			this.lockTime = lockTime;
			this.prefab = prefab;
			mortarState = Mortar.State.Seeking;
		}

		public override void OnBegin() {
			base.OnBegin();
			CreateMortars();
		}

		public override void TimeUpdated() {
			base.TimeUpdated();
			if (mortarState == Mortar.State.Seeking && t > aimTime) {
				SetStates(Mortar.State.Locking);
			}
		}

		public override void OnTerminate() {
			base.OnTerminate();
			SetStates(Mortar.State.Attack);
		}

		private void CreateMortars() {
			foreach (AttackPoint point in runParams.attackPoints) {
				mortars.Add(Object.Instantiate(prefab, point.transform.position, Quaternion.Euler(0f, 0f, point.rotation), runParams.pointsParent.parent).GetComponent<Mortar>());
			}
			foreach (Mortar mortar in mortars) {
				mortar.Init(radius, seekSpeed);
			}
		}

		private void SetStates(Mortar.State nState) {
			foreach (Mortar mortar in mortars) {
				mortar.SetState(nState);
			}
		}

	}
}
