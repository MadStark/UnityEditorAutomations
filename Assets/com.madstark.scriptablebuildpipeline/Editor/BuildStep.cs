namespace MadStark.BuildPipeline
{
    public abstract class BuildStep
    {
        public abstract void Execute(BuildContext context);
    }
}
