using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using IslandEscape.Resources;

namespace IslandEscape.UI
{

    public class InventoryGrid : MonoBehaviour
    {
        public bool showCount = false;
        public bool enableDragging = false;
        public bool enableDropping = false;
        public bool enableHighlight = true;

        public EventTrigger.TriggerEvent onClickStack;
        public EventTrigger.TriggerEvent onDragStack;
        public EventTrigger.TriggerEvent onDropStack;

        public GameObject slotBase;

        public List<InventorySlot> slots;
        public Inventory inventory;
        public Vector2 slotSize;

        private GridLayoutGroup gridLayout;
        private GameObject slotImage;
        private GameObject slotCount;

        private void Init()
        {
            gridLayout = gameObject.GetComponent<GridLayoutGroup>();
            slotSize = gridLayout.cellSize;
        }

        /*
        Set the Inventory to be displayed by the Inventory Grid.
        */
        public void Set(Inventory inv)
        {
            // remove any existing gameobjects and unregister current inventory
            foreach (InventorySlot slot in slots)
                Destroy(slot.gameObject);
            UnregisterInventory();

            // adjust UI sizes
            Init();

            // register the new inventory object
            RegisterInventory(inv);
            ResetSlots();
        }

        private void ResetSlots()
        {

            slots = new List<InventorySlot>();
            // TODO: update this for a list (was written for array)
            foreach (ResourceStack stack in inventory.slots)
            {
                AddSlot(stack);
            }
            // for (int i = 0; i < slots.Count; i++)
            // {
            //     slots[i] = AddSlot(i);
            //     slots[i].UpdateSlot();
            // }
        }

        public void AddSlot(ResourceStack stack)
        {
            GameObject slotObj = (GameObject)Instantiate(slotBase);
            slotObj.transform.SetParent(transform, false);
            slotObj.GetComponent<InventorySlot>().Init(stack);
            slots.Add(slotObj.GetComponent<InventorySlot>());
        }

        public InventorySlot AddSlot(int index)
        {
            GameObject slotObj = (GameObject)Instantiate(slotBase);
            slotObj.transform.SetParent(transform, false);
            InventorySlot slot = slotObj.GetComponent<InventorySlot>();
            slot.Init(index);
            return slot;
        }

        public void OnInventoryChanged(InventoryEventArgs args)
        {
            Debug.Log("InventoryGrid: OnInventoryChanged");
            // for (int i = 0; i < slots.Count; i++)
            //     slots[i].UpdateSlot();

            // just gonna recreate the whole thing for simplicity
            foreach (InventorySlot slot in slots)
                Destroy(slot.gameObject);

            ResetSlots();
        }

        public void RegisterInventory(Inventory inv)
        {
            inventory = inv;
            Debug.Log("Register Inventory", gameObject);
            Debug.Log(inventory);
            if (inventory != null)
                inventory.InventoryChanged?.AddListener(OnInventoryChanged);
        }

        public void UnregisterInventory()
        {
            if (inventory != null)
                inventory.InventoryChanged?.RemoveListener(OnInventoryChanged);
        }

        void OnDestroy()
        {
            UnregisterInventory();
        }
    }
}
