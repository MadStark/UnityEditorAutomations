using UnityEngine;

namespace MadStark.BuildPipeline
{
    public class BakeNavMesh : BuildStep
    {
        private readonly string[] scenes;


        public BakeNavMesh(string[] scenes)
        {
            this.scenes = scenes;
        }

        public override void Execute(BuildContext context)
        {
            Debug.LogError($"{nameof(BakeNavMesh)} is not yet implemented.");
        }
    }
}
