using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using IslandEscape.Entities;
using IslandEscape.Resources;

namespace IslandEscape.UI
{
    public class PlatformWindow : MonoBehaviour
    {
        // REMINDER THIS IS ACTING AS INVENTORY GRID
        public GameObject partSlotPrefab;

        public GameObject partSlotsPanel;
        public InventoryGrid inventoryGrid;

        // do we need to track this here?
        private PartCategory? selected = null;
        public PartCategory? Selected { get => selected; }

        private Platform platform;

        public void Start()
        {
            platform = GetComponent<Window>().source.GetComponent<Platform>();
            // inventoryGrid.Set(GameManager.instance.player.inventoryModule.Inventory);

            // set up the part slots
            foreach (var requiredPart in platform.requiredParts)
            {
                GameObject partSlot = (GameObject)Instantiate(partSlotPrefab);
                partSlot.transform.SetParent(partSlotsPanel.transform, false);
                partSlot.SetActive(true);

                PartSlotSelect slotSelect = partSlot.GetComponent<PartSlotSelect>();
                slotSelect.Platform = platform;
                slotSelect.Category = requiredPart;

                int partIndex = Array.IndexOf(platform.requiredParts, requiredPart);
                if (platform.Inventory.slots[partIndex] != null)
                    slotSelect.Stack = platform.Inventory.slots[partIndex];

            }
            // TODO: adjust height of window for num slots
        }

        public void SelectSlot(PartCategory? slot)
        {
            // TODO: inventory grid per slot instead of shared? :shrug:
            // TODO: show/hide inventory grid when nothing selected
            inventoryGrid.Filter = slot;
            selected = slot;

            inventoryGrid.gameObject.SetActive(slot != null);
        }

    }
}

