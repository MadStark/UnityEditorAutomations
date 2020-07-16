using UnityEditor;
using UnityEngine;


namespace MadStark.BuildPipeline
{
    public static class BuildPipelineSettingsProvider
    {
        [SettingsProvider]
        private static SettingsProvider Create()
        {
            var provider = new SettingsProvider("Project/Build Pipeline", SettingsScope.Project) {
                label = "Build Pipeline",
                guiHandler = OnGUI,
                keywords = new[] { "build", "pipeline" }
            };

            return provider;
        }

        private static void OnGUI(string search)
        {
            SerializedObject serializedSettings = new SerializedObject(BuildPipelineSettings.Instance);

            EditorGUILayout.LabelField("Default Build Pipeline Asset", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedSettings.FindProperty("_defaultPipeline"), new GUIContent());

            if (BuildPipelineSettings.DefaultPipeline != null)
            {
                EditorGUILayout.Space();
                var editor = Editor.CreateEditor(BuildPipelineSettings.DefaultPipeline);
                editor.OnInspectorGUI();
            }

            serializedSettings.ApplyModifiedProperties();
            BuildPipelineSettings.Save();
        }
    }
}
