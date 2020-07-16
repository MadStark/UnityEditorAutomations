using UnityEditor;

namespace MadStark.EditorAutomations
{
    public static class Automator
    {
        public static void Execute(Automation automation)
        {
            var context = new ExecutionContext {
                trigger = ExecutionTrigger.Script
            };

            automation.Execute(context);
        }

        private static void Execute()
        {
            //TODO: Read path to execute from command line args
        }
    }
}
