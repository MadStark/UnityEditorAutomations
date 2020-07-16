using UnityEngine;


namespace MadStark.BuildPipeline
{
    public abstract class BuildPipelineFeature : ScriptableObject
    {
        [SerializeField, HideInInspector] private bool _isActive = true;

        public bool IsActive => _isActive;


        public abstract void EnqueueBuildSteps(BuildContext context, BuildStepsQueue queue);
    }
}
