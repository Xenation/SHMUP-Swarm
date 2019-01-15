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
		GameObject = 3,
	}

	[System.Serializable/*, StructLayout(LayoutKind.Explicit)*/]
	public struct SequenceElementField : ISerializationCallbackReceiver {

		/*[FieldOffset(0)] */public SequenceDataType type;
		
		[System.NonSerialized/*, FieldOffset(8)*/] public int intValue;
		[System.NonSerialized/*, FieldOffset(8)*/] public float floatValue;
		[System.NonSerialized/*, FieldOffset(8)*/] public Projectile projectileValue;
		[System.NonSerialized/*, FieldOffset(8)*/] public GameObject gameObjectValue;

		[SerializeField/*, FieldOffset(16)*/] private string json;
		[SerializeField] private Object unityObject;

		public SequenceElementField(int val) {
			type = SequenceDataType.Integer;
			floatValue = 0f; // pointless af but no choice
			projectileValue = null; // pointless af but no choice
			gameObjectValue = null; // pointless af but no choice
			intValue = val;
			json = null;
			unityObject = null;
		}


		public SequenceElementField(float val) {
			type = SequenceDataType.Floating;
			intValue = 0; // pointless af but no choice
			projectileValue = null; // pointless af but no choice
			gameObjectValue = null; // pointless af but no choice
			floatValue = val;
			json = null;
			unityObject = null;
		}

		public SequenceElementField(Projectile val) {
			type = SequenceDataType.Projectile;
			intValue = 0; // pointless af but no choice
			floatValue = 0f; // pointless af but no choice
			gameObjectValue = null; // pointless af but no choice
			projectileValue = val;
			json = null;
			unityObject = null;
		}

		public SequenceElementField(GameObject val) {
			type = SequenceDataType.Projectile;
			intValue = 0; // pointless af but no choice
			floatValue = 0f; // pointless af but no choice
			projectileValue = null; // pointless af but no choice
			gameObjectValue = val;
			json = null;
			unityObject = null;
		}

#if UNITY_EDITOR
		public void DrawField(Rect rect, string label, Object tomark) {
			switch (type) {
				case SequenceDataType.Integer:
					int prevInt = intValue;
					intValue = EditorGUI.IntField(rect, label, intValue);
					if (prevInt != intValue) {
						EditorUtility.SetDirty(tomark);
					}
					break;
				case SequenceDataType.Floating:
					float prevFloat = floatValue;
					floatValue = EditorGUI.FloatField(rect, label, floatValue);
					if (prevFloat != floatValue) {
						EditorUtility.SetDirty(tomark);
					}
					break;
				case SequenceDataType.Projectile:
					Projectile prevProj = projectileValue;
					projectileValue = (Projectile) EditorGUI.ObjectField(rect, label, projectileValue, typeof(Projectile), false);
					if (prevProj != projectileValue) {
						EditorUtility.SetDirty(tomark);
					}
					break;
				case SequenceDataType.GameObject:
					GameObject prevGO = gameObjectValue;
					gameObjectValue = (GameObject) EditorGUI.ObjectField(rect, label, gameObjectValue, typeof(GameObject), false);
					if (prevGO != gameObjectValue) {
						EditorUtility.SetDirty(tomark);
					}
					break;
			}
		}
#endif

		public void ConvertTo(SequenceDataType nType) {
			switch (type) {
				case SequenceDataType.Floating:
					switch (nType) {
						case SequenceDataType.Floating: // Nothing to do
							break;
						case SequenceDataType.Integer:
							intValue = (int) floatValue;
							break;
						default:
							// Can't convert
							break;
					}
					break;
				case SequenceDataType.Integer:
					switch (nType) {
						case SequenceDataType.Floating:
							floatValue = intValue;
							break;
						case SequenceDataType.Integer: // Nothing to do
							break;
						default:
							// Can't convert
							break;
					}
					break;
				default:
					// Can't convert
					break;
			}
			type = nType;
		}

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
				case SequenceDataType.GameObject:
					unityObject = gameObjectValue;
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
					projectileValue = unityObject as Projectile;
					break;
				case SequenceDataType.GameObject:
					gameObjectValue = unityObject as GameObject;
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

		public static implicit operator SequenceElementField(GameObject val) {
			return new SequenceElementField(val);
		}

	}
}
