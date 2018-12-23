using System.Runtime.InteropServices;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Swarm {
	public enum SequenceDataType : uint {
		Integer = 0,
		Floating = 1,
		Projectile = 2,
	}

	[System.Serializable]
	public struct JsonObjectReferenceContainer {
		public UnityEngine.Object obj;
	}

	[System.Serializable/*, StructLayout(LayoutKind.Explicit)*/]
	public struct SequenceElementField : ISerializationCallbackReceiver {

		/*[FieldOffset(0)] */public SequenceDataType type;
		
		[System.NonSerialized/*, FieldOffset(8)*/] public int intValue;
		[System.NonSerialized/*, FieldOffset(8)*/] public float floatValue;
		[System.NonSerialized/*, FieldOffset(8)*/] public Projectile projectileValue;

		[SerializeField/*, FieldOffset(16)*/] private string json;
		[SerializeField] private Object unityObject;

		public SequenceElementField(int val) {
			type = SequenceDataType.Integer;
			floatValue = 0f; // pointless af but no choice
			projectileValue = null; // pointless af but no choice
			intValue = val;
			json = null;
			unityObject = null;
		}


		public SequenceElementField(float val) {
			type = SequenceDataType.Floating;
			intValue = 0; // pointless af but no choice
			projectileValue = null; // pointless af but no choice
			floatValue = val;
			json = null;
			unityObject = null;
		}

		public SequenceElementField(Projectile val) {
			type = SequenceDataType.Projectile;
			intValue = 0; // pointless af but no choice
			floatValue = 0f; // pointless af but no choice
			projectileValue = val;
			json = null;
			unityObject = null;
		}

#if UNITY_EDITOR
		public void DrawField(Rect rect, string label) {
			switch (type) {
				case SequenceDataType.Integer:
					intValue = EditorGUI.IntField(rect, label, intValue);
					break;
				case SequenceDataType.Floating:
					floatValue = EditorGUI.FloatField(rect, label, floatValue);
					break;
				case SequenceDataType.Projectile:
					projectileValue = (Projectile) EditorGUI.ObjectField(rect, label, projectileValue, typeof(Projectile), false);
					break;
			}
		}
#endif

		public void OnBeforeSerialize() {
			switch (type) {
				case SequenceDataType.Integer:
					json = intValue.ToString();
					break;
				case SequenceDataType.Floating:
					json = floatValue.ToString();
					break;
				case SequenceDataType.Projectile:
					unityObject = projectileValue;
					break;
			}
		}

		public void OnAfterDeserialize() {
			switch (type) {
				case SequenceDataType.Integer:
					intValue = int.Parse(json);
					break;
				case SequenceDataType.Floating:
					floatValue = float.Parse(json);
					break;
				case SequenceDataType.Projectile:
					if (unityObject == null) break;
					projectileValue = (Projectile) unityObject;
					break;
			}
			json = null;
		}

		public static implicit operator SequenceElementField(int val) {
			return new SequenceElementField(val);
		}

		public static implicit operator SequenceElementField(float val) {
			return new SequenceElementField(val);
		}

		public static implicit operator SequenceElementField(Projectile val) {
			return new SequenceElementField(val);
		}

	}
}
