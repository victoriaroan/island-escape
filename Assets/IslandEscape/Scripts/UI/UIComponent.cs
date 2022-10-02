using System;
using UnityEngine;

namespace IslandEscape.UI
{
    /// <summary>
    /// Base UI Component class that standardizes some Island Escape UI behavior.
    /// </summary>
    public abstract class UIComponent : MonoBehaviour
    {
        public bool Active
        {
            get { return gameObject.activeInHierarchy; }
            set { gameObject.SetActive(value); }
        }
        public abstract bool RaycastTarget { set; }

        protected RectTransform rectTransform;

        /// <summary>
        /// Baseline Awake function that creates a RectTransform reference and calls RegisterReferences.
        /// TODO: should Component set Active = false by default here?
        /// </summary>
        public virtual void Awake()
        {
            rectTransform = gameObject.GetComponent<RectTransform>();
            RegisterReferences();
        }

        /// <summary>
        /// Handy function to contain all the components references to gameobjects and other components.
        /// </summary>
        public abstract void RegisterReferences();
    }
}