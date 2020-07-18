using System;
using UnityEditor;

namespace MadStark.EditorAutomations
{
    [CustomEditor(typeof(BuildAssetBundlesAutomation))]
    public class BuildAssetBundlesAutomationEditor : Editor
    {
        private SerializedProperty outputDirectory;
        private SerializedProperty buildOptions;


        private void OnEnable()
        {
            outputDirectory = serializedObject.FindProperty(nameof(BuildAssetBundlesAutomation.outputDirectory));
            buildOptions = serializedObject.FindProperty(nameof(BuildAssetBundlesAutomation.buildOptions));
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(outputDirectory);
            EditorGUILayout.PropertyField(buildOptions);
        }
    }
}
