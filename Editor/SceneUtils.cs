using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace MadStark.EditorAutomations
{
    public static class SceneUtils
    {
        public static SceneSetup[] SceneListToSetup(IEnumerable<SceneAsset> scenes)
        {
            if (scenes == null)
                return new SceneSetup[0];

            SceneSetup[] setup = scenes
                .Where(sceneAsset => sceneAsset != null)
                .Select(sceneAsset => new SceneSetup {
                    isActive = false,
                    isLoaded = true,
                    isSubScene = false,
                    path = AssetDatabase.GetAssetPath(sceneAsset)
                })
                .ToArray();
            setup[0].isActive = true;

            return setup;
        }
    }
}
