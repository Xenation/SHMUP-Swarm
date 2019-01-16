using UnityEngine;

namespace Swarm {
	public class Boss : MonoBehaviour {
		
		public PlayerSwarm swarm;
		public Phase[] phases;

		private Pattern currentPattern;
		private bossLife bLife;
		public int phaseIndex { get; private set; }

		private void Awake() {
			bLife = GetComponent<bossLife>();
			bLife.OnStunStarted += StunStarted;
			bLife.OnStunEnded += StunEnded;
			if (phaseIndex < phases.Length && phases[phaseIndex].startPattern != null) {
				currentPattern = phases[phaseIndex].startPattern.Attach(gameObject);
				currentPattern.OnPatternEnded += PatternChange;
			}
		}

		private void PhaseChange(int prevPhase, int currentPhase) {
			phaseIndex = currentPhase;
			currentPattern = phases[phaseIndex].startPattern.Attach(gameObject);
			currentPattern.OnPatternEnded += PatternChange;
			bLife.SetPhase(currentPhase + 1);
            ScoreManager.bossPhase = currentPhase + 1;
			Debug.Log("Changed Phase: " + prevPhase + " --> " + currentPhase);
		}

		private void StunStarted() {
			currentPattern.Terminate();
			currentPattern = null;
		}

		private void StunEnded() {
			if (!CheckPhase()) {
				currentPattern = phases[phaseIndex].resetPattern.Attach(gameObject);
				currentPattern.OnPatternEnded += PatternChange;
			}
		}

		private bool CheckPhase() {
			int hp = bLife.health;
			int nPhaseIndex = 0;
			for (int i = 0; i < phases.Length; i++) {
				if (hp <= phases[i].lifeThreshold) {
					nPhaseIndex = i;
				}
			}
			if (phaseIndex != nPhaseIndex) {
				PhaseChange(phaseIndex, nPhaseIndex);
				return true;
			}
			return false;
		}

		private void PatternChange(Pattern nPattern) {
			currentPattern.OnPatternEnded -= PatternChange;
			currentPattern = nPattern;
			currentPattern.OnPatternEnded += PatternChange;
			Debug.Log("Changed Pattern!");
		}

		public void Die() {
			foreach (Transform child in transform) {
				Destroy(child.gameObject);
			}
			Component[] bossComponents = GetComponents<Component>();
			foreach (Component comp in bossComponents) {
				if (comp is Transform) continue;
				Destroy(comp);
			}
		}

	}
}
