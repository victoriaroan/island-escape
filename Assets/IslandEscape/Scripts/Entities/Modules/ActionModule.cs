using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using IslandEscape.Entities.Events;

namespace IslandEscape.Entities.Modules
{
    public class ActionModule : EntityModule
    {
        public ActionEvent AgentEntered { get; } = new ActionEvent();
        public ActionEvent AgentExited { get; } = new ActionEvent();
        public ActionEvent ActionStarted { get; } = new ActionEvent();
        public ActionEvent ActionInterrupted { get; } = new ActionEvent();
        public ActionEvent ActionCompleted = new ActionEvent();
        public ActionEvent CooldownRefreshed { get; } = new ActionEvent();
        // HA got it. need to make it not a property IE just `public ActionEvent CooldownRefreshed` - not sure what implications that has

        public float delay;
        public float cooldown;
        public int limit;

        public float interactionStarted = 0f;
        public AgentModule interactingAgent;
        public int timesCompleted;

        // TODO: show time and tool requirement in tooltip
        // TODO: show entity name in tooltip?
        public string Tip { get => (delay > 0 ? "hold" : "press") + " F"; }
        // TODO: add cooldown to available
        public bool Available { get { return limit == 0 || timesCompleted < limit; } }
        public float InteractionTime
        {
            get { return (interactionStarted > 0f && delay > 0f) ? Time.timeSinceLevelLoad - interactionStarted : 0f; }
        }
        public bool ActionReady
        {
            get { return (interactionStarted > 0f && (delay == 0f || InteractionTime >= delay)); }
        }

        protected RenderModule renderModule;
        protected AgentModule collision = null;
        public bool HasAvailableAgent { get => collision != null; }

        public virtual void Awake()
        {
            AddRequiredModule(typeof(RenderModule));
        }

        // TODO: use command module for actions? I dunno....

        // TODO: multiple actions on a single entity

        public override void Start()
        {
            base.Start();
            renderModule = GetComponent<RenderModule>();
        }

        public void Update()
        {
            /* TODO: are we okay with relying on/making the agent be the one to trigger start/finish of action? I think so
                     ehhh, I kinda feel like the ActionModule should be the one triggering finish.... but then how do we only do this on key up?
                     And what about instant/non-delayed commands?
             */
            // if (Available && interactionStarted > 0f && (delay == 0f || InteractionTime >= delay)) {
            //     FinishAction();
            // }

            if (interactionStarted > 0f && delay > 0f)
            {
                var tip = (int)Math.Floor(InteractionTime) + "s" + (ActionReady ? " READY" : "");
                renderModule.CanvasSetToolTip(tip);
            }
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            // TODO: cooldowns
            // TODO: handle overlapping trigger hitboxes
            if (Available)
            {
                AgentModule agent = other.GetComponent<AgentModule>();
                if (agent != null)
                {
                    AgentEntered?.Invoke(new ActionEventArgs(this, agent));
                    // TODO: highlight this Entity
                    // TODO: show hint to press F
                    agent.availableAction = this;
                    collision = agent;
                    GameManager.instance.ui.ToolTipAdd(Tip, Entity);
                }
            }
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            AgentModule agent = other.GetComponent<AgentModule>();
            if (agent != null)
            {
                AgentExited?.Invoke(new ActionEventArgs(this, agent));
                agent.availableAction = null;
            }

            collision = null;

            if (renderModule.HasToolTip(Tip))
                renderModule.CanvasHide();
        }

        /// <summary>
        /// Start the action with the given agent.
        /// </summary>
        /// <param name="agent"></param>
        /// <returns>Whether or not the action was successfully started.</returns>
        public bool StartAction(AgentModule agent)
        {
            // TODO: add ability to perform action within a range, not just with trigger collision
            // TODO: do we need to pass in the agent?
            if (collision == agent && Available)
            {
                interactionStarted = Time.timeSinceLevelLoad;
                interactingAgent = agent;
                ActionStarted?.Invoke(new ActionEventArgs(this, agent));
                return true;
            }

            return false;
        }

        /// <summary>
        /// Interrupt the in progress action
        /// </summary>
        /// <param name="agent"></param>
        public void InterruptAction(AgentModule agent)
        {
            // TODO: again, do we need to keep passing in the agent?
            // TODO: okay to rely on InteractCommand/AgentModule to interrupt? I feel like it should be the ActionModule triggering this
            interactionStarted = 0f;
            interactingAgent = null;
            ActionInterrupted?.Invoke(new ActionEventArgs(this, agent));
        }

        /// <summary>
        /// Attempt to finish the action with the given agent.
        /// </summary>
        /// <returns>Whether or not the action was successfully completed.</returns>
        public bool FinishAction()
        {
            if (Available && ActionReady)
            {
                if (!Available)
                    renderModule.CanvasHide();
                else
                    renderModule.CanvasSetToolTip(Tip);

                timesCompleted++;
                ActionCompleted?.Invoke(new ActionEventArgs(this, interactingAgent));

                interactionStarted = 0f;
                interactingAgent = null;

                return true;
            }

            return false;
        }
    }
}

