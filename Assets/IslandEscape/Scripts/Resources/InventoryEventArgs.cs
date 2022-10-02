using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace IslandEscape.Resources
{
    [Serializable]
    public class InventoryEventArgs
    {
        public Inventory publisher;
        public InventoryAction action;
        public ResourceStack stack;

        public InventoryEventArgs(Inventory publisher, InventoryAction action, ResourceStack stack)
        {
            this.publisher = publisher;
            this.action = action;
            this.stack = stack;
        }
    }
}