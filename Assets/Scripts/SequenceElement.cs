using System.Collections.Generic;
using UnityEngine;

namespace Swarm {
	public enum SequenceElementType {
		/// <summary>
		/// 0/int count
		/// 1/float duration
		/// 2/Projectile projectile
		/// 3/float speed
		/// </summary>
		Bullet,
		/// <summary>
		/// 0/float duration
		/// </summary>
		Lazer,
		/// <summary>
		/// 0/int count
		/// </summary>
		Mortar,
		/// <summary>
		/// 0/float duration
		/// </summary>
		Delay,
		/// <summary>
		/// 0/AttackPoint point
		/// </summary>
		EnablePoint,
		/// <summary>
		/// 0/AttackPoint point
		/// </summary>
		DisablePoint,
		/// <summary>
		/// 0/float rotation
		/// </summary>
		SetRotation,
		/// <summary>
		/// 0/float rotationSpeed
		/// </summary>
		RotationSpeed
	}

	[System.Serializable]
	public class SequenceElement : ISerializationCallbackReceiver {

		private enum DataType {
			Int,
			Float,
			Proj,
			Point
		}

		public SequenceElementType type;
		public int count;
		public float duration;
		public Projectile projectile;
		public AttackPoint point;

		public object[] objValues;
		[SerializeField] private List<DataType> dataTypes;
		[SerializeField] private List<int> integers;
		[SerializeField] private List<float> floats;
		[SerializeField] private List<Projectile> projectiles;
		[SerializeField] private List<AttackPoint> points;

		public int Int(int i) {
			return (int) objValues[i];
		}

		public float Float(int i) {
			return (float) objValues[i];
		}

		public Projectile Projectile(int i) {
			return (Projectile) objValues[i];
		}

		public AttackPoint Point(int i) {
			return (AttackPoint) objValues[i];
		}

		public void Convert() {
			switch (type) {
				case SequenceElementType.Bullet:
					objValues = new object[4];
					objValues[0] = count;
					objValues[1] = duration;
					objValues[2] = projectile;
					objValues[3] = 0f;
					break;
				case SequenceElementType.Delay:
					objValues = new object[1];
					objValues[0] = duration;
					break;
				case SequenceElementType.EnablePoint:
				case SequenceElementType.DisablePoint:
					objValues = new object[1];
					objValues[0] = point;
					break;
			}
		}

		public bool CheckDataValidity() {
			switch (type) {
				case SequenceElementType.Bullet:
					return dataTypes.Count == 4 && dataTypes[0] == DataType.Int && dataTypes[1] == DataType.Float && dataTypes[2] == DataType.Proj && dataTypes[3] == DataType.Float;
				case SequenceElementType.Delay:
					return dataTypes.Count == 1 && dataTypes[0] == DataType.Float;
				case SequenceElementType.EnablePoint:
				case SequenceElementType.DisablePoint:
					return dataTypes.Count == 1 && dataTypes[0] == DataType.Point;
				case SequenceElementType.Lazer:
					return dataTypes.Count == 1 && dataTypes[0] == DataType.Float;
				case SequenceElementType.Mortar:
					return dataTypes.Count == 1 && dataTypes[0] == DataType.Int;
				case SequenceElementType.RotationSpeed:
					return dataTypes.Count == 1 && dataTypes[0] == DataType.Float;
				case SequenceElementType.SetRotation:
					return dataTypes.Count == 1 && dataTypes[0] == DataType.Float;
				default:
					return false;
			}
		}

		public void ClearData() {
			dataTypes = new List<DataType>();
			integers = new List<int>();
			floats = new List<float>();
			projectiles = new List<Projectile>();
			points = new List<AttackPoint>();
		}

		public void OnBeforeSerialize() {
			ClearData();
			for (int i = 0; i < objValues.Length; i++) {
				switch (objValues[i]) {
					case int integer:
						dataTypes.Add(DataType.Int);
						integers.Add(integer);
						break;
					case float floating:
						dataTypes.Add(DataType.Float);
						floats.Add(floating);
						break;
					case Projectile projectile:
						dataTypes.Add(DataType.Proj);
						projectiles.Add(projectile);
						break;
					case AttackPoint point:
						dataTypes.Add(DataType.Point);
						points.Add(point);
						break;
				}
			}
		}

		public void OnAfterDeserialize() {
			int integersIndex = 0, floatsIndex = 0, projectilesIndex = 0, pointsIndex = 0;
			objValues = new object[dataTypes.Count];
			for (int i = 0; i < dataTypes.Count; i++) {
				switch (dataTypes[i]) {
					case DataType.Int:
						objValues[i] = integers[integersIndex++];
						break;
					case DataType.Float:
						objValues[i] = floats[floatsIndex++];
						break;
					case DataType.Proj:
						objValues[i] = projectiles[projectilesIndex++];
						break;
					case DataType.Point:
						objValues[i] = points[pointsIndex++];
						break;
				}
			}
		}
	}
}
