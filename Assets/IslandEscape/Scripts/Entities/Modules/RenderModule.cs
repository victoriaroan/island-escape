using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using IslandEscape.UI;

namespace IslandEscape.Entities.Modules
{
    public abstract class RenderModule : EntityModule
    {
        public Animator animator;
        public abstract Renderer Renderer { get; }

        public GameObject canvas = null;

        public virtual void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void SetAnimatorParams(Vector2 movement)
        {
            // TODO: genericize this with an `AnimatorParam` struct that accepts string, type, value and have this method accept a list of those and loop through them and set params with correct animator method
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Magnitude", movement.magnitude);
        }

        // TODO: should UI stuff be in a separate UIModule?
        // TODO: call this ShowUI?
        /// <summary>
        /// Intialize the world space UI canvas.
        /// </summary>
        private void CanvasInit()
        {
            Debug.Log("init canvas");
            canvas = (GameObject)Instantiate(GameManager.instance.ui.prefabMap[UICompKey.EntityUICanvas]);
            canvas.transform.SetParent(transform, false);
            canvas.transform.localPosition = new Vector3(0f, 0.5f, 0f);
        }

        /// <summary>
        /// Show the world space UI cavnas.
        /// </summary>
        public void CanvasShow()
        {
            Debug.Log("show canvas");
            if (canvas == null)
            {
                CanvasInit();
            }
        }

        /// <summary>
        /// Set the contents of the world space UI.
        /// </summary>
        /// <param name="display"></param>
        public void CanvasSet(GameObject display)
        {
            CanvasShow();
            display.transform.SetParent(canvas.transform, false);
        }

        /// <summary>
        /// Hide the world space UI canvas.
        /// </summary>
        public void CanvasHide()
        {
            if (canvas != null)
            {
                Destroy(canvas);
            }
        }
    }
}
