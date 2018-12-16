﻿using System.Collections.Generic;
using UnityEngine;
using Xenon;

namespace Swarm {
	[System.Serializable]
	public struct ProjectileDefinition {
		public string name;
		public Projectile prefab;
	}

	public class ProjectilePool {
		public Projectile prefab;
		public Transform parent;
		public int totalSize = 0;
		public int maxSize = 0;
		public List<Projectile> alive = new List<Projectile>();
		public List<Projectile> dead = new List<Projectile>();

		public ProjectilePool(Projectile prefab, int maxSize, Transform parent) {
			this.prefab = prefab;
			this.maxSize = maxSize;
			this.parent = parent;
		}

		public void Shoot(Vector2 position, float rotation) {
			if (totalSize >= maxSize) {
				if (alive.Count >= totalSize) {
					Debug.LogWarning("Projectile Pool Size Too Small compared to spawned projectiles!");
				} else {
					ResurrectProjectile(position, rotation);
				}
			} else {
				CreateProjectile(position, rotation);
			}
		}

		public void Die(Projectile projectile) {
			if (!alive.Remove(projectile)) {
				Debug.LogWarning("Dying projectile not found in alive pool");
			}
			dead.Add(projectile);
			projectile.gameObject.SetActive(false);
		}

		private void CreateProjectile(Vector2 position, float rotation) {
			GameObject projGo = Object.Instantiate(prefab.gameObject, position, Quaternion.Euler(0f, 0f, rotation));
			projGo.name = "proj - " + totalSize;
			projGo.transform.SetParent(parent);
			Projectile projectile = projGo.GetComponent<Projectile>();
			projectile.Launch();
			projectile.prefab = prefab;
			alive.Add(projectile);
			totalSize++;
		}

		private void ResurrectProjectile(Vector2 position, float rotation) {
			if (dead.Count == 0) {
				Debug.LogWarning("No more projectiles to resurrect!");
				return;
			}
			Projectile projectile = dead[0];
			dead.RemoveAt(0);
			alive.Add(projectile);
			projectile.transform.position = position;
			projectile.transform.rotation = Quaternion.Euler(0f, 0f, rotation);
			projectile.gameObject.SetActive(true);
			projectile.Launch();
		}
	}

	public class ProjectileManager : Singleton<ProjectileManager> {

		public int poolSize = 200;
		
		private Dictionary<Projectile, ProjectilePool> pools = new Dictionary<Projectile, ProjectilePool>();

		public void ShootProjectile(Projectile prefab, Vector2 position, float rotation) {
			ProjectilePool projPool;
			if (!pools.TryGetValue(prefab, out projPool)) {
				projPool = new ProjectilePool(prefab, poolSize, transform);
				pools.Add(prefab, projPool);
			}
			projPool.Shoot(position, rotation);
		}

		public void ProjectileDeath(Projectile projectile) {
			ProjectilePool projPool;
			if (pools.TryGetValue(projectile.prefab, out projPool)) {
				projPool.Die(projectile);
			}
		}

	}
}
