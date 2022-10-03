using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

using IslandEscape.Resources;

namespace IslandEscape.UI
{
    public class InventorySlot : MonoBehaviour, IDropHandler
    {
        public int index;
        public ResourceStack stack;

        public GameObject prefab;
        public GameObject slotDisplay;

        public bool HasStack { get { return stack != null && stack.IsSet; } }

        // TODO: inventory slot without parent grid?
        protected internal InventoryGrid parentGrid;

        public void Init(ResourceStack stack)
        {
            parentGrid = transform.parent.GetComponent<InventoryGrid>();
            index = parentGrid.inventory.slots.IndexOf(stack);
            gameObject.SetActive(true);
            UpdateSlot();
        }

        public void Init(int i)
        {
            index = i;
            parentGrid = transform.parent.GetComponent<InventoryGrid>();
            gameObject.SetActive(true);
        }

        public void UpdateSlot()
        {
            stack = parentGrid.inventory.slots[index];
            // clear out the old inventory slot display
            // TODO: do we really need to clear and recreate display every time?
            if (slotDisplay != null)
                Destroy(slotDisplay);

            // add new display if the slot has a stack
            // TODO: allow 0-stacks?
            if (HasStack)
                AddDisplay();
        }

        public void AddDisplay()
        {
            slotDisplay = (GameObject)Instantiate(prefab);
            slotDisplay.transform.SetParent(transform, false);
            slotDisplay.GetComponent<InventorySlotDisplay>().Set(stack);
            slotDisplay.SetActive(true);
        }

        public void OnDrop(PointerEventData data)
        {
            Debug.Log("Dropped on Inventory Slot", gameObject);
            Debug.Log("dropped stack display", data.pointerPress);
            if (parentGrid.enableDropping)
            {
                Debug.Log("Dropping enabled");
                // TODO: nothing being dragged
                GameObject droppedStackDisplay = data.pointerPress;
                GameObject sourceSlotObj = droppedStackDisplay.transform.parent.gameObject;
                // Debug.Log("source slot", sourceSlotObj);
                InventorySlot sourceSlot = sourceSlotObj.GetComponent<InventorySlot>();
                ResourceStack droppedStack = droppedStackDisplay.GetComponent<InventorySlotDisplay>().stack;
                if (sourceSlotObj == gameObject)
                {
                    // dropped onto the same slot. reset stack position and do nothing
                    Debug.Log("Dropped stack on original slot");
                    droppedStackDisplay.transform.position = transform.position;
                }
                else
                {
                    Debug.Log("Dropped on different slot");
                    // TODO: stack items of same blueprint (need to figure out how to deal with purchase records, not that those currently exist.)
                    if (parentGrid == sourceSlot.parentGrid)
                    {
                        Debug.Log("same inventory");
                        // same inventory
                        // parentGrid.inventory.SwapStacks(sourceSlot.index, index);
                    }
                    else
                    {
                        Debug.Log("different inventory");
                        // different inventories, add stack to destination
                        // ResourceStack existingStack = parentGrid.inventory.Add(droppedStack, index);

                        // // then either set the source slot if the destination had an existing stack, or remove the dropped stack from the source
                        // if (existingStack != null && existingStack.IsSet)
                        //     sourceSlot.parentGrid.inventory.AddStack(existingStack, sourceSlot.index);
                        // else
                        //     sourceSlot.parentGrid.inventory.RemoveStack(sourceSlot.index);

                        // et voila! the slots will automatically update when the InventoryChanged events are triggered.
                    }
                }
            }
        }
    }
}
