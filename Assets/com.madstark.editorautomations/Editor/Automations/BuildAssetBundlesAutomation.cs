using System.IO;
using UnityEditor;
using UnityEngine;

namespace MadStark.EditorAutomations
{
    [CreateAssetMenu(menuName = "Editor Automation/Build Asset Bundles", order = 9103)]
    public class BuildAssetBundlesAutomation : Automation
    {
        public string outputDirectory = "Assets/StreamingAssets";

        public BuildAssetBundleOptions buildOptions;


        public override void Execute(ExecutionContext context)
        {
            Directory.CreateDirectory(outputDirectory);

            var manifest = BuildPipeline.BuildAssetBundles(outputDirectory, buildOptions, EditorUserBuildSettings.activeBuildTarget);

            if (manifest == null)
                Debug.LogError("Something went wrong while building the asset bundles.");
            else if (outputDirectory.Contains(Application.dataPath))
                AssetDatabase.Refresh();
        }
    }
}
