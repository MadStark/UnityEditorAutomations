using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MadStark.BuildPipeline
{
    public class BuildPlayerFeature : BuildPipelineFeature
    {
        [SerializeField] private string outputDirectory;

        [SerializeField] private List<SceneAsset> scenes;


        public override void EnqueueBuildSteps(BuildContext _, BuildStepsQueue queue)
        {
            if (string.IsNullOrWhiteSpace(outputDirectory))
                throw new Exception("Output directory not set.");
            if (scenes == null || scenes.Count == 0)
                throw new Exception("No scenes to build.");

            string[] scenePaths = scenes.Select(AssetDatabase.GetAssetPath).ToArray();

            var step = new BuildPlayer(scenePaths, outputDirectory);

            queue.Enqueue(step);
        }
    }
}
