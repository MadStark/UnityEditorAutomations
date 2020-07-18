using System;
using System.Collections.Generic;
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

            if (context.trigger != ExecutionTrigger.CommandLine)
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

            SceneSetup[] cacheSetup = EditorSceneManager.GetSceneManagerSetup();

            SceneSetup[] setup = SceneUtils.SceneListToSetup(scenes);
            EditorSceneManager.RestoreSceneManagerSetup(setup);

            Lightmapping.Bake();
            EditorSceneManager.SaveOpenScenes();

            EditorSceneManager.RestoreSceneManagerSetup(cacheSetup);
        }
    }
}
