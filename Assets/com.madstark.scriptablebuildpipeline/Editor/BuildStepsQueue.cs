using System.Collections.Generic;

namespace MadStark.BuildPipeline
{
    public class BuildStepsQueue
    {
        private readonly Queue<BuildStep> queue = new Queue<BuildStep>();


        public void Enqueue(BuildStep buildStep)
        {
            queue.Enqueue(buildStep);
        }

        internal BuildStep Dequeue()
        {
            return queue.Dequeue();
        }
    }
}
