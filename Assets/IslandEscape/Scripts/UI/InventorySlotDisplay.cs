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
    // TODO: do we need PassthroughTrigger? guess we can just pass through events as necessary. EventTrigger is obsolete anyway
    public class InventorySlotDisplay : UIComponent, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Color32 normalColor = new Color32(0, 0, 0, 100);
        public Color32 highlightedColor = new Color32(255, 255, 255, 100);
        public Color32 selectedColor = new Color32(255, 255, 255, 140);
        public Color32 unavailableSpriteColor = new Color32(255, 255, 255, 75);
        public Color32 availableSpriteColor = new Color32(255, 255, 255, 255);

        public ResourceStack stack;
        public bool isSelected;
        public GameObject tooltip;

        private InventoryGrid parentGrid;
        private GameObject imagePanel;
        private GameObject countPanel;

        public bool ShowCount { get { return parentGrid.showCount; } }

        public bool IsAvailable { get => true; }
        public virtual bool IsSet { get { return stack != null; } }

        public override bool RaycastTarget
        {
            set
            {
                transform.GetComponent<Image>().raycastTarget = value;
                imagePanel.GetComponent<Image>().raycastTarget = value;
                countPanel.GetComponent<Image>().raycastTarget = value;
                countPanel.GetComponentInChildren<TextMeshProUGUI>().raycastTarget = value;
            }
        }

        // drag/drop stuff

        private Vector2 dragOffset;
        private Vector3 originalPosition;

        // TODO: available/unavailable

        public override void RegisterReferences() { }

        public void Set(ResourceStack newStack)
        {
            parentGrid = transform.parent.GetComponent<InventorySlot>().parentGrid;
            imagePanel = transform.Find("Image").gameObject;
            countPanel = transform.Find("Count").gameObject;
            RemoveHighlight();

            stack = newStack;
            imagePanel.GetComponent<Image>().sprite = stack.blueprint.icon;
            imagePanel.GetComponent<RectTransform>().sizeDelta = new Vector2(parentGrid.slotSize.x, parentGrid.slotSize.y);

            float compHeight = parentGrid.slotSize.y * 0.25f;
            float compPadding = compHeight * 0.1667f;

            if (ShowCount)
            {
                countPanel.SetActive(true);
                countPanel.GetComponentInChildren<TextMeshProUGUI>().text = stack.count.ToString();
                RectTransform countTransform = countPanel.GetComponent<RectTransform>();
                countTransform.sizeDelta = new Vector2(parentGrid.slotSize.x, compHeight);
                countTransform.anchoredPosition = new Vector2(0f, -parentGrid.slotSize.y + compHeight);
            }
        }

        private void SetHightlight()
        {
            transform.GetComponent<Image>().color = highlightedColor;
        }

        private void RemoveHighlight()
        {
            if (isSelected)
                transform.GetComponent<Image>().color = selectedColor;
            else
                transform.GetComponent<Image>().color = normalColor;
        }

        public void ShowToolTip(PointerEventData data)
        {
            if (IsSet)
                tooltip = GameManager.instance.ui.ResourceToolTipAddAtPointer(stack, data.position);
        }

        public void HideToolTip()
        {
            if (tooltip != null)
            {
                GameManager.instance.ui.ToolTipRemove(tooltip);
                tooltip = null;
            }
        }

        public void OnPointerClick(PointerEventData data)
        {
            parentGrid.onClickStack.Invoke(data);
        }

        public void OnPointerEnter(PointerEventData data)
        {
            ShowToolTip(data);
            SetHightlight();
        }

        public void OnPointerExit(PointerEventData data)
        {
            HideToolTip();
            RemoveHighlight();
        }

        public void OnBeginDrag(PointerEventData data)
        {
            // Debug.Log("Started drag", gameObject);
            // Debug.Log("pressed object", data.pointerPress);
            if (parentGrid.enableDragging && IsSet)
            {
                Vector2 cursorPos = Camera.main.ScreenToWorldPoint(data.position);
                dragOffset = new Vector2(transform.position.x - cursorPos.x, transform.position.y - cursorPos.y);
                originalPosition = transform.position;
                RaycastTarget = false;
            }
        }

        public void OnDrag(PointerEventData data)
        {
            if (parentGrid.enableDragging && IsSet)
            {
                Vector2 cursorPos = Camera.main.ScreenToWorldPoint(data.position);
                transform.position = cursorPos + dragOffset;
            }
        }
        public void OnEndDrag(PointerEventData data)
        {
            // Debug.Log("Ended drag", gameObject);
            // Debug.Log("pressed object", data.pointerPress);
            RaycastTarget = true;
            if (parentGrid.enableDragging && IsSet)
            {
                transform.position = originalPosition;
            }
        }

        void OnDestroy()
        {
            HideToolTip();
        }
    }
}
