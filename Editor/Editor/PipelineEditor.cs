using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MadStark.EditorAutomations
{
    [CustomEditor(typeof(Pipeline))]
    public class PipelineEditor : Editor
    {
        private SerializedProperty automationsList;

        private readonly List<Vector2> elementsMiddleLefts = new List<Vector2>();

        private Vector2 scrolling;

        private Dictionary<string, Editor> cachedEditors = new Dictionary<string, Editor>();


        private void OnEnable()
        {
            cachedEditors.Clear();
            automationsList = serializedObject.FindProperty(nameof(Pipeline.automations));
        }

        public override void OnInspectorGUI()
        {
            elementsMiddleLefts.Clear();
            EditorGUILayout.BeginScrollView(scrolling);
            {
                for (int i = 0; i < automationsList.arraySize; i++)
                {
                    EditorGUILayout.Space();
                    DrawAutomationsListElement(i);
                }

                EditorGUILayout.Space();
                if (GUILayout.Button("Add Existing Automation"))
                    SelectAutomationPopup(Event.current.mousePosition);
            }
            EditorGUILayout.EndScrollView();

            if (elementsMiddleLefts.Count > 0)
            {
                Vector2 lastMiddleLeft = elementsMiddleLefts.Last();
                var tbLineRect = new Rect(12, 1, 1, lastMiddleLeft.y);
                EditorGUI.DrawRect(tbLineRect, EditorUtils.AccentColor);
            }

            foreach (Vector2 middleLeft in elementsMiddleLefts)
            {
                var lrLineRect = new Rect(12, middleLeft.y, middleLeft.x, 1);
                EditorGUI.DrawRect(lrLineRect, EditorUtils.AccentColor);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawAutomationsListElement(int index)
        {
            SerializedProperty element = automationsList.GetArrayElementAtIndex(index);

            EditorGUILayout.BeginVertical(Styles.card);
            {
                Vector2 middleLeft = GUILayoutUtility.GetRect(0, 0).position;

                if (element.propertyType == SerializedPropertyType.ObjectReference && element.objectReferenceValue != null)
                {
                    EditorGUILayout.LabelField(element.objectReferenceValue.name, EditorStyles.boldLabel);
                    var titleRect = GUILayoutUtility.GetLastRect();
                    if (Event.current.type == EventType.ContextClick && titleRect.Contains(Event.current.mousePosition))
                        OnContextClick(Event.current.mousePosition, index);
                    EditorGUILayout.Space();

                    Editor ed;
                    string propertyPath = element.propertyPath;
                    if (!cachedEditors.TryGetValue(propertyPath, out Editor elementEditor))
                    {
                        elementEditor = CreateEditor(element.objectReferenceValue);
                        cachedEditors.Add(propertyPath, elementEditor);
                    }
                    elementEditor.OnInspectorGUI();
                }

                middleLeft = (middleLeft + GUILayoutUtility.GetRect(0, 0).position) * 0.5f;
                elementsMiddleLefts.Add(middleLeft);
            }
            EditorGUILayout.EndVertical();
        }

        private void OnContextClick(Vector2 pos, int index)
        {
            var menu = new GenericMenu();

            if (index == 0)
                menu.AddDisabledItem(EditorGUIUtility.TrTextContent("Move Up"));
            else
                menu.AddItem(EditorGUIUtility.TrTextContent("Move Up"), false, () => MoveComponent(index, -1));

            if (index == automationsList.arraySize - 1)
                menu.AddDisabledItem(EditorGUIUtility.TrTextContent("Move Down"));
            else
                menu.AddItem(EditorGUIUtility.TrTextContent("Move Down"), false, () => MoveComponent(index, +1));

            menu.AddSeparator(string.Empty);
            menu.AddItem(EditorGUIUtility.TrTextContent("Remove"), false, () => RemoveAutomation(index));

            menu.DropDown(new Rect(pos, Vector2.zero));
        }

        private void RemoveAutomation(int index)
        {
            var property = automationsList.GetArrayElementAtIndex(index);
            Object obj = property.objectReferenceValue;
            property.objectReferenceValue = null;

            Undo.RecordObject(target, obj == null ? "Remove Automation" : $"Remove Automation {obj.name}");
            automationsList.DeleteArrayElementAtIndex(index);
            serializedObject.ApplyModifiedProperties();
        }

        private void MoveComponent(int index, int offset)
        {
            serializedObject.Update();
            Undo.RecordObject(target, "Move Automation");
            automationsList.MoveArrayElement(index, index + offset);
            serializedObject.ApplyModifiedProperties();
        }

        private void SelectAutomationPopup(Vector2 cursorPos)
        {
            string[] automationGuids = AssetDatabase.FindAssets("t:Automation");
            string thisAssetPath = AssetDatabase.GetAssetPath(target);

            var menu = new GenericMenu();
            foreach (string guid in automationGuids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (thisAssetPath == assetPath)
                    continue;

                string assetName = Path.GetFileNameWithoutExtension(assetPath);
                menu.AddItem(new GUIContent(assetName), false, AddAutomation, assetPath);
            }

            menu.DropDown(new Rect(cursorPos, Vector2.zero));
        }

        private void AddAutomation(object path)
        {
            serializedObject.Update();

            Automation automation = AssetDatabase.LoadAssetAtPath<Automation>((string)path);
            Undo.RecordObject(target, $"Add automation {automation.name}");

            SerializedProperty automationProp = automationsList.GetArrayElementAtIndex(automationsList.arraySize++);
            automationProp.objectReferenceValue = automation;

            serializedObject.ApplyModifiedProperties();
        }

        private static class Styles
        {
            public static readonly GUIStyle card = new GUIStyle(EditorStyles.helpBox) {
                margin = new RectOffset(6, EditorStyles.helpBox.margin.right, EditorStyles.helpBox.margin.top, EditorStyles.helpBox.margin.bottom),
                padding = new RectOffset(7, EditorStyles.helpBox.padding.right, EditorStyles.helpBox.padding.top, EditorStyles.helpBox.padding.bottom)
            };
        }
    }
}
