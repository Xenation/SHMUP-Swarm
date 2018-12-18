using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Swarm.Editor {
	[CustomEditor(typeof(PatternDefinition))]
	public class PatternDefinitionEditor : UnityEditor.Editor {

		PatternDefinition patternDefinition;

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

		// Sequence
		SerializedProperty sequenceProp;
		ReorderableListProperty sequenceList;

		private void OnEnable() {
			patternDefinition = (PatternDefinition) serializedObject.targetObject;

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
			spawnPointsList = new ReorderableListProperty(spawnPointsProp, true, DrawSpawnPoint);
			spawnPointsList.List.draggable = false;

			sequenceProp = serializedObject.FindProperty("sequence");
			sequenceList = new ReorderableListProperty(sequenceProp, true);
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

			EditorGUILayout.LabelField("Sequence", EditorStyles.boldLabel);
			sequenceList.DoLayoutList();

			serializedObject.ApplyModifiedProperties();
		}

		private void DrawSpawnPoint(Rect rect, int index, bool active, bool focused) {
			//rect.height = 16;
			rect.height = EditorGUIUtility.singleLineHeight;
			rect.y += 1;

			Rect labelRect = new Rect(rect);
			labelRect.width = 25f;
			EditorGUI.LabelField(labelRect, "[" + index + "]");
			rect.x += labelRect.width;
			rect.width -= labelRect.width;

			Rect rectLeft = new Rect(rect);
			rectLeft.width /= 4f;
			Rect rectRight = new Rect(rect);
			rectRight.width /= 4f;
			rectRight.x += rectRight.width * 2f;

			EditorGUI.LabelField(rectLeft, "position");
			rectLeft.x += rectLeft.width;
			rectLeft.width /= 2f;
			patternDefinition.spawnPoints[index].position.x = EditorGUI.FloatField(rectLeft, patternDefinition.spawnPoints[index].position.x);
			rectLeft.x += rectLeft.width;
			patternDefinition.spawnPoints[index].position.y = EditorGUI.FloatField(rectLeft, patternDefinition.spawnPoints[index].position.y);
			EditorGUI.LabelField(rectRight, "rotation");
			rectRight.x += rectRight.width;
			patternDefinition.spawnPoints[index].rotation = EditorGUI.FloatField(rectRight, patternDefinition.spawnPoints[index].rotation);
			spawnPointsList.List.elementHeight = rect.height + 4.0f;
		}

	}
}
