using System.Collections.Generic;
using UnityEngine;
using Xenon;

namespace Swarm {
	[System.Serializable]
	public class Pattern {

		public string name;
		public float finishedDelay = 1f;
		public List<string> nextPossible;
		public Transform parent;
		public AttackPointsPatternModule attackPointsModule;
		public AttackSequenceModule attackSequenceModule;

		[System.NonSerialized] public bool isFinished = false;

		public Pattern Copy() {
			Pattern pattern = new Pattern();
			pattern.name = name;
			pattern.finishedDelay = finishedDelay;
			pattern.nextPossible = nextPossible;
			pattern.parent = parent;
			pattern.attackPointsModule = attackPointsModule.Copy();
			pattern.attackSequenceModule = attackSequenceModule.Copy();
			return pattern;
		}

		public void Start() {
			attackPointsModule.pattern = this;
			attackPointsModule.Initialize();
			attackSequenceModule.pattern = this;
			attackSequenceModule.Initialize();
		}

		public void Update(float dt) {
			attackPointsModule.Update(dt);
			attackSequenceModule.Update(dt);
		}

	}
	
	public abstract class PatternModule {

		public bool active = false;

		[System.NonSerialized] public Pattern pattern;

		public void Initialize() {
			if (!active) return;
			InitializeModule();
		}

		public void Update(float dt) {
			if (!active) return;
			UpdateModule(dt);
		}
		
		protected abstract void InitializeModule();
		protected abstract void UpdateModule(float dt);

	}

	[System.Serializable]
	public class AttackPointsPatternModule : PatternModule {

		public bool autoPlaced = true;
		public int count = 1;
		public float startRotation = 0f; // In Euler
		public float distanceFromCenter = 1f; // TODO have a distance from boss surface?
		public float rotationSpeed = 0f;
		public List<AttackPoint> attackPoints = new List<AttackPoint>();

		private float currentRotation = 0f;
		private Transform pointsParent;

		public AttackPointsPatternModule Copy() {
			AttackPointsPatternModule module = new AttackPointsPatternModule();
			module.autoPlaced = autoPlaced;
			module.count = count;
			module.startRotation = startRotation;
			module.distanceFromCenter = distanceFromCenter;
			module.rotationSpeed = rotationSpeed;
			module.attackPoints = attackPoints;
			return module;
		}

		protected override void InitializeModule() {
			GameObject pointsParentGo = new GameObject("AttackPoints Root");
			pointsParent = pointsParentGo.transform;
			pointsParent.SetParent(pattern.parent);
			if (autoPlaced) {
				float rotDelta = Mathf.PI * 2f / count;
				for (int i = 0; i < count; i++) {
					float perimeter = startRotation * Mathf.Deg2Rad + rotDelta * i;
					Vector2 pos = new Vector2(Mathf.Cos(perimeter), Mathf.Sin(perimeter)) * distanceFromCenter;
					GameObject go = new GameObject("AttackPoint");
					go.transform.SetParent(pointsParent);
					AttackPoint point = go.AddComponent<AttackPoint>();
					point.position = pos;
					point.rotation = Vector2.SignedAngle(Vector2.right, pos.normalized);
					attackPoints.Add(point);
				}
			}
		}

		protected override void UpdateModule(float dt) {
			currentRotation += rotationSpeed * dt;
			pointsParent.rotation = Quaternion.Euler(0f, 0f, currentRotation);
		}
	}

	[System.Serializable]
	public class AttackSequenceModule : PatternModule {

		public enum SequenceElementType {
			Bullet,
			Lazer,
			Mortar,
			Delay,
			EnablePoint,
			DisablePoint
		}

		[System.Serializable]
		public struct SequenceElement {
			public SequenceElementType type;
			public int count;
			public float duration;
			public string projectileName;
			public AttackPoint point;
		}

		public List<SequenceElement> sequence;

		private ProcessManager procManager;

		public AttackSequenceModule Copy() {
			AttackSequenceModule module = new AttackSequenceModule();
			module.sequence = sequence; // Does not copy
			return module;
		}

		protected override void InitializeModule() {
			procManager = new ProcessManager();
			SequenceProcess seqProc = new SequenceProcess(sequence, pattern.attackPointsModule.attackPoints);
			seqProc.TerminateCallback += SequenceFinished;
			procManager.LaunchProcess(seqProc);
		}

		protected override void UpdateModule(float dt) {
			procManager.UpdateProcesses(dt);
		}

		private void SequenceFinished() {
			pattern.isFinished = true;
		}
	}
}
