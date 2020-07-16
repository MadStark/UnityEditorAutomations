using System;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MadStark.BuildPipeline
{
    [CustomEditor(typeof(BuildPipeline))]
    public class BuildPipelineEditor : Editor
    {
        private SerializedProperty buildFeatures;

        private bool needsSaving;


        private void OnEnable()
        {
            if (EditorUtility.IsPersistent(target))
                buildFeatures = serializedObject.FindProperty("features");
        }

        public override void OnInspectorGUI()
        {
            DrawBuildFeaturesList();

            if (needsSaving)
                SaveNow();
        }

        private void DrawBuildFeaturesList()
        {
            EditorGUILayout.LabelField(Styles.features, EditorStyles.boldLabel);
            EditorGUILayout.Space();

            if (buildFeatures.arraySize == 0)
            {
                EditorGUILayout.HelpBox("No Build Features added", MessageType.Info);
            }
            else
            {
                CoreEditorUtils.DrawSplitter();
                for (int i = 0; i < buildFeatures.arraySize; i++)
                {
                    SerializedProperty prop = buildFeatures.GetArrayElementAtIndex(i);
                    DrawBuildFeature(i, ref prop);
                    CoreEditorUtils.DrawSplitter();
                }
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Add Build Feature", EditorStyles.miniButton))
            {
                AddFeatureMenu();
            }
        }

        private void DrawBuildFeature(int index, ref SerializedProperty prop)
        {
            Object obj = prop.objectReferenceValue;
            string title = ObjectNames.GetInspectorTitle(obj);

            if (obj != null)
            {
                var serializedFeature = new SerializedObject(obj);

                EditorGUI.BeginChangeCheck();
                bool displayContent = CoreEditorUtils.DrawHeaderToggle(title, prop, serializedFeature.FindProperty("_isActive"), pos => OnContextClick(pos, index));
                if (EditorGUI.EndChangeCheck())
                    SetNeedsSave();

                if (displayContent)
                {
                    GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);

                    EditorGUI.BeginChangeCheck();
                    var propertyName = serializedFeature.FindProperty("m_Name");
                    propertyName.stringValue = ValidateName(EditorGUILayout.DelayedTextField(Styles.featureNameField, propertyName.stringValue));
                    if (EditorGUI.EndChangeCheck())
                        SetNeedsSave();

                    GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
                    var editor = CreateEditor(obj);
                    editor.OnInspectorGUI();
                }

                if (serializedFeature.hasModifiedProperties)
                {
                    serializedFeature.ApplyModifiedProperties();
                    SetNeedsSave();
                }
            }
        }

        private void OnContextClick(Vector2 pos, int index)
        {
            var menu = new GenericMenu();

            if (index == 0)
                menu.AddDisabledItem(EditorGUIUtility.TrTextContent("Move Up"));
            else
                menu.AddItem(EditorGUIUtility.TrTextContent("Move Up"), false, () => MoveComponent(index, -1));

            if (index == buildFeatures.arraySize - 1)
                menu.AddDisabledItem(EditorGUIUtility.TrTextContent("Move Down"));
            else
                menu.AddItem(EditorGUIUtility.TrTextContent("Move Down"), false, () => MoveComponent(index, 1));

            menu.AddSeparator(string.Empty);
            menu.AddItem(EditorGUIUtility.TrTextContent("Remove"), false, () => RemoveFeature(index));

            menu.DropDown(new Rect(pos, Vector2.zero));
        }

        private void MoveComponent(int id, int offset)
        {
            Undo.SetCurrentGroupName("Move Render Feature");
            serializedObject.Update();
            buildFeatures.MoveArrayElement(id, id + offset);
            serializedObject.ApplyModifiedProperties();
            SetNeedsSave();
        }

        private void RemoveFeature(int index)
        {
            var property = buildFeatures.GetArrayElementAtIndex(index);
            Object obj = property.objectReferenceValue;
            property.objectReferenceValue = null;

            Undo.SetCurrentGroupName(obj == null ? "Remove Build Feature" : $"Remove {obj.name}");

            buildFeatures.DeleteArrayElementAtIndex(index);
            serializedObject.ApplyModifiedProperties();

            if (obj != null)
                Undo.DestroyObjectImmediate(obj);

            SaveNow();
        }

        private void AddFeatureMenu()
        {
            var menu = new GenericMenu();
            TypeCache.TypeCollection types = TypeCache.GetTypesDerivedFrom<BuildPipelineFeature>();
            foreach (Type type in types)
            {
                string path = GetMenuNameFromType(type);
                menu.AddItem(new GUIContent(path), false, AddFeature, type.Name);
            }
            menu.ShowAsContext();
        }

        private void AddFeature(object type)
        {
            serializedObject.Update();

            ScriptableObject feature = CreateInstance((string)type);
            feature.name = $"New{(string)type}";
            Undo.RegisterCreatedObjectUndo(feature, "Add Build Pipeline Feature");

            if (EditorUtility.IsPersistent(target))
                AssetDatabase.AddObjectToAsset(feature, target);

            buildFeatures.arraySize++;
            SerializedProperty featureProp = buildFeatures.GetArrayElementAtIndex(buildFeatures.arraySize - 1);
            featureProp.objectReferenceValue = feature;

            serializedObject.ApplyModifiedProperties();

            if (EditorUtility.IsPersistent(target))
                SetNeedsSave();

            serializedObject.ApplyModifiedProperties();
        }

        private string GetMenuNameFromType(Type type)
        {
            string name = type.Name;
            return Regex.Replace(Regex.Replace(name, "([a-z])([A-Z])", "$1 $2", RegexOptions.Compiled), "([A-Z])([A-Z][a-z])", "$1 $2", RegexOptions.Compiled);
        }

        private void SetNeedsSave()
        {
            needsSaving = true;
        }

        private void SaveNow()
        {
            needsSaving = false;
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
        }

        private string ValidateName(string name)
        {
            name = Regex.Replace(name, @"[^a-zA-Z0-9 ]", "");
            return name;
        }

        private static class Styles
        {
            public static readonly GUIContent features = new GUIContent("Build Features");
            public static readonly GUIContent featureNameField = new GUIContent("Name");
        }
    }
}
