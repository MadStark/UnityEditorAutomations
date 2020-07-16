using System;
using System.Collections.Generic;
using UnityEngine;

namespace MadStark.EditorAutomations
{
    [CreateAssetMenu(menuName = "Editor Automation/Pipeline", order = 9501)]
    public class Pipeline : Automation
    {
        public List<Automation> automations;


        public override void Execute(ExecutionContext context)
        {
            foreach (Automation automation in automations)
            {
                if (automation == this)
                    throw new Exception("Pipeline is being asked to execute itself. Clearly this is not going to go well. Skipping.");

                automation.Execute(context);
            }
        }
    }
}
