using System.Collections.Generic;
using UnityEngine;
using Xenon;

namespace Swarm {
	public class ShootProcess : Process {

		private Projectile prefab;
		private List<AttackPoint> points;
		private float speedOverride;

		public ShootProcess(Projectile pre, List<AttackPoint> pts, float speedOv) {
			prefab = pre;
			points = pts;
			speedOverride = speedOv;
		}

		public override void OnBegin() {
			
		}

		public override void Update(float dt) {
			foreach (AttackPoint point in points) {
				if (!point.shootingEnabled) continue;
				Projectile proj = ProjectileManager.I.ShootProjectile(prefab, point.position, point.rotation);
				if (speedOverride != 0f) {
					proj.speed = speedOverride;
				}
				proj.Launch();
			}
			Terminate();
		}

		public override void OnTerminate() {
			
		}

	}
}
