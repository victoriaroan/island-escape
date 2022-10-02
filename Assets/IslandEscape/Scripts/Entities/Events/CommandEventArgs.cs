using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using IslandEscape.Entities.Modules.Commands;

namespace IslandEscape.Entities.Events
{
    [Serializable]
    public class CommandEventArgs : EntityEventArgs
    {
        public Command command;

        public CommandEventArgs(MonoBehaviour publisher, Command command) : base(publisher)
        {
            this.command = command;
        }
    }
}