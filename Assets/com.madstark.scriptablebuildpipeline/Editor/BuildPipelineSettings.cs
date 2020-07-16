using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;


namespace MadStark.BuildPipeline
{
    public class BuildPipelineSettings : ScriptableObject
    {
        [SerializeField] private BuildPipeline _defaultPipeline;

        [SerializeField] private BuildSchema _defaultSchema;


        public static BuildPipeline DefaultPipeline => Instance._defaultPipeline;

        public static BuildSchema DefaultSchema => Instance._defaultSchema;


        #region Instance


        private const string kSettingsPath = "ProjectSettings/Packages/BuildPipelineSettings.asset";

        private static BuildPipelineSettings _instance;

        internal static BuildPipelineSettings Instance {
            [InitializeOnLoadMethod]
            get {
                if (_instance == null)
                {
                    Object[] objs = InternalEditorUtility.LoadSerializedFileAndForget(kSettingsPath);
                    if (objs != null && objs.Length > 0 && objs[0] != null)
                        _instance = objs[0] as BuildPipelineSettings;

                    if (_instance == null)
                    {
                        _instance = CreateInstance<BuildPipelineSettings>();
                        Save();
                    }
                }

                return _instance;
            }
        }

        internal static void Save()
        {
            if (_instance == null)
                return;

            Directory.CreateDirectory(Path.GetDirectoryName(kSettingsPath));
            InternalEditorUtility.SaveToSerializedFileAndForget(new[] {_instance}, kSettingsPath, true);
        }


        #endregion Instance
    }
}
