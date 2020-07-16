using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace MadStark.BuildPipeline
{
    [CustomEditor(typeof(BakeLightingFeature))]
    public class BakeLightingFeatureEditor : Editor
    {
        private SerializedProperty scenes;

        private ReorderableList scenesList;


        private void OnEnable()
        {
            scenes = serializedObject.FindProperty("scenes");

            scenesList = new ReorderableList(serializedObject, scenes);
            scenesList.drawHeaderCallback = ScenesListDrawHeader;
        }

        public override void OnInspectorGUI()
        {
            scenesList.DoLayoutList();
        }

        private void ScenesListDrawHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Scenes to Bake", EditorStyles.boldLabel);
        }
    }
}
