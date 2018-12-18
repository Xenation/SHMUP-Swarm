using System.Collections.Generic;
using UnityEngine;
using Xenon;

namespace Swarm {
	public class Pattern : MonoBehaviour {

		public PatternDefinition definition;

		// Spawn Points
		private float currentRotation = 0f;
		private Transform pointsParent;
		private List<AttackPoint> attackPoints;

		// Sequence
		private ProcessManager procManager;

		private bool suppressNext = false;

		public void Initialize(PatternDefinition def) {
			definition = def;

			// Spawn Points
			GameObject pointsParentGo = new GameObject("AttackPoints Root");
			pointsParent = pointsParentGo.transform;
			pointsParent.SetParent(transform);
			attackPoints = definition.CreateSpawnPoints(pointsParent);

			// Sequence
			procManager = new ProcessManager();
			SequenceProcess seqProc = new SequenceProcess(definition.sequence, attackPoints, definition.finishedDelay);
			seqProc.TerminateCallback += SequenceFinished;
			procManager.LaunchProcess(seqProc);

			// Simultaneous Patterns
			foreach (PatternDefinition defSimult in definition.simultaneous) {
				defSimult.Attach(gameObject).suppressNext = true;
			}
		}

		public void Update() {
			// Spawn Points
			currentRotation += definition.rotationSpeed * Time.deltaTime;
			pointsParent.rotation = Quaternion.Euler(0f, 0f, currentRotation);

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
		}

	}
}
