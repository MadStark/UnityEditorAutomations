using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MadStark.BuildPipeline
{
    public class BakeNavMeshFeature : BuildPipelineFeature
    {
        [SerializeField] private List<SceneAsset> scenes;


        public override void EnqueueBuildSteps(BuildContext context, BuildStepsQueue queue)
        {
            string[] scenePaths = scenes.Select(AssetDatabase.GetAssetPath).ToArray();
            var step = new BakeNavMesh(scenePaths);
            queue.Enqueue(step);
        }
    }
}
