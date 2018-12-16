using System.Collections.Generic;
using UnityEngine;
using Xenon;

namespace Swarm {
	public class Boss : MonoBehaviour {

		public PatternDefinition startPattern;

		private void Awake() {
			startPattern.Attach(gameObject);
		}

		private void Update() {
			
		}

	}
}
