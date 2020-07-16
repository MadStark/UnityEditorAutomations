using UnityEditor;


namespace MadStark.BuildPipeline
{
    public struct BuildContext
    {
        public BuildTarget buildTarget;

        public BuildOptions buildOptions;

        public string[] commandLineArgs;
    }
}
