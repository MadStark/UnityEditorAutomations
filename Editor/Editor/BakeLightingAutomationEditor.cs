using System;
using UnityEditor;
using UnityEngine;

namespace MadStark.EditorAutomations
{
    [CustomEditor(typeof(BakeLightingAutomation))]
    public class BakeLightingAutomationEditor : Editor
    {
        private SerializedProperty scenes;

        private ReorderableList scenesList;


        private void OnEnable()
        {
            scenes = serializedObject.FindProperty(nameof(BakeLightingAutomation.scenes));

            var emptyContent = new GUIContent();
            scenesList = new ReorderableList(serializedObject, scenes, _ => emptyContent);
        }

        public override void OnInspectorGUI()
        {
            scenesList.Draw();
        }
    }
}
