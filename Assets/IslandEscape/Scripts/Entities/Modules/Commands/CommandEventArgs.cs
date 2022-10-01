using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace IslandEscape.Entities.Modules.Commands
{
    public class CommandEventArgs : EntityEventArgs
    {
        public Command command;

        public CommandEventArgs(MonoBehaviour publisher, Command command) : base(publisher)
        {
            this.command = command;
        }
    }
}