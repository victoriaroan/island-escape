using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IslandEscape.Resources
{
    public enum InventoryAction
    {
        Add,
        Remove
    }

    // TODO: I feel like there's a better name for this exception...
    public class NotInInventoryException : System.Exception { }

    [System.Serializable]
    public class Inventory
    {
        public InventoryEvent InventoryChanged = new InventoryEvent();

        // infinite inventory
        public List<ResourceStack> slots;

        public Inventory()
        {
            slots = new List<ResourceStack>();
        }

        /// <summary>
        /// Add stack.count number of stack.blueprint to the inventory.
        /// </summary>
        /// <param name="stack"></param>
        public void AddResource(ResourceStack stack)
        {
            // add to existing stack if the inventory already has the item
            ResourceStack existing = slots.FirstOrDefault(x => x.blueprint == stack.blueprint);
            if (existing != null)
                existing.count += stack.count;
            else
                slots.Add(stack);

            InventoryChanged?.Invoke(new InventoryEventArgs(this, InventoryAction.Add, stack));
        }

        /// <summary>
        /// Remove stack.count number of stack.blueprint from the inventory
        /// </summary>
        /// <param name="stack"></param>
        public void RemoveResource(ResourceStack stack)
        {
            ResourceStack existing = slots.FirstOrDefault(x => x.blueprint == stack.blueprint);
            if (existing == null || existing.count < stack.count)
            {
                throw new NotInInventoryException();
            }

            existing.count -= stack.count;
            if (existing.count == 0)
            {
                slots.Remove(existing);
            }
            InventoryChanged?.Invoke(new InventoryEventArgs(this, InventoryAction.Remove, stack));
        }

        public bool Contains(ResourceStack stack)
        {
            ResourceStack existing = slots.FirstOrDefault(x => x.blueprint == stack.blueprint);
            return existing != null && existing.count >= stack.count;
        }
    }
}
