using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditorInternal;
using UnityEngine;

namespace Swarm.Editor {
	public class ReorderableListProperty {

		public ReorderableList List { get; private set; }
		private SerializedProperty _property;
		public SerializedProperty Property {
			get {
				return _property;
			}
			set {
				_property = value;
				List.serializedProperty = _property;
			}
		}

		public bool indentElements;

		public ReorderableListProperty(SerializedProperty prop, bool indentElems = false, ReorderableList.ElementCallbackDelegate drawCallback = null) {
			_property = prop;
			indentElements = indentElems;
			if (drawCallback != null) {
				InitList(drawCallback);
			} else {
				InitList(DrawElement);
			}
		}

		public void DoLayoutList() {
			if (!_property.isExpanded) {
				EditorGUILayout.BeginHorizontal();
				GUILayout.Space(6);
				_property.isExpanded = EditorGUILayout.ToggleLeft(string.Format("{0}[]", _property.displayName), _property.isExpanded, EditorStyles.boldLabel);
				EditorGUILayout.LabelField(string.Format("size: {0}", _property.arraySize));
				EditorGUILayout.EndHorizontal();
			} else {
				List.DoLayoutList();
			}
		}

		private void InitList(ReorderableList.ElementCallbackDelegate drawCallback) {
			List = new ReorderableList(_property.serializedObject, _property);
			List.drawHeaderCallback += rect => _property.isExpanded = EditorGUI.ToggleLeft(rect, _property.displayName, _property.isExpanded, EditorStyles.boldLabel);
			List.onCanRemoveCallback += (list) => { return List.count > 0; };
			List.drawElementCallback += drawCallback;
			List.elementHeightCallback += (idx) => { return Mathf.Max(EditorGUIUtility.singleLineHeight, EditorGUI.GetPropertyHeight(_property.GetArrayElementAtIndex(idx), GUIContent.none, true)) + 4.0f; };
		}

		private void DrawElement(Rect rect, int index, bool active, bool focused) {
			if (indentElements) {
				rect.x += 8;
				rect.width -= 8;
			}
			if (_property.GetArrayElementAtIndex(index).propertyType == SerializedPropertyType.Generic) {
				EditorGUI.LabelField(rect, _property.GetArrayElementAtIndex(index).displayName);
			}
			//rect.height = 16;
			rect.height = EditorGUI.GetPropertyHeight(_property.GetArrayElementAtIndex(index), GUIContent.none, true);
			rect.y += 1;
			EditorGUI.PropertyField(rect, _property.GetArrayElementAtIndex(index), GUIContent.none, true);
			//_property.GetArrayElementAtIndex(index).serializedObject.ApplyModifiedProperties();
			this.List.elementHeight = rect.height + 4.0f;
		}

	}
}
