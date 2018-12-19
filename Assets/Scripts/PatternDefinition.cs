using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Xenon;

namespace Swarm {
	[System.Serializable]
	public struct SpawnPointDefinition {
		public Vector2 position;
		public float rotation;
	}

	[CreateAssetMenu(menuName = "Pattern", order = 40)]
	public class PatternDefinition : ScriptableObject {
		
		public float finishedDelay = 1f;
		public List<PatternDefinition> simultaneous;
		public List<PatternDefinition> nextPossible;

		// Spawn Points
		[Header("Spawn Points")]
		public bool autoPlaced = true;
		public int count = 1;
		public float startRotation = 0f; // In Euler
		public float distanceFromCenter = 1f; // TODO have a distance from boss surface?
		public float rotationSpeed = 0f;
		public SpawnPointDefinition[] spawnPoints;

		// Sequence
		[Header("Sequence")]
		public List<SequenceElement> sequence;


		private void OnEnable() {
			if (sequence.Count != 0 && (sequence[0].count != 0 || sequence[0].duration != 0f || sequence[0].projectile != null || sequence[0].point != null)) { // Has Old Style Storage
				ConvertToObjArray();
			}
		}

		private void ConvertToObjArray() {
			for (int i = 0; i < sequence.Count; i++) {
				sequence[i].Convert();
			}
		}

		private void OnValidate() {
			if (autoPlaced) {
				spawnPoints = new SpawnPointDefinition[count];
				float rotDelta = Mathf.PI * 2f / count;
				for (int i = 0; i < count; i++) {
					float perimeter = startRotation * Mathf.Deg2Rad + rotDelta * i;
					Vector2 pos = new Vector2(Mathf.Cos(perimeter), Mathf.Sin(perimeter)) * distanceFromCenter;
					float rot = Vector2.SignedAngle(Vector2.right, pos.normalized);
					spawnPoints[i] = new SpawnPointDefinition() { position = pos, rotation = rot };
				}
			}
			foreach (SequenceElement elem in sequence) {
				if (!elem.CheckDataValidity()) {
					elem.ClearData();
				}
			}
		}

		public Pattern Attach(GameObject host) {
			Pattern pattern = host.AddComponent<Pattern>();
			pattern.Initialize(this);
			return pattern;
		}

		public List<AttackPoint> CreateSpawnPoints(Transform parent) {
			List<AttackPoint> instantiatedSpawnPoints = new List<AttackPoint>();
			for (int i = 0; i < spawnPoints.Length; i++) {
				instantiatedSpawnPoints.Add(CreateSpawnPoint(spawnPoints[i], parent));
			}
			return instantiatedSpawnPoints;
		}

		public AttackPoint CreateSpawnPoint(SpawnPointDefinition def, Transform parent) {
			GameObject go = new GameObject("AttackPoint");
			go.transform.SetParent(parent);
			AttackPoint point = go.AddComponent<AttackPoint>();
			point.position = def.position;
			point.rotation = def.rotation;
			return point;
		}

		public PatternDefinition GetRandomNext() {
			if (nextPossible.Count == 0) return null;
			return nextPossible[Random.Range(0, nextPossible.Count)];
		}

	}
}
