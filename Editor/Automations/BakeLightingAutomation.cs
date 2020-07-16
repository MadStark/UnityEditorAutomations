using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace MadStark.EditorAutomations
{
    [CreateAssetMenu(menuName = "Editor Automation/Bake Lighting", order = 9102)]
    public class BakeLightingAutomation : Automation
    {
        public List<SceneAsset> scenes;


        public override void Execute(ExecutionContext context)
        {
            if (scenes == null || scenes.Count == 0)
                throw new Exception("Add at least one scene to bake the lighting.");

            SceneSetup[] cacheSetup = EditorSceneManager.GetSceneManagerSetup();

            List<SceneSetup> setup = scenes
                .Where(x => x != null)
                .Select(x => new SceneSetup {
                    isActive = false,
                    isLoaded = true,
                    isSubScene = false,
                    path = AssetDatabase.GetAssetPath(x)
                })
                .ToList();
            setup[0].isActive = true;

            if (context.trigger != ExecutionTrigger.CommandLine)
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

            EditorSceneManager.RestoreSceneManagerSetup(setup.ToArray());

            Lightmapping.Bake();

            EditorSceneManager.RestoreSceneManagerSetup(cacheSetup);
        }
    }
}
