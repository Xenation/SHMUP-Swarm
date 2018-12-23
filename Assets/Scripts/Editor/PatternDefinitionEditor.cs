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

		private int selectedSpawnPoint = -1;
		private GUIStyle styleBoldPopup;

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
			spawnPointsList = new ReorderableListProperty(spawnPointsProp, true, DrawSpawnPoint, (i) => { return EditorGUIUtility.singleLineHeight + 2f; });
			spawnPointsList.List.draggable = false;

			sequenceProp = serializedObject.FindProperty("sequence");
			sequenceList = new ReorderableListProperty(sequenceProp, true, DrawSequenceElement, SequenceElementHeight);

			SceneView.onSceneGUIDelegate += SceneGUI;
			styleBoldPopup = new GUIStyle(EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).FindStyle("MiniPopup"));
			styleBoldPopup.font = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).FindStyle("BoldLabel").font;
		}

		private void OnDisable() {
			SceneView.onSceneGUIDelegate -= SceneGUI;
		}

		private void SceneGUI(SceneView view) {
			Color colTmp = Handles.color;
			Undo.RecordObject(patternDefinition, "Pattern Spawn Point Move");
			for (int i = 0; i < patternDefinition.spawnPoints.Length; i++) {
				if (selectedSpawnPoint == i) {
					Handles.color = Color.yellow;
				} else {
					Handles.color = Color.blue;
				}
				patternDefinition.spawnPoints[i].position = Handles.FreeMoveHandle(patternDefinition.spawnPoints[i].position, Quaternion.identity, HandleUtility.GetHandleSize(patternDefinition.spawnPoints[i].position) * 0.1f, Vector3.zero, Handles.CubeHandleCap);
				Handles.color = Color.white;
				Quaternion rot = Handles.Disc(Quaternion.Euler(0, 0, patternDefinition.spawnPoints[i].rotation), patternDefinition.spawnPoints[i].position, Vector3.forward, HandleUtility.GetHandleSize(patternDefinition.spawnPoints[i].position) * .5f, false, 0f);
				patternDefinition.spawnPoints[i].rotation = rot.eulerAngles.z;
				Vector2 directionPosition = patternDefinition.spawnPoints[i].position + (Vector2) (Quaternion.Euler(0f, 0f, patternDefinition.spawnPoints[i].rotation) * Vector2.right);
				Handles.color = Color.red;
				Handles.DrawLine(patternDefinition.spawnPoints[i].position, directionPosition);
			}
			Handles.color = colTmp;
		}

		public override void OnInspectorGUI() {
			selectedSpawnPoint = -1;
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
			if (active) {
				selectedSpawnPoint = index;
			}
			rect.height = EditorGUIUtility.singleLineHeight;
			rect.y += 1;

			Rect labelRect = new Rect(rect);
			labelRect.width = 25f;
			EditorGUI.LabelField(labelRect, "[" + index + "]");
			rect.x += labelRect.width;
			rect.width -= labelRect.width;

			Rect rectLeft = new Rect(rect);
			rectLeft.width /= 2f;
			Rect rectRight = new Rect(rect);
			rectRight.width /= 2f;
			rectRight.width -= 20f;
			rectRight.x += rectRight.width + 40f;

			rectLeft.width /= 3f;
			EditorGUI.LabelField(rectLeft, "position");
			rectLeft.x += rectLeft.width;
			patternDefinition.spawnPoints[index].position.x = EditorGUI.FloatField(rectLeft, patternDefinition.spawnPoints[index].position.x);
			rectLeft.x += rectLeft.width;
			patternDefinition.spawnPoints[index].position.y = EditorGUI.FloatField(rectLeft, patternDefinition.spawnPoints[index].position.y);
			rectRight.width /= 3f;
			EditorGUI.LabelField(rectRight, "rotation");
			rectRight.x += rectRight.width;
			rectRight.width *= 2f;
			patternDefinition.spawnPoints[index].rotation = EditorGUI.FloatField(rectRight, patternDefinition.spawnPoints[index].rotation);
			spawnPointsList.List.elementHeight = rect.height + 4.0f;
		}

		private void DrawSequenceElement(Rect rect, int index, bool active, bool focused) {
			rect.SubVerticalRect(4f);
			Rect typeRect = rect.SubVerticalRect(EditorGUIUtility.singleLineHeight, 2f);
			SequenceElementType prevType = patternDefinition.sequence[index].type;
			patternDefinition.sequence[index].type = (SequenceElementType) EditorGUI.EnumPopup(typeRect, patternDefinition.sequence[index].type, styleBoldPopup);
			if (prevType != patternDefinition.sequence[index].type) {
				patternDefinition.sequence[index].ResetData();
			}
			for (int i = 0; i < patternDefinition.sequence[index].fields.Length; i++) {
				patternDefinition.sequence[index].fields[i].DrawField(rect.SubVerticalRect(EditorGUIUtility.singleLineHeight, 2f), patternDefinition.sequence[index].GetFieldName(i));
			}
		}

		private float SequenceElementHeight(int index) {
			return (EditorGUIUtility.singleLineHeight + 2f) * (patternDefinition.sequence[index].fields.Length + 1) + 10f;
		}

	}
}
