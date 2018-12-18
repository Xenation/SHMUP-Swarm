using UnityEngine;
using UnityEditor;

namespace Swarm.Editor {
	[CustomPropertyDrawer(typeof(SpawnPointDefinition))]
	public class SpawnPointDefinitionPropEditor : PropertyDrawer {

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			return EditorGUIUtility.singleLineHeight;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			SerializedProperty posProp = property.FindPropertyRelative("position");
			SerializedProperty rotProp = property.FindPropertyRelative("rotation");
			Vector2 pos = posProp.vector2Value;
			float rot = rotProp.floatValue;

			rot = EditorGUI.FloatField(position, rot);

			rotProp.floatValue = rot;
			property.serializedObject.ApplyModifiedProperties();
		}

	}
}
