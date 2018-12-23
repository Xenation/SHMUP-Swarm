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

		// Sequence
		private ProcessManager procManager;
		
		private bool suppressNext = false;

		public void Initialize(PatternDefinition def) {
			definition = def;
			runParams = new RuntimeParameters() { rotationSpeed = def.rotationSpeed };

			// Spawn Points
			GameObject pointsParentGo = new GameObject("AttackPoints Root");
			runParams.pointsParent = pointsParentGo.transform;
			runParams.pointsParent.SetParent(transform);
			runParams.attackPoints = definition.CreateSpawnPoints(runParams.pointsParent);

			// Sequence
			procManager = new ProcessManager();
			SequenceProcess seqProc = new SequenceProcess(definition.sequence, runParams, definition.finishedDelay);
			seqProc.TerminateCallback += SequenceFinished;
			procManager.LaunchProcess(seqProc);

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
			if (!suppressNext) {
				PatternDefinition nextDef = definition.GetRandomNext();
				if (nextDef != null) {
					nextDef.Attach(gameObject);
				}
			}
			Destroy(this);
			Destroy(runParams.pointsParent.gameObject);
		}

	}
}
