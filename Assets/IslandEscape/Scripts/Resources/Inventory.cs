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

        public List<ResourceStack> slots;

        // TODO: slot requirements.

        /// <summary>
        /// The maximum size of the inventory. 0 is infinite.
        /// </summary>
        private int numSlots = 0;
        public int NumSlots
        {
            get => numSlots; set
            {
                numSlots = value;

                // TODO: handle changing from limited to unlimited
                if (numSlots != 0)
                {
                    // initialize with base number of slots. this seems dumb and there's probably a
                    // better than to do this (like making a LimitedInventory subclass or something)
                    // but oh well
                    for (int i = 0; i < numSlots; i++)
                    {
                        slots.Add(null);
                    }
                }
            }
        }

        public Inventory()
        {
            slots = new List<ResourceStack>();
        }

        public Inventory(int size) : this()
        {
            NumSlots = size;
        }

        // TODO: genericize
        public IEnumerable<ResourceStack> Filter(PartCategory? category)
        {
            Debug.Log(category);
            foreach (ResourceStack stack in slots)
            {
                if (((PartBlueprint)stack.blueprint).slots.FirstOrDefault(x => x.category == category) != null)
                {
                    yield return stack;
                }
            }
        }

        /// <summary>
        /// Add stack.count number of stack.blueprint to the inventory.
        /// </summary>
        /// <param name="stack"></param>
        public void AddResource(ResourceStack stack)
        {
            // TODO: implement size limit
            // add to existing stack if the inventory already has the item
            ResourceStack existing = slots.FirstOrDefault(x => x.blueprint == stack.blueprint);
            if (existing != null)
                existing.count += stack.count;
            else
                slots.Add(stack);

            InventoryChanged?.Invoke(new InventoryEventArgs(this, InventoryAction.Add, stack));
        }

        /// <summary>
        /// Add the resource to the specified index
        /// </summary>
        /// <param name="stack"></param>
        /// <param name="index"></param>
        /// <exception cref="System.Exception"></exception>
        public void AddResource(ResourceStack stack, int index)
        {
            if (numSlots != 0 && index >= numSlots)
                throw new System.Exception("Invalid inventory index");

            ResourceStack existing = slots[index];
            // TODO: allow replacing stack
            if (existing != null && existing.blueprint != stack.blueprint)
                throw new System.Exception("Inventory slot is full");

            if (existing != null)
                existing.count += stack.count;
            else
                slots.Insert(index, stack);

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

        // TODO: implement remove from index

        public bool Contains(ResourceStack stack)
        {
            ResourceStack existing = slots.FirstOrDefault(x => x.blueprint == stack.blueprint);
            return existing != null && existing.count >= stack.count;
        }
    }
}
