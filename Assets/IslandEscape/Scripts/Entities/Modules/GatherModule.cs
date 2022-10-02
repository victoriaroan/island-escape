using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using IslandEscape.Entities.Events;
using IslandEscape.Resources;

namespace IslandEscape.Entities.Modules
{
    public class GatherModule : EntityModule
    {
        public PartBlueprint blueprint;
        public float probability = 1.0f;
        public string failureMsg = "Unable to find salvageable parts";

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
            if (Random.value < probability)
            {
                // successful gather
                // TODO: make sure agent has inventorymodule. don't really think that's necessary in this case...
                args.agent.GetComponent<InventoryModule>().inventory.AddResource(new ResourceStack(blueprint, 1));

                // TODO: remove gameobject?

                var tip = $"Found {blueprint.DisplayName}!";
                if (actionModule.Available)
                    renderModule.CanvasSetTempTooltip(tip, 2.0f, actionModule.Tip);
                else
                    renderModule.CanvasSetTempTooltip(tip, 2.0f);
            }
            else
            {
                // failed gather
                if (actionModule.Available)
                    renderModule.CanvasSetTempTooltip(failureMsg, 2.0f, actionModule.Tip);
                else
                    renderModule.CanvasSetTempTooltip(failureMsg, 2.0f);
            }
        }
    }
}

