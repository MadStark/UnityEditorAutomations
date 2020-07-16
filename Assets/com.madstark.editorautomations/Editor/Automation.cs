using UnityEngine;


namespace MadStark.EditorAutomations
{
    public abstract class Automation : ScriptableObject
    {
        public abstract void Execute(ExecutionContext context);
    }
}
