using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MadStark.EditorAutomations
{

    [CreateAssetMenu(menuName = "Editor Automation/Build Player", order = 9101)]
    public class BuildPlayerAutomation : Automation
    {
        public string outputDirectory = "Builds";

        public bool overridePlayerName;
        public string customPlayerName;
        public bool automaticallyAppendExtension = true;

        public bool automaticallySetBuildOptions = true;
        public BuildOptions overrideBuildOptions = BuildOptions.None;

        public List<SceneAsset> scenes;


        public override void Execute(ExecutionContext context)
        {
            if (scenes == null || scenes.Count == 0)
                throw new Exception("Add at least one scene to build.");

            string outputPath = PrepareAndGetOutputPath();
            string[] scenePaths = scenes.Select(AssetDatabase.GetAssetPath).ToArray();
            BuildOptions buildOptions = GetBuildOptions();

            BuildPipeline.BuildPlayer(scenePaths, outputPath, EditorUserBuildSettings.activeBuildTarget, buildOptions);
        }

        private string PrepareAndGetOutputPath()
        {
            Directory.CreateDirectory(outputDirectory);

            string playerName = overridePlayerName ? customPlayerName : PlayerSettings.productName;
            if (string.IsNullOrWhiteSpace(playerName))
                throw new Exception("Player name cannot be null.");

            var outputPath = Path.Combine(outputDirectory, playerName);

            if (automaticallyAppendExtension)
                outputPath += GetApplicationExtension();

            return outputPath;
        }

        private BuildOptions GetBuildOptions()
        {
            if (!automaticallySetBuildOptions)
                return overrideBuildOptions;

            BuildOptions buildOptions = BuildOptions.None;

            if (EditorUserBuildSettings.development)
                buildOptions |= BuildOptions.Development;

            return buildOptions;
        }

        private static string GetApplicationExtension()
        {
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.Android:
                    return EditorUserBuildSettings.buildAppBundle ? ".aab" : ".apk";
                case BuildTarget.iOS:
                    return "";
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return ".exe";
                case BuildTarget.StandaloneOSX:
                    return ".app";
                default:
                    return "";
            }
        }
    }
}
