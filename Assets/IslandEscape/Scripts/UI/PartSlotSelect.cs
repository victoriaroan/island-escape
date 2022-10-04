using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

using IslandEscape.Entities;
using IslandEscape.Entities.Modules;
using IslandEscape.Resources;

namespace IslandEscape.UI
{
    public class PartSlotSelect : UIComponent
    {
        public PartCategory category;
        public PartCategory Category
        {
            get => category;
            set
            {
                // TODO: clear slot/selected? not gonna be changing category on the fly, so not really important
                category = value;
                slotName.text = category.ToString();
                optionsGrid.Filter = category;
            }
        }

        // TODO: want to make this private but still be able to see it in editor cause right now it creates an empty stack
        public ResourceStack stack;
        public ResourceStack Stack
        {
            get => stack;
            set
            {
                stack = value;
                resourceSlot.Stack = stack;
            }
        }

        private bool optionsOpen = false;


        private Platform platform;
        public Platform Platform
        {
            get => platform;
            set
            {
                platform = value;
            }
        }

        public override bool RaycastTarget
        {
            set
            {
                slotName.raycastTarget = value;
                selectButton.GetComponent<Image>().raycastTarget = value;
                arrowTransform.GetComponent<Image>().raycastTarget = value;
            }
        }

        private ResourceSlot resourceSlot;
        private InventoryGrid optionsGrid;
        private TextMeshProUGUI slotName;
        private Button selectButton;
        private RectTransform arrowTransform;

        public override void RegisterReferences()
        {
            slotName = transform.Find("SlotName").GetComponent<TextMeshProUGUI>();
            resourceSlot = transform.Find("Trigger/SlotBase").GetComponent<ResourceSlot>();
            selectButton = transform.Find("Trigger").GetComponent<Button>();
            arrowTransform = transform.Find("Trigger/Arrow").GetComponent<RectTransform>();
            optionsGrid = transform.Find("InventoryGrid").GetComponent<InventoryGrid>();
            optionsGrid.Set(GameManager.instance.player.inventoryModule.Inventory);
        }

        public void ToggleOptions()
        {
            // TODO: auto-close all other part options?
            if (optionsOpen)
            {
                optionsGrid.gameObject.SetActive(false);
            }
            else
            {
                optionsGrid.gameObject.SetActive(true);
            }
            optionsOpen = !optionsOpen;
        }

        public void SelectOption(PointerEventData args)
        {
            // TODO: right click to remove selection?
            // TODO: tooltip doesn't go away on select
            // TODO: also not sure why tooltips are so big
            if (resourceSlot.HasStack)
            {
                // TODO: swapping parts
                Platform.GetComponent<RenderModule>().CanvasSetTempToolTip("Slot full", 2.0f);
            }
            else
            {
                GameObject clickedStackDisplay = args.pointerPress;
                ResourceStack clickedStack = clickedStackDisplay.GetComponent<InventorySlotDisplay>().stack;
                int partIndex = Array.IndexOf(Platform.requiredParts, Category);
                ResourceStack newStack = new ResourceStack(clickedStack.blueprint, 1);
                Platform.Inventory.AddResource(newStack, partIndex);
                optionsGrid.inventory.RemoveResource(new ResourceStack(clickedStack.blueprint, 1));
                Stack = newStack; // TODO: listen to inventory?
            }
            ToggleOptions();
        }
    }
}

