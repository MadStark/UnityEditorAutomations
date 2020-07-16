using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace MadStark.BuildPipeline
{
    [CustomEditor(typeof(BuildPlayerFeature))]
    public class BuildPlayerFeatureEditor : Editor
    {
        private SerializedProperty outputDirectory;
        private SerializedProperty scenes;

        private ReorderableList scenesList;


        private void OnEnable()
        {
            outputDirectory = serializedObject.FindProperty("outputDirectory");
            scenes = serializedObject.FindProperty("scenes");

            scenesList = new ReorderableList(serializedObject, scenes);
            scenesList.drawHeaderCallback = ScenesListDrawHeader;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(outputDirectory);

            GUILayout.Space(10);
            scenesList.DoLayoutList();

            GUILayout.Space(10);
            EditorGUILayout.BeginVertical("helpBox");
            EditorGUILayout.LabelField("Command line options:", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField("-outputDirectory <path>");
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }

        private void ScenesListDrawHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Scenes to Build", EditorStyles.boldLabel);
        }
    }
}
