using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using IslandEscape.UI;

namespace IslandEscape.Entities.Modules
{
    public abstract class RenderModule : EntityModule
    {
        public Animator animator;
        public abstract Renderer Renderer { get; }

        public GameObject canvas = null;

        private GameObject tooltip = null;
        private float tooltipSet = 0f;
        private float clearTooltip = -1f;
        private string replacementTip = null;
        private Func<bool> replacementCondition = null;

        public virtual void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public virtual void Update()
        {
            if (clearTooltip > 0f && Time.timeSinceLevelLoad - tooltipSet >= clearTooltip)
            {
                if (replacementTip != null && (replacementCondition == null || replacementCondition()))
                    CanvasSetToolTip(replacementTip);
                else
                    Destroy(tooltip);

                tooltipSet = 0f;
                clearTooltip = -1f;
                replacementTip = null;
            }
        }

        public void SetAnimatorParams(Vector2 movement)
        {
            // TODO: genericize this with an `AnimatorParam` struct that accepts string, type, value and have this method accept a list of those and loop through them and set params with correct animator method
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Magnitude", movement.magnitude);
        }

        public bool HasToolTip(string msg)
        {
            return tooltip != null && tooltip.GetComponentInChildren<TextMeshProUGUI>().text == msg;
        }

        // TODO: should UI stuff be in a separate UIModule?
        // TODO: call this ShowUI?
        /// <summary>
        /// Intialize the world space UI canvas.
        /// </summary>
        private void CanvasInit()
        {
            canvas = (GameObject)Instantiate(GameManager.instance.ui.componentMap[UICompKey.EntityUICanvas]);
            canvas.transform.SetParent(transform, false);
            canvas.transform.localPosition = new Vector3(0f, 0.5f, 0f);
        }

        /// <summary>
        /// Show the world space UI cavnas.
        /// </summary>
        public void CanvasShow()
        {
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
            // TODO: things other than tooltips
            CanvasShow();
            display.transform.SetParent(canvas.transform, false);
            tooltip = display;
        }

        /// <summary>
        /// Set the text of the tooltip
        /// </summary>
        /// <param name="tip"></param>
        public void CanvasSetToolTip(string tip)
        {
            if (tooltip == null)
            {
                tooltip = GameManager.instance.ui.ToolTipAdd(tip);
                CanvasSet(tooltip);
            }
            else
                tooltip.GetComponentInChildren<TextMeshProUGUI>().text = tip;

        }

        /// <summary>
        /// Set the text of the tooltip to a temporary value, with no replacement value
        /// </summary>
        /// <param name="tip"></param>
        public void CanvasSetTempToolTip(string tip, float clearAfter)
        {
            CanvasSetToolTip(tip);
            tooltipSet = Time.timeSinceLevelLoad;
            clearTooltip = clearAfter;
            replacementTip = null;
        }

        /// <summary>
        /// Set the text of the tooltip to a temporary value, with a replacement value
        /// </summary>
        /// <param name="tip"></param>
        public void CanvasSetTempToolTip(string tip, float clearAfter, string replace)
        {
            CanvasSetTempToolTip(tip, clearAfter);
            replacementTip = replace;
        }

        /// <summary>
        /// Set the text of the tooltip to a temporary value, with a replacement value that is only
        /// used if the given condition evaluates to true at time of replacement.
        /// </summary>
        /// <param name="tip"></param>
        public void CanvasSetTempToolTip(string tip, float clearAfter, string replace, Func<bool> condition)
        {
            CanvasSetToolTip(tip);
            tooltipSet = Time.timeSinceLevelLoad;
            clearTooltip = clearAfter;
            replacementTip = replace;
            replacementCondition = condition;
        }

        /// <summary>
        /// Hide the world space UI canvas.
        /// </summary>
        public void CanvasHide()
        {
            if (canvas != null)
            {
                Destroy(canvas);
                tooltip = null;
            }
        }
    }
}
