using System.Collections.Generic;
using UnityEngine;
using Xenon;

namespace Swarm {
	public class Pattern : MonoBehaviour {

		public class RuntimeParameters {
			public Transform pointsParent;
			public List<AttackPoint> attackPoints;
			public float currentRotation = 0f;
			public float rotationSpeed = 0f;
		}

		public PatternDefinition definition;

		public RuntimeParameters runParams;

		public delegate void PatternEnd(Pattern next);
		public event PatternEnd OnPatternEnded;

		// Sequence
		private ProcessManager procManager;
		private SequenceProcess sequenceProcess;
		
		private bool suppressNext = false;

		public void Initialize(PatternDefinition def) {
			definition = def;
			Debug.Log("Starting Pattern: " + definition.name);
			runParams = new RuntimeParameters() { rotationSpeed = def.rotationSpeed };

			// Spawn Points
			GameObject pointsParentGo = new GameObject("AttackPoints Root");
			runParams.pointsParent = pointsParentGo.transform;
			runParams.pointsParent.SetParent(transform);
			runParams.attackPoints = definition.CreateSpawnPoints(runParams.pointsParent);

			// Sequence
			procManager = new ProcessManager();
			sequenceProcess = new SequenceProcess(definition.sequence, runParams, definition.finishedDelay);
			sequenceProcess.TerminateCallback += SequenceFinished;
			procManager.LaunchProcess(sequenceProcess);

			// Simultaneous Patterns
			foreach (PatternDefinition defSimult in definition.simultaneous) {
				defSimult.Attach(gameObject).suppressNext = true;
			}
		}

		public void Update() {
			// Spawn Points
			runParams.currentRotation += runParams.rotationSpeed * Time.deltaTime;
			runParams.pointsParent.rotation = Quaternion.Euler(0f, 0f, runParams.currentRotation);

			// Sequence
			procManager.UpdateProcesses(Time.deltaTime);
		}

		private void SequenceFinished() {
			Debug.Log("Finished Pattern: " + definition.name);
			if (!suppressNext) {
				PatternDefinition nextDef = definition.GetRandomNext();
				Pattern nextPat = null;
				if (nextDef != null) {
					nextPat = nextDef.Attach(gameObject);
				}
				OnPatternEnded?.Invoke(nextPat);
			}
			Destroy(this);
			Destroy(runParams.pointsParent.gameObject);
		}

		public void Terminate() {
			Debug.Log("Terminating Pattern: " + definition.name);
			sequenceProcess.Abort();
			if (!suppressNext) {
				foreach (Pattern simult in GetComponents<Pattern>()) {
					if (!simult.suppressNext) continue;
					simult.Terminate();
				}
			}
			Destroy(this);
			Destroy(runParams.pointsParent.gameObject);
		}

	}
}
