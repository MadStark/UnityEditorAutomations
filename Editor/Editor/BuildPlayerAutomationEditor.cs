using UnityEditor;
using UnityEngine;

namespace MadStark.EditorAutomations
{
    [CustomEditor(typeof(BuildPlayerAutomation))]
    public class BuildPlayerAutomationEditor : Editor
    {
        private SerializedProperty outputDirectory;
        private SerializedProperty overridePlayerName;
        private SerializedProperty customPlayerName;
        private SerializedProperty automaticallyAppendExtension;
        private SerializedProperty automaticallySetBuildOptions;
        private SerializedProperty overrideBuildOptions;
        private SerializedProperty scenes;

        private ReorderableList sceneList;


        private void OnEnable()
        {
            outputDirectory = serializedObject.FindProperty(nameof(BuildPlayerAutomation.outputDirectory));
            overridePlayerName = serializedObject.FindProperty(nameof(BuildPlayerAutomation.overridePlayerName));
            customPlayerName = serializedObject.FindProperty(nameof(BuildPlayerAutomation.customPlayerName));
            automaticallyAppendExtension = serializedObject.FindProperty(nameof(BuildPlayerAutomation.automaticallyAppendExtension));
            automaticallySetBuildOptions = serializedObject.FindProperty(nameof(BuildPlayerAutomation.automaticallySetBuildOptions));
            overrideBuildOptions = serializedObject.FindProperty(nameof(BuildPlayerAutomation.overrideBuildOptions));
            scenes = serializedObject.FindProperty(nameof(BuildPlayerAutomation.scenes));

            sceneList = new ReorderableList(serializedObject, scenes,
                i => i == 0 ? SharedStyles.mainSceneContent : new GUIContent($"{i}"));
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(outputDirectory);
            EditorGUILayout.PropertyField(overridePlayerName);
            if (overridePlayerName.boolValue)
                EditorGUILayout.PropertyField(customPlayerName);
            EditorGUILayout.PropertyField(automaticallyAppendExtension);

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(automaticallySetBuildOptions);
            if (!automaticallySetBuildOptions.boolValue)
                EditorGUILayout.PropertyField(overrideBuildOptions);

            EditorGUILayout.Space();
            sceneList.Draw();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
