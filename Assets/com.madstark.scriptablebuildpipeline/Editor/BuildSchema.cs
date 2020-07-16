using UnityEditor;
using UnityEngine;

namespace MadStark.BuildPipeline
{
    [CreateAssetMenu(fileName = "NewBuildSchema", menuName = "Build/Schema")]
    public class BuildSchema : ScriptableObject
    {
        [SerializeField] private bool _debugBuild;


        internal BuildOptions GetBuildOptions()
        {
            return BuildOptions.None |
                   (_debugBuild ? BuildOptions.Development : 0);
        }
    }
}
