using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using IslandEscape.Entities.Modules.Commands;

namespace IslandEscape.Entities.Modules
{
    /// <summary>
    /// An Axis-based (keyboard or gamepad) input module, as opposed to point-and-click. Maybe needs better name.
    /// </summary>
    public class AxisInputModule : CommandInputModule
    {
        protected MoveCommand moveCommand = null;

        // TODO: I feel like tracking the interact command should be in the agent module, but also feel like there's already kinda way too many steps in this process so...
        protected AgentModule agentModule;
        protected InteractCommand interactCommand = null;

        public override void Start()
        {
            base.Start();
            agentModule = GetComponent<AgentModule>();
        }

        public void Update()
        {
            // TODO: look into Input System Package
            CheckForMovement();
            CheckForInteraction();
        }

        private void CheckForMovement()
        {
            Vector2 movement = new Vector2((int)Input.GetAxisRaw("Horizontal"), (int)Input.GetAxisRaw("Vertical"));
            if (movement.x != 0 || movement.y != 0)
            {
                QueueMovement(movement);
            }
            else if (moveCommand != null && movement.x == 0 && movement.y == 0)
            {
                QueueMovement(movement);
                moveCommand = null;
            }
        }

        private void CheckForInteraction()
        {
            if (agentModule != null)
            {
                if (Input.GetButton("Interact"))
                {
                }

                if (Input.GetButtonDown("Interact") && agentModule.availableAction != null)
                {
                    // TODO: should we make sure interactCommand is null? I feel like it would be impossible to get a button down without first getting a button up, which clears interactCommand...
                    interactCommand = (InteractCommand)commandModule.InitCommand<InteractCommand>(
                        new Dictionary<string, dynamic> {
                            { "Action", agentModule.availableAction }
                        }
                    );
                    // TODO: hmmmm, can't move and interact at the same time, which might not be a bad thing? or at least not a big deal for this game.
                    commandModule.QueueCommand(interactCommand);
                }
                else if (Input.GetButtonUp("Interact") && interactCommand != null)
                {
                    interactCommand.keyPressed = false;
                    interactCommand = null;
                }
            }
        }

        private void QueueMovement(Vector2 movement)
        {
            if (moveCommand != null)
            {
                // TODO: better way of updating command while in progress?
                moveCommand.Movement = movement;
            }
            else
            {
                moveCommand = (MoveCommand)commandModule.InitCommand<MoveCommand>(
                    new Dictionary<string, dynamic> {
                        { "Movement", new Vector2(movement.x, movement.y) }
                    }
                );
                commandModule.QueueCommand(moveCommand);
            }
        }
    }
}
