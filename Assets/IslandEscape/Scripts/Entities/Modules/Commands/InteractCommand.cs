using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandEscape.Entities.Modules.Commands
{
    public class InteractCommand : Command
    {
        private ActionModule action;
        public ActionModule Action { get => action; set => action = value; }

        // TODO: cache?
        public AgentModule AgentModule { get => Module.GetComponent<AgentModule>(); }

        // TODO: better name/new type of command/better way to make sure the key stays pressed during interaction
        public bool keyPressed = true;

        public InteractCommand()
        {

        }

        public static new bool Available(Entity entity)
        {
            return entity.HasModule<AgentModule>();
        }

        public override bool Capable()
        {
            return Action.Available && AgentModule.availableAction == Action;
        }

        public override void Execute()
        {
            // TODO: try/catch block?
            Action.StartAction(AgentModule);
        }

        public override bool Check()
        {
            if (!keyPressed)
            {
                if (Action.ActionReady)
                    return true;
                else
                {
                    Module.InterruptCurrentCommand();
                    return false;
                }
            }

            return false;
        }

        public override bool Finish()
        {
            Action.FinishAction();
            return true;
        }
    }
}

