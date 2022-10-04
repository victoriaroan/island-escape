using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using IslandEscape.Resources;

namespace IslandEscape.UI
{
    public class ResourceSlot : UIComponent, IDropHandler
    {
        public GameObject displayPrefab;

        // TODO: figure out how to make stack private but still show in inspector via property?
        public ResourceStack stack;
        public ResourceStack Stack
        {
            get => stack;
            set
            {
                stack = value;
                SetDisplay();
            }
        }

        public bool HasStack { get { return stack != null && stack.IsSet; } }

        // TODO: implement disabling interaction (just here, both here and in display? just in display?)
        public bool interactable = true;

        // references
        private Image background;
        private GameObject display;

        public override bool RaycastTarget { set => background.raycastTarget = value; }

        public override void RegisterReferences()
        {
            background = GetComponent<Image>();
        }

        private void SetDisplay()
        {
            if (HasStack)
            {
                if (display == null)
                {
                    display = (GameObject)Instantiate(displayPrefab);
                    display.transform.SetParent(transform, false);
                    display.SetActive(true);
                }
                var slotDisplay = display.GetComponent<ResourceSlotDisplay>();
                slotDisplay.Stack = stack;
            }
            else if (display != null)
                Destroy(display);
        }

        private void RemoveDisplay()
        {

        }

        public void OnDrop(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }
    }
}

