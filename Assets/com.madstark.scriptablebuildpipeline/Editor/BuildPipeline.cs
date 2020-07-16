using System.Collections.Generic;
using UnityEngine;


namespace MadStark.BuildPipeline
{
    [CreateAssetMenu(fileName = "NewBuildPipeline", menuName = "Build/Build Pipeline")]
    public sealed class BuildPipeline : ScriptableObject
    {
        [SerializeField, HideInInspector]
        private List<BuildPipelineFeature> features;


        public void Build(BuildContext context)
        {
            BuildStepsQueue queue = new BuildStepsQueue();

            foreach (BuildPipelineFeature feature in features)
                feature.EnqueueBuildSteps(context, queue);

            BuildStep step;
            while ((step = queue.Dequeue()) != null)
                step.Execute(context);
        }
    }
}
