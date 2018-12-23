using System.Collections.Generic;
using System.Runtime.InteropServices;
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
		SetRotationAbsolute,
		/// <summary>
		/// 0/float rotationSpeed
		/// </summary>
		SetRotationSpeed
	}

	public struct SequenceElementTypeDef {

		public static SequenceElementTypeDef bullet = new SequenceElementTypeDef() { fieldNames = new string[] { "Count", "Duration", "Projectile", "Speed Override" }, fieldTypes = new SequenceDataType[] { SequenceDataType.Integer, SequenceDataType.Floating, SequenceDataType.Projectile, SequenceDataType.Floating } };
		public static SequenceElementTypeDef lazer = new SequenceElementTypeDef() { fieldNames = new string[] { "Duration" }, fieldTypes = new SequenceDataType[] { SequenceDataType.Floating } };
		public static SequenceElementTypeDef mortar = new SequenceElementTypeDef() { fieldNames = new string[] { "Count" }, fieldTypes = new SequenceDataType[] { SequenceDataType.Integer } };
		public static SequenceElementTypeDef delay = new SequenceElementTypeDef() { fieldNames = new string[] { "Duration" }, fieldTypes = new SequenceDataType[] { SequenceDataType.Floating } };
		public static SequenceElementTypeDef enablePoint = new SequenceElementTypeDef() { fieldNames = new string[] { "Point index" }, fieldTypes = new SequenceDataType[] { SequenceDataType.Integer } };
		public static SequenceElementTypeDef disablePoint = new SequenceElementTypeDef() { fieldNames = new string[] { "Point index" }, fieldTypes = new SequenceDataType[] { SequenceDataType.Integer } };
		public static SequenceElementTypeDef setRotationAbs = new SequenceElementTypeDef() { fieldNames = new string[] { "Absolute Rotation" }, fieldTypes = new SequenceDataType[] { SequenceDataType.Floating } };
		public static SequenceElementTypeDef setRotationSpeed = new SequenceElementTypeDef() { fieldNames = new string[] { "Rotation Speed" }, fieldTypes = new SequenceDataType[] { SequenceDataType.Floating } };

		public int fieldCount { get { return fieldNames.Length; } }
		public string[] fieldNames;
		public SequenceDataType[] fieldTypes;

		public int GetFieldIndex(string name) {
			for (int i = 0; i < fieldNames.Length; i++) {
				if (fieldNames[i] == name) return i;
			}
			return 0;
		}

		public void ResetFields(ref SequenceElementField[] fields) {
			fields = new SequenceElementField[fieldTypes.Length];
			for (int i = 0; i < fieldTypes.Length; i++) {
				fields[i].type = fieldTypes[i];
			}
		}

	}

	public static class SequenceElementTypeExt {
		public static int GetFieldsCount(this SequenceElementType type) {
			switch (type) {
				case SequenceElementType.Bullet:
					return 4;
				case SequenceElementType.Delay:
					return 1;
				case SequenceElementType.EnablePoint:
				case SequenceElementType.DisablePoint:
					return 1;
				case SequenceElementType.Lazer:
					return 1;
				case SequenceElementType.Mortar:
					return 1;
				case SequenceElementType.SetRotationSpeed:
					return 1;
				case SequenceElementType.SetRotationAbsolute:
					return 1;
				default:
					Debug.LogWarning("Sequence Element Type has undefined fields count!");
					return 0;
			}
		}

		public static SequenceElementTypeDef GetDef(this SequenceElementType type) {
			switch (type) {
				case SequenceElementType.Bullet:
					return SequenceElementTypeDef.bullet;
				case SequenceElementType.Delay:
					return SequenceElementTypeDef.delay;
				case SequenceElementType.EnablePoint:
					return SequenceElementTypeDef.enablePoint;
				case SequenceElementType.DisablePoint:
					return SequenceElementTypeDef.disablePoint;
				case SequenceElementType.Lazer:
					return SequenceElementTypeDef.lazer;
				case SequenceElementType.Mortar:
					return SequenceElementTypeDef.mortar;
				case SequenceElementType.SetRotationSpeed:
					return SequenceElementTypeDef.setRotationSpeed;
				case SequenceElementType.SetRotationAbsolute:
					return SequenceElementTypeDef.setRotationAbs;
				default:
					Debug.LogWarning("Sequence Element Type has undefined definition object!");
					return new SequenceElementTypeDef();
			}
		}
		
		public static int FieldNameToIndex(this SequenceElementType type, string name) {
			return type.GetDef().GetFieldIndex(name);
		}

		public static string FieldIndexToName(this SequenceElementType type, int index) {
			return type.GetDef().fieldNames[index];
		}
	}

	[System.Serializable]
	public class SequenceElement {

		public SequenceElementType type;
		public int count;
		public float duration;
		public Projectile projectile;
		
		public SequenceElementField[] fields = new SequenceElementField[0];

		public ref int Int(int i) {
			return ref fields[i].intValue;
		}

		public ref float Float(int i) {
			return ref fields[i].floatValue;
		}

		public ref Projectile Projectile(int i) {
			return ref fields[i].projectileValue;
		}

		public ref SequenceElementField GetField(string name) {
			return ref fields[type.FieldNameToIndex(name)];
		}

		public string GetFieldName(int index) {
			return type.FieldIndexToName(index);
		}

		public void Convert() {
			ResetData();
			switch (type) {
				case SequenceElementType.Bullet:
					fields[0] = count;
					fields[1] = duration;
					fields[2] = projectile;
					fields[3] = 0f;
					break;
				case SequenceElementType.Delay:
					fields[0] = duration;
					break;
			}
			count = 0;
			duration = 0f;
			projectile = null;
		}

		public bool CheckDataValidity() {
			if (fields.Length != type.GetFieldsCount()) return false;
			switch (type) {
				case SequenceElementType.Bullet:
					return fields[0].type == SequenceDataType.Integer && fields[1].type == SequenceDataType.Floating && fields[2].type == SequenceDataType.Projectile && fields[3].type == SequenceDataType.Floating;
				case SequenceElementType.Delay:
					return fields[0].type == SequenceDataType.Floating;
				case SequenceElementType.EnablePoint:
				case SequenceElementType.DisablePoint:
					return fields[0].type == SequenceDataType.Integer;
				case SequenceElementType.Lazer:
					return fields[0].type == SequenceDataType.Floating;
				case SequenceElementType.Mortar:
					return fields[0].type == SequenceDataType.Integer;
				case SequenceElementType.SetRotationSpeed:
					return fields[0].type == SequenceDataType.Floating;
				case SequenceElementType.SetRotationAbsolute:
					return fields[0].type == SequenceDataType.Floating;
				default:
					return false;
			}
		}

		public void ResetData() {
			type.GetDef().ResetFields(ref fields);
		}

	}
}
