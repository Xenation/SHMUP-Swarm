using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Swarm {
	public enum SequenceElementType {
		/// <summary>
		/// 0/int Count
		/// 1/float Duration
		/// 2/Projectile Projectile
		/// 3/float Speed Override
		/// </summary>
		Bullet,
		/// <summary>
		/// 0/float Width
		/// 1/float Telegraph Duration
		/// 2/float Duration
		/// 3/GameObject Prefab
		/// 4/GameObject Telegraph Prefab
		/// </summary>
		Lazer,
		/// <summary>
		/// 0/float Aim Time
		/// 1/float Radius
		/// 2/float Seek Speed
		/// 3/float Lock Time
		/// 4/GameObject Prefab
		/// </summary>
		Mortar,
		/// <summary>
		/// 0/float Duration
		/// </summary>
		Delay,
		/// <summary>
		/// 0/int Point index
		/// </summary>
		EnablePoint,
		/// <summary>
		/// 0/int Point index
		/// </summary>
		DisablePoint,
		/// <summary>
		/// 0/float Absolute Rotation
		/// </summary>
		SetRotationAbsolute,
		/// <summary>
		/// 0/float Rotation Speed
		/// </summary>
		SetRotationSpeed,
		/// <summary>
		/// 0/float Angle
		/// 1/float Range
		/// 2/float Telegraph Duration
		/// 3/float Duration
		/// 4/GameObject Prefab
		/// 5/GameObject Telegraph Prefab
		/// </summary>
		Cone
	}

	public struct SequenceElementTypeDef {

		public static SequenceElementTypeDef bullet = new SequenceElementTypeDef() { fieldNames = new string[] { "Count", "Duration", "Projectile", "Speed Override" }, fieldTypes = new SequenceDataType[] { SequenceDataType.Integer, SequenceDataType.Floating, SequenceDataType.Projectile, SequenceDataType.Floating } };
		public static SequenceElementTypeDef lazer = new SequenceElementTypeDef() { fieldNames = new string[] { "Width", "Telegraph Duration", "Duration", "Prefab", "Telegraph Prefab" }, fieldTypes = new SequenceDataType[] { SequenceDataType.Floating, SequenceDataType.Floating, SequenceDataType.Floating, SequenceDataType.GameObject, SequenceDataType.GameObject } };
		public static SequenceElementTypeDef mortar = new SequenceElementTypeDef() { fieldNames = new string[] { "Aim Time", "Radius", "Seek Speed", "Lock Time", "Prefab" }, fieldTypes = new SequenceDataType[] { SequenceDataType.Floating, SequenceDataType.Floating, SequenceDataType.Floating, SequenceDataType.Floating, SequenceDataType.GameObject } };
		public static SequenceElementTypeDef delay = new SequenceElementTypeDef() { fieldNames = new string[] { "Duration" }, fieldTypes = new SequenceDataType[] { SequenceDataType.Floating } };
		public static SequenceElementTypeDef enablePoint = new SequenceElementTypeDef() { fieldNames = new string[] { "Point index" }, fieldTypes = new SequenceDataType[] { SequenceDataType.Integer } };
		public static SequenceElementTypeDef disablePoint = new SequenceElementTypeDef() { fieldNames = new string[] { "Point index" }, fieldTypes = new SequenceDataType[] { SequenceDataType.Integer } };
		public static SequenceElementTypeDef setRotationAbs = new SequenceElementTypeDef() { fieldNames = new string[] { "Absolute Rotation" }, fieldTypes = new SequenceDataType[] { SequenceDataType.Floating } };
		public static SequenceElementTypeDef setRotationSpeed = new SequenceElementTypeDef() { fieldNames = new string[] { "Rotation Speed" }, fieldTypes = new SequenceDataType[] { SequenceDataType.Floating } };
		public static SequenceElementTypeDef cone = new SequenceElementTypeDef() { fieldNames = new string[] { "Angle", "Range", "Telegraph Duration", "Duration", "Prefab", "Telegraph Prefab" }, fieldTypes = new SequenceDataType[] { SequenceDataType.Floating, SequenceDataType.Floating, SequenceDataType.Floating, SequenceDataType.Floating, SequenceDataType.GameObject, SequenceDataType.GameObject } };

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

		public bool Matches(SequenceElementField[] fields) {
			if (fields.Length != fieldNames.Length) return false;
			for (int i = 0; i < fields.Length; i++) {
				if (fields[i].type != fieldTypes[i]) return false;
			}
			return true;
		}

		public void UpdateFields(ref SequenceElementField[] fields) {
			if (fields.Length != fieldNames.Length) { // fields size change
				SequenceElementField[] oldFields = new SequenceElementField[fields.Length];
				fields.CopyTo(oldFields, 0);
				fields = new SequenceElementField[fieldNames.Length];
				// Copy existing fields and trim
				for (int i = 0; i < oldFields.Length && i < fieldNames.Length; i++) {
					fields[i] = oldFields[i];
				}
				// Append new fields
				for (int i = oldFields.Length; i < fieldNames.Length; i++) {
					fields[i] = new SequenceElementField() { type = fieldTypes[i] };
				}
			}
			// Convert fields
			for (int i = 0; i < fields.Length; i++) {
				if (fields[i].type != fieldTypes[i]) { // Conversion needed
					fields[i].ConvertTo(fieldTypes[i]);
				}
			}
		}

	}

	public static class SequenceElementTypeExt {
		public static int GetFieldsCount(this SequenceElementType type) {
			return type.GetDef().fieldCount;
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
				case SequenceElementType.Cone:
					return SequenceElementTypeDef.cone;
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
	public class SequenceElement : ISerializationCallbackReceiver {

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
			return type.GetDef().Matches(fields);
		}

		public void ResetData() {
			type.GetDef().ResetFields(ref fields);
		}

		public void OnBeforeSerialize() {
			// Nothing to do
		}

		public void OnAfterDeserialize() {
			if (!CheckDataValidity()) {
				type.GetDef().UpdateFields(ref fields);
			}
		}

	}
}
