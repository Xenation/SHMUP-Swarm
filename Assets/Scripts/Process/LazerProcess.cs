using System.Collections.Generic;
using UnityEngine;
using Xenon.Processes;

namespace Swarm {
	public class LazerProcess : TimedProcess {

		private enum AttackState {
			NotLaunched,
			Telegraph,
			Attacking
		}

		private float width;
		private float telegraphDuration;
		private float attackDuration;
		private Lazer prefab;
		private GameObject telegraphEffect;
		private Pattern.RuntimeParameters runParams;

		private List<GameObject> telegraphs = new List<GameObject>();
		private List<Lazer> lazers = new List<Lazer>();
		private AttackState state = AttackState.NotLaunched;

		public LazerProcess(Pattern.RuntimeParameters rParams, float width, float telegraphDuration, float attackDuration, Lazer prefab, GameObject telegraphEffect) : base(telegraphDuration + attackDuration) {
			runParams = rParams;
			this.width = width;
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
			DestroyLayers();
		}

		public override void TimeUpdated() {
			base.TimeUpdated();
			switch (state) {
				case AttackState.NotLaunched:
					break;
				case AttackState.Telegraph:
					if (t > telegraphDuration) {
						DestroyTelegraph();
						StartLazers();
						state = AttackState.Attacking;
					}
					break;
				case AttackState.Attacking:
					break;
			}
		}

		private void StartLazers() {
			foreach (AttackPoint point in runParams.attackPoints) {
				if (!point.shootingEnabled) continue;
				Lazer lazer = Object.Instantiate(prefab.gameObject, Vector3.zero, Quaternion.Euler(0f, 0f, point.rotation), point.transform).GetComponent<Lazer>();
				lazer.SetWidth(width);
				lazers.Add(lazer);
			}
		}

		private void DestroyLayers() {
			foreach (Lazer lazer in lazers) {
				Object.Destroy(lazer.gameObject);
			}
			lazers.Clear();
		}

		private void CreateTelegraph() {
			foreach (AttackPoint point in runParams.attackPoints) {
				if (!point.shootingEnabled) continue;
				telegraphs.Add(Object.Instantiate(telegraphEffect, Vector3.zero, Quaternion.Euler(0f, 0f, point.rotation), point.transform));
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
