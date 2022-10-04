using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using IslandEscape.Entities.Events;
using IslandEscape.Resources;

namespace IslandEscape.Entities.Modules
{
    // [System.Serializable]
    // public class GatherOption
    // {

    //     // TODO: weighted gather
    //     public float weight = 20;
    // }

    public class GatherModule : EntityModule
    {
        // TODO: multiple options, weighted gather
        // public GatherOption[] options;

        public PartBlueprint blueprint;
        public float probability = 1.0f;
        public string failureMsg = "Unable to find salvageable parts";
        public PartBlueprint requiredTool = null; // TODO: be able to require more than one
        public bool consumeTool = false;
        public string missingToolMsg = "You need something to do this";
        // TODO: show any tool requirement before starting action to avoid sitting through delay just to fail?


        // TODO: add ability to require item in inventory (like a canvas sack to gather sand)

        private ActionModule actionModule;
        private RenderModule renderModule;

        public void Awake()
        {
            AddRequiredModule<ActionModule>();
        }

        public override void Start()
        {
            actionModule = GetComponent<ActionModule>();
            renderModule = GetComponent<RenderModule>();
        }

        public void Gather(ActionEventArgs args)
        {
            Inventory inventory = args.agent.GetComponent<InventoryModule>().Inventory;

            if (requiredTool != null && !inventory.Contains(new ResourceStack(requiredTool, 1)))
            {
                actionModule.timesCompleted--; // TODO: make this configurable?
                SetToolTip(missingToolMsg, 2.0f);
            }
            else
            {
                if (Random.value < probability)
                {
                    // successful gather
                    // TODO: make sure agent has inventorymodule? don't really think that's necessary in this case...
                    inventory.AddResource(new ResourceStack(blueprint, 1));
                    if (requiredTool != null && consumeTool)
                        inventory.RemoveResource(new ResourceStack(requiredTool, 1));

                    // TODO: add optional remove gameobject (but then how to still show tooltip?)

                    SetToolTip($"Found {blueprint.DisplayName}!", 2.0f);

                }
                else
                {
                    // failed gather
                    SetToolTip(failureMsg, 2.0f);
                }
            }
        }

        private void SetToolTip(string msg, float time)
        {
            renderModule.CanvasSetTempToolTip(msg, time, actionModule.Tip, () => { return actionModule.Available && actionModule.HasAvailableAgent; });
        }
    }
}

