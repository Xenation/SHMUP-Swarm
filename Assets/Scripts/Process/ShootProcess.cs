using System.Collections.Generic;
using UnityEngine;
using Xenon;

namespace Swarm {
	public class ShootProcess : Process {

		private ProjectileDefinition definition;
		private List<AttackPoint> points;

		public ShootProcess(ProjectileDefinition def, List<AttackPoint> pts) {
			definition = def;
			points = pts;
		}

		public override void OnBegin() {
			
		}

		public override void Update(float dt) {
			foreach (AttackPoint point in points) {
				if (!point.shootingEnabled) continue;
				PatternManager.I.ShootProjectile(definition, point.position, point.rotation);
			}
			Terminate();
		}

		public override void OnTerminate() {
			
		}

	}
}
