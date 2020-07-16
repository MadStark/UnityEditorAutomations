using System;
using UnityEditor;

namespace MadStark.BuildPipeline
{
    public static class Builder
    {
        public static void Build()
        {
            if (BuildPipelineSettings.DefaultPipeline != null)
            {
                BuildContext context = new BuildContext();
                context.buildTarget = EditorUserBuildSettings.activeBuildTarget;
                context.commandLineArgs = Environment.GetCommandLineArgs();
                if (BuildPipelineSettings.DefaultSchema != null)
                    context.buildOptions = BuildPipelineSettings.DefaultSchema.GetBuildOptions();

                BuildPipelineSettings.DefaultPipeline.Build(context);
            }
        }

        private static void ParseCommandLineArgs()
        {

        }

        private class CommandLineArgs
        {
            public string buildPipelinePath;

            public string buildSchemaPath;
        }
    }
}
