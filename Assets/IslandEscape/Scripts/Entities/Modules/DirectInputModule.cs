using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using IslandEscape.Entities.Modules.Commands;

namespace IslandEscape.Entities.Modules
{
    public class DirectInputModule : CommandInputModule
    {
        protected MoveCommand command = null;

        public void Update()
        {
            // TODO: fix diagonal movement being too fast (https://forum.unity.com/threads/diagonal-movement-speed-to-fast.271703/)
            Vector2 movement = new Vector2((int)Input.GetAxisRaw("Horizontal"), (int)Input.GetAxisRaw("Vertical"));
            QueueMovement(movement);
            if (movement.x == 0 && movement.y == 0)
            {
                command = null;
            }
        }

        private void QueueMovement(Vector2 movement)
        {
            if (command != null)
            {
                command.Movement = movement;
            }
            else
            {
                command = (MoveCommand)commandModule.InitCommand<MoveCommand>(
                    new Dictionary<string, dynamic> {
                        { "Movement", new Vector2(movement.x, movement.y) }
                    }
                );
                commandModule.QueueCommand(command);
            }
        }
    }
}
