using System.Collections.Generic;
using UnityEngine;
using Xenon;

namespace Swarm {
	public class Boss : MonoBehaviour {

		public string startPattern;

		private Pattern currentPattern;

		private void Awake() {
			currentPattern = PatternManager.I.GetPattern(startPattern).Copy();
			currentPattern.Start();
		}

		private void Update() {
			currentPattern.Update(Time.deltaTime);
		}

	}
}
