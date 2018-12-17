using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Swarm.Editor {
	[CustomEditor(typeof(PatternDefinition))]
	public class PatternDefinitionEditor : UnityEditor.Editor {
		
		SerializedProperty finishedDelayProp;
		SerializedProperty simultaneousProp;
		ReorderableListProperty simultaneousList;
		SerializedProperty nextPossibleProp;
		ReorderableListProperty nextPossibleList;

		// Spawn Points
		SerializedProperty autoPlacedProp;
		SerializedProperty countProp;
		SerializedProperty startRotationProp;
		SerializedProperty distanceFromCenterProp;
		SerializedProperty rotationSpeedProp;
		SerializedProperty spawnPointsProp;
		ReorderableListProperty spawnPointsList;

		private void OnEnable() {
			finishedDelayProp = serializedObject.FindProperty("finishedDelay");
			simultaneousProp = serializedObject.FindProperty("simultaneous");
			simultaneousList = new ReorderableListProperty(simultaneousProp);
			nextPossibleProp = serializedObject.FindProperty("nextPossible");
			nextPossibleList = new ReorderableListProperty(nextPossibleProp);

			autoPlacedProp = serializedObject.FindProperty("autoPlaced");
			countProp = serializedObject.FindProperty("count");
			startRotationProp = serializedObject.FindProperty("startRotation");
			distanceFromCenterProp = serializedObject.FindProperty("distanceFromCenter");
			rotationSpeedProp = serializedObject.FindProperty("rotationSpeed");
			spawnPointsProp = serializedObject.FindProperty("spawnPoints");
			spawnPointsList = new ReorderableListProperty(spawnPointsProp, true);
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			EditorGUILayout.PropertyField(finishedDelayProp);
			simultaneousList.DoLayoutList();
			nextPossibleList.DoLayoutList();
			
			EditorGUILayout.PropertyField(autoPlacedProp);
			if (autoPlacedProp.boolValue) {
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(countProp);
				EditorGUILayout.PropertyField(startRotationProp);
				EditorGUILayout.PropertyField(distanceFromCenterProp);
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.PropertyField(rotationSpeedProp);
			spawnPointsList.DoLayoutList();

			serializedObject.ApplyModifiedProperties();
		}

	}
}
