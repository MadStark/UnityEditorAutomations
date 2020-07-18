using System;
using UnityEditor;
using UnityEngine;

namespace MadStark.EditorAutomations
{
    public class ReorderableList
    {
        private readonly SerializedProperty serializedProperty;

        private readonly UnityEditorInternal.ReorderableList list;

        private readonly Func<int, GUIContent> propertyContentCallback;

        private readonly string header;


        public ReorderableList(SerializedObject serializedObject, SerializedProperty serializedProperty, Func<int, GUIContent> propertyContentCallback)
        {
            this.serializedProperty = serializedProperty;
            this.propertyContentCallback = propertyContentCallback;
            header = serializedProperty.displayName;

            list = new UnityEditorInternal.ReorderableList(serializedObject, serializedProperty);
            list.drawElementCallback = DrawElement;
            list.drawHeaderCallback = DrawHeader;
        }

        private void DrawHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, header);
        }

        private void DrawElement(Rect rect, int index, bool _, bool __)
        {
            GUIContent content = propertyContentCallback?.Invoke(index);
            EditorGUI.PropertyField(rect, serializedProperty.GetArrayElementAtIndex(index), content);
        }

        public void Draw()
        {
            list.DoLayoutList();
        }
    }
}
