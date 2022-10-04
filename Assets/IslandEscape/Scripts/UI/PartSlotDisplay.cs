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
    public class PartSlotDisplay : InventorySlotDisplay
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

        public ResourceStack Stack
        {
            get => stack;
            set
            {
                stack = value;
                resourceImage.sprite = stack.blueprint.sprite;
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

        private InventoryGrid optionsGrid;
        private TextMeshProUGUI slotName;
        private Button selectButton;
        private Image resourceImage;
        private RectTransform arrowTransform;

        public override void RegisterReferences()
        {
            base.RegisterReferences();
            slotName = transform.Find("SlotName").GetComponent<TextMeshProUGUI>();
            selectButton = transform.Find("ResourceSlot/Button").GetComponent<Button>();
            resourceImage = transform.Find("ResourceSlot/Button/Image").GetComponent<Image>();
            arrowTransform = transform.Find("ResourceSlot/Button/Arrow").GetComponent<RectTransform>();
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
            if (stack != null)
            {
                // TODO: swapping parts
                Platform.GetComponent<RenderModule>().CanvasSetTempToolTip("Slot full", 2.0f);
            }
            else
            {
                GameObject clickedStackDisplay = args.pointerPress;
                ResourceStack clickedStack = clickedStackDisplay.GetComponent<InventorySlotDisplay>().stack;
                int partIndex = Array.IndexOf(Platform.requiredParts, Category);
                Platform.Inventory.AddResource(new ResourceStack(clickedStack.blueprint, 1), partIndex);
                optionsGrid.inventory.RemoveResource(new ResourceStack(clickedStack.blueprint, 1));
            }
            ToggleOptions();
        }
    }
}

