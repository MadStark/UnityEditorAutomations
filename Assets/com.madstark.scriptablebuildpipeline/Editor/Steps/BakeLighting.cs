using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;


namespace MadStark.BuildPipeline
{
    public class BakeLighting : BuildStep
    {
        private readonly string[] scenes;

        public bool ClearBeforeBake { get; set; }


        public BakeLighting(string[] scenes)
        {
            this.scenes = scenes;
        }

        public override void Execute(BuildContext context)
        {
            SceneSetup[] cacheSceneSetup = EditorSceneManager.GetSceneManagerSetup();

            SceneSetup[] setup = scenes.Select(path => new SceneSetup {path = path, isLoaded = true, isSubScene = false}).ToArray();
            setup[0].isActive = true;

            EditorSceneManager.RestoreSceneManagerSetup(setup);

            if (ClearBeforeBake)
                Lightmapping.Clear();
            Lightmapping.Bake();

            EditorSceneManager.RestoreSceneManagerSetup(cacheSceneSetup);
        }
    }
}
