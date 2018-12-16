using System.Collections.Generic;
using UnityEngine;
using Xenon;

namespace Swarm {
	[System.Serializable]
	public struct ProjectileDefinition {
		public string name;
		public Projectile prefab;
	}

	public class PatternManager : Singleton<PatternManager> {

		[SerializeField] private List<ProjectileDefinition> projectileDefinitions;

		public List<Pattern> patterns;

		public bool GetProjectileDefinition(string name, out ProjectileDefinition definition) {
			foreach (ProjectileDefinition def in projectileDefinitions) {
				if (def.name == name) {
					definition = def;
					return true;
				}
			}
			definition = new ProjectileDefinition();
			return false;
		}

		public void ShootProjectile(ProjectileDefinition def, Vector2 position, float rotation) {
			GameObject projGo = Instantiate(def.prefab.gameObject, position, Quaternion.Euler(0f, 0f, rotation));
			projGo.GetComponent<Projectile>().Launch();
		}

		public Pattern GetPattern(string name) {
			foreach (Pattern pat in patterns) {
				if (pat.name == name) {
					return pat;
				}
			}
			return null;
		}

	}
}
