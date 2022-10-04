using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IslandEscape.UI
{
    public enum WindowType
    {
        Platform,
        Inventory,
    }

    [Serializable]
    public struct WindowPrefab
    {
        public WindowType type;
        public GameObject prefab;
    }

    public class Window : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public UIEvent WindowOpened { get; } = new UIEvent();
        public UIEvent WindowFocused { get; } = new UIEvent();
        public UIEvent WindowBlurred { get; } = new UIEvent(); // lost focus
        public UIEvent WindowClosed { get; } = new UIEvent();

        public GameObject source;

        protected bool dragging;
        protected Vector2 offset;

        protected string[] validHandles = new string[] { "Header", "Header/Text" };

        protected List<GameObject> handles;

        public virtual void Awake()
        {
            handles = new List<GameObject>();
            if (validHandles.Length > 0)
            {
                foreach (string handle in validHandles)
                    handles.Add(transform.Find(handle).gameObject);
            }

            WindowOpened?.Invoke(new UIEventArgs(this));
        }

        void OnDestroy()
        {
            WindowClosed?.Invoke(new UIEventArgs(this));
        }

        public void OnBeginDrag(PointerEventData data)
        {
            if (handles.Count > 0)
            {
                foreach (GameObject handle in handles)
                {
                    if (data.pointerEnter == handle)
                    {
                        InitDrag(data);
                        return;
                    }
                }
            }
            else
            {
                InitDrag(data);
            }
        }

        public void InitDrag(PointerEventData data)
        {
            dragging = true;
            offset = new Vector2(transform.position.x - data.position.x, transform.position.y - data.position.y);
        }

        public void OnDrag(PointerEventData data)
        {
            // Only drag if a valid handle was clicked.
            if (dragging)
            {
                transform.position = data.position + offset;
            }
        }

        public void OnEndDrag(PointerEventData data)
        {
            dragging = false;
        }

        public void OnClose()
        {
            GameManager.instance.ui.CloseWindow(gameObject);
        }
    }
}
