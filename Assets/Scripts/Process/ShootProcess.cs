using System.Collections.Generic;
using UnityEngine;
using Xenon;

namespace Swarm {
	public class ShootProcess : Process {

		private Projectile prefab;
		private List<AttackPoint> points;

		public ShootProcess(Projectile pre, List<AttackPoint> pts) {
			prefab = pre;
			points = pts;
		}

		public override void OnBegin() {
			
		}

		public override void Update(float dt) {
			foreach (AttackPoint point in points) {
				if (!point.shootingEnabled) continue;
				ProjectileManager.I.ShootProjectile(prefab, point.position, point.rotation);
			}
			Terminate();
		}

		public override void OnTerminate() {
			
		}

	}
}
