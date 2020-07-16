using UnityEditor;
using UnityEditor.Build.Reporting;

namespace MadStark.BuildPipeline
{
    public class BuildPlayer : BuildStep
    {
        private readonly string[] scenes;
        private readonly string outputDirectory;


        public BuildPlayer(string[] scenes, string outputDirectory)
        {
            this.scenes = scenes;
            this.outputDirectory = outputDirectory;
        }

        public override void Execute(BuildContext context)
        {
            BuildPlayerOptions options = new BuildPlayerOptions {
                
            };
            BuildReport report = UnityEditor.BuildPipeline.BuildPlayer(scenes, outputDirectory, context.buildTarget, context.buildOptions);
        }

        // private static string GetExtension(BuildTarget buildTarget)
        // {
        //     switch (buildTarget)
        //     {
        //         case BuildTarget.Android:
        //             return EditorUserBuildSettings.buildAppBundle ? ".aab" : ".apk";
        //     }
        // }
    }
}
