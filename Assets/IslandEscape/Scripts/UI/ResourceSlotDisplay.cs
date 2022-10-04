using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

using IslandEscape.Resources;

namespace IslandEscape.UI
{

    public class ResourceSlotDisplay : UIComponent, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private ResourceStack stack;
        public ResourceStack Stack
        {
            set
            {
                stack = value;
                resourceImage.sprite = stack.blueprint.sprite;
                SetCount();
            }
        }

        private bool showCount = false;
        public bool ShowCount
        {
            get => showCount;
            set
            {
                showCount = value;
                SetCount();
            }
        }

        public virtual bool IsSet { get { return stack != null; } }

        public override bool RaycastTarget
        {
            set
            {
                background.raycastTarget = value;
                resourceImage.raycastTarget = value;
                // countBackground.raycastTarget = value;
                // countText.raycastTarget = value;
            }
        }

        // TODO: implement disabling interaction
        public bool interactable = true;

        // references
        private Image background;
        private Image resourceImage;
        private GameObject countPanel;
        private Image countBackground;
        private TextMeshProUGUI countText;

        // drag/drop stuff
        private Vector2 dragOffset;
        private Vector3 originalPosition;

        public override void RegisterReferences()
        {
            background = GetComponent<Image>();
            resourceImage = transform.Find("Image").GetComponent<Image>();
            // countPanel = transform.Find("Count").gameObject;
            // countBackground = countPanel.GetComponent<Image>();
            // countText = countPanel.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        }

        // TODO: implement count UI
        private void SetCount()
        {
            if (showCount)
            {

            }
            else
            {

            }

        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public void OnDrag(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }
    }
}
