using UnityEngine;
using UnityEngine.SceneManagement;
using Xenon;

namespace Swarm {
	public class Boss : MonoBehaviour {

		public PatternDefinition startPattern;
		public PlayerSwarm swarm;

		private void Awake() {
			if (startPattern != null) {
				startPattern.Attach(gameObject);
			}
		}

		private void Update() {
			
		}

	}
}
