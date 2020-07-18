using UnityEditor;
using UnityEngine;

namespace MadStark.EditorAutomations
{
    public static class ContextMenuHandler
    {
        [MenuItem("Assets/Execute Automation", true, 9001)]
        private static bool ExecuteValidate()
        {
            foreach (Object o in Selection.objects)
            {
                if (!(o is Automation))
                    return false;
            }

            return true;
        }

        [MenuItem("Assets/Execute Automation", false, 9001)]
        private static void Execute()
        {
            foreach (Object o in Selection.objects)
            {
                var automation = o as Automation;
                if (automation != null)
                    Automator.Execute(automation);
            }
        }
    }
}
